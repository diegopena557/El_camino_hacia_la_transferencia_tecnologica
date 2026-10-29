using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


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

        // El panel de fade empieza completamente opaco (negro)
        if (fadePanel != null)
        {
            Color c = fadePanel.color;
            c.a = 1f;
            fadePanel.color = c;
            fadePanel.gameObject.SetActive(true);
        }
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
    // Prepara el video (si hace falta), espera su primer cuadro y lo reproduce.
    // Los objetos de objectsToHide (canvas/nodo anterior) se ocultan DESPUES de que el
    // nuevo video ya tiene imagen, para que nunca quede la pantalla vacia (flash azul).
    private IEnumerator PlayVideoWhenReady(VideoPlayer vp, params GameObject[] objectsToHide)
    {
        if (vp != null)
        {
            if (!vp.isPrepared)
            {
                vp.Prepare();
                while (!vp.isPrepared)
                    yield return null;
            }
            else
            {
                // Ya estaba preparado (repeticiones): se reinicia sin Stop()
                // para no limpiar la textura.
                vp.time = 0;
            }

            vp.Play();
        }

        if (objectsToHide != null)
        {
            foreach (GameObject go in objectsToHide)
            {
                if (go != null)
                    go.SetActive(false);
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (flagJustOneTime == false)
        {

            StartCoroutine(AdvancingTimerNode17_1());
            flagJustOneTime = true;
        }
    }

    IEnumerator AdvancingTimerNode17_1()
    {
        GO_Node17_1a.SetActive(true);
        GO_CanvaNode17_1a.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayerNode17_1a);

        // Fade in de entrada
        if (fadePanel != null)
            yield return StartCoroutine(FadeInEntrance());

        yield return new WaitForSeconds(3f);

        GO_Node17_1b.SetActive(true);
        GO_CanvaNode17_1b.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayerNode17_1b, GO_CanvaNode17_1a, GO_Node17_1a);
        videoPlayerNode17_1a.Stop();

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
        GO_Node17_2a.SetActive(true);
        GO_CanvaNode17_2a.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayerNode17_2a, GO_Node17_1b);
        videoPlayerNode17_1b.Stop();

        yield return new WaitForSeconds(3f);

        GO_Node17_2b.SetActive(true);
        GO_CanvaNode17_2b.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayerNode17_2b, GO_CanvaNode17_2a, GO_Node17_2a);
        videoPlayerNode17_2a.Stop();

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
        GO_Node17_3a.SetActive(true);
        GO_CanvaNode17_3a.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayerNode17_3a, GO_Node17_2b);
        videoPlayerNode17_2b.Stop();

        yield return new WaitForSeconds(3f);

        GO_Node17_3b.SetActive(true);
        GO_CanvaNode17_3b.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayerNode17_3b, GO_CanvaNode17_3a, GO_Node17_3a);
        videoPlayerNode17_3a.Stop();

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
        GO_Node17_4a.SetActive(true);
        GO_CanvaNode17_4a.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayerNode17_4a, GO_Node17_3b);
        videoPlayerNode17_3b.Stop();

        yield return new WaitForSeconds(3f);

        GO_Node17_4b.SetActive(true);
        GO_CanvaNode17_4b.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayerNode17_4b, GO_Node17_4a);
        videoPlayerNode17_4a.Stop();

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
        myTextNode17_4_4.gameObject.SetActive(false);

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
        SceneManager.LoadScene("Dialogue18");
    }
}