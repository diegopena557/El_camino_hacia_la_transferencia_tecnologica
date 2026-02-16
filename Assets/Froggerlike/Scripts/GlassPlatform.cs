using UnityEngine;

public class GlassPlatform : MonoBehaviour
{
    [Header("Platform Type")]
    public bool isCorrect = false;

    [Header("Visual Feedback")]
    public SpriteRenderer spriteRenderer;
    public Color correctColor = Color.green;
    public Color incorrectColor = Color.red;
    public Color defaultColor = Color.white;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip correctSound;
    public AudioClip incorrectSound;
    public AudioClip breakSound;

    private Color originalColor;
    private bool hasBeenClicked = false;

    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        originalColor = spriteRenderer.color;
    }

    void OnMouseDown()
    {
        if (hasBeenClicked) return;
        if (GlassBridgeManager.Instance.IsGameOver()) return;

        hasBeenClicked = true;
        GlassBridgeManager.Instance.OnPlatformClicked(this);
    }

    public void ShowResult()
    {
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

            // Después de un momento, romper el vidrio
            Invoke(nameof(BreakPlatform), 0.5f);
        }
    }

    void BreakPlatform()
    {
        if (audioSource && breakSound)
            audioSource.PlayOneShot(breakSound);

        // Hacer que caiga o desaparezca
        spriteRenderer.enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }

    public void ResetPlatform()
    {
        hasBeenClicked = false;
        spriteRenderer.color = originalColor;
        spriteRenderer.enabled = true;
        if (GetComponent<Collider2D>())
            GetComponent<Collider2D>().enabled = true;
    }
}