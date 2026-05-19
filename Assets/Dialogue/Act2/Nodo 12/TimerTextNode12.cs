using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;

public class TimerTextNode12 : MonoBehaviour
{
    private bool flagJustOneTime;

    public GameObject GO_CanvaNode12_1;
    public GameObject GO_CanvaNode12_2;
    public GameObject GO_CanvaNode12_3;
    public GameObject GO_CanvaNode12_4;
    public GameObject GO_CanvaNode12_5;
    public GameObject GO_CanvaNode12_6;

    public GameObject GO_Node12_1;
    public GameObject GO_Node12_2;
    public GameObject GO_Node12_3;
    public GameObject GO_Node12_4;
    public GameObject GO_Node12_5;
    public GameObject GO_Node12_6;

    public VideoPlayer videoPlayerNode12_1;
    public VideoPlayer videoPlayerNode12_2;
    public VideoPlayer videoPlayerNode12_3;
    public VideoPlayer videoPlayerNode12_4;
    public VideoPlayer videoPlayerNode12_5;
    public VideoPlayer videoPlayerNode12_6;

    public TextMeshProUGUI myTextNode12_1_1;
    public TextMeshProUGUI myTextNode12_1_2;
    public TextMeshProUGUI myTextNode12_1_3;
    public TextMeshProUGUI myTextNode12_1_4;

    public TypewriterTMP textNode12_1_1;
    public TypewriterTMP textNode12_1_2;
    public TypewriterTMP textNode12_1_3;
    public TypewriterTMP textNode12_1_4;

    public TextMeshProUGUI myTextNode12_3_1;
    public TextMeshProUGUI myTextNode12_3_2;
    public TextMeshProUGUI myTextNode12_3_3;
    public TextMeshProUGUI myTextNode12_3_4;

    public TypewriterTMP textNode12_3_1;
    public TypewriterTMP textNode12_3_2;
    public TypewriterTMP textNode12_3_3;
    public TypewriterTMP textNode12_3_4;

    public TextMeshProUGUI myTextNode12_4_1;
    public TextMeshProUGUI myTextNode12_4_2;
    public TextMeshProUGUI myTextNode12_4_3;
    public TextMeshProUGUI myTextNode12_4_4;

    public TypewriterTMP textNode12_4_1;
    public TypewriterTMP textNode12_4_2;
    public TypewriterTMP textNode12_4_3;
    public TypewriterTMP textNode12_4_4;

    public TextMeshProUGUI myTextNode12_5_1;
    public TextMeshProUGUI myTextNode12_5_2;
    public TextMeshProUGUI myTextNode12_5_3;
    public TextMeshProUGUI myTextNode12_5_4;

    public TypewriterTMP textNode12_5_1;
    public TypewriterTMP textNode12_5_2;
    public TypewriterTMP textNode12_5_3;
    public TypewriterTMP textNode12_5_4;

    public TextMeshProUGUI myTextNode12_6_1;
    public TextMeshProUGUI myTextNode12_6_2;
    public TextMeshProUGUI myTextNode12_6_3;

