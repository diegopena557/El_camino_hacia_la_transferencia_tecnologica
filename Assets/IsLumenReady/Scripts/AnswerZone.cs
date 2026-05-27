using UnityEngine;
using UnityEngine.Splines;

[ExecuteInEditMode]
public class AnswerZone : MonoBehaviour
{
    [Header("Segmento al que pertenece esta zona")]
    [Tooltip("0 = tramo entre stopPoints[0] y stopPoints[1], etc.")]
    public int segmentIndex = 0;

    [Header("Marcadores de zona (GameObjects sobre el spline)")]
    [Tooltip("Fin de zona temprana - respuestas antes de este punto son Early")]
    public Transform earlyEnd;
    [Tooltip("Fin de zona perfecta - respuestas entre earlyEnd y aqui son Perfect")]
    public Transform perfectEnd;
    [Tooltip("Fin de zona tardia - respuestas entre perfectEnd y aqui son Late")]
    public Transform lateEnd;

    [Header("Spline de referencia (mismo que usa WaypointMover)")]
    public SplineContainer splineContainer;

    // Cached raw spline-t values
    [HideInInspector] public float cachedEarlyEndT = 0.33f;
    [HideInInspector] public float cachedPerfectEndT = 0.66f;
    [HideInInspector] public float cachedLateEndT = 1f;

    private bool cached = false;

    // -- Unity -----------------------------------------------------------------

    void Start() { CacheValues(); }
    void OnEnable() { CacheValues(); }

    // -- Public API ------------------------------------------------------------

    public void CacheValues()
    {
        if (splineContainer == null) return;

        if (earlyEnd != null) cachedEarlyEndT = GetT(earlyEnd.position);
        if (perfectEnd != null) cachedPerfectEndT = GetT(perfectEnd.position);
        if (lateEnd != null) cachedLateEndT = GetT(lateEnd.position);

        cached = true;
    }

    // Returns: -1 = Early, 0 = Perfect, 1 = Late, 2 = No answer (past lateEnd)
    public int EvaluateTiming(float rawT)
    {
        if (!cached) CacheValues();

        if (rawT <= cachedEarlyEndT) return -1;
        if (rawT <= cachedPerfectEndT) return 0;
        if (rawT <= cachedLateEndT) return 1;
        return 2;   // past lateEnd - treated as no answer
    }

    // -- Helpers ---------------------------------------------------------------

    float GetT(Vector3 worldPos)
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

    // -- Gizmos ----------------------------------------------------------------

    void OnDrawGizmos()
    {
        float radius = 0.18f;

        if (earlyEnd != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(earlyEnd.position, radius);
            Gizmos.DrawLine(earlyEnd.position, earlyEnd.position + Vector3.up * 0.5f);
        }

        if (perfectEnd != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(perfectEnd.position, radius);
            Gizmos.DrawLine(perfectEnd.position, perfectEnd.position + Vector3.up * 0.5f);
        }

        if (lateEnd != null)
        {
            Gizmos.color = new Color(1f, 0.5f, 0f);
            Gizmos.DrawSphere(lateEnd.position, radius);
            Gizmos.DrawLine(lateEnd.position, lateEnd.position + Vector3.up * 0.5f);
        }

        // Draw zone bands between markers
        if (earlyEnd != null && perfectEnd != null)
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
            Gizmos.DrawLine(earlyEnd.position, perfectEnd.position);
        }

        if (perfectEnd != null && lateEnd != null)
        {
            Gizmos.color = new Color(1f, 0.5f, 0f, 0.2f);
            Gizmos.DrawLine(perfectEnd.position, lateEnd.position);
        }

#if UNITY_EDITOR
        if (earlyEnd != null)
            UnityEditor.Handles.Label(earlyEnd.position + Vector3.up * 0.6f, "Early " + segmentIndex);
        if (perfectEnd != null)
            UnityEditor.Handles.Label(perfectEnd.position + Vector3.up * 0.6f, "Perfect " + segmentIndex);
        if (lateEnd != null)
            UnityEditor.Handles.Label(lateEnd.position + Vector3.up * 0.6f, "Late " + segmentIndex);
#endif
    }
}