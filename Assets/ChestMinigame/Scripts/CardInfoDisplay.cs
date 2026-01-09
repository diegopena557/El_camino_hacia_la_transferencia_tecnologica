using UnityEngine;
using TMPro;
using System.Collections;

public class CardInfoDisplay : MonoBehaviour
{
    public static CardInfoDisplay Instance;

    [Header("UI Panel")]
    public GameObject infoPanel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public CanvasGroup canvasGroup;

    [Header("Settings")]
    public float fadeDuration = 0.4f;
    public float displayDuration = 4f;

    private Coroutine displayCoroutine;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (canvasGroup != null)
            canvasGroup.alpha = 0;

        if (infoPanel != null)
            infoPanel.SetActive(false);
    }

    public void ShowCardInfo(CardFeedbackData cardData)
    {
        if (cardData == null || string.IsNullOrEmpty(cardData.feedbackMessage))
            return;

        if (displayCoroutine != null)
            StopCoroutine(displayCoroutine);

        displayCoroutine = StartCoroutine(DisplayInfoCoroutine(cardData));
    }

    IEnumerator DisplayInfoCoroutine(CardFeedbackData cardData)
    {
        // Configurar textos
        if (titleText != null)
            titleText.text = cardData.cardTitle;

        if (descriptionText != null)
            descriptionText.text = cardData.feedbackMessage;

        infoPanel.SetActive(true);

        // Fade In
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1;

        // Mostrar
        yield return new WaitForSeconds(displayDuration);

        // Fade Out
        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0;

        infoPanel.SetActive(false);
    }

    // Método opcional para cerrar manualmente
    public void ClosePanel()
    {
        if (displayCoroutine != null)
            StopCoroutine(displayCoroutine);

        if (infoPanel != null)
            infoPanel.SetActive(false);

        if (canvasGroup != null)
            canvasGroup.alpha = 0;
    }
}