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

    [Header("Audio de introducción")]
    [Tooltip("AudioSource con Play On Awake que suena durante la intro.")]
    public AudioSource introAudio;
    [Tooltip("Si está activo, la música de intro se desvanece junto con el canvas. Si no, se corta de golpe.")]
    public bool fadeOutIntroAudio = true;

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

        // Si no hay fade de audio, cortar la música de intro de inmediato
        if (!fadeOutIntroAudio)
            StopIntroAudio();

        // Reproducir sonido
        if (audioSource != null && startSound != null)
        {
            audioSource.PlayOneShot(startSound);
        }

        // Fade out
        StartCoroutine(FadeAndStart());
    }

    void StopIntroAudio()
    {
        if (introAudio != null && introAudio.isPlaying)
            introAudio.Stop();
    }

    IEnumerator FadeAndStart()
    {
        // Volumen inicial de la intro (para el fade y para restaurarlo después)
        float introStartVolume = introAudio != null ? introAudio.volume : 1f;

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / fadeDuration);

            introCanvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

            // El fade del audio va dentro del mismo bucle
            if (fadeOutIntroAudio && introAudio != null)
                introAudio.volume = Mathf.Lerp(introStartVolume, 0f, t);

            yield return null;
        }

        introCanvasGroup.alpha = 0f;

        // Asegurar que la música de intro termine y restaurar su volumen
        if (introAudio != null)
        {
            introAudio.Stop();
            introAudio.volume = introStartVolume;
        }

        // Ocultar canvas
        introCanvas.SetActive(false);

        // Reanudar movimiento
        mover.SetPaused(false);

        // Iniciar recorrido
        mover.BeginJourney();
    }
}