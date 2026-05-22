using UnityEngine;
using UnityEngine.Splines;

// SETUP EN ESCENA:
// 1. Crea un GameObject por segmento, ej: "AnswerZone_0", "AnswerZone_1"...
// 2. Agrega este componente.
// 3. Asigna el segmentIndex (0 = primer tramo, 1 = segundo, etc.)
// 4. Crea dos GameObjects vacios hijos: "ZoneStart" y "ZoneEnd".
//    Colocalos visualmente sobre el spline donde quieres que empiece
//    y termine la zona de respuesta perfecta.
// 5. Arrastra esos GameObjects a los campos zoneStart y zoneEnd.
//
// En el Editor veras dos esferas de colores sobre el spline que puedes
// mover libremente. No hay colliders ni fisica involucrada.
//
// El componente cachea los valores t del spline en Start() para que
// la evaluacion en runtime sea instantanea.

[ExecuteInEditMode]
public class AnswerZone : MonoBehaviour
{
    [Header("Segmento al que pertenece esta zona")]
    [Tooltip("0 = tramo entre stopPoints[0] y stopPoints[1], etc.")]
    public int segmentIndex = 0;

    [Header("Marcadores visuales (GameObjects sobre el spline)")]
    [Tooltip("Inicio de la zona perfecta - muevelo sobre el spline")]
    public Transform zoneStart;
    [Tooltip("Final de la zona perfecta - muevelo sobre el spline")]
    public Transform zoneEnd;

    [Header("Spline de referencia (mismo que usa WaypointMover)")]
    public SplineContainer splineContainer;

    // Cached t values computed once at Start
    [HideInInspector] public float cachedStartT = 0f;
    [HideInInspector] public float cachedEndT = 1f;

    private bool cached = false;

    // -- Unity -----------------------------------------------------------------

    void Start()
    {
        CacheValues();
    }

    // Also cache in editor when the component is enabled or values change
    void OnEnable()
    {
        CacheValues();
    }

    // -- Public API ------------------------------------------------------------

    // Call this at runtime to refresh cached t values (e.g. if zone markers moved)
    public void CacheValues()
    {
        if (splineContainer == null || zoneStart == null || zoneEnd == null) return;

        cachedStartT = GetT(zoneStart.position);
        cachedEndT = GetT(zoneEnd.position);
        cached = true;
    }

    // Returns true if the given spline-t falls inside the perfect zone
    public bool IsInZone(float t)
    {
        if (!cached) CacheValues();
        float lo = Mathf.Min(cachedStartT, cachedEndT);
        float hi = Mathf.Max(cachedStartT, cachedEndT);
        return t >= lo && t <= hi;
    }

    // Returns Early (-1), Perfect (0), or Late (1) for a given spline-t
    public int EvaluateTiming(float t)
    {
        if (!cached) CacheValues();
        float lo = Mathf.Min(cachedStartT, cachedEndT);
        float hi = Mathf.Max(cachedStartT, cachedEndT);

        if (t < lo) return -1;  // Early
        if (t > hi) return 1;  // Late
        return 0;                // Perfect
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
        if (zoneStart != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(zoneStart.position, 0.18f);
            Gizmos.DrawLine(zoneStart.position, zoneStart.position + Vector3.up * 0.4f);
        }

        if (zoneEnd != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(zoneEnd.position, 0.18f);
            Gizmos.DrawLine(zoneEnd.position, zoneEnd.position + Vector3.up * 0.4f);
        }

        if (zoneStart != null && zoneEnd != null)
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
            Gizmos.DrawLine(zoneStart.position, zoneEnd.position);

#if UNITY_EDITOR
            UnityEditor.Handles.color = new Color(0f, 1f, 0f, 0.15f);
            UnityEditor.Handles.Label(
                (zoneStart.position + zoneEnd.position) * 0.5f + Vector3.up * 0.5f,
                "Zone " + segmentIndex
            );
#endif
        }
    }
}