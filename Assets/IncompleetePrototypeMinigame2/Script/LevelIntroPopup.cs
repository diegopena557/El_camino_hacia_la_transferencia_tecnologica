using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

public class LevelIntroPopup : MonoBehaviour
{
    public static LevelIntroPopup Instance;

    [Header("Referencias UI")]
    public GameObject popupPanel;
    public TextMeshProUGUI levelNumberText;
    public TextMeshProUGUI levelNameText;
    public TextMeshProUGUI descriptionText;
    public Button continueButton;
    public TextMeshProUGUI continueButtonText;

    [Header("Textos")]
    public string levelNumberPrefix = "Nivel ";
    public string continueButtonLabel = "Comenzar";

    [Header("Animacion")]
    public float fadeInDuration = 0.35f;
    public float fadeOutDuration = 0.25f;
    public float popupScaleFrom = 0.85f;

    [Header("Pausa")]
    [Tooltip("Pausa el juego mientras el popup esta visible (usa unscaledDeltaTime internamente)")]
    public bool pauseWhileVisible = true;

    private Action onContinueCallback;
    private CanvasGroup canvasGroup;
    private bool isInitialized = false;
    private Vector3 originalPanelScale;

    void EnsureInitialized()
    {
        if (isInitialized) return;
        isInitialized = true;

        if (popupPanel == null)
        {
            Debug.LogError("[LevelIntroPopup] popupPanel no asignado en el Inspector!");
            return;
        }

        // Guardar escala original antes de cualquier modificacion
        originalPanelScale = popupPanel.transform.localScale;

        // Obtener o crear CanvasGroup directamente en el popupPanel
        canvasGroup = popupPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = popupPanel.AddComponent<CanvasGroup>();

        // Estado inicial: completamente oculto
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        popupPanel.SetActive(false);

        // Conectar boton
        if (continueButton != null)
        {
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(OnContinueClicked);
        }

        if (continueButtonText != null)
            continueButtonText.text = continueButtonLabel;

        Debug.Log("[LevelIntroPopup] Inicializado.");
    }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        EnsureInitialized();
    }

    public void Show(LevelData levelData, Action onContinue)
    {
        EnsureInitialized();

        if (popupPanel == null)
        {
            Debug.LogError("[LevelIntroPopup] popupPanel es null, ejecutando callback directamente.");
            onContinue?.Invoke();
            return;
        }

        onContinueCallback = onContinue;

        // Rellenar textos
        if (levelNumberText != null)
            levelNumberText.text = levelNumberPrefix + levelData.levelNumber;

        if (levelNameText != null)
            levelNameText.text = levelData.levelName;

        if (descriptionText != null)
            descriptionText.text = levelData.levelDescription;

        if (continueButtonText != null)
            continueButtonText.text = continueButtonLabel;

        if (continueButton != null)
            continueButton.interactable = false;

        // Pausar ANTES de mostrar para que el fade use unscaledDeltaTime
        if (pauseWhileVisible)
            Time.timeScale = 0f;

        StopAllCoroutines();
        StartCoroutine(FadeIn());

        Debug.Log($"[LevelIntroPopup] Mostrando popup: {levelData.levelName}");
    }

    void OnContinueClicked()
    {
        Debug.Log("[LevelIntroPopup] Continuar presionado.");

        if (continueButton != null)
            continueButton.interactable = false;

        StopAllCoroutines();
        StartCoroutine(FadeOutAndCallback());
    }

    IEnumerator FadeIn()
    {
        popupPanel.SetActive(true);
        popupPanel.transform.localScale = originalPanelScale * popupScaleFrom;

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        // Un frame para que Unity procese el SetActive antes de animar
        yield return null;

        float elapsed = 0f;
        while (elapsed < fadeInDuration)
        {
            // unscaledDeltaTime funciona aunque timeScale sea 0
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / fadeInDuration));

            canvasGroup.alpha = t;
            popupPanel.transform.localScale = Vector3.Lerp(
                originalPanelScale * popupScaleFrom,
                originalPanelScale,
                t
            );

            yield return null;
        }

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        popupPanel.transform.localScale = originalPanelScale;

        if (continueButton != null)
            continueButton.interactable = true;

        Debug.Log("[LevelIntroPopup] Fade in completado.");
    }

    IEnumerator FadeOutAndCallback()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        float elapsed = 0f;
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / fadeOutDuration);

            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
            popupPanel.transform.localScale = Vector3.Lerp(
                originalPanelScale,
                originalPanelScale * popupScaleFrom,
                t
            );

            yield return null;
        }

        canvasGroup.alpha = 0f;
        popupPanel.SetActive(false);
        popupPanel.transform.localScale = originalPanelScale;

        // Reanudar juego ANTES de ejecutar el callback para que el spawn funcione
        if (pauseWhileVisible)
            Time.timeScale = 1f;

        Debug.Log("[LevelIntroPopup] Fade out completado, iniciando nivel.");

        onContinueCallback?.Invoke();
        onContinueCallback = null;
    }
}