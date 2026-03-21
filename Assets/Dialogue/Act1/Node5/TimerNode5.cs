using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections.Generic;

public class TimerNode5 : MonoBehaviour
{
    public GameObject GO_CanvaNode5a;
    public GameObject GO_CanvaNode5D1;
    public GameObject GO_CanvaNode5b;
    public VideoPlayer videoPlayerNode5a;
    public VideoPlayer videoPlayerNode5b;

    public TextMeshProUGUI myTextNode5a_1;
    public TextMeshProUGUI myTextNode5a_2;
    public TextMeshProUGUI myTextNode5a_3;
    public TextMeshProUGUI myTextNode5a_4;
    public TextMeshProUGUI myTextNode5a_5;
    public TextMeshProUGUI myTextNode5a_6;
    public TextMeshProUGUI myTextNode5a_7;

    public TypewriterTMP textNode5a_1;
    public TypewriterTMP textNode5a_2;
    public TypewriterTMP textNode5a_3;
    public TypewriterTMP textNode5a_4;
    public TypewriterTMP textNode5a_5;
    public TypewriterTMP textNode5a_6;
    public TypewriterTMP textNode5a_7;

    public TextMeshProUGUI myTextNode5b_1;
    public TextMeshProUGUI myTextNode5b_2;

    public TypewriterTMP textNode5b_1;
    public TypewriterTMP textNode5b_2;

    public bool flagOneTime;

    public int OptionTecnology { set; get; }

    //////////////////// FADE OUT ////////////////////////
    [Header("Fade Out Settings")]
    public Image fadePanel;
    public float fadeOutDuration = 1.5f;

    //////////////////// AUDIOS ////////////////////////
    [Header("Audio Sources")]
    public AudioSource[] sources;

    private Dictionary<string, AudioSource> audioDict;

    // Clave compartida con GameModeManager
    public const string GAME_MODE_KEY = "SelectedGameMode";

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
            c.a = 0f;
            fadePanel.color = c;
            fadePanel.gameObject.SetActive(false);
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
        /*if(flagOneTime == false){
            StartTimerNode5();
            flagOneTime = true;
        }*/
    }

    public void StartTimerNode5()
    {
        StartCoroutine(AdvancingTimerNode5());
    }

    IEnumerator AdvancingTimerNode5()
    {
        GO_CanvaNode5a.SetActive(true);

        videoPlayerNode5a.Stop();
        videoPlayerNode5a.time = 0;
        videoPlayerNode5a.frame = 0;
        videoPlayerNode5a.Play();
        videoPlayerNode5a.playbackSpeed = 1f;

        myTextNode5a_1.gameObject.SetActive(true);
        textNode5a_1.StartTyping();
        PlayByName("5a_1");

        yield return new WaitForSeconds(4.5f);

        myTextNode5a_1.gameObject.SetActive(false);
        myTextNode5a_2.gameObject.SetActive(true);
        textNode5a_2.StartTyping();
        PlayByName("5a_2");

        yield return new WaitForSeconds(10.2f);

        myTextNode5a_2.gameObject.SetActive(false);
        myTextNode5a_3.gameObject.SetActive(true);
        textNode5a_3.StartTyping();
        PlayByName("5a_3");

        yield return new WaitForSeconds(7f);

        myTextNode5a_3.gameObject.SetActive(false);
        myTextNode5a_4.gameObject.SetActive(true);
        textNode5a_4.StartTyping();
        PlayByName("5a_4");

        yield return new WaitForSeconds(4f);

        myTextNode5a_4.gameObject.SetActive(false);
        myTextNode5a_5.gameObject.SetActive(true);
        textNode5a_5.StartTyping();
        PlayByName("5a_5");

        yield return new WaitForSeconds(6f);

        myTextNode5a_5.gameObject.SetActive(false);
        myTextNode5a_6.gameObject.SetActive(true);
        textNode5a_6.StartTyping();
        PlayByName("5a_6");

        yield return new WaitForSeconds(7f);

        myTextNode5a_6.gameObject.SetActive(false);
        myTextNode5a_7.gameObject.SetActive(true);
        textNode5a_7.StartTyping();
        PlayByName("5a_7");

        yield return new WaitForSeconds(5.5f);

        GO_CanvaNode5a.SetActive(false);
        GO_CanvaNode5D1.SetActive(true);
    }

    public void OpcionTecnologia(int optTecnologia)
    {
        if (optTecnologia == 0)
        {
            OptionTecnology = 0;
            // Opción 0  Modo 1
            PlayerPrefs.SetInt(GAME_MODE_KEY, 2);
            PlayerPrefs.Save();
            StartCoroutine(AdvancingTimerNode5_2());
        }
        else if (optTecnologia == 1)
        {
            OptionTecnology = 1;
            // Opción 1  Modo 2
            PlayerPrefs.SetInt(GAME_MODE_KEY, 1);
            PlayerPrefs.Save();
            StartCoroutine(AdvancingTimerNode5_2());
        }
        else
        {
            Debug.Log("La opcion seleccionada no existe");
        }
    }

    IEnumerator AdvancingTimerNode5_2()
    {
        GO_CanvaNode5D1.SetActive(false);
        GO_CanvaNode5b.SetActive(true);

        videoPlayerNode5b.Stop();
        videoPlayerNode5b.time = 0;
        videoPlayerNode5b.frame = 0;
        videoPlayerNode5b.Play();
        videoPlayerNode5b.playbackSpeed = 1f;

        myTextNode5b_1.gameObject.SetActive(true);
        textNode5b_1.StartTyping();
        PlayByName("5b");

        yield return new WaitForSeconds(5f);

        myTextNode5b_1.gameObject.SetActive(false);
        myTextNode5b_2.gameObject.SetActive(true);
        textNode5b_2.StartTyping();

        yield return new WaitForSeconds(5f);

        if (fadePanel != null)
            yield return StartCoroutine(FadeOutPanel());

        UnityEngine.SceneManagement.SceneManager.LoadScene("Incompleteprototypeminigame2");
    }

    IEnumerator FadeOutPanel()
    {
        fadePanel.gameObject.SetActive(true);

        float elapsed = 0f;
        Color c = fadePanel.color;

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
}