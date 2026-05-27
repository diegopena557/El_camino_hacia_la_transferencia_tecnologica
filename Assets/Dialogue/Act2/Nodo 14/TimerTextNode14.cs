using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections.Generic; //Necesario para los audios


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

    //////////////////// AUDIOS ////////////////////////
    /// SFX Anim Sec ///
    //[Header("SFX Anim Sec")]
    //public AudioSource Anim1;
    //public AudioSource Anim2;
    /// Dialogos ///
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
        PlayByName("14a_1");

        yield return new WaitForSeconds(4.44f);
        myTextNode14_1_1.gameObject.SetActive(false);
        myTextNode14_1_2.gameObject.SetActive(true);
        textNode14_1_2.StartTyping();
        PlayByName("14a_2");
        
        yield return new WaitForSeconds(2.72f);
        myTextNode14_1_2.gameObject.SetActive(false);
        myTextNode14_1_3.gameObject.SetActive(true);
        textNode14_1_3.StartTyping();
        PlayByName("14a_3");

        yield return new WaitForSeconds(2.98f);
        myTextNode14_1_3.gameObject.SetActive(false);
        myTextNode14_1_4.gameObject.SetActive(true);
        textNode14_1_4.StartTyping();
        PlayByName("14a_4");

        yield return new WaitForSeconds(4.1f);
        //StartCoroutine(AdvancingTimerNode12_2());
    }
}
