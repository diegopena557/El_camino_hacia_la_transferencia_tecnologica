using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource moment1;
    public AudioSource moment2;
    public AudioSource ambientLoop;

    [Header("Mixer & Parameters")]
    public AudioMixer mixer;
    public string moment1Param = "VolumeLead1";
    public string moment2Param = "VolumeLead2";

    [Header("Music Settings")]
    public double bpm = 120.0;
    public int beatsPerBar = 4;

    [Header("Fade Durations")]
    public float fadeOutDuration = 3f;
    public float fadeInDuration = 0.5f;

    private double secPerBeat;
    private double secPerBar;

    //  Referencia al CardSpawner para suscribirse a su evento
    public CardSpawner spawner;

    void OnEnable()
    {
        // Suscribirse al evento cuando el objeto se habilita
        if (spawner != null)
            spawner.OnHardModeEntered += HandleHardMode;
    }

    void OnDisable()
    {
        // Desuscribirse para evitar errores si el objeto se destruye
        if (spawner != null)
            spawner.OnHardModeEntered -= HandleHardMode;
    }

    private void HandleHardMode()
    {
        // Método que se ejecuta cuando CardSpawner entra en modo difícil
        Debug.Log("[MusicManager] Hard mode entered → triggering transition!");
        TriggerTransition();
    }

    void Start()
    {
        secPerBeat = 60.0 / bpm;
        secPerBar = secPerBeat * beatsPerBar;

        moment1.loop = true;
        moment2.loop = true;
        ambientLoop.loop = true;

        moment1.Play();
        moment2.Play();
        ambientLoop.Play();

        mixer.SetFloat(moment1Param, 0f);
        mixer.SetFloat(moment2Param, -80f);

        Debug.Log($"[MusicManager] Started playback. BPM={bpm}, BeatsPerBar={beatsPerBar}, SecPerBar={secPerBar:F3}");
    }

    public void TriggerTransition()
    {
        double dspTime = AudioSettings.dspTime;
        double nextBar = Mathf.CeilToInt((float)(dspTime / secPerBar)) * secPerBar;

        Debug.Log($"[MusicManager] Scheduling transition at next bar={nextBar:F3} (in {(nextBar - dspTime):F3} sec)");

        Invoke(nameof(StartCrossfade), (float)(nextBar - dspTime));
    }

    private void StartCrossfade()
    {
        Debug.Log("[MusicManager] Starting crossfade now!");
        StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        float elapsedOut = 0f;
        float elapsedIn = 0f;

        mixer.SetFloat(moment1Param, 0f);
        mixer.SetFloat(moment2Param, -80f);

        while (elapsedOut < fadeOutDuration || elapsedIn < fadeInDuration)
        {
            if (elapsedOut < fadeOutDuration)
            {
                elapsedOut += Time.deltaTime;
                float tOut = elapsedOut / fadeOutDuration;
                float vol1 = Mathf.Lerp(0f, -80f, tOut);
                mixer.SetFloat(moment1Param, vol1);
                Debug.Log($"[MusicManager] Fading OUT moment1: {vol1:F2} dB");
            }

            if (elapsedIn < fadeInDuration)
            {
                elapsedIn += Time.deltaTime;
                float tIn = elapsedIn / fadeInDuration;
                float vol2 = Mathf.Lerp(-80f, 0f, tIn);
                mixer.SetFloat(moment2Param, vol2);
                Debug.Log($"[MusicManager] Fading IN moment2: {vol2:F2} dB");
            }

            yield return null;
        }

        mixer.SetFloat(moment1Param, -80f);
        mixer.SetFloat(moment2Param, 0f);

        Debug.Log("[MusicManager] Crossfade complete. moment1 muted, moment2 full volume.");
    }
}
