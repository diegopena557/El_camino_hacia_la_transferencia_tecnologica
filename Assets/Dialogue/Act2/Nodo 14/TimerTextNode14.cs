using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections.Generic;

public class TimerTextNode14 : MonoBehaviour
{
    private bool flagJustOneTime;

    public GameObject GO_CanvaNode14_1;
    public GameObject GO_CanvaNode14_2;

    public GameObject GO_Node14_1;
    public GameObject GO_Node14_2;

    public VideoPlayer videoPlayerNode14_1;

    public TextMeshProUGUI myTextNode14_1_1;
    public TextMeshProUGUI myTextNode14_1_2;
    public TextMeshProUGUI myTextNode14_1_3;
    public TextMeshProUGUI myTextNode14_1_4;

    public TypewriterTMP textNode14_1_1;
    public TypewriterTMP textNode14_1_2;
    public TypewriterTMP textNode14_1_3;
    public TypewriterTMP textNode14_1_4;

    //////////////////// FADE IN ////////////////////////
    [Header("Fade In Settings")]
    public Image fadePanel;
    public float fadeInDuration = 1.5f;

    //////////////////// PANEL FINAL ////////////////////////
    [Header("End Panel")]
    public GameObject endPanel;
    public Button quitButton;
    public float fadeDuration = 1f;

    private CanvasGroup endPanelCanvasGroup;

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

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
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
            StartCoroutine(AdvancingTimerNode14_1());
            flagJustOneTime = true;
        }
    }

    IEnumerator AdvancingTimerNode14_1()
    {
        GO_Node14_1.SetActive(true);
        GO_CanvaNode14_1.SetActive(true);

        videoPlayerNode14_1.Stop();
        videoPlayerNode14_1.time = 0;
        videoPlayerNode14_1.frame = 0;
        videoPlayerNode14_1.Play();

        if (fadePanel != null)
            yield return StartCoroutine(FadeInEntrance());

        yield return new WaitForSeconds(1f);
        myTextNode14_1_1.gameObject.SetActive(true);
        textNode14_1_1.StartTyping();
        PlayByName("14a_1");

        yield return new WaitForSeconds(4.44f);
        myTextNode14_1_1.gameObject.SetActive(false);
        myTextNode14_1_2.gameObject.SetActive(true);
        textNode14_1_2.StartTyping();
        PlayByName("14a_2");

        yield return new WaitForSeconds(2.72f);
        myTextNode14_1_2.gameObject.SetActive(false);
        myTextNode14_1_3.gameObject.SetActive(true);
        textNode14_1_3.StartTyping();
        PlayByName("14a_3");

        yield return new WaitForSeconds(2.98f);
        myTextNode14_1_3.gameObject.SetActive(false);
        myTextNode14_1_4.gameObject.SetActive(true);
        textNode14_1_4.StartTyping();
        PlayByName("14a_4");

        yield return new WaitForSeconds(4.1f);
        yield return StartCoroutine(AdvancingTimerNode14_2());
    }

    IEnumerator AdvancingTimerNode14_2()
    {
        GO_Node14_2.SetActive(true);
        GO_CanvaNode14_2.SetActive(true);

        GO_Node14_1.SetActive(true);
        GO_CanvaNode14_1.SetActive(true);

        yield return new WaitForSeconds(1.0f);
        myTextNode14_1_4.gameObject.SetActive(false);

        // El end panel ya NO se dispara aquí,
        // sino cuando el jugador elige una opción (OpenCanvasOnClick)
    }

    // Llamado desde OpenCanvasOnClick al elegir una opción
    public void TriggerEndPanel()
    {
        StartCoroutine(FadeInEndPanel());
    }

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

    IEnumerator FadeInEndPanel()
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