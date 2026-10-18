using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections.Generic;

public class TimerNode2 : MonoBehaviour
{
    private bool flagJustOneTime;

    public GameObject GO_CanvaNode2a;

    public VideoPlayer videoPlayerNode2a;

    public TextMeshProUGUI myTextNode2a_1;
    public TextMeshProUGUI myTextNode2a_2;

    public TypewriterTMP textNode2a_1;
    public TypewriterTMP textNode2a_2;

    public CanvasFader canvaFader;
    public GameObject GO_Node3;

    //////////////////// FADE IN ////////////////////////
    [Header("Fade In Settings")]
    public Image fadePanel;          // Panel negro (Image) que cubre toda la pantalla
    public float fadeInDuration = 1.5f;

    //////////////////// AUDIOS ////////////////////////
    /// Dialogos ///
    [Header("Audio Sources")]
    public AudioSource[] sources;

    private Dictionary<string, AudioSource> audioDict;

    void Awake()
    {
        audioDict = new Dictionary<string, AudioSource>();
        foreach (AudioSource src in sources)
        {
            if (src != null)
                audioDict[src.gameObject.name] = src;
        }

        // Asegura que el panel empiece completamente opaco (negro)
        if (fadePanel != null)
        {
            Color c = fadePanel.color;
            c.a = 1f;
            fadePanel.color = c;
            fadePanel.gameObject.SetActive(true);
        }
    }

    public void PlayByName(string name)
    {
        if (audioDict.ContainsKey(name))
            audioDict[name].Play();
        else
            Debug.LogWarning("No AudioSource found with name: " + name);
    }

    public void StopByName(string name)
    {
        if (audioDict.ContainsKey(name))
            audioDict[name].Stop();
    }

    void Start()
    {
        // Prepara el video con anticipacion para que el primer cuadro
        // ya este decodificado cuando se necesite (evita el flash azul).
        if (videoPlayerNode2a != null)
            videoPlayerNode2a.Prepare();
    }

    void Update()
    {
        if (flagJustOneTime == false)
        {
            StartCoroutine(AdvancingTimerNode2());
            flagJustOneTime = true;
        }
    }

    // Espera a que el video tenga su primer cuadro listo.
    private IEnumerator WaitUntilPrepared(VideoPlayer vp)
    {
        if (vp == null) yield break;

        // Por si aun no se habia llamado a Prepare()
        if (!vp.isPrepared && !vp.isPlaying)
            vp.Prepare();

        while (!vp.isPrepared)
            yield return null;
    }

    IEnumerator AdvancingTimerNode2()
    {
        // Espera a que el video este listo. Mientras tanto el panel negro
        // sigue opaco, asi que no se ve nada raro en pantalla.
        yield return WaitUntilPrepared(videoPlayerNode2a);

        // Inicia el video antes del fade para que ya este corriendo debajo
        videoPlayerNode2a.playbackSpeed = 1.0f;
        videoPlayerNode2a.Play();

        // Fade in: el panel pasa de opaco a transparente
        if (fadePanel != null)
            yield return StartCoroutine(FadeInPanel());

        myTextNode2a_1.gameObject.SetActive(true);
        textNode2a_1.StartTyping();
        PlayByName("2a_1");

        yield return new WaitForSeconds(8f);

        myTextNode2a_1.gameObject.SetActive(false);
        myTextNode2a_2.gameObject.SetActive(true);
        textNode2a_2.StartTyping();
        PlayByName("2a_2");

        yield return new WaitForSeconds(3f);

        GO_Node3.SetActive(true);
        canvaFader.FadeAToB();
    }

    IEnumerator FadeInPanel()
    {
        float elapsed = 0f;
        Color c = fadePanel.color;

        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, elapsed / fadeInDuration);
            fadePanel.color = c;
            yield return null;
        }

        // Asegura alpha final exacto y desactiva el panel
        c.a = 0f;
        fadePanel.color = c;
        fadePanel.gameObject.SetActive(false);
    }
}