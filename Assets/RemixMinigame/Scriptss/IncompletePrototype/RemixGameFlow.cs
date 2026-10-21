using UnityEngine;

public class RemixGameFlow : MonoBehaviour
{
    [Header("Minijuego actual")]
    [Tooltip("LevelManager del Minijuego 2")]
    public LevelManager levelManager;

    [Header("Transición")]
    [Tooltip("TransitionManager encargado de pasar al Minijuego 3")]
    public TransitionManager transitionManager;

    [Header("Configuración")]
    [Tooltip("Cantidad de niveles que deben completarse antes de pasar al siguiente minijuego.")]
    public int levelsRequired = 2;

    private int lastLevelIndex = -1;
    private int completedLevels = 0;

    private bool transitionStarted = false;


    // =========================================================
    // UNITY
    // =========================================================

    private void Start()
    {
        if (levelManager == null)
        {
            Debug.LogError(
                "[RemixGameFlow] No se asignó el LevelManager."
            );

            return;
        }

        if (transitionManager == null)
        {
            transitionManager =
                TransitionManager.Instance;
        }

        lastLevelIndex =
            levelManager.GetCurrentLevelIndex();


        Debug.Log(
            "[RemixGameFlow] Iniciado. " +
            "Nivel actual: " +
            lastLevelIndex
        );
    }


    private void Update()
    {
        if (transitionStarted)
            return;

        if (levelManager == null)
            return;


        int currentLevelIndex =
            levelManager.GetCurrentLevelIndex();


        // -----------------------------------------------------
        // DETECTAR COMPLETADO DE NIVEL
        // -----------------------------------------------------

        if (currentLevelIndex > lastLevelIndex)
        {
            int levelsCompleted =
                currentLevelIndex - lastLevelIndex;


            completedLevels +=
                levelsCompleted;


            Debug.Log(
                "[RemixGameFlow] Nivel completado. " +
                "Progreso: " +
                completedLevels +
                "/" +
                levelsRequired
            );


            if (completedLevels >= levelsRequired)
            {
                StartRemixTransition();
            }
        }


        lastLevelIndex =
            currentLevelIndex;
    }


    // =========================================================
    // TRANSICIÓN AL MINIJUEGO 3
    // =========================================================

    private void StartRemixTransition()
    {
        if (transitionStarted)
            return;


        if (transitionManager == null)
        {
            Debug.LogError(
                "[RemixGameFlow] No se encontró TransitionManager."
            );

            return;
        }


        transitionStarted = true;


        Debug.Log(
            "[RemixGameFlow] Se completaron los " +
            levelsRequired +
            " niveles. " +
            "Pasando al Minijuego 3."
        );


        transitionManager.TriggerRemixTransition();
    }


    // =========================================================
    // INFORMACIÓN
    // =========================================================

    public int GetCompletedLevels()
    {
        return completedLevels;
    }


    public bool HasFinished()
    {
        return completedLevels >= levelsRequired;
    }
}