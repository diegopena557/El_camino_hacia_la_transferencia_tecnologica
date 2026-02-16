using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;

public class TimerNode7 : MonoBehaviour
{
    private bool flagJustOneTime;

    public GameObject GO_CanvaNode7a;

    public VideoPlayer videoPlayerNode7a;

    public TextMeshProUGUI myTextNode7a_1;
    public TextMeshProUGUI myTextNode7a_2;
    public TextMeshProUGUI myTextNode7a_3;
    public TextMeshProUGUI myTextNode7a_4;

    public TypewriterTMP textNode7a_1;
    public TypewriterTMP textNode7a_2;
    public TypewriterTMP textNode7a_3;
    public TypewriterTMP textNode7a_4;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(flagJustOneTime == false){
            StartCoroutine(AdvancingTimerNode7());
            flagJustOneTime = true;
        }
    }

    IEnumerator AdvancingTimerNode7()
    {
        videoPlayerNode7a.Stop();
        videoPlayerNode7a.time = 0;        
        videoPlayerNode7a.frame = 0;
        videoPlayerNode7a.Play();

        videoPlayerNode7a.playbackSpeed = 1f;

        myTextNode7a_1.gameObject.SetActive(true);
        textNode7a_1.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode7a_1.gameObject.SetActive(false);
        myTextNode7a_2.gameObject.SetActive(true);
        textNode7a_2.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode7a_2.gameObject.SetActive(false);
        myTextNode7a_3.gameObject.SetActive(true);
        textNode7a_3.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode7a_3.gameObject.SetActive(false);
        myTextNode7a_4.gameObject.SetActive(true);
        textNode7a_4.StartTyping();

        yield return new WaitForSeconds(5f);
    }
}
