using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;

public class TimerTextNode16_2_2 : MonoBehaviour
{
    private bool flagJustOneTime;

    public GameObject GO_CanvaNode16_2_1;
    public GameObject GO_CanvaNode16_2_2;
    
    public GameObject GO_Node16_2_1;
    public GameObject GO_Node16_2_2;
    
    public VideoPlayer videoPlayerNode16_2_1;
    public VideoPlayer videoPlayerNode16_2_2;
    
    public TextMeshProUGUI myTextNode16_2_1;
    public TextMeshProUGUI myTextNode16_2_2;
    public TextMeshProUGUI myTextNode16_2_3;
    public TextMeshProUGUI myTextNode16_2_4;

    public TextMeshProUGUI myTextNode16_2_1_b;
    public TextMeshProUGUI myTextNode16_2_2_b;
    
    public TypewriterTMP textNode16_2_1;
    public TypewriterTMP textNode16_2_2;
    public TypewriterTMP textNode16_2_3;
    public TypewriterTMP textNode16_2_4;

    public TypewriterTMP textNode16_2_1_b;
    public TypewriterTMP textNode16_2_2_b;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(flagJustOneTime == false){
            StartCoroutine(AdvancingTimerNode16_2_1());
            
            flagJustOneTime = true;
        }
    }

    IEnumerator AdvancingTimerNode16_2_1()
    {
        GO_Node16_2_1.SetActive(true);
        GO_CanvaNode16_2_1.SetActive(true);

        videoPlayerNode16_2_1.Stop();
        videoPlayerNode16_2_1.time = 0;        
        videoPlayerNode16_2_1.frame = 0;
        videoPlayerNode16_2_1.Play();

        myTextNode16_2_1.gameObject.SetActive(true);
        textNode16_2_1.StartTyping();
        
        yield return new WaitForSeconds(4f);

        myTextNode16_2_1.gameObject.SetActive(false);
        myTextNode16_2_2.gameObject.SetActive(true);
        textNode16_2_2.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode16_2_2.gameObject.SetActive(false);
        myTextNode16_2_3.gameObject.SetActive(true);
        textNode16_2_3.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode16_2_3.gameObject.SetActive(false);
        myTextNode16_2_4.gameObject.SetActive(true);
        textNode16_2_4.StartTyping();

        yield return new WaitForSeconds(5f);

        StartCoroutine(AdvancingTimerNode16_2_2());
    }

    IEnumerator AdvancingTimerNode16_2_2()
    {
        videoPlayerNode16_2_1.Stop();
        GO_Node16_2_1.SetActive(false);
        GO_Node16_2_2.SetActive(true);
        GO_CanvaNode16_2_2.SetActive(true);

        videoPlayerNode16_2_2.Stop();
        videoPlayerNode16_2_2.time = 0;        
        videoPlayerNode16_2_2.frame = 0;
        videoPlayerNode16_2_2.Play();

        myTextNode16_2_1_b.gameObject.SetActive(true);
        textNode16_2_1_b.StartTyping();

         yield return new WaitForSeconds(5f);

        myTextNode16_2_1_b.gameObject.SetActive(false);
        myTextNode16_2_2_b.gameObject.SetActive(true);
        textNode16_2_2_b.StartTyping();

        yield return new WaitForSeconds(5f);
    }
}
