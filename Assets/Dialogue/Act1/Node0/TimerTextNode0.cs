using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;

public class TimerTextNode0 : MonoBehaviour
{
    private bool flagJustOneTime;
    public TextMeshProUGUI myText;

    public TextMeshProUGUI myTextNode0_1;
    public TextMeshProUGUI myTextNode0_2;
    public TextMeshProUGUI myTextNode0_3;

    public GameObject GO_CanvaNode0a;
    public GameObject GO_CanvaNode0b;

    public CanvasFader canvasFader;

    public VideoPlayer videoPlayerNode0a;
    public VideoPlayer videoPlayerNode0b;

    public TypewriterTMP textInicio;

    public TypewriterTMP textNode0_1;
    public TypewriterTMP textNode0_2;
    public TypewriterTMP textNode0_3;

    public Button myButtonSiguiente;

    public GameObject GO_Node0;
    public GameObject GO_Node1;

    public TimerNode1 timerNode1;

    //////////////////// AUDIOS ////////////////////////
    [Header("Audio Sources")]
    public AudioSource DN0_0;
    public AudioSource DN0_1;
    public AudioSource DN0_2;
    public AudioSource DN0_3;
    public AudioSource Cinematic1;
    public AudioSource Music;

    [Header("Button Feedback")]
    public AudioSource buttonAudioSource;
    public AudioClip buttonClickSound;

    void Start()
    {
        // Prepara los videos con anticipacion para que el primer cuadro
        // ya este decodificado cuando se necesiten (evita el flash azul).
        if (videoPlayerNode0a != null) videoPlayerNode0a.Prepare();
        if (videoPlayerNode0b != null) videoPlayerNode0b.Prepare();
    }

    void Update()
    {
        if (flagJustOneTime == false)
        {
            StartCoroutine(AdvancingTimerNode0());
            flagJustOneTime = true;
        }
    }

    public void ChangeNode0To1()
    {
        if (buttonAudioSource != null && buttonClickSound != null)
            buttonAudioSource.PlayOneShot(buttonClickSound);

        GO_Node0.SetActive(false);
        GO_Node1.SetActive(true);
        timerNode1.StartTimerNode1();
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

    IEnumerator AdvancingTimerNode0()
    {
        // Video A: se espera a que este preparado y se reproduce desde el inicio.
        yield return WaitUntilPrepared(videoPlayerNode0a);
        videoPlayerNode0a.Play();
        Music.Play(); // Inicio Musica de Fondo

        yield return new WaitForSeconds(4f);
        myText.gameObject.SetActive(true);
        DN0_0.Play();
        textInicio.StartTyping();
        yield return new WaitForSeconds(10.8f);
        Cinematic1.Play(); //musica cinematica
        yield return new WaitForSeconds(2.1f);
        //GO_CanvaNode0a.SetActive(false);
        //GO_CanvaNode0b.SetActive(true)

        // Video B: se asegura de que tenga cuadro listo ANTES de iniciar el fundido.
        yield return WaitUntilPrepared(videoPlayerNode0b);
        canvasFader.FadeAToB();

        Music.Stop(); // Fin musica de Fondo
        videoPlayerNode0b.Play();

        yield return new WaitForSeconds(9f);
        myTextNode0_1.gameObject.SetActive(true);
        textNode0_1.StartTyping();
        DN0_1.Play(); //dialogo
        yield return new WaitForSeconds(3f);
        videoPlayerNode0b.playbackSpeed = 1f;
        myTextNode0_1.gameObject.SetActive(false);
        myTextNode0_2.gameObject.SetActive(true);
        textNode0_2.StartTyping();
        DN0_2.Play(); //dialogo
        yield return new WaitForSeconds(5f);
        myTextNode0_2.gameObject.SetActive(false);
        myTextNode0_3.gameObject.SetActive(true);
        textNode0_3.StartTyping();
        DN0_3.Play(); //dialogo
        yield return new WaitForSeconds(5f);
        myTextNode0_3.gameObject.SetActive(false);
        videoPlayerNode0b.playbackSpeed = 1.0f;

        yield return new WaitForSeconds(4f);
        myButtonSiguiente.gameObject.SetActive(true);
    }
}