    public TypewriterTMP textNode12_6_1;
    public TypewriterTMP textNode12_6_2;
    public TypewriterTMP textNode12_6_3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(flagJustOneTime == false){
            StartCoroutine(AdvancingTimerNode12_1());
            flagJustOneTime = true;
        }
    }

    IEnumerator AdvancingTimerNode12_1()
    {
        GO_Node12_1.SetActive(true);
        GO_CanvaNode12_1.SetActive(true);

        videoPlayerNode12_1.Stop();
        videoPlayerNode12_1.time = 0;        
        videoPlayerNode12_1.frame = 0;
        videoPlayerNode12_1.Play();

        yield return new WaitForSeconds(1f);
        myTextNode12_1_1.gameObject.SetActive(true);
        textNode12_1_1.StartTyping();
        
        yield return new WaitForSeconds(4f);
        myTextNode12_1_1.gameObject.SetActive(false);
        myTextNode12_1_2.gameObject.SetActive(true);
        textNode12_1_2.StartTyping();
        
        yield return new WaitForSeconds(5f);
        myTextNode12_1_2.gameObject.SetActive(false);
        myTextNode12_1_3.gameObject.SetActive(true);
        textNode12_1_3.StartTyping();

        yield return new WaitForSeconds(5f);
        myTextNode12_1_3.gameObject.SetActive(false);
        myTextNode12_1_4.gameObject.SetActive(true);
        textNode12_1_4.StartTyping();

        yield return new WaitForSeconds(4f);
        StartCoroutine(AdvancingTimerNode12_2());
    }

    IEnumerator AdvancingTimerNode12_2()
    {
        GO_Node12_2.SetActive(true);
        GO_CanvaNode12_2.SetActive(true);

        GO_Node12_1.SetActive(false);
        GO_CanvaNode12_1.SetActive(false);

        videoPlayerNode12_1.Stop();

        videoPlayerNode12_2.Stop();
        videoPlayerNode12_2.time = 0;        
        videoPlayerNode12_2.frame = 0;
        videoPlayerNode12_2.Play();

        yield return new WaitForSeconds(10f);
        StartCoroutine(AdvancingTimerNode12_3());
    }

    IEnumerator AdvancingTimerNode12_3()
    {
        GO_Node12_3.SetActive(true);
        GO_CanvaNode12_3.SetActive(true);

        GO_Node12_2.SetActive(false);
        GO_CanvaNode12_2.SetActive(false);

        videoPlayerNode12_2.Stop();

        videoPlayerNode12_3.Stop();
        videoPlayerNode12_3.time = 0;        
        videoPlayerNode12_3.frame = 0;
        videoPlayerNode12_3.Play();

        yield return new WaitForSeconds(1f);
        myTextNode12_3_1.gameObject.SetActive(true);
        textNode12_3_1.StartTyping();
        
        yield return new WaitForSeconds(4f);
        myTextNode12_3_1.gameObject.SetActive(false);
        myTextNode12_3_2.gameObject.SetActive(true);
        textNode12_3_2.StartTyping();
        
        yield return new WaitForSeconds(5f);
        myTextNode12_3_2.gameObject.SetActive(false);
        myTextNode12_3_3.gameObject.SetActive(true);
        textNode12_3_3.StartTyping();

        yield return new WaitForSeconds(5f);
        myTextNode12_3_3.gameObject.SetActive(false);
        myTextNode12_3_4.gameObject.SetActive(true);
        textNode12_3_4.StartTyping();

        yield return new WaitForSeconds(4f);
        StartCoroutine(AdvancingTimerNode12_4());
    }


    IEnumerator AdvancingTimerNode12_4()
    {
        GO_Node12_4.SetActive(true);
        GO_CanvaNode12_4.SetActive(true);

        GO_Node12_3.SetActive(false);
        GO_CanvaNode12_3.SetActive(false);

        videoPlayerNode12_3.Stop();

        videoPlayerNode12_4.Stop();
        videoPlayerNode12_4.time = 0;        
        videoPlayerNode12_4.frame = 0;
        videoPlayerNode12_4.Play();

        yield return new WaitForSeconds(1f);
        myTextNode12_4_1.gameObject.SetActive(true);
        textNode12_4_1.StartTyping();
        
        yield return new WaitForSeconds(4f);
        myTextNode12_4_1.gameObject.SetActive(false);
        myTextNode12_4_2.gameObject.SetActive(true);
        textNode12_4_2.StartTyping();
        
        yield return new WaitForSeconds(5f);
        myTextNode12_4_2.gameObject.SetActive(false);
        myTextNode12_4_3.gameObject.SetActive(true);
        textNode12_4_3.StartTyping();

        yield return new WaitForSeconds(5f);
        myTextNode12_4_3.gameObject.SetActive(false);
        myTextNode12_4_4.gameObject.SetActive(true);
        textNode12_4_4.StartTyping();

        yield return new WaitForSeconds(4f);
        StartCoroutine(AdvancingTimerNode12_5());

    }

    IEnumerator AdvancingTimerNode12_5()
    {
        GO_Node12_5.SetActive(true);
        GO_CanvaNode12_5.SetActive(true);

        GO_Node12_4.SetActive(false);
        GO_CanvaNode12_4.SetActive(false);

        videoPlayerNode12_4.Stop();

        videoPlayerNode12_5.Stop();
        videoPlayerNode12_5.time = 0;        
        videoPlayerNode12_5.frame = 0;
        videoPlayerNode12_5.Play();

        yield return new WaitForSeconds(1f);
        myTextNode12_5_1.gameObject.SetActive(true);
        textNode12_5_1.StartTyping();
        
        yield return new WaitForSeconds(4f);
        myTextNode12_5_1.gameObject.SetActive(false);
        myTextNode12_5_2.gameObject.SetActive(true);
        textNode12_5_2.StartTyping();
        
        yield return new WaitForSeconds(5f);
        myTextNode12_5_2.gameObject.SetActive(false);
        myTextNode12_5_3.gameObject.SetActive(true);
        textNode12_5_3.StartTyping();

        yield return new WaitForSeconds(5f);
        myTextNode12_5_3.gameObject.SetActive(false);
        myTextNode12_5_4.gameObject.SetActive(true);
        textNode12_5_4.StartTyping();

        yield return new WaitForSeconds(4f);
        StartCoroutine(AdvancingTimerNode12_6());

    }

    IEnumerator AdvancingTimerNode12_6()
    {
        GO_Node12_6.SetActive(true);
        GO_CanvaNode12_6.SetActive(true);

        GO_Node12_5.SetActive(false);
        GO_CanvaNode12_5.SetActive(false);

        videoPlayerNode12_5.Stop();

        videoPlayerNode12_6.Stop();
        videoPlayerNode12_6.time = 0;        
        videoPlayerNode12_6.frame = 0;
        videoPlayerNode12_6.Play();

        yield return new WaitForSeconds(1f);
        myTextNode12_6_1.gameObject.SetActive(true);
        textNode12_6_1.StartTyping();
        
        yield return new WaitForSeconds(4f);
        myTextNode12_6_1.gameObject.SetActive(false);
        myTextNode12_6_2.gameObject.SetActive(true);
        textNode12_6_2.StartTyping();
        
        yield return new WaitForSeconds(5f);
        myTextNode12_6_2.gameObject.SetActive(false);
        myTextNode12_6_3.gameObject.SetActive(true);
        textNode12_6_3.StartTyping();

        yield return new WaitForSeconds(5f);
    }
}
