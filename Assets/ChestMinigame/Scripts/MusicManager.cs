using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource menuMusic;       // 🔹 Música de menú
    public AudioSource moment1;         // 🔹 Primer momento
    public AudioSource moment2;         // 🔹 Segundo momento
    public AudioSource ambientLoop;     // 🔹 Ambient loop (bajo/fondo)

    [Header("Mixer & Parameters")]
    public AudioMixer mixer;
    public string menuParam = "VolumeMenu";          // Param del menú
    public string moment1Param = "VolumeLead1";      // Param del momento 1
    public string moment2Param = "VolumeLead2";      // Param del momento 2
    public string ambientParam = "VolumeBass";       // Param del ambient loop

    [Header("Music Settings")]
    public double bpm = 120.0;   // Tempo en beats por minuto
    public int beatsPerBar = 4;  // Compás (ej: 4/4)

    [Header("Fade Durations Menu → Gameplay")]
    public float fadeOutMenuDuration = 2f;       // Tiempo de salida del menú
    public float fadeInMoment1Duration = 1f;     // Tiempo de entrada de momento 1
    public float fadeInAmbientDuration = 1.5f;   // Tiempo de entrada del ambient loop

    [Header("Fade Durations 1→2")]
    public float fadeOutDuration12 = 3f;   // Tiempo de salida de momento 1
    public float fadeInDuration12 = 0.5f;  // Tiempo de entrada de momento 2

    private double secPerBeat;
    private double secPerBar;

    public CardSpawner spawner;
    private bool lastHardModeState = false;

    void Start()
    {
        // Calcular tiempos en segundos
        secPerBeat = 60.0 / bpm;
        secPerBar = secPerBeat * beatsPerBar;

        // Configurar loops
        menuMusic.loop = true;
        moment1.loop = true;
        moment2.loop = true;
        ambientLoop.loop = true;

        // Arrancar música solo menú (todo en silencio excepto menú)
        menuMusic.Play();
        


        // Volúmenes iniciales
        mixer.SetFloat(menuParam, 0f);          // menú activo
        mixer.SetFloat(moment1Param, -80f);     // momento 1 silenciado
        mixer.SetFloat(moment2Param, -80f);     // momento 2 silenciado
        mixer.SetFloat(ambientParam, -80f);     // ambient silenciado

        Debug.Log($"[MusicManager] Started playback. BPM={bpm}, BeatsPerBar={beatsPerBar}, SecPerBar={secPerBar:F3}");
    }

    // 🔹 Señal desde IntroManager para arrancar gameplay
    public void StartGameplayMusic()
    {
        Debug.Log("[MusicManager] Gameplay start signal received → switching from menu to gameplay music.");
        StopMenuMusic();
    }

    // 🔹 Apagar música de menú y activar momento 1 + ambient
    private void StopMenuMusic()
    {
        StartCoroutine(FadeOutMenu());
    }

    // 🔹 Fade out del menú + fade in de momento 1 y ambient loop
    private IEnumerator FadeOutMenu()
    {
        // AArranca la música
        moment1.Play();
        moment2.Play();
        ambientLoop.Play();

        float elapsed = 0f;

        // Estado inicial de cada pista
        mixer.SetFloat(menuParam, 0f);          // menú activo
        mixer.SetFloat(moment1Param, -80f);     // momento 1 silenciado
        mixer.SetFloat(ambientParam, -80f);     // ambient silenciado

        // Duraciones independientes
        float durOut = fadeOutMenuDuration;
        float durInMoment1 = fadeInMoment1Duration;
        float durInAmbient = fadeInAmbientDuration;

        // Bucle hasta que todos los fades terminen
        while (elapsed < Mathf.Max(durOut, durInMoment1, durInAmbient))
        {
            elapsed += Time.deltaTime;

            // Fade out menú
            if (elapsed < durOut)
            {
                float tOut = elapsed / durOut;
                float volOut = Mathf.Lerp(0f, -80f, tOut);
                mixer.SetFloat(menuParam, volOut);
            }
            else
            {
                mixer.SetFloat(menuParam, -80f);
                if (menuMusic.isPlaying) menuMusic.Stop();
            }

            // Fade in momento 1
            if (elapsed < durInMoment1)
            {
                float tIn1 = elapsed / durInMoment1;
                float volIn1 = Mathf.Lerp(-80f, 0f, tIn1);
                mixer.SetFloat(moment1Param, volIn1);
            }
            else
            {
                mixer.SetFloat(moment1Param, 0f);
            }

            // Fade in ambient
            if (elapsed < durInAmbient)
            {
                float tInAmb = elapsed / durInAmbient;
                float volInAmb = Mathf.Lerp(-80f, 0f, tInAmb);
                mixer.SetFloat(ambientParam, volInAmb);
            }
            else
            {
                mixer.SetFloat(ambientParam, 0f);
            }

            yield return null;
        }

        Debug.Log("[MusicManager] Menu faded out, Moment1 + Ambient faded in.");
    }


    //////////////////////////////////////////////////// 🔹 Transición momento 1 → momento 2 ////////////////////////////////////////

public void TriggerTransition12()
{
    double dspTime = AudioSettings.dspTime;
    int currentBar = Mathf.FloorToInt((float)(dspTime / secPerBar));
    int targetBar = currentBar + 1;
    double targetBarTime = targetBar * secPerBar;

    Debug.Log($"[MusicManager] Scheduling transition 1→2 at bar {targetBar}");
    StartCoroutine(FadeRoutine12AtDSP(moment1Param, moment2Param, targetBarTime));
}

private IEnumerator FadeRoutine12AtDSP(string fadeOutParam, string fadeInParam, double targetDSPTime)
{
    while (AudioSettings.dspTime < targetDSPTime)
        yield return null;

    StartCoroutine(FadeRoutine12(fadeOutParam, fadeInParam));
}

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
            mixer.SetFloat(fadeOutParam, Mathf.Lerp(0f, -80f, tOut));
        }

        if (elapsedIn < fadeInDuration12)
        {
            elapsedIn += Time.deltaTime;
            float tIn = elapsedIn / fadeInDuration12;
            mixer.SetFloat(fadeInParam, Mathf.Lerp(-80f, 0f, tIn));
        }

        yield return null;
    }

    mixer.SetFloat(fadeOutParam, -80f);
    mixer.SetFloat(fadeInParam, 0f);

    Debug.Log("[MusicManager] Crossfade 1→2 complete.");
}

//////////////////////////////// 🔹 Update para probar transiciones ////////////////
    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Debug.Log("[MusicManager] Botón 2 presionado → TriggerTransition12()");
            TriggerTransition12();
        }
////////////////////////////////////////// DETECTOR HARDMODE ///////////////////////////
        if (spawner != null)
        {
            bool currentHardMode = spawner.IsHardMode();

            // Detectar transición de fácil → difícil
            if (!lastHardModeState && currentHardMode)
            {
                Debug.Log("[MusicManager] Detectado cambio a HardMode → disparando transición 1→2");
                TriggerTransition12();
            }

            lastHardModeState = currentHardMode;
        }
    }
}