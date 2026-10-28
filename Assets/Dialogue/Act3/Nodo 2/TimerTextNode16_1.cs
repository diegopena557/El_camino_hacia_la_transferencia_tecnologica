using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;

public class TimerTextNode16_1 : MonoBehaviour
{
    private bool flagJustOneTime;

    public GameObject GO_CanvaNode16_1;
    public GameObject GO_CanvaNode16_2;
    public GameObject GO_CanvaNode16_3;
    
    public GameObject GO_Node16_1;
    public GameObject GO_Node16_2;
    public GameObject GO_Node16_3;
    
    public VideoPlayer videoPlayerNode16_1;
    public VideoPlayer videoPlayerNode16_2;
    public VideoPlayer videoPlayerNode16_3;
    
    public TextMeshProUGUI myTextNode16_1_1;
    public TextMeshProUGUI myTextNode16_1_2;
    public TextMeshProUGUI myTextNode16_1_3;

    public TextMeshProUGUI myTextNode16_2_1;
    public TextMeshProUGUI myTextNode16_3_1;
    
    public TypewriterTMP textNode16_1_1;
    public TypewriterTMP textNode16_1_2;
    public TypewriterTMP textNode16_1_3;

    public TypewriterTMP textNode16_2_1;
    public TypewriterTMP textNode16_3_1;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(flagJustOneTime == false){
            StartCoroutine(AdvancingTimerNode16_1());
            
            flagJustOneTime = true;
        }
    }

    IEnumerator AdvancingTimerNode16_1()
    {
        GO_Node16_1.SetActive(true);
        GO_CanvaNode16_1.SetActive(true);

        videoPlayerNode16_1.Stop();
        videoPlayerNode16_1.time = 0;        
        videoPlayerNode16_1.frame = 0;
        videoPlayerNode16_1.Play();

        myTextNode16_1_1.gameObject.SetActive(true);
        textNode16_1_1.StartTyping();
        
        yield return new WaitForSeconds(5f);

        myTextNode16_1_1.gameObject.SetActive(false);
        myTextNode16_1_2.gameObject.SetActive(true);
        textNode16_1_2.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode16_1_2.gameObject.SetActive(false);
        myTextNode16_1_3.gameObject.SetActive(true);
        textNode16_1_3.StartTyping();

        yield return new WaitForSeconds(5f);

        StartCoroutine(AdvancingTimerNode16_2());
    }

    IEnumerator AdvancingTimerNode16_2()
    {
        videoPlayerNode16_1.Stop();
        GO_Node16_1.SetActive(false);
        GO_Node16_2.SetActive(true);
        GO_CanvaNode16_2.SetActive(true);

        videoPlayerNode16_2.Stop();
        videoPlayerNode16_2.time = 0;        
        videoPlayerNode16_2.frame = 0;
        videoPlayerNode16_2.Play();

        myTextNode16_2_1.gameObject.SetActive(true);
        textNode16_2_1.StartTyping();

        yield return new WaitForSeconds(5f);

        StartCoroutine(AdvancingTimerNode16_3());
    }

    IEnumerator AdvancingTimerNode16_3()
    {
        videoPlayerNode16_2.Stop();
        GO_Node16_2.SetActive(false);
        GO_Node16_3.SetActive(true);
        GO_CanvaNode16_3.SetActive(true);

        videoPlayerNode16_3.Stop();
        videoPlayerNode16_3.time = 0;        
        videoPlayerNode16_3.frame = 0;
        videoPlayerNode16_3.Play();

        myTextNode16_3_1.gameObject.SetActive(true);
        textNode16_3_1.StartTyping();

        yield return new WaitForSeconds(5f);
    }
}
