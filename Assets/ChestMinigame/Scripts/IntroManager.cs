using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class IntroManager : MonoBehaviour
{
    public static IntroManager Instance;

    [Header("Pantallas")]
    public GameObject schoolSelectionScreen;   // Pantalla de selección de escuela (PRIMERO)
    public GameObject instructionsScreen;      // Pantalla de instrucciones (SEGUNDO)
    public GameObject gameplayPanel;           // Panel de juego (TERCERO)

    [Header("Imágenes de Fondo")]
    public Image schoolSelectionBackground;
    public Sprite schoolSelectionBackgroundSprite;
    public Image instructionsBackground;
    public Sprite instructionsBackgroundSprite;

    [Header("Animación de Transición (Opcional)")]
    public float fadeDuration = 0.5f;
    public CanvasGroup schoolSelectionCanvasGroup;
    public CanvasGroup instructionsCanvasGroup;

    [Header("Audio (Opcional)")]
    public AudioSource audioSource;
    public AudioClip buttonClickSound;

    private SchoolData selectedSchool;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        Debug.Log("=== IntroManager.Start() ===");

        // Configurar estado inicial: Mostrar selección de escuela
        ShowSchoolSelection();

        // Configurar imágenes de fondo si están asignadas
        if (schoolSelectionBackground != null && schoolSelectionBackgroundSprite != null)
        {
            schoolSelectionBackground.sprite = schoolSelectionBackgroundSprite;
            Debug.Log("Fondo de School Selection asignado");
        }

        if (instructionsBackground != null && instructionsBackgroundSprite != null)
        {
            instructionsBackground.sprite = instructionsBackgroundSprite;
            Debug.Log("Fondo de Instructions asignado");
        }
    }

    void ShowSchoolSelection()
    {
        Debug.Log("=== ShowSchoolSelection ===");

        // Activar school selection (debe estar activo en jerarquía)
        if (schoolSelectionScreen != null)
        {
            schoolSelectionScreen.SetActive(true);
            Debug.Log($"SchoolSelectionScreen activado: {schoolSelectionScreen.activeSelf}");
        }
        else
        {
            Debug.LogError("SchoolSelectionScreen no está asignado en IntroManager!");
        }

        if (instructionsScreen != null)
        {
            instructionsScreen.SetActive(false);
            Debug.Log("InstructionsScreen desactivado");
        }

        if (gameplayPanel != null)
        {
            gameplayPanel.SetActive(false);
            Debug.Log("GameplayPanel desactivado");
        }

        // Resetear alpha si usa fade
        if (schoolSelectionCanvasGroup != null)
            schoolSelectionCanvasGroup.alpha = 1;

        // Notificar a SchoolSelectionMenu para que genere los botones
        if (SchoolSelectionMenu.Instance != null)
        {
            SchoolSelectionMenu.Instance.OnScreenActivated();
            Debug.Log("SchoolSelectionMenu.OnScreenActivated() llamado");
        }
        else
        {
            Debug.LogWarning("SchoolSelectionMenu.Instance es null!");
        }
    }

    /// <summary>
    /// Llamado desde SchoolSelectionMenu cuando se presiona "Iniciar Juego"
    /// </summary>
    public void OnSchoolSelected(SchoolData school)
    {
        selectedSchool = school;
        PlayButtonSound();
        StartCoroutine(TransitionToInstructions());
    }

    IEnumerator TransitionToInstructions()
    {
        Debug.Log("Transición a instrucciones...");

        // Fade out de school selection
        if (schoolSelectionCanvasGroup != null)
        {
            yield return StartCoroutine(FadeOut(schoolSelectionCanvasGroup));
        }
        else
        {
            yield return new WaitForSeconds(0.2f);
        }

        // Cambiar pantallas
        if (schoolSelectionScreen != null)
            schoolSelectionScreen.SetActive(false);

        if (instructionsScreen != null)
            instructionsScreen.SetActive(true);

        // Fade in de instructions
        if (instructionsCanvasGroup != null)
        {
            instructionsCanvasGroup.alpha = 0;
            yield return StartCoroutine(FadeIn(instructionsCanvasGroup));
        }
    }

    /// <summary>
    /// Llamado cuando se presiona "Continuar" en las instrucciones
    /// </summary>
    public void OnContinueFromInstructions()
    {
        Debug.Log("=== OnContinueFromInstructions llamado ===");
        PlayButtonSound();
        StartCoroutine(TransitionToGameplay());
    }

    IEnumerator TransitionToGameplay()
    {
        Debug.Log("Iniciando juego...");

        // Fade out de instructions
        if (instructionsCanvasGroup != null)
        {
            yield return StartCoroutine(FadeOut(instructionsCanvasGroup));
        }
        else
        {
            yield return new WaitForSeconds(0.2f);
        }

        // Cambiar pantallas
        if (instructionsScreen != null)
            instructionsScreen.SetActive(false);

        if (gameplayPanel != null)
            gameplayPanel.SetActive(true);

        // Iniciar el juego con la escuela seleccionada
        if (CardSpawner.Instance != null && selectedSchool != null && SchoolCardManager.Instance != null)
        {
            CardSpawner.Instance.SetupSchools(selectedSchool, SchoolCardManager.Instance);
        }

       
    }

    IEnumerator FadeOut(CanvasGroup canvasGroup)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0;
    }

    IEnumerator FadeIn(CanvasGroup canvasGroup)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1;
    }

    void PlayButtonSound()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }

    public SchoolData GetSelectedSchool() => selectedSchool;

    // Método para saltar directamente al juego (útil para testing)
    [ContextMenu("Skip to Gameplay")]
    public void SkipToGameplay()
    {
        if (schoolSelectionScreen != null)
            schoolSelectionScreen.SetActive(false);

        if (instructionsScreen != null)
            instructionsScreen.SetActive(false);

        if (gameplayPanel != null)
            gameplayPanel.SetActive(true);
    }
}