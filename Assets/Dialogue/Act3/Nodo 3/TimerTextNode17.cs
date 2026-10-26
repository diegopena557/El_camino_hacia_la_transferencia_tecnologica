using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;

public class TimerTextNode17 : MonoBehaviour
{
    private bool flagJustOneTime;

    public GameObject GO_CanvaNode17_1;
    public GameObject GO_CanvaNode17_2;
    public GameObject GO_CanvaNode17_3;
    public GameObject GO_CanvaNode17_4;
    
    public GameObject GO_Node17_1;
    public GameObject GO_Node17_2;
    public GameObject GO_Node17_3;
    public GameObject GO_Node17_4;
    
    public VideoPlayer videoPlayerNode17_1;
    public VideoPlayer videoPlayerNode17_2;
    public VideoPlayer videoPlayerNode17_3;
    public VideoPlayer videoPlayerNode17_4;
    
    public TextMeshProUGUI myTextNode17_1_1;
    public TextMeshProUGUI myTextNode17_1_2;
    public TextMeshProUGUI myTextNode17_1_3;
    public TextMeshProUGUI myTextNode17_1_4;
    public TextMeshProUGUI myTextNode17_1_5;
    
    public TypewriterTMP textNode17_1_1;
    public TypewriterTMP textNode17_1_2;
    public TypewriterTMP textNode17_1_3;
    public TypewriterTMP textNode17_1_4;
    public TypewriterTMP textNode17_1_5;

    public TextMeshProUGUI myTextNode17_2_1;
    public TextMeshProUGUI myTextNode17_2_2;
    public TextMeshProUGUI myTextNode17_2_3;
    public TextMeshProUGUI myTextNode17_2_4;
    public TextMeshProUGUI myTextNode17_2_5;
    public TextMeshProUGUI myTextNode17_2_6;
    
    public TypewriterTMP textNode17_2_1;
    public TypewriterTMP textNode17_2_2;
    public TypewriterTMP textNode17_2_3;
    public TypewriterTMP textNode17_2_4;
    public TypewriterTMP textNode17_2_5;
    public TypewriterTMP textNode17_2_6;

    public TypewriterTMP textNode17_3_1;
    public TypewriterTMP textNode17_3_2;
    public TypewriterTMP textNode17_3_3;
    public TypewriterTMP textNode17_3_4;
    public TypewriterTMP textNode17_3_5;
    
    public TextMeshProUGUI myTextNode17_3_1;
    public TextMeshProUGUI myTextNode17_3_2;
    public TextMeshProUGUI myTextNode17_3_3;
    public TextMeshProUGUI myTextNode17_3_4;
    public TextMeshProUGUI myTextNode17_3_5;

    public TypewriterTMP textNode17_4_1;
    public TypewriterTMP textNode17_4_2;
    public TypewriterTMP textNode17_4_3;
    public TypewriterTMP textNode17_4_4;
    
