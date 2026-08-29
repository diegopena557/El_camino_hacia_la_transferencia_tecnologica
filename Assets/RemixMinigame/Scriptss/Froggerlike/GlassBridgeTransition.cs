using System;
using UnityEngine;

public class GlassBridgeStageTransition : MonoBehaviour
{
    [Header("Condición de transición")]
    [Tooltip("Cantidad de plataformas que el jugador debe completar para pasar al Minijuego 4")]
    public int platformsToComplete = 3;

    [Header("Referencias de escena")]
    [Tooltip("Padre del Minijuego 3 (Puente de Cristal - Remix)")]
    public GameObject stage3Parent;

    [Tooltip("Padre del Minijuego 4")]
    public GameObject stage4Parent;

    [Header("Opciones")]
    [Tooltip("Si está activo, solo cuenta las plataformas acertadas (usa correctCount en vez del avance de nivel)")]
    public bool onlyCountCorrectPlatforms = false;

    private bool transitionTriggered = false;

    private void Start()
    {
        if (GlassBridgeManager.Instance != null)
        {
            GlassBridgeManager.Instance.OnLevelCompleted += HandleLevelCompleted;
        }
        else
        {
            Debug.LogError(
                "[GlassBridgeStageTransition] No se encontró GlassBridgeManager.Instance en la escena."
            );
        }
    }

    private void OnDestroy()
    {
        if (GlassBridgeManager.Instance != null)
        {
            GlassBridgeManager.Instance.OnLevelCompleted -= HandleLevelCompleted;
        }
    }

    private void HandleLevelCompleted(int levelsCompleted)
    {
        if (transitionTriggered) return;

        int progress = onlyCountCorrectPlatforms
            ? GlassBridgeManager.Instance.correctCount
            : levelsCompleted;

        if (progress >= platformsToComplete)
        {
            transitionTriggered = true;
            TriggerTransitionToStage4();
        }
    }

    private void TriggerTransitionToStage4()
    {
        if (TransitionManager.Instance == null)
        {
            Debug.LogError(
                "[GlassBridgeStageTransition] No se encontró TransitionManager.Instance en la escena."
            );
            return;
        }

        if (stage3Parent == null || stage4Parent == null)
        {
            Debug.LogError(
                "[GlassBridgeStageTransition] Falta asignar stage3Parent o stage4Parent en el Inspector."
            );
            return;
        }

        Debug.Log(
            $"[GlassBridgeStageTransition] {platformsToComplete} plataformas completadas. " +
            "Disparando transición al Minijuego 4."
        );

        TransitionManager.Instance.TriggerCustomTransition(stage3Parent, stage4Parent);
    }

    /// <summary>
    /// Permite forzar la transición manualmente (ej: desde un botón de debug).
    /// </summary>
    public void ForceTransition()
    {
        if (transitionTriggered) return;
        transitionTriggered = true;
        TriggerTransitionToStage4();
    }
}