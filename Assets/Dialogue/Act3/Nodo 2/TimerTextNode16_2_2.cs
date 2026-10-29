using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TimerTextNode16_2_2 : MonoBehaviour
{
    private bool flagJustOneTime;

    public GameObject GO_CanvaNode16_2_1;
    public GameObject GO_CanvaNode16_2_2;

    public GameObject GO_Node16_2_1;
    public GameObject GO_Node16_2_2;

    public VideoPlayer videoPlayerNode16_2_1;
    public VideoPlayer videoPlayerNode16_2_2;

    public TextMeshProUGUI myTextNode16_2_1;
    public TextMeshProUGUI myTextNode16_2_2;
    public TextMeshProUGUI myTextNode16_2_3;
    public TextMeshProUGUI myTextNode16_2_4;

    public TextMeshProUGUI myTextNode16_2_1_b;
    public TextMeshProUGUI myTextNode16_2_2_b;

    public TypewriterTMP textNode16_2_1;
    public TypewriterTMP textNode16_2_2;
    public TypewriterTMP textNode16_2_3;
    public TypewriterTMP textNode16_2_4;

    public TypewriterTMP textNode16_2_1_b;
    public TypewriterTMP textNode16_2_2_b;

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
    // Si se indica canvasToHide, ese nodo/canvas anterior se oculta DESPUES de que el
    // nuevo video ya tiene imagen, para que nunca quede la pantalla vacia (flash azul).
    private IEnumerator PlayVideoWhenReady(VideoPlayer vp, GameObject canvasToHide = null)
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

        if (canvasToHide != null)
            canvasToHide.SetActive(false);
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
            StartCoroutine(AdvancingTimerNode16_2_1());

            flagJustOneTime = true;
        }
    }

    IEnumerator AdvancingTimerNode16_2_1()
    {
        GO_Node16_2_1.SetActive(true);
        GO_CanvaNode16_2_1.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayerNode16_2_1);

        // Fade in de entrada
        if (fadePanel != null)
            yield return StartCoroutine(FadeInEntrance());

        myTextNode16_2_1.gameObject.SetActive(true);
        textNode16_2_1.StartTyping();
        PlayByName("16_2_1");
        yield return new WaitForSeconds(1.5f);

        myTextNode16_2_1.gameObject.SetActive(false);
        myTextNode16_2_2.gameObject.SetActive(true);
        textNode16_2_2.StartTyping();
        PlayByName("16_2_2");

        yield return new WaitForSeconds(4.8f);

        myTextNode16_2_2.gameObject.SetActive(false);
        myTextNode16_2_3.gameObject.SetActive(true);
        textNode16_2_3.StartTyping();
        PlayByName("16_2_3");

        yield return new WaitForSeconds(3.9f);

        myTextNode16_2_3.gameObject.SetActive(false);
        myTextNode16_2_4.gameObject.SetActive(true);
        textNode16_2_4.StartTyping();
        PlayByName("16_2_4");

        yield return new WaitForSeconds(4f);

        StartCoroutine(AdvancingTimerNode16_2_2());
    }

    IEnumerator AdvancingTimerNode16_2_2()
    {
        GO_Node16_2_2.SetActive(true);
        GO_CanvaNode16_2_2.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayerNode16_2_2, GO_Node16_2_1);
        videoPlayerNode16_2_1.Stop();

        myTextNode16_2_1_b.gameObject.SetActive(true);
        textNode16_2_1_b.StartTyping();
        PlayByName("16_2_5");
        yield return new WaitForSeconds(2.3f);

        myTextNode16_2_1_b.gameObject.SetActive(false);
        myTextNode16_2_2_b.gameObject.SetActive(true);
        textNode16_2_2_b.StartTyping();
        PlayByName("16_2_6");
        yield return new WaitForSeconds(4.4f);
        myTextNode16_2_2_b.gameObject.SetActive(false);

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
        SceneManager.LoadScene("Dialogue17");
    }
}