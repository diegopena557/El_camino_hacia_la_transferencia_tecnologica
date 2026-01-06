using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class FeedbackTextManager : MonoBehaviour
{
    public static FeedbackTextManager Instance;

    [Header("UI References")]
    public TextMeshProUGUI feedbackText;
    public CanvasGroup feedbackCanvasGroup;

    [Header("Animation Settings")]
    public float fadeDuration = 0.3f;
    public float displayDuration = 1.5f;
    public float moveUpDistance = 30f;

    [Header("Mensajes por Categoría")]
    public List<string> cienciaMessages = new List<string>
    {
        "¡Excelente conocimiento científico!",
        "¡La ciencia está de tu lado!",
        "¡Descubrimiento correcto!",
        "¡Científico brillante!"
    };

    public List<string> tecnologiaMessages = new List<string>
    {
        "¡Dominas la tecnología!",
        "¡Innovación tecnológica correcta!",
        "¡Tech Master!",
        "¡El futuro es tuyo!"
    };

    public List<string> innovacionMessages = new List<string>
    {
        "¡Mente innovadora!",
        "¡Creatividad en acción!",
        "¡Innovación perfecta!",
        "¡Pensamiento disruptivo!"
    };

    [Header("Mensajes Generales")]
    public List<string> generalMessages = new List<string>
    {
        "¡Muy bien!",
        "¡Correcto!",
        "¡Perfecto!",
        "¡Excelente!",
        "¡Increíble!",
        "¡Fantástico!"
    };

    private Vector3 originalPosition;
    private Coroutine currentFeedback;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (feedbackText != null)
            originalPosition = feedbackText.transform.localPosition;

        if (feedbackCanvasGroup != null)
            feedbackCanvasGroup.alpha = 0;
    }

    public void ShowFeedback(CardType cardType)
    {
        if (currentFeedback != null)
            StopCoroutine(currentFeedback);

        string message = GetRandomMessage(cardType);
        currentFeedback = StartCoroutine(ShowFeedbackCoroutine(message));
    }

    string GetRandomMessage(CardType cardType)
    {
        List<string> messagePool = cardType switch
        {
            CardType.Ciencia => cienciaMessages,
            CardType.Tecnologia => tecnologiaMessages,
            CardType.Innovacion => innovacionMessages,
            _ => generalMessages
        };

        if (messagePool.Count == 0)
            messagePool = generalMessages;

        return messagePool[Random.Range(0, messagePool.Count)];
    }

    IEnumerator ShowFeedbackCoroutine(string message)
    {
        if (feedbackText == null || feedbackCanvasGroup == null)
            yield break;

        // Configurar mensaje
        feedbackText.text = message;
        feedbackText.transform.localPosition = originalPosition;

        // Fade In
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            feedbackCanvasGroup.alpha = Mathf.Lerp(0, 1, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        feedbackCanvasGroup.alpha = 1;

        // Mostrar y mover hacia arriba
        elapsed = 0f;
        Vector3 startPos = feedbackText.transform.localPosition;
        Vector3 endPos = startPos + Vector3.up * moveUpDistance;

        while (elapsed < displayDuration)
        {
            feedbackText.transform.localPosition = Vector3.Lerp(startPos, endPos, elapsed / displayDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Fade Out
        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            feedbackCanvasGroup.alpha = Mathf.Lerp(1, 0, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        feedbackCanvasGroup.alpha = 0;

        // Resetear posición
        feedbackText.transform.localPosition = originalPosition;
    }
}