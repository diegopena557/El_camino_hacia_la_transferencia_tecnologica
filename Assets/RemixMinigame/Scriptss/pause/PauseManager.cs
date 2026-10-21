using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

[DefaultExecutionOrder(-100)]
public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    private const string VolumePrefKey = "MasterVolume";

    [Header("Boton de pausa (en pantalla)")]
    [SerializeField] private Button pauseButton;

    [Header("Panel de pausa")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    [Header("Volumen")]
    [Tooltip("Slider con Min Value = 0 y Max Value = 1. Controla el volumen de todo el audio del juego.")]
    [SerializeField] private Slider volumeSlider;
    [Range(0f, 1f)]
    [SerializeField] private float defaultVolume = 1f;

    [Header("Configuracion")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape; // opcional, ademas del boton UI

    [Header("Escenas donde NO se puede pausar")]
    [Tooltip("Escribe aqui el nombre exacto de cada escena (el mismo que aparece en Build Settings) en la que el boton/tecla de pausa debe quedar deshabilitado. Ej: MainMenu, Loading, Cinematic.")]
    [SerializeField] private List<string> scenesWherePauseIsDisabled = new List<string>();

    // Videos que estaban reproduciendose al pausar (para reanudar solo esos).
    private readonly List<VideoPlayer> pausedVideos = new List<VideoPlayer>();

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

        if (restartButton != null)
            restartButton.onClick.AddListener(RestartScene);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(GoToMainMenu);

        SetupVolume();

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
        // Evalua la escena activa al iniciar (por si el prefab
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

        // Si la escena nueva no permite pausa y el juego quedo pausado
        // de la escena anterior, forzamos la reanudacion.
        if (!CanPauseInCurrentScene && IsPaused)
        {
            Resume();
        }

        // Oculta el boton de pausa en pantalla en las escenas prohibidas.
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

    // ------------- Volumen -------------

    private void SetupVolume()
    {
        float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, defaultVolume);
        ApplyVolume(savedVolume);

        if (volumeSlider != null)
        {
            // Asigna el valor sin disparar el evento y luego se suscribe.
            volumeSlider.SetValueWithoutNotify(savedVolume);
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    /// <summary>
    /// Cambia el volumen global (0 a 1) y lo guarda para la proxima vez que se abra el juego.
    /// </summary>
    public void SetVolume(float value)
    {
        value = Mathf.Clamp01(value);
        ApplyVolume(value);
        PlayerPrefs.SetFloat(VolumePrefKey, value);
    }

    private void ApplyVolume(float value)
    {
        // AudioListener.volume afecta a TODOS los AudioSource del juego.
        AudioListener.volume = value;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }

    // ------------- Pausa -------------

    /// <summary>
    /// Conecta este metodo al OnClick() del boton de pausa en pantalla
    /// (ya viene conectado automaticamente si asignaste "pauseButton" en el Inspector).
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

        // Pausa todos los AudioSource (excepto los que tengan "Ignore Listener Pause").
        AudioListener.pause = true;

        // Pausa los videos que se esten reproduciendo y los recuerda.
        pausedVideos.Clear();
        foreach (VideoPlayer vp in FindObjectsByType<VideoPlayer>(FindObjectsSortMode.None))
        {
            if (vp.isPlaying)
            {
                vp.Pause();
                pausedVideos.Add(vp);
            }
        }

        if (pausePanel != null)
            pausePanel.SetActive(true);
    }

    public void Resume()
    {
        IsPaused = false;
        Time.timeScale = 1f;

        AudioListener.pause = false;

        foreach (VideoPlayer vp in pausedVideos)
        {
            if (vp != null) vp.Play();
        }
        pausedVideos.Clear();

        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    public void RestartScene()
    {
        // Restaura timeScale, IsPaused y cierra el panel antes de recargar,
        // si no la escena reiniciada arrancaria congelada.
        Resume();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        // Reutiliza Resume() para restaurar timeScale, IsPaused
        // y cerrar el panel de pausa, asi no queda abierto
        // al volver a esta escena mas adelante.
        Resume();

        SceneManager.LoadScene(mainMenuSceneName);
    }
}