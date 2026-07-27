using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(-100)]
public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    [Header("Botón de pausa (en pantalla)")]
    [SerializeField] private Button pauseButton;

    [Header("Panel de pausa")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;

    [Header("Configuración")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape; // opcional, además del botón UI

    [Header("Escenas donde NO se puede pausar")]
    [Tooltip("Escribe aquí el nombre exacto de cada escena (el mismo que aparece en Build Settings) en la que el botón/tecla de pausa debe quedar deshabilitado. Ej: MainMenu, Loading, Cinematic.")]
    [SerializeField] private List<string> scenesWherePauseIsDisabled = new List<string>();

    public bool IsPaused { get; private set; }
    public bool CanPauseInCurrentScene { get; private set; } = true;

    private void Awake()
    {
        // Evita duplicados si esta escena ya tiene un PauseManager
        // (por ejemplo si volviste a cargar la escena donde vive el prefab).
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (pauseButton != null)
            pauseButton.onClick.AddListener(TogglePause);

        if (resumeButton != null)
            resumeButton.onClick.AddListener(Resume);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(GoToMainMenu);

        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        // Evalúa la escena activa al iniciar (por si el prefab
        // arranca en una escena ya marcada como "sin pausa").
        UpdatePauseAvailability(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdatePauseAvailability(scene.name);
    }

    private void UpdatePauseAvailability(string sceneName)
    {
        CanPauseInCurrentScene = !scenesWherePauseIsDisabled.Contains(sceneName);

        // Si la escena nueva no permite pausa y el juego quedó pausado
        // de la escena anterior, forzamos la reanudación.
        if (!CanPauseInCurrentScene && IsPaused)
        {
            Resume();
        }

        // Oculta el botón de pausa en pantalla en las escenas prohibidas.
        if (pauseButton != null)
            pauseButton.gameObject.SetActive(CanPauseInCurrentScene);
    }

    private void Update()
    {
        if (CanPauseInCurrentScene && Input.GetKeyDown(pauseKey))
        {
            TogglePause();
        }
    }

    /// <summary>
    /// Conecta este método al OnClick() del botón de pausa en pantalla
    /// (ya viene conectado automáticamente si asignaste "pauseButton" en el Inspector).
    /// </summary>
    public void TogglePause()
    {
        if (!CanPauseInCurrentScene) return;

        if (IsPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        if (!CanPauseInCurrentScene) return;

        IsPaused = true;
        Time.timeScale = 0f;

        if (pausePanel != null)
            pausePanel.SetActive(true);
    }

    public void Resume()
    {
        IsPaused = false;
        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    public void GoToMainMenu()
    {
        // Reutiliza Resume() para restaurar timeScale, IsPaused
        // y cerrar el panel de pausa, así no queda abierto
        // al volver a esta escena más adelante.
        Resume();

        SceneManager.LoadScene(mainMenuSceneName);
    }
}