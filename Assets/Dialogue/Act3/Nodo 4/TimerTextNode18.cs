using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;

public class TimerTextNode18 : MonoBehaviour
{
    private bool flagJustOneTime;

    public GameObject GO_CanvaNode18_1;
    public GameObject GO_CanvaNode18_2;
    public GameObject GO_CanvaNode18_3;
    
    public GameObject GO_Node18_1;
    public GameObject GO_Node18_2;
    public GameObject GO_Node18_3;
    
    public VideoPlayer videoPlayerNode18_1;
    public VideoPlayer videoPlayerNode18_2;
    public VideoPlayer videoPlayerNode18_3;
    
    public TextMeshProUGUI myTextNode18_1_1;
    public TextMeshProUGUI myTextNode18_1_2;
    public TextMeshProUGUI myTextNode18_1_3;
    public TextMeshProUGUI myTextNode18_1_4;
    public TextMeshProUGUI myTextNode18_1_5;
    public TextMeshProUGUI myTextNode18_1_6;
    public TextMeshProUGUI myTextNode18_1_7;
    public TextMeshProUGUI myTextNode18_1_8;
    public TextMeshProUGUI myTextNode18_1_9;
    
    public TypewriterTMP textNode18_1_1;
    public TypewriterTMP textNode18_1_2;
    public TypewriterTMP textNode18_1_3;
    public TypewriterTMP textNode18_1_4;
    public TypewriterTMP textNode18_1_5;
    public TypewriterTMP textNode18_1_6;
    public TypewriterTMP textNode18_1_7;
    public TypewriterTMP textNode18_1_8;
    public TypewriterTMP textNode18_1_9;

    public TextMeshProUGUI myTextNode18_2_1;
    public TextMeshProUGUI myTextNode18_2_2;
    public TextMeshProUGUI myTextNode18_2_3;
    public TextMeshProUGUI myTextNode18_2_4;
    public TextMeshProUGUI myTextNode18_2_5;
    public TextMeshProUGUI myTextNode18_2_6;
    public TextMeshProUGUI myTextNode18_2_7;
    
    public TypewriterTMP textNode18_2_1;
    public TypewriterTMP textNode18_2_2;
    public TypewriterTMP textNode18_2_3;
    public TypewriterTMP textNode18_2_4;
    public TypewriterTMP textNode18_2_5;
    public TypewriterTMP textNode18_2_6;
    public TypewriterTMP textNode18_2_7;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(flagJustOneTime == false){

            StartCoroutine(AdvancingTimerNode18_1());
            flagJustOneTime = true;
        }
    }

    IEnumerator AdvancingTimerNode18_1()
    {
        videoPlayerNode18_1.Stop();
        GO_Node18_1.SetActive(true);
        GO_CanvaNode18_1.SetActive(true);

        videoPlayerNode18_1.Stop();
        videoPlayerNode18_1.time = 0;        
        videoPlayerNode18_1.frame = 0;
        videoPlayerNode18_1.Play();

        myTextNode18_1_1.gameObject.SetActive(true);
        textNode18_1_1.StartTyping();
        
        yield return new WaitForSeconds(4f);
        
        myTextNode18_1_1.gameObject.SetActive(false);
        myTextNode18_1_2.gameObject.SetActive(true);
        textNode18_1_2.StartTyping();
        
        yield return new WaitForSeconds(5f);
        
        myTextNode18_1_2.gameObject.SetActive(false);
        myTextNode18_1_3.gameObject.SetActive(true);
        textNode18_1_3.StartTyping();

        yield return new WaitForSeconds(5f);
        
        myTextNode18_1_3.gameObject.SetActive(false);
        myTextNode18_1_4.gameObject.SetActive(true);
        textNode18_1_4.StartTyping();

        yield return new WaitForSeconds(5f);
        
        myTextNode18_1_4.gameObject.SetActive(false);
        myTextNode18_1_5.gameObject.SetActive(true);
        textNode18_1_5.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode18_1_5.gameObject.SetActive(false);
        myTextNode18_1_6.gameObject.SetActive(true);
        textNode18_1_6.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode18_1_6.gameObject.SetActive(false);
        myTextNode18_1_7.gameObject.SetActive(true);
        textNode18_1_7.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode18_1_7.gameObject.SetActive(false);
        myTextNode18_1_8.gameObject.SetActive(true);
        textNode18_1_8.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode18_1_8.gameObject.SetActive(false);
        myTextNode18_1_9.gameObject.SetActive(true);
        textNode18_1_9.StartTyping();

        yield return new WaitForSeconds(5f);

        StartCoroutine(AdvancingTimerNode18_2());
    }

    IEnumerator AdvancingTimerNode18_2()
    {
        videoPlayerNode18_1.Stop();
        GO_Node18_1.SetActive(false);
        GO_Node18_2.SetActive(true);
        GO_CanvaNode18_2.SetActive(true);

        videoPlayerNode18_2.Stop();
        videoPlayerNode18_2.time = 0;        
        videoPlayerNode18_2.frame = 0;
        videoPlayerNode18_2.Play();

        myTextNode18_2_1.gameObject.SetActive(true);
        textNode18_2_1.StartTyping();
        
        yield return new WaitForSeconds(4f);
        
        myTextNode18_2_1.gameObject.SetActive(false);
        myTextNode18_2_2.gameObject.SetActive(true);
        textNode18_2_2.StartTyping();
        
        yield return new WaitForSeconds(5f);
        
        myTextNode18_2_2.gameObject.SetActive(false);
        myTextNode18_2_3.gameObject.SetActive(true);
        textNode18_2_3.StartTyping();

        yield return new WaitForSeconds(5f);
        
        myTextNode18_2_3.gameObject.SetActive(false);
        myTextNode18_2_4.gameObject.SetActive(true);
        textNode18_2_4.StartTyping();

        yield return new WaitForSeconds(5f);
        
        myTextNode18_2_4.gameObject.SetActive(false);
        myTextNode18_2_5.gameObject.SetActive(true);
        textNode18_2_5.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode18_2_5.gameObject.SetActive(false);
        myTextNode18_2_6.gameObject.SetActive(true);
        textNode18_2_6.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode18_2_6.gameObject.SetActive(false);
        myTextNode18_2_7.gameObject.SetActive(true);
        textNode18_2_7.StartTyping();

        yield return new WaitForSeconds(5f);

        StartCoroutine(AdvancingTimerNode18_3());
    }

    IEnumerator AdvancingTimerNode18_3()
    {
        videoPlayerNode18_2.Stop();
        GO_Node18_2.SetActive(false);
        GO_Node18_3.SetActive(true);
        GO_CanvaNode18_3.SetActive(true);

        videoPlayerNode18_3.Stop();
        videoPlayerNode18_3.time = 0;        
        videoPlayerNode18_3.frame = 0;
        videoPlayerNode18_3.Play();
        
        yield return new WaitForSeconds(5f);
    }
}
