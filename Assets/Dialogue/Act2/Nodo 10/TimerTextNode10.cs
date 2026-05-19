using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;

public class TimerNode10 : MonoBehaviour
{
     private bool flagJustOneTime;

    public GameObject GO_Node10_1;

    public GameObject GO_CanvaNode10_1;

    public VideoPlayer videoPlayerNode10_1;

    public TextMeshProUGUI myTextNode10_1_1;
    public TextMeshProUGUI myTextNode10_1_2;
    public TextMeshProUGUI myTextNode10_1_3;
    public TextMeshProUGUI myTextNode10_1_4;
    public TextMeshProUGUI myTextNode10_1_5;
    public TextMeshProUGUI myTextNode10_1_6;
    public TextMeshProUGUI myTextNode10_1_7;
    public TextMeshProUGUI myTextNode10_1_8;

    public TypewriterTMP textNode10_1_1;
    public TypewriterTMP textNode10_1_2;
    public TypewriterTMP textNode10_1_3;
    public TypewriterTMP textNode10_1_4;
    public TypewriterTMP textNode10_1_5;
    public TypewriterTMP textNode10_1_6;
    public TypewriterTMP textNode10_1_7;
    public TypewriterTMP textNode10_1_8;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(flagJustOneTime == false){
            StartCoroutine(AdvancingTimerNode10_1());
            flagJustOneTime = true;
        }
    }

    IEnumerator AdvancingTimerNode10_1()
    {
        GO_Node10_1.SetActive(true);
        GO_CanvaNode10_1.SetActive(true);

        videoPlayerNode10_1.Stop();
        videoPlayerNode10_1.time = 0;        
        videoPlayerNode10_1.frame = 0;
        videoPlayerNode10_1.Play();

        yield return new WaitForSeconds(1f);
        myTextNode10_1_1.gameObject.SetActive(true);
        textNode10_1_1.StartTyping();
        
        yield return new WaitForSeconds(4f);
        myTextNode10_1_1.gameObject.SetActive(false);
        myTextNode10_1_2.gameObject.SetActive(true);
        textNode10_1_2.StartTyping();
        
        yield return new WaitForSeconds(5f);
        myTextNode10_1_2.gameObject.SetActive(false);
        myTextNode10_1_3.gameObject.SetActive(true);
        textNode10_1_3.StartTyping();

        yield return new WaitForSeconds(5f);
        myTextNode10_1_3.gameObject.SetActive(false);
        myTextNode10_1_4.gameObject.SetActive(true);
        textNode10_1_4.StartTyping();

        yield return new WaitForSeconds(6f);
        myTextNode10_1_4.gameObject.SetActive(false);
        myTextNode10_1_5.gameObject.SetActive(true);
        textNode10_1_5.StartTyping();

        yield return new WaitForSeconds(5f);
        myTextNode10_1_5.gameObject.SetActive(false);
        myTextNode10_1_6.gameObject.SetActive(true);
        textNode10_1_6.StartTyping();

        yield return new WaitForSeconds(5f);
        myTextNode10_1_6.gameObject.SetActive(false);
        myTextNode10_1_7.gameObject.SetActive(true);
        textNode10_1_7.StartTyping();

        yield return new WaitForSeconds(5f);
        myTextNode10_1_7.gameObject.SetActive(false);
        myTextNode10_1_8.gameObject.SetActive(true);
        textNode10_1_8.StartTyping();

    }
}
