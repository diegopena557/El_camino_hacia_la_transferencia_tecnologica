using UnityEngine;
using TMPro;
using System.Collections;

public class TokenTooltip : MonoBehaviour
{
    public static TokenTooltip Instance;

    [Header("Referencias UI")]
    public GameObject tooltipPanel;
    public TextMeshProUGUI tooltipText;
    public CanvasGroup canvasGroup;

    [Header("Configuración")]
    public Vector2 offset = new Vector2(0, 100); // Offset desde el cursor
    public float fadeDuration = 0.2f;
    public bool followCursor = true;

    [Header("Estilo")]
    public Color backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.9f);

    private RectTransform rectTransform;
    private Canvas canvas;
    private bool isShowing = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        rectTransform = tooltipPanel.GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        if (canvasGroup == null)
        {
            canvasGroup = tooltipPanel.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = tooltipPanel.AddComponent<CanvasGroup>();
        }

        HideTooltip();
    }

    void Update()
    {
        if (isShowing && followCursor)
        {
            UpdatePosition();
        }
    }

    public void ShowTooltip(string description)
    {
        if (string.IsNullOrEmpty(description))
        {
            HideTooltip();
            return;
        }

        // Si hay feedback activo, no mostrar tooltip
        if (TokenFeedbackUI.Instance != null && TokenFeedbackUI.Instance.feedbackPanel != null)
        {
            if (TokenFeedbackUI.Instance.feedbackPanel.activeSelf)
            {
                Debug.Log("[TokenTooltip] Feedback activo, no mostrando tooltip");
                return;
            }
        }

        tooltipText.text = description;
        tooltipPanel.SetActive(true);
        isShowing = true;

        UpdatePosition();

        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }

    public void HideTooltip()
    {
        isShowing = false;
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    void UpdatePosition()
    {
        if (canvas == null) return;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.worldCamera,
            out localPoint
        );

        // Aplicar offset
        localPoint += offset;

        // Mantener dentro de la pantalla
        Vector2 pivotOffset = new Vector2(
            rectTransform.rect.width * rectTransform.pivot.x,
            rectTransform.rect.height * rectTransform.pivot.y
        );

        localPoint -= pivotOffset;

        RectTransform canvasRect = canvas.transform as RectTransform;
        Vector2 canvasSize = canvasRect.rect.size;

        // Clamp horizontal
        if (localPoint.x + rectTransform.rect.width > canvasSize.x / 2)
            localPoint.x = canvasSize.x / 2 - rectTransform.rect.width;
        if (localPoint.x < -canvasSize.x / 2)
            localPoint.x = -canvasSize.x / 2;

        // Clamp vertical
        if (localPoint.y + rectTransform.rect.height > canvasSize.y / 2)
            localPoint.y = canvasSize.y / 2 - rectTransform.rect.height;
        if (localPoint.y < -canvasSize.y / 2)
            localPoint.y = -canvasSize.y / 2;

        rectTransform.localPosition = localPoint + pivotOffset;
    }

    IEnumerator FadeIn()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    IEnumerator FadeOut()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        tooltipPanel.SetActive(false);
    }
}