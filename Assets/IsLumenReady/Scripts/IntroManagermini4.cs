using System.Collections;
using UnityEngine;

public class IntroManagermini4 : MonoBehaviour
{
    [Header("UI")]
    public GameObject introCanvas;
    public CanvasGroup introCanvasGroup;

    [Header("Referencias")]
    public WaypointMover mover;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip startSound;

    [Header("Fade")]
    public float fadeDuration = 1f;

    private bool gameStarted = false;

    void Start()
    {
        // Mostrar intro
        introCanvas.SetActive(true);

        // Canvas visible
        introCanvasGroup.alpha = 1f;

        // Pausar personaje
        mover.SetPaused(true);
    }

    // Asignar al boton "Comenzar"
    public void StartGame()
    {
        if (gameStarted) return;

        gameStarted = true;

        // Reproducir sonido
        if (audioSource != null && startSound != null)
        {
            audioSource.PlayOneShot(startSound);
        }

        // Fade out
        StartCoroutine(FadeAndStart());
    }

    IEnumerator FadeAndStart()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);

            introCanvasGroup.alpha = alpha;

            yield return null;
        }

        introCanvasGroup.alpha = 0f;

        // Ocultar canvas
        introCanvas.SetActive(false);

        // Reanudar movimiento
        mover.SetPaused(false);

        // Iniciar recorrido
        mover.BeginJourney();
    }
}