using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource moment1;
    public AudioSource moment2;
    public AudioSource ambientLoop; // third source

    [Header("Mixer & Parameters")]
    public AudioMixer mixer;
    public string moment1Param = "VolumeLead1";
    public string moment2Param = "VolumeLead2";

    [Header("Music Settings")]
    public double bpm = 120.0;
    public int beatsPerBar = 4;

    [Header("Fade Durations")]
    public float fadeOutDuration = 1.5f; // seconds for outgoing track
    public float fadeInDuration = 2.5f;  // seconds for incoming track

    private double secPerBeat;
    private double secPerBar;

    void Start()
    {
        secPerBeat = 60.0 / bpm;
        secPerBar = secPerBeat * beatsPerBar;

        // Start all sources immediately
        moment1.loop = true;
        moment2.loop = true;
        ambientLoop.loop = true;

        moment1.Play();
        moment2.Play();
        ambientLoop.Play();

        // Initial volumes: moment1 audible, moment2 muted
        mixer.SetFloat(moment1Param, 0f);
        mixer.SetFloat(moment2Param, -80f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TriggerTransition();
        }
    }

    public void TriggerTransition()
    {
        double dspTime = AudioSettings.dspTime;
        double nextBar = Mathf.CeilToInt((float)(dspTime / secPerBar)) * secPerBar;

        Invoke(nameof(StartCrossfade), (float)(nextBar - dspTime));
    }

    private void StartCrossfade()
    {
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
            }

            if (elapsedIn < fadeInDuration)
            {
                elapsedIn += Time.deltaTime;
                float tIn = elapsedIn / fadeInDuration;
                float vol2 = Mathf.Lerp(-80f, 0f, tIn);
                mixer.SetFloat(moment2Param, vol2);
            }

            yield return null;
        }

        // Ensure final values
        mixer.SetFloat(moment1Param, -80f);
        mixer.SetFloat(moment2Param, 0f);
    }
}