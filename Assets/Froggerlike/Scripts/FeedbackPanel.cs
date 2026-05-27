using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class FeedbackPanel : MonoBehaviour
{
    public static FeedbackPanel Instance;

    [Header("Referencias UI")]
    public Image backgroundImage;
    public Image feedbackImage;
    public TextMeshProUGUI feedbackTitle;
    public TextMeshProUGUI feedbackText;

    [Header("Títulos")]
    public string correctTitle = "¡CORRECTO!";
    public string incorrectTitle = "INCORRECTO";

    [Header("Colores de Título")]
    public Color correctTitleColor = Color.green;
    public Color incorrectTitleColor = Color.red;

    [Header("Animación")]
    public float fadeInDuration = 0.3f;
    public float displayDuration = 2f;
    public float fadeOutDuration = 1.0f;

    [Header("Escala (pop)")]
    public float popScale = 1.1f;
    public float popDuration = 0.15f;

    private Coroutine activeRoutine;

    // Almacenar colores originales
    private Color originalBgColor;
    private Color originalImgColor;
    private Color originalTextColor;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Guardar colores originales
        if (backgroundImage != null)
            originalBgColor = backgroundImage.color;
        if (feedbackImage != null)
            originalImgColor = feedbackImage.color;
        if (feedbackText != null)
            originalTextColor = feedbackText.color;

        // Empezar invisible
        SetAlpha(0f);
        transform.localScale = Vector3.one;
    }

   
    public void Show(bool isCorrect, string text)
    {
        if (activeRoutine != null)
            StopCoroutine(activeRoutine);

        // Configurar título según resultado
        if (feedbackTitle != null)
        {
            feedbackTitle.text = isCorrect ? correctTitle : incorrectTitle;
            feedbackTitle.color = isCorrect ? correctTitleColor : incorrectTitleColor;
        }

        if (feedbackText != null)
            feedbackText.text = text;

        activeRoutine = StartCoroutine(ShowRoutine());
    }

    IEnumerator ShowRoutine()
    {
        // Fade in + pop
        float elapsed = 0f;
        Vector3 startScale = Vector3.one * 0.8f;
        Vector3 targetScale = Vector3.one * popScale;

        transform.localScale = startScale;

        while (elapsed < fadeInDuration)
        {
            float t = elapsed / fadeInDuration;
            SetAlpha(Mathf.Lerp(0f, 1f, t));
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        SetAlpha(1f);

        // Pop de vuelta a escala 1
        elapsed = 0f;
        while (elapsed < popDuration)
        {
            float t = elapsed / popDuration;
            transform.localScale = Vector3.Lerp(targetScale, Vector3.one, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = Vector3.one;

        
        yield return new WaitForSeconds(displayDuration);

        
        elapsed = 0f;
        while (elapsed < fadeOutDuration)
        {
            float t = elapsed / fadeOutDuration;
            SetAlpha(Mathf.Lerp(1f, 0f, t));
            elapsed += Time.deltaTime;
            yield return null;
        }

        SetAlpha(0f);
        transform.localScale = Vector3.one;
        activeRoutine = null;
    }

    /// <summary>
    /// Establece el alpha de todos los componentes UI del panel.
    /// </summary>
    void SetAlpha(float alpha)
    {
        if (backgroundImage != null)
        {
            Color c = originalBgColor;
            c.a = alpha;
            backgroundImage.color = c;
        }

        if (feedbackImage != null)
        {
            Color c = originalImgColor;
            c.a = alpha;
            feedbackImage.color = c;
        }

        if (feedbackTitle != null)
        {
            Color c = feedbackTitle.color;
            c.a = alpha;
            feedbackTitle.color = c;
        }

        if (feedbackText != null)
        {
            Color c = originalTextColor;
            c.a = alpha;
            feedbackText.color = c;
        }
    }
}