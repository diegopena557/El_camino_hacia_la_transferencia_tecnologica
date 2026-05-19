using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;

public class TimerTextNode11 : MonoBehaviour
{
    private bool flagJustOneTime;

    public GameObject GO_Node11_1;
    public GameObject GO_Node11_3;
    public GameObject GO_Node11_5;

    public GameObject GO_CanvaNode11_1;
    public GameObject GO_CanvaNode11_3;
    public GameObject GO_CanvaNode11_5;

    public VideoPlayer videoPlayerNode11_1;
    public VideoPlayer videoPlayerNode11_3;
    public VideoPlayer videoPlayerNode11_5;

    public TextMeshProUGUI myTextNode11_3_1;
    public TextMeshProUGUI myTextNode11_3_2;
    public TextMeshProUGUI myTextNode11_3_3;
    public TextMeshProUGUI myTextNode11_3_4;
    public TextMeshProUGUI myTextNode11_3_5;

    public TextMeshProUGUI myTextNode11_5_1;
    public TextMeshProUGUI myTextNode11_5_2;
    public TextMeshProUGUI myTextNode11_5_3;
    public TextMeshProUGUI myTextNode11_5_4;
    public TextMeshProUGUI myTextNode11_5_5;

    public TypewriterTMP textNode11_3_1;
    public TypewriterTMP textNode11_3_2;
    public TypewriterTMP textNode11_3_3;
    public TypewriterTMP textNode11_3_4;
    public TypewriterTMP textNode11_3_5;

    public TypewriterTMP textNode11_5_1;
    public TypewriterTMP textNode11_5_2;
    public TypewriterTMP textNode11_5_3;
    public TypewriterTMP textNode11_5_4;
    public TypewriterTMP textNode11_5_5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(flagJustOneTime == false){
            StartCoroutine(AdvancingTimerNode11_1());
            flagJustOneTime = true;
        }
    }

    IEnumerator AdvancingTimerNode11_1()
    {
        GO_Node11_1.SetActive(true);
        GO_CanvaNode11_1.SetActive(true);

        videoPlayerNode11_1.Stop();
        videoPlayerNode11_1.time = 0;        
        videoPlayerNode11_1.frame = 0;
        videoPlayerNode11_1.Play();

        yield return new WaitForSeconds(10f);
        StartCoroutine(AdvancingTimerNode11_3());
    }

    IEnumerator AdvancingTimerNode11_3()
    {
        GO_Node11_3.SetActive(true);
        GO_CanvaNode11_3.SetActive(true);

        GO_Node11_1.SetActive(false);
        GO_CanvaNode11_1.SetActive(false);

        videoPlayerNode11_1.Stop();

        videoPlayerNode11_3.Stop();
        videoPlayerNode11_3.time = 0;        
        videoPlayerNode11_3.frame = 0;
        videoPlayerNode11_3.Play();

        yield return new WaitForSeconds(1f);
        myTextNode11_3_1.gameObject.SetActive(true);
        textNode11_3_1.StartTyping();
        
        yield return new WaitForSeconds(4f);
        myTextNode11_3_1.gameObject.SetActive(false);
        myTextNode11_3_2.gameObject.SetActive(true);
        textNode11_3_2.StartTyping();
        
        yield return new WaitForSeconds(5f);
        myTextNode11_3_2.gameObject.SetActive(false);
        myTextNode11_3_3.gameObject.SetActive(true);
        textNode11_3_3.StartTyping();

        yield return new WaitForSeconds(5f);
        myTextNode11_3_3.gameObject.SetActive(false);
        myTextNode11_3_4.gameObject.SetActive(true);
        textNode11_3_4.StartTyping();

        yield return new WaitForSeconds(5f);
        myTextNode11_3_4.gameObject.SetActive(false);
        myTextNode11_3_5.gameObject.SetActive(true);
        textNode11_3_5.StartTyping();

        yield return new WaitForSeconds(5f);
        StartCoroutine(AdvancingTimerNode11_5());
    }

    IEnumerator AdvancingTimerNode11_5()
    {
        GO_Node11_5.SetActive(true);
        GO_CanvaNode11_5.SetActive(true);

        GO_Node11_3.SetActive(false);
        GO_CanvaNode11_3.SetActive(false);

        videoPlayerNode11_3.Stop();

        videoPlayerNode11_5.Stop();
        videoPlayerNode11_5.time = 0;        
        videoPlayerNode11_5.frame = 0;
        videoPlayerNode11_5.Play();

        yield return new WaitForSeconds(1f);
        myTextNode11_5_1.gameObject.SetActive(true);
        textNode11_5_1.StartTyping();
        
        yield return new WaitForSeconds(4f);
        myTextNode11_5_1.gameObject.SetActive(false);
        myTextNode11_5_2.gameObject.SetActive(true);
        textNode11_5_2.StartTyping();
        
        yield return new WaitForSeconds(5f);
        myTextNode11_5_2.gameObject.SetActive(false);
        myTextNode11_5_3.gameObject.SetActive(true);
        textNode11_5_3.StartTyping();

        yield return new WaitForSeconds(5f);
        myTextNode11_5_3.gameObject.SetActive(false);
        myTextNode11_5_4.gameObject.SetActive(true);
        textNode11_5_4.StartTyping();

        yield return new WaitForSeconds(5f);
        myTextNode11_5_4.gameObject.SetActive(false);
        myTextNode11_5_5.gameObject.SetActive(true);
        textNode11_5_5.StartTyping();

        yield return new WaitForSeconds(5f);
    }
}
