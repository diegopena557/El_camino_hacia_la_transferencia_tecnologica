using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TimerTextNode11 : MonoBehaviour
{
    private bool flagJustOneTime;

    public GameObject GO_Node11_1;
    public GameObject GO_Node11_2;
    public GameObject GO_Node11_3;
    public GameObject GO_Node11_5;

    public GameObject GO_CanvaNode11_1;
    public GameObject GO_CanvaNode11_2;
    public GameObject GO_CanvaNode11_3;
    public GameObject GO_CanvaNode11_5;

    public VideoPlayer videoPlayerNode11_1;
    public VideoPlayer videoPlayerNode11_3;
    public VideoPlayer videoPlayerNode11_5;

    public TextMeshProUGUI myTextNode11_3_1;
    public TextMeshProUGUI myTextNode11_3_2;
    public TextMeshProUGUI myTextNode11_3_3;
    public TextMeshProUGUI myTextNode11_3_4;
    public TextMeshProUGUI myTextNode11_3_5;

    public TextMeshProUGUI myTextNode11_5_1;
    public TextMeshProUGUI myTextNode11_5_2;
    public TextMeshProUGUI myTextNode11_5_3;
    public TextMeshProUGUI myTextNode11_5_4;
    public TextMeshProUGUI myTextNode11_5_5;

    public TypewriterTMP textNode11_3_1;
    public TypewriterTMP textNode11_3_2;
    public TypewriterTMP textNode11_3_3;
    public TypewriterTMP textNode11_3_4;
    public TypewriterTMP textNode11_3_5;

    public TypewriterTMP textNode11_5_1;
    public TypewriterTMP textNode11_5_2;
    public TypewriterTMP textNode11_5_3;
    public TypewriterTMP textNode11_5_4;
    public TypewriterTMP textNode11_5_5;

    public bool flagContinue3;
    public bool flagBoton1;
    public bool flagBoton2;
    public bool flagBoton3;
    public bool flagBoton4;

    //////////////////// FADE IN ////////////////////////
    [Header("Fade In Settings")]
    public Image fadePanel;
    public float fadeInDuration = 1.5f;

    //////////////////// FADE OUT ////////////////////////
    [Header("Fade Out Settings")]
    public float fadeOutDuration = 1.5f;

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
                audioDict[src.gameObject.name] = src;
        }

        if (fadePanel != null)
        {
            Color c = fadePanel.color;
            c.a = 1f;
            fadePanel.color = c;
            fadePanel.gameObject.SetActive(true);
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

    void Start() 
    { 
        flagContinue3 = false;

        flagBoton1 = false;
        flagBoton2 = false;
        flagBoton3 = false;
        flagBoton4 = false;
    }

    public void SetValueBoton1(){
        flagBoton1 = true;
    }

    public void SetValueBoton2(){
        flagBoton2 = true;
    }

    public void SetValueBoton3(){
        flagBoton3 = true;
    }

    public void SetValueBoton4(){
        flagBoton4 = true;
    }

    void Update()
    {
        if (flagJustOneTime == false)
        {
            StartCoroutine(AdvancingTimerNode11_1());
            flagJustOneTime = true;
        }

        if(flagBoton1 == true && flagBoton2 == true && flagBoton3 == true && flagBoton4 == true){
            flagContinue3 = true;
        }

        if(flagContinue3 == true){
            flagContinue3 = false;
            flagBoton1 = false;
            flagBoton2 = false;
            flagBoton3 = false;
            flagBoton4 = false;

            StartCoroutine(AdvancingTimerNode11_3());
        }
    }

    IEnumerator AdvancingTimerNode11_1()
    {
        GO_Node11_1.SetActive(true);
        GO_CanvaNode11_1.SetActive(true);

        videoPlayerNode11_1.Stop();
        videoPlayerNode11_1.time = 0;
        videoPlayerNode11_1.frame = 0;
        videoPlayerNode11_1.Play();

        // Fade in de entrada
        if (fadePanel != null)
            yield return StartCoroutine(FadeInEntrance());

        yield return new WaitForSeconds(10f);
        yield return StartCoroutine(AdvancingTimerNode11_2());
    }

    IEnumerator AdvancingTimerNode11_2()
    {
        GO_Node11_2.SetActive(true);
        GO_CanvaNode11_2.SetActive(true);

        GO_Node11_1.SetActive(false);
        GO_CanvaNode11_1.SetActive(false);

        videoPlayerNode11_1.Stop();

        yield return new WaitForSeconds(1f);
    }

    IEnumerator AdvancingTimerNode11_3()
    {
        GO_Node11_3.SetActive(true);
        GO_CanvaNode11_3.SetActive(true);

        GO_Node11_2.SetActive(false);
        GO_CanvaNode11_2.SetActive(false);

        videoPlayerNode11_1.Stop();

        videoPlayerNode11_3.Stop();
        videoPlayerNode11_3.time = 0;
        videoPlayerNode11_3.frame = 0;
        videoPlayerNode11_3.Play();

        yield return new WaitForSeconds(1f);
        myTextNode11_3_1.gameObject.SetActive(true);
        textNode11_3_1.StartTyping();
        PlayByName("11_3_1");

        yield return new WaitForSeconds(7.68f);
        myTextNode11_3_1.gameObject.SetActive(false);
        myTextNode11_3_2.gameObject.SetActive(true);
        textNode11_3_2.StartTyping();
        PlayByName("11_3_2");

        yield return new WaitForSeconds(4.52f);
        myTextNode11_3_2.gameObject.SetActive(false);
        myTextNode11_3_3.gameObject.SetActive(true);
        textNode11_3_3.StartTyping();
        PlayByName("11_3_3");

        yield return new WaitForSeconds(5.96f);
        myTextNode11_3_3.gameObject.SetActive(false);
        myTextNode11_3_4.gameObject.SetActive(true);
        textNode11_3_4.StartTyping();
        PlayByName("11_3_4");

        yield return new WaitForSeconds(5.172f);
        myTextNode11_3_4.gameObject.SetActive(false);
        myTextNode11_3_5.gameObject.SetActive(true);
        textNode11_3_5.StartTyping();
        PlayByName("11_3_5");

        yield return new WaitForSeconds(6.713f);
        yield return StartCoroutine(AdvancingTimerNode11_5());
    }

    IEnumerator AdvancingTimerNode11_5()
    {
        GO_Node11_5.SetActive(true);
        GO_CanvaNode11_5.SetActive(true);

        GO_Node11_3.SetActive(false);
        GO_CanvaNode11_3.SetActive(false);

        videoPlayerNode11_3.Stop();

        videoPlayerNode11_5.Stop();
        videoPlayerNode11_5.time = 0;
        videoPlayerNode11_5.frame = 0;
        videoPlayerNode11_5.Play();

        yield return new WaitForSeconds(1f);
        myTextNode11_5_1.gameObject.SetActive(true);
        textNode11_5_1.StartTyping();
        PlayByName("11_5_1");

        yield return new WaitForSeconds(4.885f);
        myTextNode11_5_1.gameObject.SetActive(false);
        myTextNode11_5_2.gameObject.SetActive(true);
        textNode11_5_2.StartTyping();
        PlayByName("11_5_2");

        yield return new WaitForSeconds(3.19f);
        myTextNode11_5_2.gameObject.SetActive(false);
        myTextNode11_5_3.gameObject.SetActive(true);
        textNode11_5_3.StartTyping();
        PlayByName("11_5_3");

        yield return new WaitForSeconds(2.612f);
        myTextNode11_5_3.gameObject.SetActive(false);
        myTextNode11_5_4.gameObject.SetActive(true);
        textNode11_5_4.StartTyping();
        PlayByName("11_5_4");

        yield return new WaitForSeconds(4.65f);
        myTextNode11_5_4.gameObject.SetActive(false);
        myTextNode11_5_5.gameObject.SetActive(true);
        textNode11_5_5.StartTyping();
        PlayByName("11_5_5");

        yield return new WaitForSeconds(5.695f);
        myTextNode11_5_5.gameObject.SetActive(false);

        // Fade out y carga de escena
        yield return StartCoroutine(FadeOutToBlack());
        LoadNextScene();
    }

    // Fade in de entrada: negro a transparente
    IEnumerator FadeInEntrance()
    {
        float elapsed = 0f;
        Color c = fadePanel.color;

        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, elapsed / fadeInDuration);
            fadePanel.color = c;
            yield return null;
        }

        c.a = 0f;
        fadePanel.color = c;
        fadePanel.gameObject.SetActive(false);
    }

    // Fade out de salida: transparente a negro
    IEnumerator FadeOutToBlack()
    {
        fadePanel.gameObject.SetActive(true);
        Color c = fadePanel.color;
        c.a = 0f;
        fadePanel.color = c;

        float elapsed = 0f;

        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, elapsed / fadeOutDuration);
            fadePanel.color = c;
            yield return null;
        }

        c.a = 1f;
        fadePanel.color = c;
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene("Dialogue12");
    }
}