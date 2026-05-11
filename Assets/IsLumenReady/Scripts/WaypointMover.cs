using UnityEngine;
using UnityEngine.Splines;

// SETUP:
// 1. Asigna el SplineContainer al campo splineContainer.
// 2. Asigna los 9 GameObjects de parada al array stopPoints en orden.
// 3. Asigna los 8 SegmentData (uno por tramo) al array segmentData.
//    segmentData[0] = tramo entre stopPoints[0] y stopPoints[1], etc.
//
// El segmento activo se determina automaticamente comparando la posicion
// del personaje con los pares de stopPoints adyacentes, sin necesidad
// de knots ni indices manuales.
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
    private float currentT = 0f;
    private float targetT = 0f;
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
        Invoke(nameof(BeginJourney), 0.1f);
    }

    void BeginJourney()
    {
        AdvanceToNextStop();
    }

    void Update()
    {
        if (!isMoving) return;

        float step = (speed / splineLength) * Time.deltaTime;
        currentT = Mathf.MoveTowards(currentT, targetT, step);

        Vector3 pos = splineContainer.EvaluatePosition(currentT);
        pos.z = transform.position.z;

        FaceDirection(pos - transform.position);
        transform.position = pos;

        if (Mathf.Abs(currentT - targetT) < 0.0005f)
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
        OnReachStop?.Invoke(currentStopIndex);
    }

    // -- Public API ------------------------------------------------------------

    // Called by QuestionManager after the question for the current segment
    // has been resolved (correctly or not).
    public void AdvanceToNextStop()
    {
        int next = currentStopIndex + 1;

        if (next >= stopPoints.Length)
        {
            Debug.Log("Recorrido terminado.");
            SetWalking(false);
            return;
        }

        currentStopIndex = next;
        targetT = GetClosestT(stopPoints[currentStopIndex].position);
        isMoving = true;
        SetWalking(true);
    }

    public void ModifySpeed(float multiplier)
    {
        speed = baseSpeed * multiplier;
    }

    // -- Segment detection -----------------------------------------------------

    // Returns the index of the segment the character is currently travelling.
    // Segment N = path from stopPoints[N] to stopPoints[N+1].
    // Detected by finding which consecutive pair of stop-point t-values
    // brackets the current currentT.
    public int CurrentSegmentIndex
    {
        get
        {
            // Cache t values for each stop point at runtime
            int count = stopPoints.Length;
            if (count < 2) return 0;

            for (int i = 0; i < count - 1; i++)
            {
                float tA = GetClosestT(stopPoints[i].position);
                float tB = GetClosestT(stopPoints[i + 1].position);

                float lo = Mathf.Min(tA, tB);
                float hi = Mathf.Max(tA, tB);

                if (currentT >= lo && currentT <= hi)
                    return i;
            }

            // Fallback: last segment
            return count - 2;
        }
    }

    public SegmentData CurrentSegmentData
    {
        get
        {
            int idx = CurrentSegmentIndex;
            if (segmentData == null || idx >= segmentData.Length) return null;
            return segmentData[idx];
        }
    }

    // -- Helpers ---------------------------------------------------------------

    // Caches t per stop to avoid redundant calls in CurrentSegmentIndex.
    // Call once after Start if the spline is static for better performance.
    float[] cachedStopT;

    public void CacheStopTs()
    {
        cachedStopT = new float[stopPoints.Length];
        for (int i = 0; i < stopPoints.Length; i++)
            cachedStopT[i] = GetClosestT(stopPoints[i].position);
    }

    float GetClosestT(Vector3 worldPos)
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