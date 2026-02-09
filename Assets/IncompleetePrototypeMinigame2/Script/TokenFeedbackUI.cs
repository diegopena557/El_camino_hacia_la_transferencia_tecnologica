using UnityEngine;
using TMPro;
using System.Collections;

public class TokenFeedbackUI : MonoBehaviour
{
    public static TokenFeedbackUI Instance;

    [Header("Referencias UI")]
    public GameObject feedbackPanel;
    public TextMeshProUGUI feedbackText;
    public CanvasGroup canvasGroup;

    [Header("Mensajes de Feedback")]
    [TextArea(2, 3)]
    public string correctMessage = "¡Correcto!";
    [TextArea(2, 3)]
    public string wrongMessage = "Categoría incorrecta";
    [TextArea(2, 3)]
    public string slotFullMessage = "El slot está lleno";

    [Header("Colores")]
    public Color correctColor = new Color(0.2f, 0.8f, 0.2f);
    public Color wrongColor = new Color(0.9f, 0.2f, 0.2f);
    public Color neutralColor = new Color(0.9f, 0.7f, 0.2f);

    [Header("Configuración de Animación")]
    public float fadeDuration = 0.5f;
    public float displayDuration = 2.5f;
    public float scaleUpAmount = 1.1f;
    public float animationSpeed = 8f;

    [Header("Debug")]
    public bool showDebugLogs = true;

    private Vector3 originalScale;
    private Coroutine currentFeedback;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (feedbackPanel != null)
        {
            originalScale = feedbackPanel.transform.localScale;

            if (showDebugLogs)
                Debug.Log($"[TokenFeedbackUI] Panel configurado. Posición: {feedbackPanel.transform.position}");
        }
        else
        {
            Debug.LogError("[TokenFeedbackUI] ¡Feedback Panel no asignado!");
        }

        if (canvasGroup == null && feedbackPanel != null)
        {
            canvasGroup = feedbackPanel.GetComponent<CanvasGroup>();

            if (canvasGroup == null)
            {
                canvasGroup = feedbackPanel.AddComponent<CanvasGroup>();
                if (showDebugLogs)
                    Debug.Log("[TokenFeedbackUI] CanvasGroup agregado automáticamente");
            }
        }

        if (feedbackText == null)
        {
            Debug.LogError("[TokenFeedbackUI] ¡Feedback Text no asignado!");
        }
    }

    void Start()
    {
        // Asegurar que el panel esté oculto al inicio
        if (feedbackPanel != null)
        {
            feedbackPanel.SetActive(false);
            Debug.Log("[TokenFeedbackUI] Sistema de feedback inicializado correctamente");
        }
    }

    // MÉTODO DE PRUEBA - Presiona T para probar
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("[TokenFeedbackUI] Tecla T presionada - Mostrando feedback de prueba");
            ShowCorrectFeedback("PRUEBA");
        }
    }

    public void ShowCorrectFeedback(string tokenName = "")
    {
        Debug.Log($"[TokenFeedbackUI] ShowCorrectFeedback llamado. Token: {tokenName}");

        string message = correctMessage;
        if (!string.IsNullOrEmpty(tokenName))
        {
            message = $"¡Correcto!\n{tokenName}";
        }

        if (currentFeedback != null)
        {
            Debug.Log("[TokenFeedbackUI] Deteniendo feedback anterior");
            StopCoroutine(currentFeedback);
        }

        currentFeedback = StartCoroutine(ShowFeedbackCoroutine(message, correctColor));
    }

    public void ShowWrongFeedback(string expectedCategory = "", string receivedCategory = "")
    {
        Debug.Log($"[TokenFeedbackUI] ShowWrongFeedback llamado. Esperado: {expectedCategory}, Recibido: {receivedCategory}");

        string message = wrongMessage;

        if (!string.IsNullOrEmpty(expectedCategory) && !string.IsNullOrEmpty(receivedCategory))
        {
            message = $"Categoría incorrecta\nSe esperaba: {expectedCategory}\nRecibido: {receivedCategory}";
        }

        if (currentFeedback != null)
        {
            Debug.Log("[TokenFeedbackUI] Deteniendo feedback anterior");
            StopCoroutine(currentFeedback);
        }

        currentFeedback = StartCoroutine(ShowFeedbackCoroutine(message, wrongColor));
    }

    public void ShowSlotFullFeedback()
    {
        Debug.Log("[TokenFeedbackUI] ShowSlotFullFeedback llamado");

        if (currentFeedback != null)
        {
            Debug.Log("[TokenFeedbackUI] Deteniendo feedback anterior");
            StopCoroutine(currentFeedback);
        }

        currentFeedback = StartCoroutine(ShowFeedbackCoroutine(slotFullMessage, neutralColor));
    }

    public void ShowCustomFeedback(string message, Color color)
    {
        if (currentFeedback != null)
            StopCoroutine(currentFeedback);

        currentFeedback = StartCoroutine(ShowFeedbackCoroutine(message, color));
    }

    IEnumerator ShowFeedbackCoroutine(string message, Color color)
    {
        if (feedbackPanel == null || feedbackText == null)
        {
            Debug.LogWarning("[TokenFeedbackUI] Panel o texto no asignado");
            yield break;
        }

        if (showDebugLogs)
            Debug.Log($"[TokenFeedbackUI] Mostrando feedback: {message}");

        // Configurar texto y color
        feedbackText.text = message;
        feedbackText.color = color;

        // Resetear posición y escala
        feedbackPanel.transform.localScale = originalScale;
        feedbackPanel.transform.localPosition = Vector3.zero;

        // Activar panel
        feedbackPanel.SetActive(true);

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;

        // Fade in con escala suave
        float elapsed = 0f;
        Vector3 startScale = originalScale * 0.9f;
        Vector3 targetScale = originalScale * scaleUpAmount;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / fadeDuration);

            if (canvasGroup != null)
                canvasGroup.alpha = t;

            feedbackPanel.transform.localScale = Vector3.Lerp(startScale, targetScale, t);

            yield return null;
        }

        // Asegurar valores finales
        if (canvasGroup != null)
            canvasGroup.alpha = 1f;
        feedbackPanel.transform.localScale = targetScale;

        if (showDebugLogs)
            Debug.Log($"[TokenFeedbackUI] Mostrando durante {displayDuration} segundos");

        // Mantener en pantalla
        yield return new WaitForSeconds(displayDuration);

        // Bajar escala suavemente a normal
        elapsed = 0f;
        float scaleBackDuration = fadeDuration * 0.3f;

        while (elapsed < scaleBackDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / scaleBackDuration;

            feedbackPanel.transform.localScale = Vector3.Lerp(targetScale, originalScale, t);

            yield return null;
        }

        feedbackPanel.transform.localScale = originalScale;

        // Fade out
        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;

            if (canvasGroup != null)
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

            yield return null;
        }

        // Desactivar panel
        if (canvasGroup != null)
            canvasGroup.alpha = 0f;

        feedbackPanel.SetActive(false);

        if (showDebugLogs)
            Debug.Log("[TokenFeedbackUI] Feedback ocultado");

        currentFeedback = null;
    }

    // Método para detener el feedback actual
    public void HideFeedback()
    {
        if (currentFeedback != null)
        {
            StopCoroutine(currentFeedback);
            currentFeedback = null;
        }

        if (feedbackPanel != null)
            feedbackPanel.SetActive(false);
    }
}