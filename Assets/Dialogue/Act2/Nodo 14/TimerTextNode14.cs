using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;

public class TimerTextNode14 : MonoBehaviour
{
    private bool flagJustOneTime;

    public GameObject GO_CanvaNode14_1;

    public GameObject GO_Node14_1;

    public VideoPlayer videoPlayerNode14_1;

    public TextMeshProUGUI myTextNode14_1_1;
    public TextMeshProUGUI myTextNode14_1_2;
    public TextMeshProUGUI myTextNode14_1_3;
    public TextMeshProUGUI myTextNode14_1_4;

    public TypewriterTMP textNode14_1_1;
    public TypewriterTMP textNode14_1_2;
    public TypewriterTMP textNode14_1_3;
    public TypewriterTMP textNode14_1_4;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(flagJustOneTime == false){
            StartCoroutine(AdvancingTimerNode14_1());
            flagJustOneTime = true;
        }
    }

    IEnumerator AdvancingTimerNode14_1()
    {
        GO_Node14_1.SetActive(true);
        GO_CanvaNode14_1.SetActive(true);

        videoPlayerNode14_1.Stop();
        videoPlayerNode14_1.time = 0;        
        videoPlayerNode14_1.frame = 0;
        videoPlayerNode14_1.Play();

        yield return new WaitForSeconds(1f);
        myTextNode14_1_1.gameObject.SetActive(true);
        textNode14_1_1.StartTyping();
        
        yield return new WaitForSeconds(4f);
        myTextNode14_1_1.gameObject.SetActive(false);
        myTextNode14_1_2.gameObject.SetActive(true);
        textNode14_1_2.StartTyping();
        
        yield return new WaitForSeconds(5f);
        myTextNode14_1_2.gameObject.SetActive(false);
        myTextNode14_1_3.gameObject.SetActive(true);
        textNode14_1_3.StartTyping();

        yield return new WaitForSeconds(5f);
        myTextNode14_1_3.gameObject.SetActive(false);
        myTextNode14_1_4.gameObject.SetActive(true);
        textNode14_1_4.StartTyping();

        yield return new WaitForSeconds(4f);
        //StartCoroutine(AdvancingTimerNode12_2());
    }
}
