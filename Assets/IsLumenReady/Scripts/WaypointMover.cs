using UnityEngine;
using UnityEngine.Splines;


[RequireComponent(typeof(Animator))]
public class WaypointMover : MonoBehaviour
{
    [Header("Spline")]
    public SplineContainer splineContainer;

    [Header("Puntos de parada (orden del recorrido)")]
    public Transform[] stopPoints;

    [Header("Datos por segmento - segmentData[0] va de stopPoints[0] a stopPoints[1]")]
    public SegmentData[] segmentData;

    [Header("Velocidad")]
    public float speed = 3f;
    private float baseSpeed;

    // -- Estado ----------------------------------------------------------------
    private int currentStopIndex = 0;
    private int currentSegment = -1;
    private float currentT = 0f;
    private float segmentStartT = 0f;   // spline-t at the start of this segment
    private float segmentEndT = 0f;   // spline-t at the end of this segment
    private bool isMoving = false;
    private float splineLength;

    private Animator anim;

    // Fired when the character arrives at a stop. Passes the stop index.
    public System.Action<int> OnReachStop;

    // -- Unity -----------------------------------------------------------------

    void Start()
    {
        baseSpeed = speed;
        anim = GetComponent<Animator>();
        splineLength = splineContainer.CalculateLength();

        if (stopPoints.Length > 0)
        {
            currentT = GetClosestT(stopPoints[0].position);
            transform.position = stopPoints[0].position;
        }

        SetWalking(false);
    }

    public void BeginJourney()
    {
        AdvanceToNextStop();
    }

    void Update()
    {
        if (!isMoving) return;

        float step = (speed / splineLength) * Time.deltaTime;
        currentT = Mathf.MoveTowards(currentT, segmentEndT, step);

        Vector3 pos = splineContainer.EvaluatePosition(currentT);
        pos.z = transform.position.z;

        FaceDirection(pos - transform.position);
        transform.position = pos;

        if (Mathf.Abs(currentT - segmentEndT) < 0.0005f)
            ArriveAtStop();
    }

    // -- Arrival ---------------------------------------------------------------

    void ArriveAtStop()
    {
        Vector3 snap = stopPoints[currentStopIndex].position;
        snap.z = transform.position.z;
        transform.position = snap;

        isMoving = false;
        SetWalking(false);

        // Si es el último punto del recorrido
        if (currentStopIndex >= stopPoints.Length - 1)
        {
            Debug.Log("Recorrido terminado.");

            if (GameResultsManager.Instance != null)
                GameResultsManager.Instance.ShowResults();

            return;
        }

        OnReachStop?.Invoke(currentStopIndex);
    }

    // -- Public API ------------------------------------------------------------

    public void AdvanceToNextStop()
    {
        int next = currentStopIndex + 1;

        if (next >= stopPoints.Length)
        {
            Debug.Log("Recorrido terminado.");
            SetWalking(false);
            return;
        }

        currentSegment = next - 1;
        currentStopIndex = next;

        // Record the spline-t range of this segment so SegmentProgress is accurate
        segmentStartT = GetClosestT(stopPoints[currentStopIndex - 1].position);
        segmentEndT = GetClosestT(stopPoints[currentStopIndex].position);

        isMoving = true;
        SetWalking(true);
    }

    public void ModifySpeed(float multiplier)
    {
        speed = baseSpeed * multiplier;
    }

    // Pauses or resumes movement without resetting state.
    // Used by StopQuestionManager to freeze the character mid-segment.
    public void SetPaused(bool paused)
    {
        isMoving = !paused;
        SetWalking(!paused);
    }

    // -- Segment info ----------------------------------------------------------

    // How far along the current segment the character is, 0 = just left the
    // previous stop, 1 = arrived at the next stop.
    // QuestionManager reads this when the player answers to evaluate timing.
    public float SegmentProgress
    {
        get
        {
            float range = segmentEndT - segmentStartT;
            if (Mathf.Abs(range) < 0.0001f) return 0f;
            return Mathf.Clamp01((currentT - segmentStartT) / range);
        }
    }

    // Raw spline-t at the character's current position.
    // Pass this directly to AnswerZone.EvaluateTiming - no conversion needed.
    public float CurrentRawT => currentT;

    public int CurrentSegmentIndex => currentSegment;

    // Converts a segment-local progress value [0..1] back to raw spline-t.
    // Used by QuestionManager to compare against AnswerZone's cached t values.
    public float GetRawT(float segmentProgress)
    {
        return segmentStartT + segmentProgress * (segmentEndT - segmentStartT);
    }

    public SegmentData CurrentSegmentData
    {
        get
        {
            if (segmentData == null || currentSegment < 0 || currentSegment >= segmentData.Length)
                return null;
            return segmentData[currentSegment];
        }
    }

    // -- Helpers ---------------------------------------------------------------

    public float GetClosestT(Vector3 worldPos)
    {
        SplineUtility.GetNearestPoint(
            splineContainer.Spline,
            splineContainer.transform.InverseTransformPoint(worldPos),
            out _,
            out float t,
            resolution: 64,
            iterations: 8
        );
        return t;
    }

    void FaceDirection(Vector3 delta)
    {
        if (Mathf.Abs(delta.x) < 0.01f) return;
        Vector3 s = transform.localScale;
        s.x = Mathf.Abs(s.x) * Mathf.Sign(delta.x);
        transform.localScale = s;
    }

    void SetWalking(bool value)
    {
        if (anim != null)
            anim.SetBool("isWalking", value);
    }
}