using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameFadeIn : MonoBehaviour
{
    public static GameFadeIn Instance;

    [Header("Configuración de Fade")]
    public Image fadeImage;
    public float fadeInDuration = 1.5f;
    public Color fadeColor = Color.black;

    [Header("Delay Opcional")]
    public float delayBeforeFade = 0.2f;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Opcional: mantener entre escenas
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Configurar la imagen de fade
        if (fadeImage != null)
        {
            fadeImage.color = fadeColor;
            canvasGroup = fadeImage.GetComponent<CanvasGroup>();

            if (canvasGroup == null)
            {
                canvasGroup = fadeImage.gameObject.AddComponent<CanvasGroup>();
            }

            // Empezar completamente opaco
            canvasGroup.alpha = 1f;
            fadeImage.gameObject.SetActive(true);
        }
    }

    void Start()
    {
        // Iniciar fade in automáticamente
        StartCoroutine(FadeInCoroutine());
    }

    IEnumerator FadeInCoroutine()
    {
        // Delay opcional antes de empezar el fade
        if (delayBeforeFade > 0)
        {
            yield return new WaitForSeconds(delayBeforeFade);
        }

        float elapsed = 0f;

        while (elapsed < fadeInDuration)
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeInDuration);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Asegurar que quede completamente transparente
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }

        // Desactivar la imagen para que no bloquee raycast
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(false);
        }
    }

    // Método público para hacer fade in manualmente
    public void DoFadeIn(float duration = -1f)
    {
        if (duration < 0)
            duration = fadeInDuration;

        StartCoroutine(FadeInManual(duration));
    }

    IEnumerator FadeInManual(float duration)
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);

            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
            }
        }

        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }

        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(false);
        }
    }

    // Método para hacer fade out
    public void DoFadeOut(float duration = -1f)
    {
        if (duration < 0)
            duration = fadeInDuration;

        StartCoroutine(FadeOutManual(duration));
    }

    IEnumerator FadeOutManual(float duration)
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);

            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
            }
        }

        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
        }
    }
}