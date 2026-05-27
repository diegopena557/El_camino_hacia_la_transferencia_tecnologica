using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections.Generic; //Necesario para los audios

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
        PlayByName("10_1_1");
        
        yield return new WaitForSeconds(2.62f);
        myTextNode10_1_1.gameObject.SetActive(false);
        myTextNode10_1_2.gameObject.SetActive(true);
        textNode10_1_2.StartTyping();
        PlayByName("10_1_2");
        
        yield return new WaitForSeconds(6.12f);
        myTextNode10_1_2.gameObject.SetActive(false);
        myTextNode10_1_3.gameObject.SetActive(true);
        textNode10_1_3.StartTyping();
        PlayByName("10_1_3");

        yield return new WaitForSeconds(5.23f);
        myTextNode10_1_3.gameObject.SetActive(false);
        myTextNode10_1_4.gameObject.SetActive(true);
        textNode10_1_4.StartTyping();
        PlayByName("10_1_4");

        yield return new WaitForSeconds(4.44f);
        myTextNode10_1_4.gameObject.SetActive(false);
        myTextNode10_1_5.gameObject.SetActive(true);
        textNode10_1_5.StartTyping();
        PlayByName("10_1_5");

        yield return new WaitForSeconds(3.82f);
        myTextNode10_1_5.gameObject.SetActive(false);
        myTextNode10_1_6.gameObject.SetActive(true);
        textNode10_1_6.StartTyping();
        PlayByName("10_1_6");

        yield return new WaitForSeconds(2.95f);
        myTextNode10_1_6.gameObject.SetActive(false);
        myTextNode10_1_7.gameObject.SetActive(true);
        textNode10_1_7.StartTyping();
        PlayByName("10_1_7");

        yield return new WaitForSeconds(4.05f);
        myTextNode10_1_7.gameObject.SetActive(false);
        myTextNode10_1_8.gameObject.SetActive(true);
        textNode10_1_8.StartTyping();
        PlayByName("10_1_8");
    }
}
