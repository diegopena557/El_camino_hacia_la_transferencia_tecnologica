using UnityEngine;
using System.Collections;

public class ChestFeedback : MonoBehaviour
{
    [Header("Shake Settings")]
    public float shakeDuration = 0.25f;
    public float shakeMagnitude = 0.1f;

    [Header("Highlight Settings")]
    public SpriteRenderer highlightSprite;
    public Color correctColor = Color.green;
    public float highlightDuration = 0.4f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip correctSound;
    public AudioClip wrongSound;

    private Vector3 originalPosition;
    private Color originalColor;

    void Awake()
    {
        originalPosition = transform.position;

        if (highlightSprite != null)
            originalColor = highlightSprite.color;
    }

    public void PlayCorrectFeedback()
    {
        if (audioSource && correctSound)
            audioSource.PlayOneShot(correctSound);

        if (highlightSprite != null)
            StartCoroutine(HighlightCoroutine());
    }

    public void PlayWrongFeedback()
    {
        if (audioSource && wrongSound)
            audioSource.PlayOneShot(wrongSound);

        StartCoroutine(ShakeCoroutine());
    }

    IEnumerator ShakeCoroutine()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            Vector2 randomOffset = Random.insideUnitCircle * shakeMagnitude;
            transform.position = originalPosition + (Vector3)randomOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
    }

    IEnumerator HighlightCoroutine()
    {
        highlightSprite.color = correctColor;
        yield return new WaitForSeconds(highlightDuration);
        highlightSprite.color = originalColor;
    }
}

