using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MinigameIntroManager : MonoBehaviour
{
    [Header("Fade In inicial")]
    [Tooltip("Imagen negra que cubre toda la pantalla")]
    public Image blackPanel;
    public float startDelay = 0.3f;
    public float fadeInDuration = 1.5f;

    [Header("Panel de instrucciones")]
    public GameObject instructionsPanel;
    [Tooltip("Opcional: si lo asignas, el panel aparece con fade. Si no, se agrega uno automaticamente.")]
    public CanvasGroup instructionsGroup;
    public float instructionsFadeDuration = 0.4f;
    public Button continueButton;

    [Header("Inicio del Minijuego 1")]
    [Tooltip("Padre del Minijuego 1. Se desactiva al iniciar y se activa al pulsar Continuar. " +
             "Dejar vacio si lo arrancas con el evento de abajo.")]
    public GameObject minigame1Parent;

    [Tooltip("Se ejecuta al pulsar Continuar (ej. arrancar timers, spawners, etc.)")]
    public UnityEvent onIntroFinished;

    void Awake()
    {
        // El panel negro empieza opaco y bloquea clics
        if (blackPanel != null)
        {
            blackPanel.gameObject.SetActive(true);
            Color c = blackPanel.color;
            c.a = 1f;
            blackPanel.color = c;
            blackPanel.raycastTarget = true;
        }

        // Instrucciones ocultas
        if (instructionsPanel != null)
        {
            if (instructionsGroup == null)
            {
                instructionsGroup = instructionsPanel.GetComponent<CanvasGroup>();
                if (instructionsGroup == null)
                    instructionsGroup = instructionsPanel.AddComponent<CanvasGroup>();
            }

            instructionsGroup.alpha = 0f;
            instructionsPanel.SetActive(false);
        }

        // El minijuego 1 no empieza hasta pulsar Continuar
        if (minigame1Parent != null)
            minigame1Parent.SetActive(false);

        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinuePressed);
    }

    void Start()
    {
        StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {
        yield return new WaitForSecondsRealtime(startDelay);

        // 1. Fade in: negro -> escena
        if (blackPanel != null)
        {
            float elapsed = 0f;
            Color c = blackPanel.color;

            while (elapsed < fadeInDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                c.a = Mathf.Lerp(1f, 0f, elapsed / fadeInDuration);
                blackPanel.color = c;
                yield return null;
            }

            c.a = 0f;
            blackPanel.color = c;
            blackPanel.raycastTarget = false;
            blackPanel.gameObject.SetActive(false);
        }

        // 2. Mostrar instrucciones
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(true);

            float elapsed = 0f;
            while (elapsed < instructionsFadeDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                instructionsGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / instructionsFadeDuration);
                yield return null;
            }

            instructionsGroup.alpha = 1f;
        }
    }

    public void OnContinuePressed()
    {
        if (continueButton != null)
            continueButton.interactable = false; // Evita doble clic

        StartCoroutine(FinishIntro());
    }

    IEnumerator FinishIntro()
    {
        // Ocultar instrucciones con fade
        if (instructionsPanel != null)
        {
            float elapsed = 0f;
            while (elapsed < instructionsFadeDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                instructionsGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / instructionsFadeDuration);
                yield return null;
            }

            instructionsPanel.SetActive(false);
        }

        // Iniciar minijuego 1
        if (minigame1Parent != null)
            minigame1Parent.SetActive(true);

        onIntroFinished?.Invoke();
    }

    void OnDestroy()
    {
        if (continueButton != null)
            continueButton.onClick.RemoveListener(OnContinuePressed);
    }
}