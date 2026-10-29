using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TimerTextNode15 : MonoBehaviour
{
    private bool flagJustOneTime;

    public GameObject GO_CanvaNode15_1;
    public GameObject GO_CanvaNode15_2;
    public GameObject GO_CanvaNode15_3;
    public GameObject GO_CanvaNode15_4;
    public GameObject GO_CanvaNode15_5;

    public GameObject GO_Node15_1;
    public GameObject GO_Node15_2;
    public GameObject GO_Node15_3;
    public GameObject GO_Node15_4;
    public GameObject GO_Node15_5;

    public VideoPlayer videoPlayerNode15_1;
    public VideoPlayer videoPlayerNode15_2;
    public VideoPlayer videoPlayerNode15_3;
    public VideoPlayer videoPlayerNode15_4;
    public VideoPlayer videoPlayerNode15_5;

    public TextMeshProUGUI myTextNode15_5_1;
    public TextMeshProUGUI myTextNode15_5_2;
    public TextMeshProUGUI myTextNode15_5_3;
    public TextMeshProUGUI myTextNode15_5_4;
    public TextMeshProUGUI myTextNode15_5_5;
    public TextMeshProUGUI myTextNode15_5_6;
    public TextMeshProUGUI myTextNode15_5_7;
    public TextMeshProUGUI myTextNode15_5_8;

    public TypewriterTMP textNode15_5_1;
    public TypewriterTMP textNode15_5_2;
    public TypewriterTMP textNode15_5_3;
    public TypewriterTMP textNode15_5_4;
    public TypewriterTMP textNode15_5_5;
    public TypewriterTMP textNode15_5_6;
    public TypewriterTMP textNode15_5_7;
    public TypewriterTMP textNode15_5_8;

    //0 -- Des1
    //1 -- Des2
    //2 -- Des3
    public int flagDecisionChoosen;

    [Header("Panel Final")]
    public GameObject endPanel;

    [Header("Fade In Settings")]
    public Image fadePanel;
    public float fadeInDuration = 1.5f;

    [Header("Act Title (se muestra sobre el panel negro al inicio)")]
    [Tooltip("Primer texto TMP del titulo (por ejemplo, 'ACTO II'). Debe dibujarse encima del panel (por ejemplo, hijo de fadePanel).")]
    public TextMeshProUGUI actTitleText1;
    [Tooltip("Segundo texto TMP del titulo (por ejemplo, el nombre del acto). Debe dibujarse encima del panel.")]
    public TextMeshProUGUI actTitleText2;
    [Tooltip("Segundos totales que el panel permanece negro mostrando el titulo (incluye el fade in/out del texto).")]
    public float actTitleDuration = 4f;
    [Tooltip("Segundos que tarda el texto en aparecer y en desaparecer.")]
    public float actTitleFadeDuration = 0.5f;

    [Header("Fade Out Settings")]
    public float fadeOutDuration = 1.5f;

    //////////////////// AUDIOS ////////////////////////
    [Header("Audio Sources")]
    public AudioSource[] sources;

    private Dictionary<string, AudioSource> audioDict;
    private CanvasGroup endPanelCanvasGroup;

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

        if (endPanel != null)
        {
            endPanelCanvasGroup = endPanel.GetComponent<CanvasGroup>();
            if (endPanelCanvasGroup == null)
                endPanelCanvasGroup = endPanel.AddComponent<CanvasGroup>();

            endPanelCanvasGroup.alpha = 0f;
            endPanelCanvasGroup.interactable = false;
            endPanelCanvasGroup.blocksRaycasts = false;
            endPanel.SetActive(false);
        }

        if (fadePanel != null)
        {
            Color c = fadePanel.color;
            c.a = 1f;
            fadePanel.color = c;
            fadePanel.gameObject.SetActive(true);
        }

        // El titulo del acto empieza invisible; se muestra en ShowActTitle()
        SetActTitleAlpha(0f);
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

            if (flagDecisionChoosen == 0)
            {
                StartCoroutine(AdvancingTimerNode15_1());
            }

            if (flagDecisionChoosen == 1)
            {
                StartCoroutine(AdvancingTimerNode15_2());
            }

            if (flagDecisionChoosen == 2)
            {
                StartCoroutine(AdvancingTimerNode15_3());
            }
            flagJustOneTime = true;
        }
    }

    IEnumerator AdvancingTimerNode15_1()
    {
        GO_Node15_1.SetActive(true);
        GO_CanvaNode15_1.SetActive(true);

        // Prepara el video mientras se muestra el titulo del acto
        videoPlayerNode15_1.Prepare();

        // Titulo del acto sobre el panel negro (el video arranca despues)
        yield return StartCoroutine(ShowActTitle());

        yield return PlayVideoWhenReady(videoPlayerNode15_1);

        // Fade in de entrada
        if (fadePanel != null)
            yield return StartCoroutine(FadeInEntrance());

        yield return new WaitForSeconds(10f);

        StartCoroutine(AdvancingTimerNode15_4());
    }

    IEnumerator AdvancingTimerNode15_2()
    {
        GO_Node15_2.SetActive(true);
        GO_CanvaNode15_2.SetActive(true);

        // Prepara el video mientras se muestra el titulo del acto
        videoPlayerNode15_2.Prepare();

        // Titulo del acto sobre el panel negro (el video arranca despues)
        yield return StartCoroutine(ShowActTitle());

        yield return PlayVideoWhenReady(videoPlayerNode15_2, GO_Node15_1);
        videoPlayerNode15_1.Stop();

        // Fade in de entrada
        if (fadePanel != null)
            yield return StartCoroutine(FadeInEntrance());

        yield return new WaitForSeconds(10f);

        StartCoroutine(AdvancingTimerNode15_4());
    }

    IEnumerator AdvancingTimerNode15_3()
    {
        GO_Node15_3.SetActive(true);
        GO_CanvaNode15_3.SetActive(true);

        // Prepara el video mientras se muestra el titulo del acto
        videoPlayerNode15_3.Prepare();

        // Titulo del acto sobre el panel negro (el video arranca despues)
        yield return StartCoroutine(ShowActTitle());

        yield return PlayVideoWhenReady(videoPlayerNode15_3, GO_Node15_2);
        videoPlayerNode15_2.Stop();

        // Fade in de entrada
        if (fadePanel != null)
            yield return StartCoroutine(FadeInEntrance());

        yield return new WaitForSeconds(10f);

        StartCoroutine(AdvancingTimerNode15_4());
    }

    IEnumerator AdvancingTimerNode15_4()
    {
        GO_Node15_4.SetActive(true);
        GO_CanvaNode15_4.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayerNode15_4, GO_Node15_3);
        videoPlayerNode15_3.Stop();

        yield return new WaitForSeconds(4f);

        StartCoroutine(AdvancingTimerNode15_5());
    }

    IEnumerator AdvancingTimerNode15_5()
    {
        GO_Node15_5.SetActive(true);
        GO_CanvaNode15_5.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayerNode15_5, GO_Node15_4);
        videoPlayerNode15_4.Stop();

        myTextNode15_5_1.gameObject.SetActive(true);
        textNode15_5_1.StartTyping();
        PlayByName("15_5_1");

        yield return new WaitForSeconds(2.2f);

        myTextNode15_5_1.gameObject.SetActive(false);
        myTextNode15_5_2.gameObject.SetActive(true);
        textNode15_5_2.StartTyping();
        PlayByName("15_5_2");

        yield return new WaitForSeconds(1.6f);

        myTextNode15_5_2.gameObject.SetActive(false);
        myTextNode15_5_3.gameObject.SetActive(true);
        textNode15_5_3.StartTyping();
        PlayByName("15_5_3");

        yield return new WaitForSeconds(7.5f);

        myTextNode15_5_3.gameObject.SetActive(false);
        myTextNode15_5_4.gameObject.SetActive(true);
        textNode15_5_4.StartTyping();
        PlayByName("15_5_4");

        yield return new WaitForSeconds(1.1f);

        myTextNode15_5_4.gameObject.SetActive(false);
        myTextNode15_5_5.gameObject.SetActive(true);
        textNode15_5_5.StartTyping();
        PlayByName("15_5_5");

        yield return new WaitForSeconds(2f);

        myTextNode15_5_5.gameObject.SetActive(false);
        myTextNode15_5_6.gameObject.SetActive(true);
        textNode15_5_6.StartTyping();
        PlayByName("15_5_6");

        yield return new WaitForSeconds(4.8f);

        myTextNode15_5_6.gameObject.SetActive(false);
        myTextNode15_5_7.gameObject.SetActive(true);
        textNode15_5_7.StartTyping();
        PlayByName("15_5_7");

        yield return new WaitForSeconds(1.3f);

        myTextNode15_5_7.gameObject.SetActive(false);
        myTextNode15_5_8.gameObject.SetActive(true);
        textNode15_5_8.StartTyping();
        PlayByName("15_5_8");

        yield return new WaitForSeconds(6f);
        myTextNode15_5_8.gameObject.SetActive(false);

        // Fade out y carga de escena
        yield return StartCoroutine(FadeOutToBlack());
        LoadNextScene();
    }

    // Muestra el titulo del acto (los dos textos) sobre el panel negro durante actTitleDuration segundos
    IEnumerator ShowActTitle()
    {
        if (actTitleText1 == null && actTitleText2 == null) yield break;

        SetActTitleActive(true);

        // Aparecen
        yield return StartCoroutine(FadeActTitle(0f, 1f, actTitleFadeDuration));

        // Se mantienen (el total incluye el fade in y el fade out del texto)
        float hold = Mathf.Max(0f, actTitleDuration - 2f * actTitleFadeDuration);
        yield return new WaitForSeconds(hold);

        // Desaparecen
        yield return StartCoroutine(FadeActTitle(1f, 0f, actTitleFadeDuration));

        // Quedan ocultos para que no se vean en el fade out final
        SetActTitleActive(false);
    }

    IEnumerator FadeActTitle(float from, float to, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            SetActTitleAlpha(Mathf.Lerp(from, to, elapsed / duration));
            yield return null;
        }

        SetActTitleAlpha(to);
    }

    private void SetActTitleActive(bool active)
    {
        if (actTitleText1 != null) actTitleText1.gameObject.SetActive(active);
        if (actTitleText2 != null) actTitleText2.gameObject.SetActive(active);
    }

    private void SetActTitleAlpha(float alpha)
    {
        SetTextAlpha(actTitleText1, alpha);
        SetTextAlpha(actTitleText2, alpha);
    }

    private static void SetTextAlpha(TextMeshProUGUI tmp, float alpha)
    {
        if (tmp == null) return;
        Color c = tmp.color;
        c.a = alpha;
        tmp.color = c;
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
        SceneManager.LoadScene("Dialogue16_1");
    }
}