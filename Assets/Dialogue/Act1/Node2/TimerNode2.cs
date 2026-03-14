using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections.Generic; //Necesario para los audios

public class TimerNode2 : MonoBehaviour
{
    private bool flagJustOneTime;

    public GameObject GO_CanvaNode2a;

    public VideoPlayer videoPlayerNode2a;

    public TextMeshProUGUI myTextNode2a_1;
    public TextMeshProUGUI myTextNode2a_2;

    public TypewriterTMP textNode2a_1;
    public TypewriterTMP textNode2a_2;

    public CanvasFader canvaFader;
    public GameObject GO_Node3;

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
            StartCoroutine(AdvancingTimerNode2());
            flagJustOneTime = true;
        }
    }

    IEnumerator AdvancingTimerNode2()
    {
        videoPlayerNode2a.Stop();
        videoPlayerNode2a.time = 0;        
        videoPlayerNode2a.frame = 0;
        videoPlayerNode2a.Play();

        videoPlayerNode2a.playbackSpeed = 1.0f;

        myTextNode2a_1.gameObject.SetActive(true);
        textNode2a_1.StartTyping();
        PlayByName("2a_1");

        yield return new WaitForSeconds(8f);

        myTextNode2a_1.gameObject.SetActive(false);
        myTextNode2a_2.gameObject.SetActive(true);
        textNode2a_2.StartTyping();
        PlayByName("2a_2");
        

        yield return new WaitForSeconds(3f);

        GO_Node3.SetActive(true);
        canvaFader.FadeAToB();
    }

}
