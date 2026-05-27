using UnityEngine;
using TMPro;

public class GlassPlatform : MonoBehaviour
{
    [Header("Platform Type")]
    public bool isCorrect = false;

    [Header("Platform Text")]
    [TextArea(3, 6)]
    public string platformText = "Texto de la plataforma";
    public TextMeshProUGUI textDisplayUI; // Para UI (Canvas)
    public int normalFontSize = 18;
    public int expandedFontSize = 24;

    [Header("Visual Feedback")]
    public SpriteRenderer spriteRenderer;
    public Color correctColor = Color.green;
    public Color incorrectColor = Color.red;
    public Color defaultColor = Color.white;
    public Color hoverColor = new Color(1f, 1f, 0.8f, 1f); // Amarillo claro

    [Header("Hover Scale")]
    public float normalScale = 1f;
    public float hoverScale = 1.2f;
    public float scaleSpeed = 8f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip correctSound;
    public AudioClip incorrectSound;
    public AudioClip breakSound;

    private Color originalColor;
    private Vector3 targetScale;
    private Vector3 originalScale;
    private bool isHovering = false;
    private bool hasBeenClicked = false;

    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        originalColor = spriteRenderer.color;

        // INICIAR INVISIBLE - El manager hará el fade in
        Color startColor = spriteRenderer.color;
        startColor.a = 0f;
        spriteRenderer.color = startColor;

        originalScale = transform.localScale;
        targetScale = originalScale * normalScale;

        // Configurar el texto UI
        if (textDisplayUI != null)
        {
            textDisplayUI.text = platformText;
            textDisplayUI.fontSize = normalFontSize;
            textDisplayUI.gameObject.SetActive(false); // OCULTAR por defecto
        }

        // Desactivar collider al inicio (se activa con el fade in)
        if (GetComponent<Collider2D>())
            GetComponent<Collider2D>().enabled = false;
    }

    void Update()
    {
        // Animar la escala suavemente
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
    }

    void OnMouseEnter()
    {
        if (GlassBridgeManager.Instance.IsGameOver()) return;
        if (GlassBridgeManager.Instance.IsMoving()) return;

        isHovering = true;

        // Expandir la plataforma
        targetScale = originalScale * hoverScale;

        // Cambiar color a hover si no ha sido clickeada
        if (!hasBeenClicked)
        {
            Color hoverColorWithAlpha = hoverColor;
            hoverColorWithAlpha.a = spriteRenderer.color.a; // Mantener el alpha actual
            spriteRenderer.color = hoverColorWithAlpha;
        }

        // MOSTRAR y expandir texto UI
        if (textDisplayUI != null)
        {
            textDisplayUI.gameObject.SetActive(true); // MOSTRAR texto
            textDisplayUI.fontSize = expandedFontSize;
        }
    }

    void OnMouseExit()
    {
        isHovering = false;

        // Volver a escala normal
        targetScale = originalScale * normalScale;

        // Restaurar color si no ha sido clickeada
        if (!hasBeenClicked)
        {
            Color restoredColor;
            if (originalColor == Color.black || originalColor == Color.clear || originalColor.a < 0.1f)
            {
                restoredColor = Color.white;
            }
            else
            {
                restoredColor = originalColor;
            }

            // Mantener el alpha actual
            restoredColor.a = spriteRenderer.color.a;
            spriteRenderer.color = restoredColor;
        }

        // OCULTAR texto UI
        if (textDisplayUI != null)
        {
            textDisplayUI.gameObject.SetActive(false); // OCULTAR texto
            textDisplayUI.fontSize = normalFontSize;
        }
    }

    void OnMouseDown()
    {
        if (GlassBridgeManager.Instance.IsGameOver()) return;
        if (GlassBridgeManager.Instance.IsMoving()) return; // No permitir clics mientras se mueve

        GlassBridgeManager.Instance.OnPlatformClicked(this);
    }

    public void ShowResult()
    {
        hasBeenClicked = true;

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

            // NO romper inmediatamente - solo cambiar color
            // Se romperá cuando el jugador avance al siguiente nivel
        }
    }

    public void BreakPlatformNow()
    {
        if (audioSource && breakSound)
            audioSource.PlayOneShot(breakSound);

        // Hacer que caiga o desaparezca
        spriteRenderer.enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }

    public void ResetColor()
    {
        hasBeenClicked = false;

        // Volver al color original (blanco)
        if (originalColor == Color.black || originalColor == Color.clear || originalColor.a < 0.1f)
        {
            spriteRenderer.color = Color.white;
        }
        else
        {
            spriteRenderer.color = originalColor;
        }
    }

    public void SetAlpha(float alpha)
    {
        // Cambiar solo el alpha, manteniendo el color actual
        Color currentColor = spriteRenderer.color;
        currentColor.a = alpha;
        spriteRenderer.color = currentColor;

        // También ajustar alpha del texto UI si existe
        if (textDisplayUI != null)
        {
            Color textColor = textDisplayUI.color;
            textColor.a = alpha;
            textDisplayUI.color = textColor;
        }

        // Desactivar/activar collider según visibilidad
        if (GetComponent<Collider2D>())
        {
            GetComponent<Collider2D>().enabled = alpha > 0.1f;
        }
    }

    public void ResetPlatform()
    {
        hasBeenClicked = false;

        // Resetear a color original pero mantener alpha en 0
        Color resetColor;
        if (originalColor == Color.black || originalColor == Color.clear || originalColor.a < 0.1f)
        {
            resetColor = Color.white;
        }
        else
        {
            resetColor = originalColor;
        }
        resetColor.a = 0f; // Empezar invisible
        spriteRenderer.color = resetColor;

        spriteRenderer.enabled = true;

        // Desactivar collider (se activará con el fade in)
        if (GetComponent<Collider2D>())
            GetComponent<Collider2D>().enabled = false;

        // Resetear escala y ocultar texto UI
        targetScale = originalScale * normalScale;
        if (textDisplayUI != null)
        {
            textDisplayUI.gameObject.SetActive(false); // OCULTAR texto
            textDisplayUI.fontSize = normalFontSize;
        }
    }
}