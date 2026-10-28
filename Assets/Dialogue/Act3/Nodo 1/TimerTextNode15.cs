using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;

public class TimerTextNode15 : MonoBehaviour
{
    private bool flagJustOneTime;

    public GameObject GO_CanvaNode15_1;
    public GameObject GO_CanvaNode15_2;
    public GameObject GO_CanvaNode15_3;
    public GameObject GO_CanvaNode15_4;
    public GameObject GO_CanvaNode15_5;

    public GameObject GO_Node15_1;
    public GameObject GO_Node15_2;
    public GameObject GO_Node15_3;
    public GameObject GO_Node15_4;
    public GameObject GO_Node15_5;

    public VideoPlayer videoPlayerNode15_1;
    public VideoPlayer videoPlayerNode15_2;
    public VideoPlayer videoPlayerNode15_3;
    public VideoPlayer videoPlayerNode15_4;
    public VideoPlayer videoPlayerNode15_5;

    public TextMeshProUGUI myTextNode15_5_1;
    public TextMeshProUGUI myTextNode15_5_2;
    public TextMeshProUGUI myTextNode15_5_3;
    public TextMeshProUGUI myTextNode15_5_4;
    public TextMeshProUGUI myTextNode15_5_5;
    public TextMeshProUGUI myTextNode15_5_6;
    public TextMeshProUGUI myTextNode15_5_7;
    public TextMeshProUGUI myTextNode15_5_8;

    public TypewriterTMP textNode15_5_1;
    public TypewriterTMP textNode15_5_2;
    public TypewriterTMP textNode15_5_3;
    public TypewriterTMP textNode15_5_4;
    public TypewriterTMP textNode15_5_5;
    public TypewriterTMP textNode15_5_6;
    public TypewriterTMP textNode15_5_7;
    public TypewriterTMP textNode15_5_8;

    //0 -- Des1
    //1 -- Des2
    //2 -- Des3
    public int flagDecisionChoosen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(flagJustOneTime == false){

            if(flagDecisionChoosen == 0){
                StartCoroutine(AdvancingTimerNode15_1());
            }

            if(flagDecisionChoosen == 1){
                StartCoroutine(AdvancingTimerNode15_2());
            }

            if(flagDecisionChoosen == 2){
                StartCoroutine(AdvancingTimerNode15_3());
            }
            flagJustOneTime = true;
        }
    }

    IEnumerator AdvancingTimerNode15_1()
    {
        GO_Node15_1.SetActive(true);
        GO_CanvaNode15_1.SetActive(true);

        videoPlayerNode15_1.Stop();
        videoPlayerNode15_1.time = 0;        
        videoPlayerNode15_1.frame = 0;
        videoPlayerNode15_1.Play();

        yield return new WaitForSeconds(10f);

        StartCoroutine(AdvancingTimerNode15_4());
    }

    IEnumerator AdvancingTimerNode15_2()
    {
        videoPlayerNode15_1.Stop();
        GO_Node15_1.SetActive(false);
        GO_Node15_2.SetActive(true);
        GO_CanvaNode15_2.SetActive(true);

        videoPlayerNode15_2.Stop();
        videoPlayerNode15_2.time = 0;        
        videoPlayerNode15_2.frame = 0;
        videoPlayerNode15_2.Play();

        yield return new WaitForSeconds(10f);

        StartCoroutine(AdvancingTimerNode15_4());
    }

    IEnumerator AdvancingTimerNode15_3()
    {
        videoPlayerNode15_2.Stop();
        GO_Node15_2.SetActive(false);
        GO_Node15_3.SetActive(true);
        GO_CanvaNode15_3.SetActive(true);

        videoPlayerNode15_3.Stop();
        videoPlayerNode15_3.time = 0;        
        videoPlayerNode15_3.frame = 0;
        videoPlayerNode15_3.Play();

        yield return new WaitForSeconds(10f);

        StartCoroutine(AdvancingTimerNode15_4());
    }

    IEnumerator AdvancingTimerNode15_4()
    {
        videoPlayerNode15_3.Stop();
        GO_Node15_3.SetActive(false);
        GO_Node15_4.SetActive(true);
        GO_CanvaNode15_4.SetActive(true);

        videoPlayerNode15_4.Stop();
        videoPlayerNode15_4.time = 0;        
        videoPlayerNode15_4.frame = 0;
        videoPlayerNode15_4.Play();

        yield return new WaitForSeconds(4f);

        StartCoroutine(AdvancingTimerNode15_5());
    }

    IEnumerator AdvancingTimerNode15_5()
    {
        videoPlayerNode15_4.Stop();
        GO_Node15_4.SetActive(false);
        GO_Node15_5.SetActive(true);
        GO_CanvaNode15_5.SetActive(true);

        videoPlayerNode15_5.Stop();
        videoPlayerNode15_5.time = 0;        
        videoPlayerNode15_5.frame = 0;
        videoPlayerNode15_5.Play();

        myTextNode15_5_1.gameObject.SetActive(true);
        textNode15_5_1.StartTyping();
        
        yield return new WaitForSeconds(4f);
        
        myTextNode15_5_1.gameObject.SetActive(false);
        myTextNode15_5_2.gameObject.SetActive(true);
        textNode15_5_2.StartTyping();
        
        yield return new WaitForSeconds(5f);
        
        myTextNode15_5_2.gameObject.SetActive(false);
        myTextNode15_5_3.gameObject.SetActive(true);
        textNode15_5_3.StartTyping();

        yield return new WaitForSeconds(5f);
        
        myTextNode15_5_3.gameObject.SetActive(false);
        myTextNode15_5_4.gameObject.SetActive(true);
        textNode15_5_4.StartTyping();

        yield return new WaitForSeconds(5f);
        
        myTextNode15_5_4.gameObject.SetActive(false);
        myTextNode15_5_5.gameObject.SetActive(true);
        textNode15_5_5.StartTyping();

        yield return new WaitForSeconds(5f);
        
        myTextNode15_5_5.gameObject.SetActive(false);
        myTextNode15_5_6.gameObject.SetActive(true);
        textNode15_5_6.StartTyping();

        yield return new WaitForSeconds(5f);
        
        myTextNode15_5_6.gameObject.SetActive(false);
        myTextNode15_5_7.gameObject.SetActive(true);
        textNode15_5_7.StartTyping();

        yield return new WaitForSeconds(5f);
        
        myTextNode15_5_7.gameObject.SetActive(false);
        myTextNode15_5_8.gameObject.SetActive(true);
        textNode15_5_8.StartTyping();

        yield return new WaitForSeconds(5f);
    }
}
