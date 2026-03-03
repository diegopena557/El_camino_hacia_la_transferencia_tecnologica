using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class MusicManager2 : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource moment1;
    public AudioSource moment2;
    public AudioSource ambientLoop;

    [Header("Mixer & Parameters")]
    public AudioMixer mixer;
    public string moment1Param = "VolumeLead1";
    public string moment2Param = "VolumeLead2";
    public string ambientParam = "VolumeBass";

    [Header("Fade Durations")]
    public float fadeOutDuration12 = 3f;
    public float fadeInDuration12 = 0.5f;
    public float fadeOutDuration21 = 3f;
    public float fadeInDuration21 = 0.5f;

    public GameModeManager gameModeManager;
    private int lastMode = 1; // arrancamos en momento1

    void Start()
    {
        moment1.loop = true;
        moment2.loop = true;
        ambientLoop.loop = true;

        moment1.Play();
        moment2.Play();
        ambientLoop.Play();

        mixer.SetFloat(moment1Param, 0f);     // momento 1 activo
        mixer.SetFloat(moment2Param, -80f);   // momento 2 silenciado
        mixer.SetFloat(ambientParam, 0f);     // ambient activo
    }

    // 🔹 Transición momento 1 → momento 2
    public void TriggerTransition12()
    {
        StartCoroutine(FadeRoutine(moment1Param, moment2Param, fadeOutDuration12, fadeInDuration12));
    }

    // 🔹 Transición momento 2 → momento 1
    public void TriggerTransition21()
    {
        StartCoroutine(FadeRoutine(moment2Param, moment1Param, fadeOutDuration21, fadeInDuration21));
    }

    private IEnumerator FadeRoutine(string fadeOutParam, string fadeInParam, float fadeOutDur, float fadeInDur)
    {
        float elapsedOut = 0f;
        float elapsedIn = 0f;

        mixer.SetFloat(fadeOutParam, 0f);
        mixer.SetFloat(fadeInParam, -80f);

        while (elapsedOut < fadeOutDur || elapsedIn < fadeInDur)
        {
            if (elapsedOut < fadeOutDur)
            {
                elapsedOut += Time.deltaTime;
                mixer.SetFloat(fadeOutParam, Mathf.Lerp(0f, -80f, elapsedOut / fadeOutDur));
            }

            if (elapsedIn < fadeInDur)
            {
                elapsedIn += Time.deltaTime;
                mixer.SetFloat(fadeInParam, Mathf.Lerp(-80f, 0f, elapsedIn / fadeInDur));
            }

            yield return null;
        }

        mixer.SetFloat(fadeOutParam, -80f);
        mixer.SetFloat(fadeInParam, 0f);
    }

    private void Update()
    {
        if (gameModeManager != null)
        {
            int currentMode = gameModeManager.GetCurrentMode();

            if (lastMode != currentMode)
            {
                Debug.Log($"[MusicManager] Cambio de modo detectado: {lastMode} → {currentMode}");

                if (lastMode == 1 && currentMode == 2)
                    TriggerTransition12();
                else if (lastMode == 2 && currentMode == 1)
                    TriggerTransition21();

                lastMode = currentMode;
            }
        }
    }
}