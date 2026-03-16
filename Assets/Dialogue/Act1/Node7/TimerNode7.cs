using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections.Generic; //Necesario para los audios

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
        PlayByName("7a_1");

        yield return new WaitForSeconds(5.8f);

        myTextNode7a_1.gameObject.SetActive(false);
        myTextNode7a_2.gameObject.SetActive(true);
        textNode7a_2.StartTyping();
        PlayByName("7a_2");

        yield return new WaitForSeconds(6f);

        myTextNode7a_2.gameObject.SetActive(false);
        myTextNode7a_3.gameObject.SetActive(true);
        textNode7a_3.StartTyping();
        PlayByName("7a_3");

        yield return new WaitForSeconds(5.7f);

        myTextNode7a_3.gameObject.SetActive(false);
        myTextNode7a_4.gameObject.SetActive(true);
        textNode7a_4.StartTyping();
        PlayByName("7a_4");

        yield return new WaitForSeconds(6f);
    }
}
