using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TimerTextNode18 : MonoBehaviour
{
    public GameObject GO_CanvaNode18_1;
    public GameObject GO_CanvaNode18_2;
    public GameObject GO_CanvaNode18_3;

    public GameObject GO_Node18_1;
    public GameObject GO_Node18_2;
    public GameObject GO_Node18_3;

    public VideoPlayer videoPlayerNode18_1;
    public VideoPlayer videoPlayerNode18_2;
    public VideoPlayer videoPlayerNode18_3;

    public TextMeshProUGUI myTextNode18_1_1;
    public TextMeshProUGUI myTextNode18_1_2;
    public TextMeshProUGUI myTextNode18_1_3;
    public TextMeshProUGUI myTextNode18_1_4;
    public TextMeshProUGUI myTextNode18_1_5;
    public TextMeshProUGUI myTextNode18_1_6;
    public TextMeshProUGUI myTextNode18_1_7;
    public TextMeshProUGUI myTextNode18_1_8;
    public TextMeshProUGUI myTextNode18_1_9;

    public TypewriterTMP textNode18_1_1;
    public TypewriterTMP textNode18_1_2;
    public TypewriterTMP textNode18_1_3;
    public TypewriterTMP textNode18_1_4;
    public TypewriterTMP textNode18_1_5;
    public TypewriterTMP textNode18_1_6;
    public TypewriterTMP textNode18_1_7;
    public TypewriterTMP textNode18_1_8;
    public TypewriterTMP textNode18_1_9;

    public TextMeshProUGUI myTextNode18_2_1;
    public TextMeshProUGUI myTextNode18_2_2;
    public TextMeshProUGUI myTextNode18_2_3;
    public TextMeshProUGUI myTextNode18_2_4;
    public TextMeshProUGUI myTextNode18_2_5;
    public TextMeshProUGUI myTextNode18_2_6;
    public TextMeshProUGUI myTextNode18_2_7;

    public TypewriterTMP textNode18_2_1;
    public TypewriterTMP textNode18_2_2;
    public TypewriterTMP textNode18_2_3;
    public TypewriterTMP textNode18_2_4;
    public TypewriterTMP textNode18_2_5;
    public TypewriterTMP textNode18_2_6;
    public TypewriterTMP textNode18_2_7;

    //////////////////// FADE IN ////////////////////////
    [Header("Fade In Settings")]
    public Image fadePanel;
    public float fadeInDuration = 1.5f;

    //////////////////// SCENE ////////////////////////
    [Header("Next Scene")]
    public string nextSceneName = "";

    //////////////////// AUDIOS ////////////////////////
    [Header("Audio Sources")]
    public AudioSource[] sources;

    private Dictionary<string, AudioSource> audioDict;

    // Tiempo de espera al iniciar cada nodo (despues de que el video ya tiene imagen)
    private const float NodeStartDelay = 1f;

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

            // Espera a que el primer cuadro llegue a la textura (con limite de seguridad)
            float timeout = 2f;
            while (vp.frame < 1 && timeout > 0f)
            {
                timeout -= Time.deltaTime;
                yield return null;
            }
            yield return new WaitForEndOfFrame();
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

    void Start()
    {
        StartCoroutine(AdvancingTimerNode18_1());
    }

    IEnumerator AdvancingTimerNode18_1()
    {
        GO_Node18_1.SetActive(true);
        GO_CanvaNode18_1.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayerNode18_1);

        // Fade in de entrada
        if (fadePanel != null)
            yield return StartCoroutine(FadeInEntrance());

        yield return new WaitForSeconds(NodeStartDelay);
        myTextNode18_1_1.gameObject.SetActive(true);
        textNode18_1_1.StartTyping();
        PlayByName("18_1_1");

        yield return new WaitForSeconds(2.6f);

        myTextNode18_1_1.gameObject.SetActive(false);
        myTextNode18_1_2.gameObject.SetActive(true);
        textNode18_1_2.StartTyping();
        PlayByName("18_1_2");

        yield return new WaitForSeconds(4.8f);

        myTextNode18_1_2.gameObject.SetActive(false);
        myTextNode18_1_3.gameObject.SetActive(true);
        textNode18_1_3.StartTyping();
        PlayByName("18_1_3");

        yield return new WaitForSeconds(4.7f);

        myTextNode18_1_3.gameObject.SetActive(false);
        myTextNode18_1_4.gameObject.SetActive(true);
        textNode18_1_4.StartTyping();
        PlayByName("18_1_4");

        yield return new WaitForSeconds(3.9f);

        myTextNode18_1_4.gameObject.SetActive(false);
        myTextNode18_1_5.gameObject.SetActive(true);
        textNode18_1_5.StartTyping();
        PlayByName("18_1_5");

        yield return new WaitForSeconds(5f);

        myTextNode18_1_5.gameObject.SetActive(false);
        myTextNode18_1_6.gameObject.SetActive(true);
        textNode18_1_6.StartTyping();
        PlayByName("18_1_6");

        yield return new WaitForSeconds(3.2f);

        myTextNode18_1_6.gameObject.SetActive(false);
        myTextNode18_1_7.gameObject.SetActive(true);
        textNode18_1_7.StartTyping();
        PlayByName("18_1_7");

        yield return new WaitForSeconds(5f);

        myTextNode18_1_7.gameObject.SetActive(false);
        myTextNode18_1_8.gameObject.SetActive(true);
        textNode18_1_8.StartTyping();
        PlayByName("18_1_8");

        yield return new WaitForSeconds(2.6f);

        myTextNode18_1_8.gameObject.SetActive(false);
        myTextNode18_1_9.gameObject.SetActive(true);
        textNode18_1_9.StartTyping();
        PlayByName("18_1_9");

        yield return new WaitForSeconds(2.7f);

        yield return StartCoroutine(AdvancingTimerNode18_2());
    }

    IEnumerator AdvancingTimerNode18_2()
    {
        GO_Node18_2.SetActive(true);
        GO_CanvaNode18_2.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayerNode18_2, GO_CanvaNode18_1, GO_Node18_1);
        videoPlayerNode18_1.Stop();

        yield return new WaitForSeconds(NodeStartDelay);
        myTextNode18_2_1.gameObject.SetActive(true);
        textNode18_2_1.StartTyping();
        PlayByName("18_2_1");

        yield return new WaitForSeconds(5.1f);

        myTextNode18_2_1.gameObject.SetActive(false);
        myTextNode18_2_2.gameObject.SetActive(true);
        textNode18_2_2.StartTyping();
        PlayByName("18_2_2");

        yield return new WaitForSeconds(6.75f);

        myTextNode18_2_2.gameObject.SetActive(false);
        myTextNode18_2_3.gameObject.SetActive(true);
        textNode18_2_3.StartTyping();
        PlayByName("18_2_3");

        yield return new WaitForSeconds(3.4f);

        myTextNode18_2_3.gameObject.SetActive(false);
        myTextNode18_2_4.gameObject.SetActive(true);
        textNode18_2_4.StartTyping();
        PlayByName("18_2_4");

        yield return new WaitForSeconds(3.4f);

        myTextNode18_2_4.gameObject.SetActive(false);
        myTextNode18_2_5.gameObject.SetActive(true);
        textNode18_2_5.StartTyping();
        PlayByName("18_2_5");

        yield return new WaitForSeconds(4.7f);

        myTextNode18_2_5.gameObject.SetActive(false);
        myTextNode18_2_6.gameObject.SetActive(true);
        textNode18_2_6.StartTyping();
        PlayByName("18_2_6");

        yield return new WaitForSeconds(4.44f);

        myTextNode18_2_6.gameObject.SetActive(false);
        myTextNode18_2_7.gameObject.SetActive(true);
        textNode18_2_7.StartTyping();
        PlayByName("18_2_7");

        yield return new WaitForSeconds(4f);

        yield return StartCoroutine(AdvancingTimerNode18_3());
    }

    IEnumerator AdvancingTimerNode18_3()
    {
        GO_Node18_3.SetActive(true);
        GO_CanvaNode18_3.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayerNode18_3, GO_CanvaNode18_2, GO_Node18_2);
        videoPlayerNode18_2.Stop();

        yield return new WaitForSeconds(20f);

        // Final de todo: cambio de escena
        LoadNextScene();
    }

    public void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
        else
            Debug.LogWarning("nextSceneName esta vacio, no se carga ninguna escena.");
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

}
