using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections.Generic; //Necesario para los audios

public class TimerTextNode9 : MonoBehaviour
{
    private bool flagJustOneTime;

    public GameObject GO_Node9_1;
    public GameObject GO_Node9_2;
    public GameObject GO_Node9_3;
    public GameObject GO_Node9_4;
    public GameObject GO_Node9_5;

    public GameObject GO_CanvaNode9_1;
    public GameObject GO_CanvaNode9_2;
    public GameObject GO_CanvaNode9_3;
    public GameObject GO_CanvaNode9_4;
    public GameObject GO_CanvaNode9_5;

    public VideoPlayer videoPlayerNode9_1;
    public VideoPlayer videoPlayerNode9_2;
    public VideoPlayer videoPlayerNode9_3;
    public VideoPlayer videoPlayerNode9_4;
    public VideoPlayer videoPlayerNode9_5;

    public TextMeshProUGUI myTextNode9_1_1;
    public TextMeshProUGUI myTextNode9_1_2;
    public TextMeshProUGUI myTextNode9_1_3;
    public TextMeshProUGUI myTextNode9_1_4;
    public TextMeshProUGUI myTextNode9_1_5;

    public TextMeshProUGUI myTextNode9_2_1;
    public TextMeshProUGUI myTextNode9_2_2;
    public TextMeshProUGUI myTextNode9_2_3;

    public TextMeshProUGUI myTextNode9_3_1;
    public TextMeshProUGUI myTextNode9_3_2;
    public TextMeshProUGUI myTextNode9_3_3;
    public TextMeshProUGUI myTextNode9_3_4;

    public TextMeshProUGUI myTextNode9_4_1;

    public TextMeshProUGUI myTextNode9_5_1;
    public TextMeshProUGUI myTextNode9_5_2;
    public TextMeshProUGUI myTextNode9_5_3;
    public TextMeshProUGUI myTextNode9_5_4;

    public TypewriterTMP textNode9_1_1;
    public TypewriterTMP textNode9_1_2;
    public TypewriterTMP textNode9_1_3;
    public TypewriterTMP textNode9_1_4;
    public TypewriterTMP textNode9_1_5;

    public TypewriterTMP textNode9_2_1;
    public TypewriterTMP textNode9_2_2;
    public TypewriterTMP textNode9_2_3;

    public TypewriterTMP textNode9_3_1;
    public TypewriterTMP textNode9_3_2;
    public TypewriterTMP textNode9_3_3;
    public TypewriterTMP textNode9_3_4;

    public TypewriterTMP textNode9_4_1;

