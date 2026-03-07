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
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*videoPlayerNode0a.Stop();
        videoPlayerNode0a.time = 0;        
        videoPlayerNode0a.frame = 0;
        videoPlayerNode0a.Play();*/
    }

    // Update is called once per frame
    void Update()
    {
        if(flagJustOneTime == false){
            StartCoroutine(AdvancingTimerNode0());
            flagJustOneTime = true;
        }
    }

    public void ChangeNode0To1(){
        GO_Node0.SetActive(false);
        GO_Node1.SetActive(true);
        timerNode1.StartTimerNode1();
    }

    IEnumerator AdvancingTimerNode0()
    {
        videoPlayerNode0a.Stop();
        videoPlayerNode0a.time = 0;        
        videoPlayerNode0a.frame = 0;
        videoPlayerNode0a.Play();

        yield return new WaitForSeconds(4f);
        myText.gameObject.SetActive(true);
        DN0_0.Play();
        textInicio.StartTyping();
        yield return new WaitForSeconds(10.8f);
        Cinematic1.Play(); //musica cinematica
        yield return new WaitForSeconds(2.1f);
        //GO_CanvaNode0a.SetActive(false);
        //GO_CanvaNode0b.SetActive(true)

        canvasFader.FadeAToB();
        videoPlayerNode0b.Stop();
        videoPlayerNode0b.time = 0;        
        videoPlayerNode0b.frame = 0;
        
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
