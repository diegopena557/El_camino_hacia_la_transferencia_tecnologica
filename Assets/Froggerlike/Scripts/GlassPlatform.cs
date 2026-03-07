using UnityEngine;
using TMPro;

public class GlassPlatform : MonoBehaviour
{
    [Header("Platform Type")]
    public bool isCorrect = false;

    [Header("Platform Text")]
    [TextArea(3, 6)]
    public string platformText = "Texto de la plataforma";
    public TextMeshProUGUI textDisplay;
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

        // Asegurar que el color inicial sea visible (blanco por defecto)
        if (spriteRenderer.color == Color.clear || spriteRenderer.color.a < 0.1f)
        {
            spriteRenderer.color = defaultColor;
        }

        originalColor = spriteRenderer.color;
        originalScale = transform.localScale;
        targetScale = originalScale * normalScale;

        // Configurar el texto
        if (textDisplay != null)
        {
            textDisplay.text = platformText;
            textDisplay.fontSize = normalFontSize;
        }
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
            spriteRenderer.color = hoverColor;
        }

        // Expandir texto
        if (textDisplay != null)
        {
            textDisplay.fontSize = expandedFontSize;
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
            if (originalColor == Color.black || originalColor == Color.clear || originalColor.a < 0.1f)
            {
                spriteRenderer.color = Color.white;
            }
            else
            {
                spriteRenderer.color = originalColor;
            }
        }

        // Reducir texto
        if (textDisplay != null)
        {
            textDisplay.fontSize = normalFontSize;
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

    public void ResetPlatform()
    {
        hasBeenClicked = false;

        // Resetear a blanco si el color original era negro/transparente
        if (originalColor == Color.black || originalColor == Color.clear || originalColor.a < 0.1f)
        {
            spriteRenderer.color = Color.white;
        }
        else
        {
            spriteRenderer.color = originalColor;
        }

        spriteRenderer.enabled = true;
        if (GetComponent<Collider2D>())
            GetComponent<Collider2D>().enabled = true;

        // Resetear escala y texto
        targetScale = originalScale * normalScale;
        if (textDisplay != null)
        {
            textDisplay.fontSize = normalFontSize;
        }
    }
}