using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

// Maneja tanto el panel de instrucciones inicial como los paneles de introduccion
// de cada nivel (Entender, Imaginar, Probar). Todos usan la misma logica de fade.
public class InstructionsPanel : MonoBehaviour
{
    public static InstructionsPanel Instance;

    [Header("Panel de Instrucciones Inicial")]
    public CanvasGroup instructionsCanvasGroup;
    public GameObject instructionsPanel;
    public Button instructionsContinueButton;

    [Header("Panel de Intro de Nivel")]
    public GameObject levelIntroPanel;
    public CanvasGroup levelIntroCanvasGroup;
    public Button levelIntroContinueButton;

    [Header("Textos del Panel de Nivel")]
    public TextMeshProUGUI levelNumberText;
    public TextMeshProUGUI levelNameText;
    public TextMeshProUGUI levelDescriptionText;
    public TextMeshProUGUI categoryText;
    public TextMeshProUGUI levelIntroContinueButtonText;

    [Header("Colores por Categoria")]
    public Color entenderColor = new Color(0.2f, 0.6f, 1f);
    public Color imaginarColor = new Color(1f, 0.6f, 0.2f);
    public Color probarColor = new Color(0.4f, 1f, 0.4f);

    [Header("Configuracion de Fade")]
    public float fadeDuration = 0.5f;

    [Header("Audio (Opcional)")]
    public AudioSource audioSource;
    public AudioClip buttonClickSound;

    private bool isFading = false;
    private Action onLevelIntroComplete;
    private Action onInstructionsClosedCallback;
    private bool isShowingInstructions = true; // true hasta que el jugador cierre las instrucciones iniciales


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // -- Panel de instrucciones inicial --
        if (instructionsCanvasGroup == null && instructionsPanel != null)
        {
            instructionsCanvasGroup = instructionsPanel.GetComponent<CanvasGroup>();
            if (instructionsCanvasGroup == null)
                instructionsCanvasGroup = instructionsPanel.AddComponent<CanvasGroup>();
        }

        // Empieza oculto; FadeInPanel lo activara
        if (instructionsCanvasGroup != null)
        {
            instructionsCanvasGroup.alpha = 0f;
            instructionsCanvasGroup.interactable = false;
            instructionsCanvasGroup.blocksRaycasts = false;
        }

        if (instructionsContinueButton != null)
        {
            instructionsContinueButton.interactable = false;
            instructionsContinueButton.onClick.AddListener(OnInstructionsContinueClicked);
        }

        // -- Panel de intro de nivel --
        if (levelIntroPanel != null)
            levelIntroPanel.SetActive(false);

        if (levelIntroCanvasGroup == null && levelIntroPanel != null)
        {
            levelIntroCanvasGroup = levelIntroPanel.GetComponent<CanvasGroup>();
            if (levelIntroCanvasGroup == null)
                levelIntroCanvasGroup = levelIntroPanel.AddComponent<CanvasGroup>();
        }

        if (levelIntroCanvasGroup != null)
        {
            levelIntroCanvasGroup.alpha = 0f;
            levelIntroCanvasGroup.interactable = false;
            levelIntroCanvasGroup.blocksRaycasts = false;
        }

        if (levelIntroContinueButton != null)
            levelIntroContinueButton.onClick.AddListener(OnLevelIntroContinueClicked);

        // Pausar juego mientras se muestran instrucciones
        Time.timeScale = 0f;
        // Mostrar instrucciones directamente, sin fade in
        if (instructionsPanel != null)
            instructionsPanel.SetActive(true);

        if (instructionsCanvasGroup != null)
        {
            instructionsCanvasGroup.alpha = 1f;
            instructionsCanvasGroup.interactable = true;
            instructionsCanvasGroup.blocksRaycasts = true;
        }

