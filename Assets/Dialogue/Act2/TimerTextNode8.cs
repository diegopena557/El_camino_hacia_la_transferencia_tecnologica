using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;

public class TimerTextNode8 : MonoBehaviour
{
    private bool flagJustOneTime;

    public GameObject GO_Node8_0;
    public GameObject GO_Node8_1;

    public GameObject GO_CanvaNode8_0;
    public GameObject GO_CanvaNode8_1;

    public VideoPlayer videoPlayerNode8_0;
    public VideoPlayer videoPlayerNode8_1;

    public TextMeshProUGUI myTextNode8_1;
    public TextMeshProUGUI myTextNode8_2;
    public TextMeshProUGUI myTextNode8_3;
    public TextMeshProUGUI myTextNode8_4;
    public TextMeshProUGUI myTextNode8_5;
    public TextMeshProUGUI myTextNode8_6;
    public TextMeshProUGUI myTextNode8_7;
    public TextMeshProUGUI myTextNode8_8;

    public TypewriterTMP textNode8_1;
    public TypewriterTMP textNode8_2;
    public TypewriterTMP textNode8_3;
    public TypewriterTMP textNode8_4;
    public TypewriterTMP textNode8_5;
    public TypewriterTMP textNode8_6;
    public TypewriterTMP textNode8_7;
    public TypewriterTMP textNode8_8;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(flagJustOneTime == false){
            StartCoroutine(AdvancingTimerNode8_0());
            flagJustOneTime = true;
        }
    }

    IEnumerator AdvancingTimerNode8_0()
    {
        GO_Node8_0.SetActive(true);
        GO_Node8_1.SetActive(false);

        videoPlayerNode8_0.Stop();
        videoPlayerNode8_0.time = 0;        
        videoPlayerNode8_0.frame = 0;
        videoPlayerNode8_0.Play();

        yield return new WaitForSeconds(18f);

        GO_CanvaNode8_1.SetActive(true);
        GO_Node8_1.SetActive(true);

        GO_CanvaNode8_0.SetActive(false);
        GO_Node8_0.SetActive(false);

        videoPlayerNode8_1.Stop();
        videoPlayerNode8_1.time = 0;        
        videoPlayerNode8_1.frame = 0;
        videoPlayerNode8_1.Play();

        yield return new WaitForSeconds(4f);
        myTextNode8_1.gameObject.SetActive(true);
        textNode8_1.StartTyping();
        
        yield return new WaitForSeconds(4f);
        myTextNode8_1.gameObject.SetActive(false);
        myTextNode8_2.gameObject.SetActive(true);
        textNode8_2.StartTyping();
        
        yield return new WaitForSeconds(5f);
        myTextNode8_2.gameObject.SetActive(false);
        myTextNode8_3.gameObject.SetActive(true);
        textNode8_3.StartTyping();

        yield return new WaitForSeconds(3f);
        myTextNode8_3.gameObject.SetActive(false);
        myTextNode8_4.gameObject.SetActive(true);
        textNode8_4.StartTyping();

        yield return new WaitForSeconds(6f);
        myTextNode8_4.gameObject.SetActive(false);
        myTextNode8_5.gameObject.SetActive(true);
        textNode8_5.StartTyping();

        yield return new WaitForSeconds(6f);
        myTextNode8_5.gameObject.SetActive(false);
        myTextNode8_6.gameObject.SetActive(true);
        textNode8_6.StartTyping();

        yield return new WaitForSeconds(6f);
        myTextNode8_6.gameObject.SetActive(false);
        myTextNode8_7.gameObject.SetActive(true);
        textNode8_7.StartTyping();

        yield return new WaitForSeconds(6f);
        myTextNode8_7.gameObject.SetActive(false);
        myTextNode8_8.gameObject.SetActive(true);
        textNode8_8.StartTyping();

    }
}