    public TextMeshProUGUI myTextNode17_4_1;
    public TextMeshProUGUI myTextNode17_4_2;
    public TextMeshProUGUI myTextNode17_4_3;
    public TextMeshProUGUI myTextNode17_4_4;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(flagJustOneTime == false){

            StartCoroutine(AdvancingTimerNode17_1());
            flagJustOneTime = true;
        }
    }

    IEnumerator AdvancingTimerNode17_1()
    {
        videoPlayerNode17_1.Stop();
        //GO_Node17_1.SetActive(false);
        GO_Node17_1.SetActive(true);
        GO_CanvaNode17_1.SetActive(true);

        videoPlayerNode17_1.Stop();
        videoPlayerNode17_1.time = 0;        
        videoPlayerNode17_1.frame = 0;
        videoPlayerNode17_1.Play();

        myTextNode17_1_1.gameObject.SetActive(true);
        textNode17_1_1.StartTyping();
        
        yield return new WaitForSeconds(4f);
        
        myTextNode17_1_1.gameObject.SetActive(false);
        myTextNode17_1_2.gameObject.SetActive(true);
        textNode17_1_2.StartTyping();
        
        yield return new WaitForSeconds(5f);
        
        myTextNode17_1_2.gameObject.SetActive(false);
        myTextNode17_1_3.gameObject.SetActive(true);
        textNode17_1_3.StartTyping();

        yield return new WaitForSeconds(5f);
        
        myTextNode17_1_3.gameObject.SetActive(false);
        myTextNode17_1_4.gameObject.SetActive(true);
        textNode17_1_4.StartTyping();

        yield return new WaitForSeconds(5f);
        
        myTextNode17_1_4.gameObject.SetActive(false);
        myTextNode17_1_5.gameObject.SetActive(true);
        textNode17_1_5.StartTyping();

        yield return new WaitForSeconds(5f);

        StartCoroutine(AdvancingTimerNode17_2());
    }

    IEnumerator AdvancingTimerNode17_2()
    {
        videoPlayerNode17_1.Stop();
        GO_Node17_1.SetActive(false);
        GO_Node17_2.SetActive(true);
        GO_CanvaNode17_2.SetActive(true);

        videoPlayerNode17_2.Stop();
        videoPlayerNode17_2.time = 0;        
        videoPlayerNode17_2.frame = 0;
        videoPlayerNode17_2.Play();

        myTextNode17_2_1.gameObject.SetActive(true);
        textNode17_2_1.StartTyping();
        
        yield return new WaitForSeconds(4f);
        
        myTextNode17_2_1.gameObject.SetActive(false);
        myTextNode17_2_2.gameObject.SetActive(true);
        textNode17_2_2.StartTyping();
        
        yield return new WaitForSeconds(5f);
        
        myTextNode17_2_2.gameObject.SetActive(false);
        myTextNode17_2_3.gameObject.SetActive(true);
        textNode17_2_3.StartTyping();

        yield return new WaitForSeconds(5f);
        
        myTextNode17_2_3.gameObject.SetActive(false);
        myTextNode17_2_4.gameObject.SetActive(true);
        textNode17_2_4.StartTyping();

        yield return new WaitForSeconds(5f);
        
        myTextNode17_2_4.gameObject.SetActive(false);
        myTextNode17_2_5.gameObject.SetActive(true);
        textNode17_2_5.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode17_2_5.gameObject.SetActive(false);
        myTextNode17_2_6.gameObject.SetActive(true);
        textNode17_2_6.StartTyping();

        yield return new WaitForSeconds(5f);

        StartCoroutine(AdvancingTimerNode17_3());
    }

    IEnumerator AdvancingTimerNode17_3()
    {
        videoPlayerNode17_2.Stop();
        GO_Node17_2.SetActive(false);
        GO_Node17_3.SetActive(true);
        GO_CanvaNode17_3.SetActive(true);

        videoPlayerNode17_3.Stop();
        videoPlayerNode17_3.time = 0;        
        videoPlayerNode17_3.frame = 0;
        videoPlayerNode17_3.Play();

        myTextNode17_3_1.gameObject.SetActive(true);
        textNode17_3_1.StartTyping();
        
        yield return new WaitForSeconds(4f);
        
        myTextNode17_3_1.gameObject.SetActive(false);
        myTextNode17_3_2.gameObject.SetActive(true);
        textNode17_3_2.StartTyping();
        
        yield return new WaitForSeconds(5f);
        
        myTextNode17_3_2.gameObject.SetActive(false);
        myTextNode17_3_3.gameObject.SetActive(true);
        textNode17_3_3.StartTyping();

        yield return new WaitForSeconds(5f);
        
        myTextNode17_3_3.gameObject.SetActive(false);
        myTextNode17_3_4.gameObject.SetActive(true);
        textNode17_3_4.StartTyping();

        yield return new WaitForSeconds(5f);
        
        myTextNode17_3_4.gameObject.SetActive(false);
        myTextNode17_3_5.gameObject.SetActive(true);
        textNode17_3_5.StartTyping();

        yield return new WaitForSeconds(5f);

        StartCoroutine(AdvancingTimerNode17_4());
    }

    IEnumerator AdvancingTimerNode17_4()
    {
        videoPlayerNode17_3.Stop();
        GO_Node17_3.SetActive(false);
        GO_Node17_4.SetActive(true);
        GO_CanvaNode17_4.SetActive(true);

        videoPlayerNode17_4.Stop();
        videoPlayerNode17_4.time = 0;        
        videoPlayerNode17_4.frame = 0;
        videoPlayerNode17_4.Play();

        myTextNode17_4_1.gameObject.SetActive(true);
        textNode17_4_1.StartTyping();
        
        yield return new WaitForSeconds(4f);
        
        myTextNode17_4_1.gameObject.SetActive(false);
        myTextNode17_4_2.gameObject.SetActive(true);
        textNode17_4_2.StartTyping();
        
        yield return new WaitForSeconds(5f);
        
        myTextNode17_4_2.gameObject.SetActive(false);
        myTextNode17_4_3.gameObject.SetActive(true);
        textNode17_4_3.StartTyping();

        yield return new WaitForSeconds(5f);
        
        myTextNode17_4_3.gameObject.SetActive(false);
        myTextNode17_4_4.gameObject.SetActive(true);
        textNode17_4_4.StartTyping();

        yield return new WaitForSeconds(5f);
    }
}
