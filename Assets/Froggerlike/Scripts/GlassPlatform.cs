using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GlassPlatform : MonoBehaviour
{
    [Header("Platform Type")]
    public bool isCorrect = false;

    [Header("Platform Text")]
    [TextArea(3, 6)]
    public string platformText = "Texto de la plataforma";
    public TextMeshProUGUI textDisplayUI;
    public int normalFontSize = 18;
    public int expandedFontSize = 24;

    [Header("Feedback")]
    [TextArea(3, 6)]
    public string feedbackText = "Explicación de esta opción...";

    [Header("Visual Feedback")]
    public SpriteRenderer spriteRenderer;
    public Color correctColor = Color.green;
    public Color incorrectColor = Color.red;
    public Color defaultColor = Color.white;
    public Color hoverColor = new Color(1f, 1f, 0.8f, 1f);

    [Header("Hover Scale")]
    public float normalScale = 1f;
    public float hoverScale = 1.2f;
    public float scaleSpeed = 8f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip correctSound;
    public AudioClip incorrectSound;
    public AudioClip breakSound;

    private Vector3 targetScale;
    private Vector3 originalScale;
    private bool hasBeenClicked = false;

    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        Color startColor = spriteRenderer.color;
        startColor.a = 0f;
        spriteRenderer.color = startColor;

        originalScale = transform.localScale;
        targetScale = originalScale * normalScale;

        if (textDisplayUI != null)
        {
            textDisplayUI.text = platformText;
            textDisplayUI.fontSize = normalFontSize;
            textDisplayUI.gameObject.SetActive(false);
        }

        if (GetComponent<Collider2D>())
            GetComponent<Collider2D>().enabled = false;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
    }

    void OnMouseEnter()
    {
        if (GlassBridgeManager.Instance.IsGameOver()) return;
        if (GlassBridgeManager.Instance.IsMoving()) return;
        if (hasBeenClicked) return;

        targetScale = originalScale * hoverScale;

        Color hoverColorWithAlpha = hoverColor;
        hoverColorWithAlpha.a = spriteRenderer.color.a;
        spriteRenderer.color = hoverColorWithAlpha;

        if (textDisplayUI != null)
        {
            textDisplayUI.gameObject.SetActive(true);
            textDisplayUI.fontSize = expandedFontSize;
        }
    }

    void OnMouseExit()
    {
        targetScale = originalScale * normalScale;

        if (!hasBeenClicked)
        {
            Color restored = defaultColor;
            restored.a = spriteRenderer.color.a;
            spriteRenderer.color = restored;
        }

        if (textDisplayUI != null)
        {
            textDisplayUI.gameObject.SetActive(false);
            textDisplayUI.fontSize = normalFontSize;
        }
    }

    void OnMouseDown()
    {
        if (GlassBridgeManager.Instance.IsGameOver()) return;
        if (GlassBridgeManager.Instance.IsMoving()) return;
        if (hasBeenClicked) return;

        GlassBridgeManager.Instance.OnPlatformClicked(this);
    }

    public void ShowResult()
    {
        hasBeenClicked = true;

        if (textDisplayUI != null)
            textDisplayUI.gameObject.SetActive(false);

        if (isCorrect)
        {
            spriteRenderer.color = correctColor;
            if (audioSource && correctSound)
                audioSource.PlayOneShot(correctSound);
        }
        else
        {
            spriteRenderer.color = incorrectColor;
            if (audioSource && incorrectSound)
                audioSource.PlayOneShot(incorrectSound);
        }

        // Mostrar feedback único de esta plataforma
        if (FeedbackPanel.Instance != null)
            FeedbackPanel.Instance.Show(isCorrect, feedbackText);
    }

    public void BreakPlatformNow()
    {
        StartCoroutine(BreakAndRestore());
    }

    IEnumerator BreakAndRestore()
    {
        if (audioSource && breakSound)
            audioSource.PlayOneShot(breakSound);

        spriteRenderer.enabled = false;
        if (GetComponent<Collider2D>())
            GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(0.5f);

        spriteRenderer.enabled = true;

        Color c = incorrectColor;
        c.a = 1f;
        spriteRenderer.color = c;
    }

    public void SetAlpha(float alpha)
    {
        Color currentColor = spriteRenderer.color;
        currentColor.a = alpha;
        spriteRenderer.color = currentColor;

        if (textDisplayUI != null)
        {
            Color textColor = textDisplayUI.color;
            textColor.a = alpha;
            textDisplayUI.color = textColor;
        }

        if (GetComponent<Collider2D>())
            GetComponent<Collider2D>().enabled = alpha > 0.1f;
    }

    public void ResetPlatform()
    {
        hasBeenClicked = false;

        Color resetColor = defaultColor;
        resetColor.a = 0f;
        spriteRenderer.color = resetColor;

        spriteRenderer.enabled = true;

        if (GetComponent<Collider2D>())
            GetComponent<Collider2D>().enabled = false;

        targetScale = originalScale * normalScale;

        if (textDisplayUI != null)
        {
            textDisplayUI.gameObject.SetActive(false);
            textDisplayUI.fontSize = normalFontSize;
        }
    }
}