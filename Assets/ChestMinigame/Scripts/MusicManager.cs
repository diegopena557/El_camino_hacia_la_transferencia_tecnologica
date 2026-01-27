using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource menuMusic;       // 🔹 Música de menú
    public AudioSource moment1;         // 🔹 Primer momento de gameplay
    public AudioSource moment2;         // 🔹 Segundo momento
    public AudioSource moment3;         // 🔹 Tercer momento
    public AudioSource ambientLoop;     // 🔹 Ambient loop (bajo/fondo)
    public AudioSource drumFillSource;  // 🔹 Drum fill para transición 2→3

    [Header("Mixer & Parameters")]
    public AudioMixer mixer;
    public string menuParam = "VolumeMenu";          // Param del menú
    public string moment1Param = "VolumeLead1";      // Param del momento 1
    public string moment2Param = "VolumeLead2";      // Param del momento 2
    public string moment3Param = "VolumeLead3";      // Param del momento 3
    public string ambientParam = "VolumeBass";       // Param del ambient loop

    [Header("Music Settings")]
    public double bpm = 120.0;   // Tempo en beats por minuto
    public int beatsPerBar = 4;  // Compás (ej: 4/4)

    [Header("Fade Durations 1→2")]
    public float fadeOutDuration12 = 3f;   // Tiempo de salida de momento 1
    public float fadeInDuration12 = 0.5f;  // Tiempo de entrada de momento 2

    [Header("Fade Durations 2→3")]
    public float fadeOutDuration23 = 2f;   // Tiempo de salida de momento 2
    public float fadeInDuration23 = 1f;    // Tiempo de entrada de momento 3

    [Header("Fade Durations Menu → Gameplay")]
    public float fadeOutMenuDuration = 2f;       // Tiempo de salida del menú
    public float fadeInMoment1Duration = 1f;     // Tiempo de entrada de momento 1
    public float fadeInAmbientDuration = 1.5f;   // Tiempo de entrada del ambient loop

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
        moment3.loop = true;
        ambientLoop.loop = true;

        // Arrancar música (todo en silencio excepto menú)
        menuMusic.Play();
        moment1.Play();
        moment2.Play();
        moment3.Play();
        ambientLoop.Play();


        // Volúmenes iniciales
        mixer.SetFloat(menuParam, 0f);          // menú activo
        mixer.SetFloat(moment1Param, -80f);     // momento 1 silenciado
        mixer.SetFloat(moment2Param, -80f);     // momento 2 silenciado
        mixer.SetFloat(moment3Param, -80f);     // momento 3 silenciado
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
        yield return WaitForBeat(3);

        drumFillSource.Play();
        Debug.Log("[MusicManager] Drum fill iniciado en beat 3");

        yield return WaitForBeats(5);

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
    // 🔹 Corrutina para 2→3 (con fades independientes)
    private IEnumerator FadeRoutine23(string fadeOutParam, string fadeInParam)
    {
        float elapsedOut = 0f;  // tiempo acumulado del fade out (momento 2)
        float elapsedIn = 0f;   // tiempo acumulado del fade in (momento 3)

        // Estado inicial de los parámetros en el mixer
        mixer.SetFloat(fadeOutParam, 0f);    // momento 2 audible
        mixer.SetFloat(fadeInParam, -80f);   // momento 3 silenciado

        // Bucle de crossfade: avanza ambos tiempos de forma independiente
        while (elapsedOut < fadeOutDuration23 || elapsedIn < fadeInDuration23)
        {
            // Fade out del momento 2
            if (elapsedOut < fadeOutDuration23)
            {
                elapsedOut += Time.deltaTime;
                float tOut = elapsedOut / fadeOutDuration23;
                float volOut = Mathf.Lerp(0f, -80f, tOut);
                mixer.SetFloat(fadeOutParam, volOut);
            }

            // Fade in del momento 3
            if (elapsedIn < fadeInDuration23)
            {
                elapsedIn += Time.deltaTime;
                float tIn = elapsedIn / fadeInDuration23;
                float volIn = Mathf.Lerp(-80f, 0f, tIn);
                mixer.SetFloat(fadeInParam, volIn);
            }

            // 🔹 Este yield mantiene la corrutina viva y evita CS0161
            yield return null;
        }

        // Asegurar estados finales exactos
        mixer.SetFloat(fadeOutParam, -80f);  // momento 2 completamente fuera
        mixer.SetFloat(fadeInParam, 0f);     // momento 3 completamente dentro

        Debug.Log("[MusicManager] Crossfade 2→3 complete.");
    }
    // 🔹 Corrutinas de sincronización
    IEnumerator WaitForBeat(int beatNumber)
    {
        // Espera hasta el beat indicado dentro del compás
        float waitTime = (float)(secPerBeat * (beatNumber - 1));
        yield return new WaitForSeconds(waitTime);
    }

    IEnumerator WaitForBeats(int beats)
    {
        // Espera un número de beats completo
        float waitTime = (float)(secPerBeat * beats);
        yield return new WaitForSeconds(waitTime);
    }

    void Update()
    {
        // 🔹 Detectar cambio de estado en CardSpawner (mantener transiciones)
        if (spawner != null)
        {
            if (spawner.IsHardMode() && !lastHardModeState)
            {
                Debug.Log("[MusicManager] Detectado cambio a Hard Mode → disparando transición 1→2");
                TriggerTransition12();
            }
            lastHardModeState = spawner.IsHardMode();
        }

        // 🔹 Prueba rápida con tecla 3 → dispara transición 2→3 con drum fill
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TriggerFillAndTransition();
        }
    }
}