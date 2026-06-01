using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TimerTextNode12 : MonoBehaviour
{
    private bool flagJustOneTime;

    public GameObject GO_CanvaNode12_1;
    public GameObject GO_CanvaNode12_2;
    public GameObject GO_CanvaNode12_3;
    public GameObject GO_CanvaNode12_4;
    public GameObject GO_CanvaNode12_5;
    public GameObject GO_CanvaNode12_6;

    public GameObject GO_Node12_1;
    public GameObject GO_Node12_2;
    public GameObject GO_Node12_3;
    public GameObject GO_Node12_4;
    public GameObject GO_Node12_5;
    public GameObject GO_Node12_6;

    public VideoPlayer videoPlayerNode12_1;
    public VideoPlayer videoPlayerNode12_2;
    public VideoPlayer videoPlayerNode12_3;
    public VideoPlayer videoPlayerNode12_4;
    public VideoPlayer videoPlayerNode12_5;
    public VideoPlayer videoPlayerNode12_6;

    public TextMeshProUGUI myTextNode12_1_1;
    public TextMeshProUGUI myTextNode12_1_2;
    public TextMeshProUGUI myTextNode12_1_3;
    public TextMeshProUGUI myTextNode12_1_4;

    public TypewriterTMP textNode12_1_1;
    public TypewriterTMP textNode12_1_2;
    public TypewriterTMP textNode12_1_3;
    public TypewriterTMP textNode12_1_4;

    public TextMeshProUGUI myTextNode12_3_1;
    public TextMeshProUGUI myTextNode12_3_2;
    public TextMeshProUGUI myTextNode12_3_3;
    public TextMeshProUGUI myTextNode12_3_4;

    public TypewriterTMP textNode12_3_1;
    public TypewriterTMP textNode12_3_2;
    public TypewriterTMP textNode12_3_3;
    public TypewriterTMP textNode12_3_4;

    public TextMeshProUGUI myTextNode12_4_1;
    public TextMeshProUGUI myTextNode12_4_2;
    public TextMeshProUGUI myTextNode12_4_3;
    public TextMeshProUGUI myTextNode12_4_4;

    public TypewriterTMP textNode12_4_1;
    public TypewriterTMP textNode12_4_2;
    public TypewriterTMP textNode12_4_3;
    public TypewriterTMP textNode12_4_4;

    public TextMeshProUGUI myTextNode12_5_1;
    public TextMeshProUGUI myTextNode12_5_2;
    public TextMeshProUGUI myTextNode12_5_3;
    public TextMeshProUGUI myTextNode12_5_4;

    public TypewriterTMP textNode12_5_1;
    public TypewriterTMP textNode12_5_2;
    public TypewriterTMP textNode12_5_3;
    public TypewriterTMP textNode12_5_4;

    public TextMeshProUGUI myTextNode12_6_1;
    public TextMeshProUGUI myTextNode12_6_2;
    public TextMeshProUGUI myTextNode12_6_3;

    public TypewriterTMP textNode12_6_1;
    public TypewriterTMP textNode12_6_2;
    public TypewriterTMP textNode12_6_3;

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

    void Start() { }

    void Update()
    {
        if (flagJustOneTime == false)
        {
            StartCoroutine(AdvancingTimerNode12_1());
            flagJustOneTime = true;
        }
    }

    IEnumerator AdvancingTimerNode12_1()
    {
        GO_Node12_1.SetActive(true);
        GO_CanvaNode12_1.SetActive(true);

        videoPlayerNode12_1.Stop();
        videoPlayerNode12_1.time = 0;
        videoPlayerNode12_1.frame = 0;
        videoPlayerNode12_1.Play();

        // Fade in de entrada
        if (fadePanel != null)
            yield return StartCoroutine(FadeInEntrance());

        yield return new WaitForSeconds(1f);
        myTextNode12_1_1.gameObject.SetActive(true);
        textNode12_1_1.StartTyping();
        PlayByName("12a_1");

        yield return new WaitForSeconds(4.05f);
        myTextNode12_1_1.gameObject.SetActive(false);
        myTextNode12_1_2.gameObject.SetActive(true);
        textNode12_1_2.StartTyping();
        PlayByName("12a_2");

        yield return new WaitForSeconds(2.9f);
        myTextNode12_1_2.gameObject.SetActive(false);
        myTextNode12_1_3.gameObject.SetActive(true);
        textNode12_1_3.StartTyping();
        PlayByName("12a_3");

        yield return new WaitForSeconds(4.7f);
        myTextNode12_1_3.gameObject.SetActive(false);
        myTextNode12_1_4.gameObject.SetActive(true);
        textNode12_1_4.StartTyping();
        PlayByName("12a_4");

        yield return new WaitForSeconds(1.7f);
        yield return StartCoroutine(AdvancingTimerNode12_2());
    }

    IEnumerator AdvancingTimerNode12_2()
    {
        GO_Node12_2.SetActive(true);
        GO_CanvaNode12_2.SetActive(true);

        GO_Node12_1.SetActive(false);
        GO_CanvaNode12_1.SetActive(false);

        videoPlayerNode12_1.Stop();

        videoPlayerNode12_2.Stop();
        videoPlayerNode12_2.time = 0;
        videoPlayerNode12_2.frame = 0;
        videoPlayerNode12_2.Play();

        yield return new WaitForSeconds(10f);
        yield return StartCoroutine(AdvancingTimerNode12_3());
    }

    IEnumerator AdvancingTimerNode12_3()
    {
        GO_Node12_3.SetActive(true);
        GO_CanvaNode12_3.SetActive(true);

        GO_Node12_2.SetActive(false);
        GO_CanvaNode12_2.SetActive(false);

        videoPlayerNode12_2.Stop();

        videoPlayerNode12_3.Stop();
        videoPlayerNode12_3.time = 0;
        videoPlayerNode12_3.frame = 0;
        videoPlayerNode12_3.Play();

        yield return new WaitForSeconds(1f);
        myTextNode12_3_1.gameObject.SetActive(true);
        textNode12_3_1.StartTyping();
        PlayByName("12c_1");

        yield return new WaitForSeconds(7.13f);
        myTextNode12_3_1.gameObject.SetActive(false);
        myTextNode12_3_2.gameObject.SetActive(true);
        textNode12_3_2.StartTyping();
        PlayByName("12c_2");

        yield return new WaitForSeconds(5.77f);
        myTextNode12_3_2.gameObject.SetActive(false);
        myTextNode12_3_3.gameObject.SetActive(true);
        textNode12_3_3.StartTyping();
        PlayByName("12c_3");

        yield return new WaitForSeconds(3.08f);
        myTextNode12_3_3.gameObject.SetActive(false);
        myTextNode12_3_4.gameObject.SetActive(true);
        textNode12_3_4.StartTyping();
        PlayByName("12c_4");

        yield return new WaitForSeconds(2.66f);
        yield return StartCoroutine(AdvancingTimerNode12_4());
    }

    IEnumerator AdvancingTimerNode12_4()
    {
        GO_Node12_4.SetActive(true);
        GO_CanvaNode12_4.SetActive(true);

        GO_Node12_3.SetActive(false);
        GO_CanvaNode12_3.SetActive(false);

        videoPlayerNode12_3.Stop();

        videoPlayerNode12_4.Stop();
        videoPlayerNode12_4.time = 0;
        videoPlayerNode12_4.frame = 0;
        videoPlayerNode12_4.Play();

        yield return new WaitForSeconds(1f);
        myTextNode12_4_1.gameObject.SetActive(true);
        textNode12_4_1.StartTyping();
        PlayByName("12d_1");

        yield return new WaitForSeconds(6.79f);
        myTextNode12_4_1.gameObject.SetActive(false);
        myTextNode12_4_2.gameObject.SetActive(true);
        textNode12_4_2.StartTyping();
        PlayByName("12d_2");

        yield return new WaitForSeconds(4.99f);
        myTextNode12_4_2.gameObject.SetActive(false);
        myTextNode12_4_3.gameObject.SetActive(true);
        textNode12_4_3.StartTyping();
        PlayByName("12d_3");

        yield return new WaitForSeconds(3.6f);
        myTextNode12_4_3.gameObject.SetActive(false);
        myTextNode12_4_4.gameObject.SetActive(true);
        textNode12_4_4.StartTyping();
        PlayByName("12d_4");

        yield return new WaitForSeconds(4.86f);
        yield return StartCoroutine(AdvancingTimerNode12_5());
    }

    IEnumerator AdvancingTimerNode12_5()
    {
        GO_Node12_5.SetActive(true);
        GO_CanvaNode12_5.SetActive(true);

        GO_Node12_4.SetActive(false);
        GO_CanvaNode12_4.SetActive(false);

        videoPlayerNode12_4.Stop();

        videoPlayerNode12_5.Stop();
        videoPlayerNode12_5.time = 0;
        videoPlayerNode12_5.frame = 0;
        videoPlayerNode12_5.Play();

        yield return new WaitForSeconds(1f);
        myTextNode12_5_1.gameObject.SetActive(true);
        textNode12_5_1.StartTyping();
        PlayByName("12e_1");

        yield return new WaitForSeconds(4.94f);
        myTextNode12_5_1.gameObject.SetActive(false);
        myTextNode12_5_2.gameObject.SetActive(true);
        textNode12_5_2.StartTyping();
        PlayByName("12e_2");

        yield return new WaitForSeconds(5.04f);
        myTextNode12_5_2.gameObject.SetActive(false);
        myTextNode12_5_3.gameObject.SetActive(true);
        textNode12_5_3.StartTyping();
        PlayByName("12e_3");

        yield return new WaitForSeconds(4.34f);
        myTextNode12_5_3.gameObject.SetActive(false);
        myTextNode12_5_4.gameObject.SetActive(true);
        textNode12_5_4.StartTyping();
        PlayByName("12e_4");

        yield return new WaitForSeconds(4.89f);
        yield return StartCoroutine(AdvancingTimerNode12_6());
    }

    IEnumerator AdvancingTimerNode12_6()
    {
        GO_Node12_6.SetActive(true);
        GO_CanvaNode12_6.SetActive(true);

        GO_Node12_5.SetActive(false);
        GO_CanvaNode12_5.SetActive(false);

        videoPlayerNode12_5.Stop();

        videoPlayerNode12_6.Stop();
        videoPlayerNode12_6.time = 0;
        videoPlayerNode12_6.frame = 0;
        videoPlayerNode12_6.Play();

        yield return new WaitForSeconds(1f);
        myTextNode12_6_1.gameObject.SetActive(true);
        textNode12_6_1.StartTyping();
        PlayByName("12f_1");

        yield return new WaitForSeconds(4.28f);
        myTextNode12_6_1.gameObject.SetActive(false);
        myTextNode12_6_2.gameObject.SetActive(true);
        textNode12_6_2.StartTyping();
        PlayByName("12f_2");

        yield return new WaitForSeconds(3.74f);
        myTextNode12_6_2.gameObject.SetActive(false);
        myTextNode12_6_3.gameObject.SetActive(true);
        textNode12_6_3.StartTyping();
        PlayByName("12f_3");

        yield return new WaitForSeconds(3.32f);
        myTextNode12_6_3.gameObject.SetActive(false);

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
        SceneManager.LoadScene("IsLumemReady");
    }
}