using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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

        // El panel de fade in empieza completamente opaco (negro)
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

    void Start() { }

    void Update()
    {
        if (flagJustOneTime == false)
        {
            StartCoroutine(AdvancingTimerNode9_1());
            flagJustOneTime = true;
        }
    }

    IEnumerator AdvancingTimerNode9_1()
    {
        GO_Node9_1.SetActive(true);
        GO_CanvaNode9_1.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayerNode9_1);

        // Fade in de entrada
        if (fadePanel != null)
            yield return StartCoroutine(FadeInEntrance());

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
        yield return StartCoroutine(AdvancingTimerNode9_2());
    }

    IEnumerator AdvancingTimerNode9_2()
    {
        GO_Node9_2.SetActive(true);
        GO_CanvaNode9_2.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayerNode9_2, GO_CanvaNode9_1, GO_Node9_1);
        videoPlayerNode9_1.Stop();

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
        yield return StartCoroutine(AdvancingTimerNode9_3());
    }

    IEnumerator AdvancingTimerNode9_3()
    {
        GO_Node9_3.SetActive(true);
        GO_CanvaNode9_3.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayerNode9_3, GO_CanvaNode9_2, GO_Node9_2);
        videoPlayerNode9_2.Stop();

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
        yield return StartCoroutine(AdvancingTimerNode9_4());
    }

    IEnumerator AdvancingTimerNode9_4()
    {
        GO_Node9_4.SetActive(true);
        GO_CanvaNode9_4.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayerNode9_4, GO_CanvaNode9_3, GO_Node9_3);
        videoPlayerNode9_3.Stop();

        yield return new WaitForSeconds(1f);
        myTextNode9_4_1.gameObject.SetActive(true);
        textNode9_4_1.StartTyping();
        PlayByName("9_4");

        yield return new WaitForSeconds(3.74f);
        yield return StartCoroutine(AdvancingTimerNode9_5());
    }

    IEnumerator AdvancingTimerNode9_5()
    {
        GO_Node9_5.SetActive(true);
        GO_CanvaNode9_5.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayerNode9_5, GO_CanvaNode9_4, GO_Node9_4);
        videoPlayerNode9_4.Stop();

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

        yield return new WaitForSeconds(10f); // Ajusta al tiempo real del ultimo audio
        myTextNode9_5_4.gameObject.SetActive(false);

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
        SceneManager.LoadScene("Froggerlike");
    }
}