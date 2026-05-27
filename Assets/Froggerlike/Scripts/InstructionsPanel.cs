using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class InstructionsPanelmini3 : MonoBehaviour
{
    [Header("Referencias UI")]
    public Image backgroundImage;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI instructionsText;
    public Button startButton;
    public TextMeshProUGUI buttonText;

    [Header("Animación")]
    public float fadeInDuration = 0.8f;
    public float fadeOutDuration = 0.6f;

    [Header("Escala del botón")]
    public float buttonPopScale = 1.05f;
    public float buttonPopSpeed = 2f;

    public AudioSource audioSource;
    public AudioClip Continuesound;

    private bool isShowing = true;
    private Color originalBgColor;
    private Color originalTitleColor;
    private Color originalInstructionsColor;
    private Color originalButtonColor;
    private Color originalButtonTextColor;

    private Vector3 buttonOriginalScale;
    private float buttonScaleTime = 0f;

    void Start()
    {
        // Guardar colores y escalas originales
        if (backgroundImage != null)
            originalBgColor = backgroundImage.color;
        if (titleText != null)
            originalTitleColor = titleText.color;
        if (instructionsText != null)
            originalInstructionsColor = instructionsText.color;
        if (startButton != null)
        {
            originalButtonColor = startButton.GetComponent<Image>().color;
            buttonOriginalScale = startButton.transform.localScale;
        }
        if (buttonText != null)
            originalButtonTextColor = buttonText.color;

        // Configurar el botón
        if (startButton != null)
            startButton.onClick.AddListener(OnStartButtonClicked);

        // Empezar invisible y hacer fade in
        SetAlpha(0f);
        StartCoroutine(FadeIn());
    }

    void Update()
    {

    }

    void OnStartButtonClicked()
    {
        if (!isShowing) return;

        isShowing = false;
        StartCoroutine(FadeOutAndStart());
        audioSource.PlayOneShot(Continuesound);
        
    }

    IEnumerator FadeIn()
    {
        float elapsed = 0f;

        while (elapsed < fadeInDuration)
        {
            float t = elapsed / fadeInDuration;
            SetAlpha(Mathf.Lerp(0f, 1f, t));
            elapsed += Time.deltaTime;
            yield return null;
        }

        SetAlpha(1f);
    }

    IEnumerator FadeOutAndStart()
    {
        
        if (startButton != null)
            startButton.interactable = false;

        float elapsed = 0f;

        while (elapsed < fadeOutDuration)
        {
            float t = elapsed / fadeOutDuration;
            SetAlpha(Mathf.Lerp(1f, 0f, t));
            elapsed += Time.deltaTime;
            yield return null;
        }

        SetAlpha(0f);

        // Ocultar el panel completamente
        gameObject.SetActive(false);

        // Iniciar el juego
        if (GlassBridgeManager.Instance != null)
        {
            GlassBridgeManager.Instance.StartGame();
        }
    }

    void SetAlpha(float alpha)
    {
        if (backgroundImage != null)
        {
            Color c = originalBgColor;
            c.a = alpha;
            backgroundImage.color = c;
        }

        if (titleText != null)
        {
            Color c = originalTitleColor;
            c.a = alpha;
            titleText.color = c;
        }

        if (instructionsText != null)
        {
            Color c = originalInstructionsColor;
            c.a = alpha;
            instructionsText.color = c;
        }

        if (startButton != null)
        {
            Image buttonImage = startButton.GetComponent<Image>();
            if (buttonImage != null)
            {
                Color c = originalButtonColor;
                c.a = alpha;
                buttonImage.color = c;
            }
        }

        if (buttonText != null)
        {
            Color c = originalButtonTextColor;
            c.a = alpha;
            buttonText.color = c;
        }
    }
}