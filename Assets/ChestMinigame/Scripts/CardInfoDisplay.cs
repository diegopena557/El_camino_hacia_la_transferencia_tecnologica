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

    [Header("Error Feedback")]
    public string errorPrefix = "Esta carta pertenece a: ";
    public Color errorTitleColor = Color.red;

    private Coroutine displayCoroutine;
    private Color originalTitleColor;

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

        if (titleText != null)
            originalTitleColor = titleText.color;
    }

    // Mostrar información cuando hay un ERROR
    public void ShowCardInfoOnError(CardFeedbackData cardData)
    {
        if (cardData == null || string.IsNullOrEmpty(cardData.feedbackMessage))
            return;

        if (displayCoroutine != null)
            StopCoroutine(displayCoroutine);

        displayCoroutine = StartCoroutine(DisplayErrorInfoCoroutine(cardData));
    }

    IEnumerator DisplayErrorInfoCoroutine(CardFeedbackData cardData)
    {
        // Configurar textos con estilo de error
        if (titleText != null)
        {
            titleText.text = errorPrefix + cardData.cardTitle;
            titleText.color = errorTitleColor;
        }

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

        // Restaurar color original del título
        if (titleText != null)
            titleText.color = originalTitleColor;
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

        if (titleText != null)
            titleText.color = originalTitleColor;
    }
}