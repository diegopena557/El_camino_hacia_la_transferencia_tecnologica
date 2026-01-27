using UnityEngine;
using System.Collections;

public class TokenFeedback : MonoBehaviour
{
    [Header("Shake Settings")]
    public float shakeDuration = 0.25f;
    public float shakeMagnitude = 0.1f;

    [Header("Highlight Settings")]
    public SpriteRenderer highlightSprite;
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;
    public float highlightDuration = 0.4f;

    [Header("Scale Feedback")]
    public float correctScaleBounce = 1.3f;
    public float wrongScaleSquash = 0.9f;
    public float scaleDuration = 0.3f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip correctSound;
    public AudioClip wrongSound;

    private Vector3 originalPosition;
    private Vector3 originalScale;
    private Color originalColor;

    void Awake()
    {
        originalPosition = transform.position;
        originalScale = transform.localScale;

        if (highlightSprite != null)
            originalColor = highlightSprite.color;
    }

    public void PlayCorrectFeedback()
    {
        if (audioSource && correctSound)
            audioSource.PlayOneShot(correctSound);

        StartCoroutine(CorrectFeedbackCoroutine());
    }

    public void PlayWrongFeedback()
    {
        if (audioSource && wrongSound)
            audioSource.PlayOneShot(wrongSound);

        StartCoroutine(WrongFeedbackCoroutine());
    }

    IEnumerator CorrectFeedbackCoroutine()
    {
        // Highlight verde
        if (highlightSprite != null)
        {
            highlightSprite.color = correctColor;
        }

        // Bounce scale
        float elapsed = 0f;
        while (elapsed < scaleDuration)
        {
            float t = elapsed / scaleDuration;
            float scale = Mathf.Lerp(correctScaleBounce, 1f, t);
            transform.localScale = originalScale * scale;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;

        // Esperar un poco antes de quitar el highlight
        yield return new WaitForSeconds(highlightDuration - scaleDuration);

        if (highlightSprite != null)
        {
            highlightSprite.color = originalColor;
        }
    }

    IEnumerator WrongFeedbackCoroutine()
    {
        // Highlight rojo
        if (highlightSprite != null)
        {
            highlightSprite.color = wrongColor;
        }

        // Shake y squash simultáneos
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            // Shake
            Vector2 randomOffset = Random.insideUnitCircle * shakeMagnitude;
            transform.position = originalPosition + (Vector3)randomOffset;

            // Squash
            float t = elapsed / shakeDuration;
            float scale = Mathf.Lerp(wrongScaleSquash, 1f, t);
            transform.localScale = originalScale * scale;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
        transform.localScale = originalScale;

        // Esperar antes de quitar el highlight
        yield return new WaitForSeconds(highlightDuration - shakeDuration);

        if (highlightSprite != null)
        {
            highlightSprite.color = originalColor;
        }
    }

    // Método para feedback neutro (opcional)
    public void PlayNeutralFeedback()
    {
        StartCoroutine(NeutralFeedbackCoroutine());
    }

    IEnumerator NeutralFeedbackCoroutine()
    {
        if (highlightSprite != null)
        {
            highlightSprite.color = Color.yellow;
        }

        yield return new WaitForSeconds(highlightDuration);

        if (highlightSprite != null)
        {
            highlightSprite.color = originalColor;
        }
    }
}