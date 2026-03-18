using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections.Generic;

public class TimerNode7 : MonoBehaviour
{
    private bool flagJustOneTime;

    public GameObject GO_CanvaNode7a;

    public VideoPlayer videoPlayerNode7a;

    public TextMeshProUGUI myTextNode7a_1;
    public TextMeshProUGUI myTextNode7a_2;
    public TextMeshProUGUI myTextNode7a_3;
    public TextMeshProUGUI myTextNode7a_4;

    public TypewriterTMP textNode7a_1;
    public TypewriterTMP textNode7a_2;
    public TypewriterTMP textNode7a_3;
    public TypewriterTMP textNode7a_4;

    [Header("Panel Final")]
    public GameObject endPanel;
    public TextMeshProUGUI endPanelText;
    public float fadeDuration = 1f;

    //////////////////// FADE IN ////////////////////////
    [Header("Fade In Settings")]
    public Image fadePanel;          // Panel negro (Image) que cubre toda la pantalla
    public float fadeInDuration = 1.5f;

    //////////////////// AUDIOS ////////////////////////
    [Header("Audio Sources")]
    public AudioSource[] sources;

    private Dictionary<string, AudioSource> audioDict;
    private CanvasGroup endPanelCanvasGroup;

    void Awake()
    {
        audioDict = new Dictionary<string, AudioSource>();
        foreach (AudioSource src in sources)
        {
            if (src != null)
                audioDict[src.gameObject.name] = src;
        }

        // Prepara el CanvasGroup para el fade del panel final
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

    void Start() { }

    void Update()
    {
        if (flagJustOneTime == false)
        {
            StartCoroutine(AdvancingTimerNode7());
            flagJustOneTime = true;
        }
    }

    IEnumerator AdvancingTimerNode7()
    {
        // Inicia el video antes del fade para que ya esté corriendo debajo
        videoPlayerNode7a.Stop();
        videoPlayerNode7a.time = 0;
        videoPlayerNode7a.frame = 0;
        videoPlayerNode7a.Play();
        videoPlayerNode7a.playbackSpeed = 1f;

        // Fade in: el panel pasa de opaco a transparente
        if (fadePanel != null)
            yield return StartCoroutine(FadeInEntrance());

        myTextNode7a_1.gameObject.SetActive(true);
        textNode7a_1.StartTyping();
        PlayByName("7a_1");

        yield return new WaitForSeconds(5.8f);

        myTextNode7a_1.gameObject.SetActive(false);
        myTextNode7a_2.gameObject.SetActive(true);
        textNode7a_2.StartTyping();
        PlayByName("7a_2");

        yield return new WaitForSeconds(6f);

        myTextNode7a_2.gameObject.SetActive(false);
        myTextNode7a_3.gameObject.SetActive(true);
        textNode7a_3.StartTyping();
        PlayByName("7a_3");

        yield return new WaitForSeconds(5.7f);

        myTextNode7a_3.gameObject.SetActive(false);
        myTextNode7a_4.gameObject.SetActive(true);
        textNode7a_4.StartTyping();
        PlayByName("7a_4");

        yield return new WaitForSeconds(6f);

        myTextNode7a_4.gameObject.SetActive(false);

        if (endPanel != null)
            StartCoroutine(FadeInPanel());
    }

    // Fade in de entrada: negro  transparente
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

    // Fade in del panel final al terminar el último diálogo
    IEnumerator FadeInPanel()
    {
        endPanel.SetActive(true);
        endPanelCanvasGroup.alpha = 0f;

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            endPanelCanvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        endPanelCanvasGroup.alpha = 1f;
        endPanelCanvasGroup.interactable = true;
        endPanelCanvasGroup.blocksRaycasts = true;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}