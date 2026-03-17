using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections.Generic; //Necesario para los audios

public class TimerNode4 : MonoBehaviour
{
    public GameObject GO_CanvaNode4_0;
    public GameObject GO_CanvaNode4a;

    public VideoPlayer videoPlayerNode4_0;
    public VideoPlayer videoPlayerNode4a;

    public TextMeshProUGUI myTextNode4a_1;
    public TextMeshProUGUI myTextNode4a_2;
    public TextMeshProUGUI myTextNode4a_3;
    public TextMeshProUGUI myTextNode4a_4;
    public TextMeshProUGUI myTextNode4a_5;
    public TextMeshProUGUI myTextNode4a_6;
    public TextMeshProUGUI myTextNode4a_7;

    public TypewriterTMP textNode4a_1;
    public TypewriterTMP textNode4a_2;
    public TypewriterTMP textNode4a_3;
    public TypewriterTMP textNode4a_4;
    public TypewriterTMP textNode4a_5;
    public TypewriterTMP textNode4a_6;
    public TypewriterTMP textNode4a_7;

    public bool flagOneTime;

    public GameObject GO_Node5;
    public TimerNode5 timerNode5;
    public GameObject GO_Node4;

    //////////////////// AUDIOS ////////////////////////
    [Header("Audio Sources")]
    public AudioSource[] sources;

    private Dictionary<string, AudioSource> audioDict;

    void Awake()
    {
        audioDict = new Dictionary<string, AudioSource>();
        foreach (AudioSource src in sources)
        {
            if (src != null)
            {
                audioDict[src.gameObject.name] = src;
            }
        }
    }

    public void PlayByName(string name)
    {
        if (audioDict.ContainsKey(name))
        {
            audioDict[name].Play();
        }
        else
        {
            Debug.LogWarning("No AudioSource found with name: " + name);
        }
    }

    public void StopByName(string name)
    {
        if (audioDict.ContainsKey(name))
        {
            audioDict[name].Stop();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(flagOneTime == false){

            StartTimerNode4();

            flagOneTime = true;
        }
    }

    public void StartTimerNode4(){
        StartCoroutine(AdvancingTimerNode4());
    }

    IEnumerator AdvancingTimerNode4()
    {
        GO_CanvaNode4_0.SetActive(true);

        videoPlayerNode4_0.Stop();
        videoPlayerNode4_0.time = 0;        
        videoPlayerNode4_0.frame = 0;
        videoPlayerNode4_0.Play();

        yield return new WaitForSeconds(5f);

        GO_CanvaNode4_0.SetActive(false);
        GO_CanvaNode4a.SetActive(true);

        videoPlayerNode4a.Stop();
        videoPlayerNode4a.time = 0;        
        videoPlayerNode4a.frame = 0;
        videoPlayerNode4a.Play();

        videoPlayerNode4a.playbackSpeed = 1f;

        myTextNode4a_1.gameObject.SetActive(true);
        textNode4a_1.StartTyping();
        PlayByName("4a_1");

        yield return new WaitForSeconds(8f);

        myTextNode4a_1.gameObject.SetActive(false);
        myTextNode4a_2.gameObject.SetActive(true);
        textNode4a_2.StartTyping();
        PlayByName("4a_2");

        yield return new WaitForSeconds(7.5f);

        myTextNode4a_2.gameObject.SetActive(false);
        myTextNode4a_3.gameObject.SetActive(true);
        textNode4a_3.StartTyping();
        PlayByName("4a_3");

        yield return new WaitForSeconds(7.5f);

        myTextNode4a_3.gameObject.SetActive(false);
        myTextNode4a_4.gameObject.SetActive(true);
        textNode4a_4.StartTyping();
        PlayByName("4a_4");

        yield return new WaitForSeconds(4.5f);

        myTextNode4a_4.gameObject.SetActive(false);
        myTextNode4a_5.gameObject.SetActive(true);
        textNode4a_5.StartTyping();
        PlayByName("4a_5");

        yield return new WaitForSeconds(7f);

        myTextNode4a_5.gameObject.SetActive(false);
        myTextNode4a_6.gameObject.SetActive(true);
        textNode4a_6.StartTyping();
        PlayByName("4a_6");

        yield return new WaitForSeconds(5.5f);

        myTextNode4a_6.gameObject.SetActive(false);
        myTextNode4a_7.gameObject.SetActive(true);
        textNode4a_7.StartTyping();
        PlayByName("4a_7");

        yield return new WaitForSeconds(7.5f);

        GO_Node5.SetActive(true);
        timerNode5.StartTimerNode5();
        GO_Node4.SetActive(false);
    }
}
