using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource moment1;
    public AudioSource moment2;
    public AudioSource moment3;
    public AudioSource ambientLoop;
    public AudioSource drumFillSource; // 🔹 nuevo para el fill

    [Header("Mixer & Parameters")]
    public AudioMixer mixer;
    public string moment1Param = "VolumeLead1";
    public string moment2Param = "VolumeLead2";
    public string moment3Param = "VolumeLead3";

    [Header("Music Settings")]
    public double bpm = 120.0;
    public int beatsPerBar = 4;

    [Header("Fade Durations 1→2")]
    public float fadeOutDuration12 = 3f;
    public float fadeInDuration12 = 0.5f;

    [Header("Fade Durations 2→3")]
    public float fadeOutDuration23 = 2f;
    public float fadeInDuration23 = 1f;

    private double secPerBeat;
    private double secPerBar;

    // Referencia al CardSpawner para suscribirse a su evento
    public CardSpawner spawner;

    void OnEnable()
    {
        if (spawner != null)
            spawner.OnHardModeEntered += HandleHardMode;
    }

    void OnDisable()
    {
        if (spawner != null)
            spawner.OnHardModeEntered -= HandleHardMode;
    }

    private void HandleHardMode()
    {
        Debug.Log("[MusicManager] Hard mode entered → triggering transition!");
        TriggerTransition12(); // transición momento 1 → momento 2
    }

    void Start()
    {
        secPerBeat = 60.0 / bpm;
        secPerBar = secPerBeat * beatsPerBar;

        moment1.loop = true;
        moment2.loop = true;
        moment3.loop = true;
        ambientLoop.loop = true;

        moment1.Play();
        moment2.Play();
        moment3.Play();
        ambientLoop.Play();

        mixer.SetFloat(moment1Param, 0f);
        mixer.SetFloat(moment2Param, -80f);
        mixer.SetFloat(moment3Param, -80f);

        Debug.Log($"[MusicManager] Started playback. BPM={bpm}, BeatsPerBar={beatsPerBar}, SecPerBar={secPerBar:F3}");
    }

    // 🔹 transición momento 1 → momento 2
    public void TriggerTransition12()
    {
        double dspTime = AudioSettings.dspTime;
        double nextBar = Mathf.CeilToInt((float)(dspTime / secPerBar)) * secPerBar;

        Debug.Log($"[MusicManager] Scheduling transition 1→2 at next bar={nextBar:F3}");
        Invoke(nameof(StartCrossfade12), (float)(nextBar - dspTime));
    }

    private void StartCrossfade12()
    {
        Debug.Log("[MusicManager] Starting crossfade 1→2 now!");
        StartCoroutine(FadeRoutine12(moment1Param, moment2Param));
    }

    // 🔹 transición momento 2 → momento 3 con drum fill
    public void TriggerFillAndTransition()
    {
        StartCoroutine(PlayFillThenTransition());
    }

    private IEnumerator PlayFillThenTransition()
    {
        // Espera hasta el beat 3
        yield return WaitForBeat(3);

        // Reproduce el fill encima de momento 2
        drumFillSource.Play();
        Debug.Log("[MusicManager] Drum fill iniciado en beat 3");

        // Espera la duración del fill (5 beats)
        yield return WaitForBeats(5);

        // Ahora dispara el crossfade hacia momento 3
        StartCoroutine(FadeRoutine23(moment2Param, moment3Param));
        Debug.Log("[MusicManager] Transición 2→3 iniciada tras drum fill");
    }

    // 🔹 Corrutina para 1→2
    private IEnumerator FadeRoutine12(string fadeOutParam, string fadeInParam)
    {
        float elapsedOut = 0f;
        float elapsedIn = 0f;

        mixer.SetFloat(fadeOutParam, 0f);
        mixer.SetFloat(fadeInParam, -80f);

        while (elapsedOut < fadeOutDuration12 || elapsedIn < fadeInDuration12)
        {
            if (elapsedOut < fadeOutDuration12)
            {
                elapsedOut += Time.deltaTime;
                float tOut = elapsedOut / fadeOutDuration12;
                float volOut = Mathf.Lerp(0f, -80f, tOut);
                mixer.SetFloat(fadeOutParam, volOut);
            }

            if (elapsedIn < fadeInDuration12)
            {
                elapsedIn += Time.deltaTime;
                float tIn = elapsedIn / fadeInDuration12;
                float volIn = Mathf.Lerp(-80f, 0f, tIn);
                mixer.SetFloat(fadeInParam, volIn);
            }

            yield return null;
        }

        mixer.SetFloat(fadeOutParam, -80f);
        mixer.SetFloat(fadeInParam, 0f);

        Debug.Log("[MusicManager] Crossfade 1→2 complete.");
    }

    // 🔹 Corrutina para 2→3
    private IEnumerator FadeRoutine23(string fadeOutParam, string fadeInParam)
    {
        float elapsedOut = 0f;
        float elapsedIn = 0f;

        mixer.SetFloat(fadeOutParam, 0f);
        mixer.SetFloat(fadeInParam, -80f);

        while (elapsedOut < fadeOutDuration23 || elapsedIn < fadeInDuration23)
        {
            if (elapsedOut < fadeOutDuration23)
            {
                elapsedOut += Time.deltaTime;
                float tOut = elapsedOut / fadeOutDuration23;
                float volOut = Mathf.Lerp(0f, -80f, tOut);
                mixer.SetFloat(fadeOutParam, volOut);
            }

            if (elapsedIn < fadeInDuration23)
            {
                elapsedIn += Time.deltaTime;
                float tIn = elapsedIn / fadeInDuration23;
                float volIn = Mathf.Lerp(-80f, 0f, tIn);
                mixer.SetFloat(fadeInParam, volIn);
            }

            yield return null;
        }

        mixer.SetFloat(fadeOutParam, -80f);
        mixer.SetFloat(fadeInParam, 0f);

        Debug.Log("[MusicManager] Crossfade 2→3 complete.");
    }

    // 🔹 Corrutinas de sincronización
    IEnumerator WaitForBeat(int beatNumber)
    {
        float waitTime = (float)(secPerBeat * (beatNumber - 1));
        yield return new WaitForSeconds(waitTime);
    }

    IEnumerator WaitForBeats(int beats)
    {
        float waitTime = (float)(secPerBeat * beats);
        yield return new WaitForSeconds(waitTime);
    }

    void Update()
    {
        // 🔹 Prueba rápida con tecla 3
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TriggerFillAndTransition();
        }
    }
}