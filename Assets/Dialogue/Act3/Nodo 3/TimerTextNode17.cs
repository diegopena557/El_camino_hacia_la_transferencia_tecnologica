using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections.Generic;


public class TimerTextNode17 : MonoBehaviour
{
    private bool flagJustOneTime;

    public GameObject GO_CanvaNode17_1b;
    public GameObject GO_CanvaNode17_2b;
    public GameObject GO_CanvaNode17_3b;
    public GameObject GO_CanvaNode17_4b;

    public GameObject GO_CanvaNode17_1a;
    public GameObject GO_CanvaNode17_2a;
    public GameObject GO_CanvaNode17_3a;
    public GameObject GO_CanvaNode17_4a;
    
    public GameObject GO_Node17_1b;
    public GameObject GO_Node17_2b;
    public GameObject GO_Node17_3b;
    public GameObject GO_Node17_4b;

    public GameObject GO_Node17_1a;
    public GameObject GO_Node17_2a;
    public GameObject GO_Node17_3a;
    public GameObject GO_Node17_4a;
    
    public VideoPlayer videoPlayerNode17_1b;
    public VideoPlayer videoPlayerNode17_2b;
    public VideoPlayer videoPlayerNode17_3b;
    public VideoPlayer videoPlayerNode17_4b;

    public VideoPlayer videoPlayerNode17_1a;
    public VideoPlayer videoPlayerNode17_2a;
    public VideoPlayer videoPlayerNode17_3a;
    public VideoPlayer videoPlayerNode17_4a;
    
    public TextMeshProUGUI myTextNode17_1_1;
    public TextMeshProUGUI myTextNode17_1_2;
    public TextMeshProUGUI myTextNode17_1_3;
    public TextMeshProUGUI myTextNode17_1_4;
    public TextMeshProUGUI myTextNode17_1_5;
    
    public TypewriterTMP textNode17_1_1;
    public TypewriterTMP textNode17_1_2;
    public TypewriterTMP textNode17_1_3;
    public TypewriterTMP textNode17_1_4;
    public TypewriterTMP textNode17_1_5;

    public TextMeshProUGUI myTextNode17_2_1;
    public TextMeshProUGUI myTextNode17_2_2;
    public TextMeshProUGUI myTextNode17_2_3;
    public TextMeshProUGUI myTextNode17_2_4;
    public TextMeshProUGUI myTextNode17_2_5;
    public TextMeshProUGUI myTextNode17_2_6;
    
    public TypewriterTMP textNode17_2_1;
    public TypewriterTMP textNode17_2_2;
    public TypewriterTMP textNode17_2_3;
    public TypewriterTMP textNode17_2_4;
    public TypewriterTMP textNode17_2_5;
    public TypewriterTMP textNode17_2_6;

    public TypewriterTMP textNode17_3_1;
    public TypewriterTMP textNode17_3_2;
    public TypewriterTMP textNode17_3_3;
    public TypewriterTMP textNode17_3_4;
    public TypewriterTMP textNode17_3_5;
    
    public TextMeshProUGUI myTextNode17_3_1;
    public TextMeshProUGUI myTextNode17_3_2;
    public TextMeshProUGUI myTextNode17_3_3;
    public TextMeshProUGUI myTextNode17_3_4;
    public TextMeshProUGUI myTextNode17_3_5;

    public TypewriterTMP textNode17_4_1;
    public TypewriterTMP textNode17_4_2;
    public TypewriterTMP textNode17_4_3;
    public TypewriterTMP textNode17_4_4;
    
    public TextMeshProUGUI myTextNode17_4_1;
    public TextMeshProUGUI myTextNode17_4_2;
    public TextMeshProUGUI myTextNode17_4_3;
    public TextMeshProUGUI myTextNode17_4_4;
    
       //////////////////// AUDIOS ////////////////////////
    [Header("Audio Sources")]
    public AudioSource[] sources;

    private Dictionary<string, AudioSource> audioDict;


    void Awake()
    {
        audioDict = new Dictionary<string, AudioSource>();
        if (sources != null)
        {
            foreach (AudioSource src in sources)
            {
                if (src != null)
                    audioDict[src.gameObject.name] = src;
            }
        }

    }
    
    public void PlayByName(string name)
    {
        if (audioDict.ContainsKey(name))
            audioDict[name].Play();
        else
            Debug.LogWarning("No AudioSource found with name: " + name);
    }

    public void StopByName(string name)
    {
        if (audioDict.ContainsKey(name))
            audioDict[name].Stop();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(flagJustOneTime == false){

            StartCoroutine(AdvancingTimerNode17_1());
            flagJustOneTime = true;
        }
    }

