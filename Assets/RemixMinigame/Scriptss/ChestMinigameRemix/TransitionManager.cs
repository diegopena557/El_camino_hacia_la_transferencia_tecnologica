using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;

    [Header("Post Processing (Lens Distortion)")]
    public Volume postProcessVolume; // Volume de URP que contiene el override de Lens Distortion
    public float transitionDuration = 1.5f;    // Duracion de la fase de entrada (hacia 1 / 0.01)
    public float returnTransitionDuration = 1.5f; // Duracion de la fase de regreso (hacia los valores originales)

    [Header("Curvas de Animacion (opcional, para efecto mas dinamico)")]
    public AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public AnimationCurve returnTransitionCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Header("Control de Etapas")]
    public GameObject currentStageParent; // Objeto que se desactiva al iniciar la transicion
    public GameObject nextStageParent;    // Objeto que se activa al terminar la transicion

    private LensDistortion lensDistortion;
    private bool isTransitioning = false;

    private float originalIntensity;
    private float originalScale;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        SetupLensDistortion();
    }

    void SetupLensDistortion()
    {
        if (postProcessVolume == null || postProcessVolume.profile == null)
        {
            Debug.LogWarning("TransitionManager: no se asigno 'postProcessVolume' o no tiene un Profile. El efecto de transicion no se aplicara.");
            return;
        }

        if (!postProcessVolume.profile.TryGet(out lensDistortion))
        {
            Debug.LogWarning("TransitionManager: el Volume Profile asignado no tiene un override de 'Lens Distortion'.");
            return;
        }

        // Asegurar que los parametros que vamos a animar esten activos (override) en el profile
        lensDistortion.intensity.overrideState = true;
        lensDistortion.scale.overrideState = true;

        // Guardar los valores originales del Volume tal como estaban configurados,
        // para poder restaurarlos exactamente al activar la siguiente etapa
        originalIntensity = lensDistortion.intensity.value;
        originalScale = lensDistortion.scale.value;
    }

    //
    // METODO PUBLICO: llamado por otros scripts (ej. CardSpawner) para iniciar el cambio de etapa
    //

    public void TriggerTransition()
    {
        Debug.Log("TransitionManager: TriggerTransition() llamado.");

        if (isTransitioning)
        {
            Debug.LogWarning("TransitionManager: ya hay una transicion en curso, se ignora este llamado.");
            return;
        }

        StartCoroutine(TransitionCoroutine());
    }

    IEnumerator TransitionCoroutine()
    {
        Debug.Log($"TransitionManager: iniciando transicion. currentStageParent={(currentStageParent != null ? currentStageParent.name : "NULL")}, nextStageParent={(nextStageParent != null ? nextStageParent.name : "NULL")}, lensDistortion encontrado={(lensDistortion != null)}");

        isTransitioning = true;

        float startIntensity = lensDistortion != null ? lensDistortion.intensity.value : 0f;
        float startScale = lensDistortion != null ? lensDistortion.scale.value : 1f;

        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / transitionDuration);
            float curvedT = transitionCurve.Evaluate(t);

            if (lensDistortion != null)
            {
                lensDistortion.intensity.value = Mathf.LerpUnclamped(startIntensity, 1f, curvedT);
                lensDistortion.scale.value = Mathf.LerpUnclamped(startScale, 0.01f, curvedT);
            }

            yield return null;
        }

        // Asegurar los valores finales exactos
        if (lensDistortion != null)
        {
            lensDistortion.intensity.value = 1f;
            lensDistortion.scale.value = 0.01f;
        }

        // Apagar la etapa actual y encender la siguiente
        if (currentStageParent != null)
            currentStageParent.SetActive(false);

        if (nextStageParent != null)
            nextStageParent.SetActive(true);

        // Revertir el efecto de Lens Distortion a los valores originales del Volume,
        // de forma gradual (igual que la entrada) en vez de un salto instantaneo
        yield return StartCoroutine(ReturnToOriginalLensDistortion());

        Debug.Log("TransitionManager: transicion completada. Etapa cambiada.");

        isTransitioning = false;
    }

    IEnumerator ReturnToOriginalLensDistortion()
    {
        if (lensDistortion == null)
            yield break;

        float startIntensity = lensDistortion.intensity.value;
        float startScale = lensDistortion.scale.value;

        float elapsed = 0f;

        while (elapsed < returnTransitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / returnTransitionDuration);
            float curvedT = returnTransitionCurve.Evaluate(t);

            lensDistortion.intensity.value = Mathf.LerpUnclamped(startIntensity, originalIntensity, curvedT);
            lensDistortion.scale.value = Mathf.LerpUnclamped(startScale, originalScale, curvedT);

            yield return null;
        }

        // Asegurar los valores finales exactos
        lensDistortion.intensity.value = originalIntensity;
        lensDistortion.scale.value = originalScale;
    }

    //
    // METODO PUBLICO: revertir la distorsion a los valores originales de forma instantanea
    // (util si algun otro script necesita resetear el efecto sin animacion)
    //

    public void ResetLensDistortion()
    {
        if (lensDistortion != null)
        {
            lensDistortion.intensity.value = originalIntensity;
            lensDistortion.scale.value = originalScale;
        }
    }
}