using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;

    [Header("Post Processing")]
    public Volume postProcessVolume;

    [Header("Duración")]
    public float transitionDuration = 1.5f;
    public float returnTransitionDuration = 1.5f;

    [Header("Curvas")]
    public AnimationCurve transitionCurve =
        AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    public AnimationCurve returnTransitionCurve =
        AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);


    // =========================================================
    // TRANSICIÓN ORIGINAL
    // MINIJUEGO 1 → MINIJUEGO 2
    // =========================================================

    [Header("Transición inicial")]

    [Tooltip("Padre del Minijuego 1")]
    public GameObject currentStageParent;

    [Tooltip("Padre del Minijuego 2")]
    public GameObject nextStageParent;


    // =========================================================
    // TRANSICIÓN REMIX
    // MINIJUEGO 2 MINIJUEGO 3
    // =========================================================

    [Header("Transición Remix")]

    [Tooltip("Padre del Minijuego 2")]
    public GameObject remixCurrentStageParent;

    [Tooltip("Padre del Minijuego 3")]
    public GameObject remixNextStageParent;


    // =========================================================
    // INTERNOS
    // =========================================================

    private LensDistortion lensDistortion;

    private bool isTransitioning = false;

    private float originalIntensity;
    private float originalScale;


    // =========================================================
    // UNITY
    // =========================================================

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        SetupLensDistortion();
    }

    public void TriggerCustomTransition(GameObject currentParent, GameObject nextParent)
    {
        TriggerTransition(currentParent, nextParent, null);
    }
    // =========================================================
    // LENS DISTORTION
    // =========================================================

    private void SetupLensDistortion()
    {
        if (postProcessVolume == null ||
            postProcessVolume.profile == null)
        {
            Debug.LogWarning(
                "[TransitionManager] No se asignó " +
                "Post Process Volume o no tiene Profile."
            );

            return;
        }

        if (!postProcessVolume.profile.TryGet(out lensDistortion))
        {
            Debug.LogWarning(
                "[TransitionManager] El Volume Profile no tiene " +
                "un override de Lens Distortion."
            );

            return;
        }

        lensDistortion.intensity.overrideState = true;
        lensDistortion.scale.overrideState = true;

        originalIntensity =
            lensDistortion.intensity.value;

        originalScale =
            lensDistortion.scale.value;
    }


    // =========================================================
    // TRANSICIÓN ORIGINAL
    // MINIJUEGO 1  MINIJUEGO 2
    // =========================================================

    /// <summary>
    /// Mantiene el funcionamiento original.
    /// Utiliza:
    /// Current Stage Parent
    /// Next Stage Parent
    /// </summary>
    public void TriggerTransition()
    {
        TriggerTransition(
            currentStageParent,
            nextStageParent,
            null
        );
    }


    // =========================================================
    // TRANSICIÓN REMIX
    // MINIJUEGO 2 MINIJUEGO 3
    // =========================================================

    /// <summary>
    /// Ejecuta específicamente la transición Remix:
    /// Minijuego 2  Minijuego 3.
    /// </summary>
    public void TriggerRemixTransition()
    {
        TriggerTransition(
            remixCurrentStageParent,
            remixNextStageParent,
            null
        );
    }


    // =========================================================
    // TRANSICIÓN GENERAL
    // =========================================================

    private void TriggerTransition(
        GameObject currentParent,
        GameObject nextParent,
        Action onComplete)
    {
        if (isTransitioning)
        {
            Debug.LogWarning(
                "[TransitionManager] Ya hay una transición en curso."
            );

            return;
        }

        if (currentParent == null)
        {
            Debug.LogError(
                "[TransitionManager] El Current Parent es NULL."
            );

            return;
        }

        if (nextParent == null)
        {
            Debug.LogError(
                "[TransitionManager] El Next Parent es NULL."
            );

            return;
        }

        StartCoroutine(
            TransitionCoroutine(
                currentParent,
                nextParent,
                onComplete
            )
        );
    }


    // =========================================================
    // CORUTINA
    // =========================================================

    private IEnumerator TransitionCoroutine(
        GameObject currentParent,
        GameObject nextParent,
        Action onComplete)
    {
        isTransitioning = true;

        Debug.Log(
            "[TransitionManager] Transición: " +
            currentParent.name +
            " " +
            nextParent.name
        );


        // -----------------------------------------------------
        // VALORES INICIALES
        // -----------------------------------------------------

        float startIntensity =
            lensDistortion != null
                ? lensDistortion.intensity.value
                : 0f;

        float startScale =
            lensDistortion != null
                ? lensDistortion.scale.value
                : 1f;


        // -----------------------------------------------------
        // CERRAR LA VISIÓN
        // -----------------------------------------------------

        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;

            float t =
                Mathf.Clamp01(
                    elapsed / transitionDuration
                );

            float curvedT =
                transitionCurve.Evaluate(t);


            if (lensDistortion != null)
            {
                lensDistortion.intensity.value =
                    Mathf.LerpUnclamped(
                        startIntensity,
                        1f,
                        curvedT
                    );

                lensDistortion.scale.value =
                    Mathf.LerpUnclamped(
                        startScale,
                        0.01f,
                        curvedT
                    );
            }

            yield return null;
        }


        // Asegurar valores finales

        if (lensDistortion != null)
        {
            lensDistortion.intensity.value = 1f;
            lensDistortion.scale.value = 0.01f;
        }


        // -----------------------------------------------------
        // CAMBIO DE MINIJUEGO
        // -----------------------------------------------------

        currentParent.SetActive(false);

        nextParent.SetActive(true);


        // -----------------------------------------------------
        // ABRIR LA VISIÓN
        // -----------------------------------------------------

        yield return StartCoroutine(
            ReturnToOriginalLensDistortion()
        );


        isTransitioning = false;


        Debug.Log(
            "[TransitionManager] Transición completada."
        );


        if (onComplete != null)
        {
            onComplete.Invoke();
        }
    }


    // =========================================================
    // RETORNO DEL LENS DISTORTION
    // =========================================================

    private IEnumerator ReturnToOriginalLensDistortion()
    {
        if (lensDistortion == null)
            yield break;


        float startIntensity =
            lensDistortion.intensity.value;

        float startScale =
            lensDistortion.scale.value;


        float elapsed = 0f;


        while (elapsed < returnTransitionDuration)
        {
            elapsed += Time.deltaTime;

            float t =
                Mathf.Clamp01(
                    elapsed / returnTransitionDuration
                );

            float curvedT =
                returnTransitionCurve.Evaluate(t);


            lensDistortion.intensity.value =
                Mathf.LerpUnclamped(
                    startIntensity,
                    originalIntensity,
                    curvedT
                );

            lensDistortion.scale.value =
                Mathf.LerpUnclamped(
                    startScale,
                    originalScale,
                    curvedT
                );


            yield return null;
        }


        lensDistortion.intensity.value =
            originalIntensity;

        lensDistortion.scale.value =
            originalScale;
    }


    // =========================================================
    // UTILIDADES
    // =========================================================

    public void ResetLensDistortion()
    {
        if (lensDistortion != null)
        {
            lensDistortion.intensity.value =
                originalIntensity;

            lensDistortion.scale.value =
                originalScale;
        }
    }


    public bool IsTransitioning()
    {
        return isTransitioning;
    }
}