    IEnumerator AdvancingTimerNode17_1()
    {
        videoPlayerNode17_1a.Stop();
        GO_Node17_1a.SetActive(true);
        GO_CanvaNode17_1a.SetActive(true);

        videoPlayerNode17_1a.Stop();
        videoPlayerNode17_1a.time = 0;        
        videoPlayerNode17_1a.frame = 0;
        videoPlayerNode17_1a.Play();

        yield return new WaitForSeconds(3f);

        videoPlayerNode17_1a.Stop();
        GO_Node17_1a.SetActive(false);
        GO_CanvaNode17_1a.SetActive(false);

        videoPlayerNode17_1b.Stop();
        GO_Node17_1b.SetActive(true);
        GO_CanvaNode17_1b.SetActive(true);

        videoPlayerNode17_1b.Stop();
        videoPlayerNode17_1b.time = 0;        
        videoPlayerNode17_1b.frame = 0;
        videoPlayerNode17_1b.Play();

        myTextNode17_1_1.gameObject.SetActive(true);
        textNode17_1_1.StartTyping();
        PlayByName("17_1_1");
        
        yield return new WaitForSeconds(2.8f);
        
        myTextNode17_1_1.gameObject.SetActive(false);
        myTextNode17_1_2.gameObject.SetActive(true);
        textNode17_1_2.StartTyping();
        PlayByName("17_1_2");
        
        yield return new WaitForSeconds(5.3f);
        
        myTextNode17_1_2.gameObject.SetActive(false);
        myTextNode17_1_3.gameObject.SetActive(true);
        textNode17_1_3.StartTyping();
        PlayByName("17_1_3");

        yield return new WaitForSeconds(4.5f);
        
        myTextNode17_1_3.gameObject.SetActive(false);
        myTextNode17_1_4.gameObject.SetActive(true);
        textNode17_1_4.StartTyping();
        PlayByName("17_1_4");

        yield return new WaitForSeconds(3.2f);
        
        myTextNode17_1_4.gameObject.SetActive(false);
        myTextNode17_1_5.gameObject.SetActive(true);
        textNode17_1_5.StartTyping();
        PlayByName("17_1_5");

        yield return new WaitForSeconds(4f);

        StartCoroutine(AdvancingTimerNode17_2());
    }

    IEnumerator AdvancingTimerNode17_2()
    {
        videoPlayerNode17_1b.Stop();
        GO_Node17_1b.SetActive(false);
        GO_Node17_2a.SetActive(true);
        GO_CanvaNode17_2a.SetActive(true);

        videoPlayerNode17_2a.Stop();
        videoPlayerNode17_2a.time = 0;        
        videoPlayerNode17_2a.frame = 0;
        videoPlayerNode17_2a.Play();

        yield return new WaitForSeconds(3f);

        videoPlayerNode17_2a.Stop();
        GO_Node17_2a.SetActive(false);
        GO_CanvaNode17_2a.SetActive(false);

        videoPlayerNode17_1b.Stop();
        GO_Node17_1b.SetActive(false);
        GO_Node17_2b.SetActive(true);
        GO_CanvaNode17_2b.SetActive(true);

        videoPlayerNode17_2b.Stop();
        videoPlayerNode17_2b.time = 0;        
        videoPlayerNode17_2b.frame = 0;
        videoPlayerNode17_2b.Play();

        myTextNode17_2_1.gameObject.SetActive(true);
        textNode17_2_1.StartTyping();
        PlayByName("17_2_1");

        yield return new WaitForSeconds(1.8f);
        
        myTextNode17_2_1.gameObject.SetActive(false);
        myTextNode17_2_2.gameObject.SetActive(true);
        textNode17_2_2.StartTyping();
        PlayByName("17_2_2");

        yield return new WaitForSeconds(3.1f);
        
        myTextNode17_2_2.gameObject.SetActive(false);
        myTextNode17_2_3.gameObject.SetActive(true);
        textNode17_2_3.StartTyping();
        PlayByName("17_2_3");

        yield return new WaitForSeconds(4.6f);
        
        myTextNode17_2_3.gameObject.SetActive(false);
        myTextNode17_2_4.gameObject.SetActive(true);
        textNode17_2_4.StartTyping();
        PlayByName("17_2_4");

        yield return new WaitForSeconds(2.1f);
        
        myTextNode17_2_4.gameObject.SetActive(false);
        myTextNode17_2_5.gameObject.SetActive(true);
        textNode17_2_5.StartTyping();
        PlayByName("17_2_5");

        yield return new WaitForSeconds(3.6f);

        myTextNode17_2_5.gameObject.SetActive(false);
        myTextNode17_2_6.gameObject.SetActive(true);
        textNode17_2_6.StartTyping();
        PlayByName("17_2_6");

        yield return new WaitForSeconds(2.5f);

        StartCoroutine(AdvancingTimerNode17_3());
    }