        if (instructionsContinueButton != null)
            instructionsContinueButton.interactable = true;
    }



    void OnInstructionsContinueClicked()
    {
        if (isFading) return;
        PlayClickSound();
        StartCoroutine(FadeOutPanel(instructionsCanvasGroup, instructionsPanel, OnInstructionsClosed));
    }

    void OnInstructionsClosed()
    {
        Debug.Log("[InstructionsPanel] Instrucciones cerradas.");
        isShowingInstructions = false;
        Time.timeScale = 1f;

        // Avisar al LevelManager que ya puede cargar el nivel
        onInstructionsClosedCallback?.Invoke();
        onInstructionsClosedCallback = null;
    }



    public void ShowLevelIntro(LevelData levelData, Action onComplete)
    {
        if (levelIntroPanel == null)
        {
            Debug.LogWarning("[InstructionsPanel] levelIntroPanel no asignado. Ejecutando callback.");
            onComplete?.Invoke();
            return;
        }

        onLevelIntroComplete = onComplete;

        // Rellenar textos
        if (levelNumberText != null)
            levelNumberText.text = "Nivel " + levelData.levelNumber;

        if (levelNameText != null)
            levelNameText.text = levelData.levelName;

        if (levelDescriptionText != null)
            levelDescriptionText.text = levelData.levelDescription;

        if (categoryText != null)
        {
            categoryText.text = GetCategoryName(levelData.focusCategory);
            categoryText.color = GetCategoryColor(levelData.focusCategory);
        }

        if (levelIntroContinueButton != null)
            levelIntroContinueButton.interactable = false;

        // Pausar juego durante el popup de nivel
        Time.timeScale = 0f;

        StartCoroutine(FadeInPanel(levelIntroCanvasGroup, levelIntroPanel, () =>
        {
            if (levelIntroContinueButton != null)
                levelIntroContinueButton.interactable = true;
        }));

        Debug.Log($"[InstructionsPanel] Mostrando intro nivel: {levelData.levelName}");
    }

    void OnLevelIntroContinueClicked()
    {
        if (isFading) return;
        PlayClickSound();

        if (levelIntroContinueButton != null)
            levelIntroContinueButton.interactable = false;

        StartCoroutine(FadeOutPanel(levelIntroCanvasGroup, levelIntroPanel, () =>
        {
            Time.timeScale = 1f;
            Debug.Log("[InstructionsPanel] Intro de nivel cerrada. Iniciando nivel.");
            onLevelIntroComplete?.Invoke();
            onLevelIntroComplete = null;
        }));
    }




    IEnumerator FadeInPanel(CanvasGroup cg, GameObject panel, Action onComplete = null)
    {
        isFading = true;

        panel.SetActive(true);
        cg.alpha = 0f;
        cg.interactable = false;
        cg.blocksRaycasts = false;

        yield return null; // frame para que Unity procese el SetActive

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            cg.alpha = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / fadeDuration));
            yield return null;
        }

        cg.alpha = 1f;
        cg.interactable = true;
        cg.blocksRaycasts = true;

        isFading = false;
        onComplete?.Invoke();
    }

    IEnumerator FadeOutPanel(CanvasGroup cg, GameObject panel, Action onComplete = null)
    {
        isFading = true;

        cg.interactable = false;
        cg.blocksRaycasts = false;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            cg.alpha = Mathf.Lerp(1f, 0f, Mathf.Clamp01(elapsed / fadeDuration));
            yield return null;
        }

        cg.alpha = 0f;
        panel.SetActive(false);

        isFading = false;
        onComplete?.Invoke();
    }


    // LevelManager llama esto para saber si debe esperar antes de cargar el nivel
    public bool IsShowingInstructions() => isShowingInstructions;

    // LevelManager registra aqui el callback que se ejecuta al cerrar instrucciones
    public void SetOnInstructionsClosed(Action callback)
    {
        onInstructionsClosedCallback = callback;
    }



    void PlayClickSound()
    {
        if (audioSource != null && buttonClickSound != null)
            audioSource.PlayOneShot(buttonClickSound);
    }

    string GetCategoryName(TokenCategory category)
    {
        return category switch
        {
            TokenCategory.Entender => "Entender",
            TokenCategory.Imaginar => "Imaginar",
            TokenCategory.Probar => "Probar",
            _ => ""
        };
    }

    Color GetCategoryColor(TokenCategory category)
    {
        return category switch
        {
            TokenCategory.Entender => entenderColor,
            TokenCategory.Imaginar => imaginarColor,
            TokenCategory.Probar => probarColor,
            _ => Color.white
        };
    }

    void OnDestroy()
    {
        if (Time.timeScale == 0f)
            Time.timeScale = 1f;
    }
}