    public TypewriterTMP textNode9_5_1;
    public TypewriterTMP textNode9_5_2;
    public TypewriterTMP textNode9_5_3;
    public TypewriterTMP textNode9_5_4;

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
            StartCoroutine(AdvancingTimerNode9_1());
            flagJustOneTime = true;
        }
    }

    IEnumerator AdvancingTimerNode9_1()
    {
        GO_Node9_1.SetActive(true);
        GO_CanvaNode9_1.SetActive(true);

        videoPlayerNode9_1.Stop();
        videoPlayerNode9_1.time = 0;        
        videoPlayerNode9_1.frame = 0;
        videoPlayerNode9_1.Play();

        yield return new WaitForSeconds(1f);
        myTextNode9_1_1.gameObject.SetActive(true);
        textNode9_1_1.StartTyping();
        PlayByName("9_1_1");
        
        yield return new WaitForSeconds(1.73f);
        myTextNode9_1_1.gameObject.SetActive(false);
        myTextNode9_1_2.gameObject.SetActive(true);
        textNode9_1_2.StartTyping();
        PlayByName("9_1_2");
        
        yield return new WaitForSeconds(3.32f);
        myTextNode9_1_2.gameObject.SetActive(false);
        myTextNode9_1_3.gameObject.SetActive(true);
        textNode9_1_3.StartTyping();
        PlayByName("9_1_3");

        yield return new WaitForSeconds(2.7f);
        myTextNode9_1_3.gameObject.SetActive(false);
        myTextNode9_1_4.gameObject.SetActive(true);
        textNode9_1_4.StartTyping();
        PlayByName("9_1_4");

        yield return new WaitForSeconds(6.3f);
        myTextNode9_1_4.gameObject.SetActive(false);
        myTextNode9_1_5.gameObject.SetActive(true);
        textNode9_1_5.StartTyping();
        PlayByName("9_1_5");

        yield return new WaitForSeconds(1f);
        StartCoroutine(AdvancingTimerNode9_2());
    }

    IEnumerator AdvancingTimerNode9_2()
    {
        GO_Node9_2.SetActive(true);
        GO_CanvaNode9_2.SetActive(true);

        GO_Node9_1.SetActive(false);
        GO_CanvaNode9_1.SetActive(false);

        videoPlayerNode9_1.Stop();

        videoPlayerNode9_2.Stop();
        videoPlayerNode9_2.time = 0;        
        videoPlayerNode9_2.frame = 0;
        videoPlayerNode9_2.Play();

        yield return new WaitForSeconds(1f);
        myTextNode9_2_1.gameObject.SetActive(true);
        textNode9_2_1.StartTyping();
        PlayByName("9_2_1");

        yield return new WaitForSeconds(2.97f);
        myTextNode9_2_1.gameObject.SetActive(false);
        myTextNode9_2_2.gameObject.SetActive(true);
        textNode9_2_2.StartTyping();
        PlayByName("9_2_2");
        
        yield return new WaitForSeconds(5.18f);
        myTextNode9_2_2.gameObject.SetActive(false);
        myTextNode9_2_3.gameObject.SetActive(true);
        textNode9_2_3.StartTyping();
        PlayByName("9_2_3");

        yield return new WaitForSeconds(7.45f);
        StartCoroutine(AdvancingTimerNode9_3());
    }

    IEnumerator AdvancingTimerNode9_3()
    {
        GO_Node9_3.SetActive(true);
        GO_CanvaNode9_3.SetActive(true);

        GO_Node9_2.SetActive(false);
        GO_CanvaNode9_2.SetActive(false);

        videoPlayerNode9_2.Stop();

        videoPlayerNode9_3.Stop();
        videoPlayerNode9_3.time = 0;        
        videoPlayerNode9_3.frame = 0;
        videoPlayerNode9_3.Play();

        yield return new WaitForSeconds(1f);
        myTextNode9_3_1.gameObject.SetActive(true);
        textNode9_3_1.StartTyping();
        PlayByName("9_3_1");

        yield return new WaitForSeconds(1.65f);
        myTextNode9_3_1.gameObject.SetActive(false);
        myTextNode9_3_2.gameObject.SetActive(true);
        textNode9_3_2.StartTyping();
        PlayByName("9_3_2");
        
        yield return new WaitForSeconds(10.97f);
        myTextNode9_3_2.gameObject.SetActive(false);
        myTextNode9_3_3.gameObject.SetActive(true);
        textNode9_3_3.StartTyping();
        PlayByName("9_3_3");

        yield return new WaitForSeconds(9.3f);
        myTextNode9_3_3.gameObject.SetActive(false);
        myTextNode9_3_4.gameObject.SetActive(true);
        textNode9_3_4.StartTyping();
        PlayByName("9_3_4");

        yield return new WaitForSeconds(8f);
        StartCoroutine(AdvancingTimerNode9_4());
    }

    IEnumerator AdvancingTimerNode9_4()
    {
        GO_Node9_4.SetActive(true);
        GO_CanvaNode9_4.SetActive(true);

        GO_Node9_3.SetActive(false);
        GO_CanvaNode9_3.SetActive(false);

        videoPlayerNode9_3.Stop();

        videoPlayerNode9_4.Stop();
        videoPlayerNode9_4.time = 0;        
        videoPlayerNode9_4.frame = 0;
        videoPlayerNode9_4.Play();

        yield return new WaitForSeconds(1f);
        myTextNode9_4_1.gameObject.SetActive(true);
        textNode9_4_1.StartTyping();
        PlayByName("9_4");

        yield return new WaitForSeconds(3.74f);
        StartCoroutine(AdvancingTimerNode9_5());
    }

    IEnumerator AdvancingTimerNode9_5()
    {
        GO_Node9_5.SetActive(true);
        GO_CanvaNode9_5.SetActive(true);

        GO_Node9_4.SetActive(false);
        GO_CanvaNode9_4.SetActive(false);

        videoPlayerNode9_4.Stop();

        videoPlayerNode9_5.Stop();
        videoPlayerNode9_5.time = 0;        
        videoPlayerNode9_5.frame = 0;
        videoPlayerNode9_5.Play();

        yield return new WaitForSeconds(1f);
        myTextNode9_5_1.gameObject.SetActive(true);
        textNode9_5_1.StartTyping();
        PlayByName("9_5_1");
        
        yield return new WaitForSeconds(2.77f);
        myTextNode9_5_1.gameObject.SetActive(false);
        myTextNode9_5_2.gameObject.SetActive(true);
        textNode9_5_2.StartTyping();
        PlayByName("9_5_2");
        
        yield return new WaitForSeconds(5.83f);
        myTextNode9_5_2.gameObject.SetActive(false);
        myTextNode9_5_3.gameObject.SetActive(true);
        textNode9_5_3.StartTyping();
        PlayByName("9_5_3");

        yield return new WaitForSeconds(3.22f);
        myTextNode9_5_3.gameObject.SetActive(false);
        myTextNode9_5_4.gameObject.SetActive(true);
        textNode9_5_4.StartTyping();
        PlayByName("9_5_4");
    }
}