    IEnumerator AdvancingTimerNode17_3()
    {
        videoPlayerNode17_2b.Stop();
        GO_Node17_2b.SetActive(false);
        GO_Node17_3a.SetActive(true);
        GO_CanvaNode17_3a.SetActive(true);

        videoPlayerNode17_3a.Stop();
        videoPlayerNode17_3a.time = 0;        
        videoPlayerNode17_3a.frame = 0;
        videoPlayerNode17_3a.Play();

        yield return new WaitForSeconds(3f);

        videoPlayerNode17_3a.Stop();
        GO_Node17_3a.SetActive(false);
        GO_CanvaNode17_3a.SetActive(false);

        GO_Node17_3b.SetActive(true);
        GO_CanvaNode17_3b.SetActive(true);

        videoPlayerNode17_3b.Stop();
        videoPlayerNode17_3b.time = 0;        
        videoPlayerNode17_3b.frame = 0;
        videoPlayerNode17_3b.Play();

        myTextNode17_3_1.gameObject.SetActive(true);
        textNode17_3_1.StartTyping();
        PlayByName("17_3_1");
        
        yield return new WaitForSeconds(1.7f);
        
        myTextNode17_3_1.gameObject.SetActive(false);
        myTextNode17_3_2.gameObject.SetActive(true);
        textNode17_3_2.StartTyping();
        PlayByName("17_3_2");
        
        yield return new WaitForSeconds(3f);
        
        myTextNode17_3_2.gameObject.SetActive(false);
        myTextNode17_3_3.gameObject.SetActive(true);
        textNode17_3_3.StartTyping();
        PlayByName("17_3_3");

        yield return new WaitForSeconds(4.5f);
        
        myTextNode17_3_3.gameObject.SetActive(false);
        myTextNode17_3_4.gameObject.SetActive(true);
        textNode17_3_4.StartTyping();
        PlayByName("17_3_4");

        yield return new WaitForSeconds(3f);
        
        myTextNode17_3_4.gameObject.SetActive(false);
        myTextNode17_3_5.gameObject.SetActive(true);
        textNode17_3_5.StartTyping();
        PlayByName("17_3_5");

        yield return new WaitForSeconds(3.9f);

        StartCoroutine(AdvancingTimerNode17_4());
    }

    IEnumerator AdvancingTimerNode17_4()
    {
        videoPlayerNode17_3b.Stop();
        GO_Node17_3b.SetActive(false);
        GO_Node17_4a.SetActive(true);
        GO_CanvaNode17_4a.SetActive(true);

        videoPlayerNode17_4a.Stop();
        videoPlayerNode17_4a.time = 0;        
        videoPlayerNode17_4a.frame = 0;
        videoPlayerNode17_4a.Play();

        yield return new WaitForSeconds(3f);

        videoPlayerNode17_4a.Stop();
        GO_Node17_4a.SetActive(false);
        GO_Node17_4b.SetActive(true);
        GO_CanvaNode17_4b.SetActive(true);

        videoPlayerNode17_4b.Stop();
        videoPlayerNode17_4b.time = 0;        
        videoPlayerNode17_4b.frame = 0;
        videoPlayerNode17_4b.Play();

        myTextNode17_4_1.gameObject.SetActive(true);
        textNode17_4_1.StartTyping();
        PlayByName("17_4_1");
        
        yield return new WaitForSeconds(4.3f);
        
        myTextNode17_4_1.gameObject.SetActive(false);
        myTextNode17_4_2.gameObject.SetActive(true);
        textNode17_4_2.StartTyping();
        PlayByName("17_4_2");
        
        yield return new WaitForSeconds(4f);
        
        myTextNode17_4_2.gameObject.SetActive(false);
        myTextNode17_4_3.gameObject.SetActive(true);
        textNode17_4_3.StartTyping();
        PlayByName("17_4_3");

        yield return new WaitForSeconds(4f);
        
        myTextNode17_4_3.gameObject.SetActive(false);
        myTextNode17_4_4.gameObject.SetActive(true);
        textNode17_4_4.StartTyping();
        PlayByName("17_4_4");

        yield return new WaitForSeconds(4.2f);
    }
}
