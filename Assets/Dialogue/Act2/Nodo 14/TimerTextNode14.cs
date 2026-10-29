using UnityEngine;
using UnityEngine.SceneManagement;
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
    public VideoPlayer videoPlayerNode14_2;

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

    //////////////////// FADE OUT + CAMBIO DE ESCENA ////////////////////////
    [Header("Fade Out Settings")]
    public float fadeOutDuration = 1.5f;
    public string nextSceneName = "Dialogue15";

    private bool isFadingOut;

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

    // Prepara el video (si hace falta), espera su primer cuadro y lo reproduce.
    // Si se indica canvasToHide, ese canvas anterior se oculta DESPUES de que el
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
    private IEnumerator PlayVideoAndWaitFirstFrame(VideoPlayer vp)
    {
        if (!vp.isPrepared)
        {
            vp.Prepare();
            while (!vp.isPrepared)
                yield return null;
        }

        vp.Play();

        // Espera a que se dibuje realmente el primer cuadro (con tope de 2 s por seguridad)
        float timeout = 2f;
        while (vp.frame < 1 && timeout > 0f)
        {
            timeout -= Time.deltaTime;
            yield return null;
        }
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

        yield return PlayVideoWhenReady(videoPlayerNode14_1);

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
        // 1) Activa SOLO el nodo del video (sin mostrar aún el canvas de decisión)
        GO_Node14_2.SetActive(true);

        // 2) Prepara, reproduce y espera a que el primer cuadro esté dibujado.
        //    Mientras tanto el 14_1 sigue visible, así que nunca hay pantalla vacía.
        if (videoPlayerNode14_2 != null)
            yield return PlayVideoAndWaitFirstFrame(videoPlayerNode14_2);

        // 3) Recién ahora se muestra el canvas del 14_2
        GO_CanvaNode14_2.SetActive(true);

        // Un frame para que el canvas se dibuje antes de quitar lo anterior
        yield return null;

        yield return new WaitForSeconds(1.0f);
        myTextNode14_1_4.gameObject.SetActive(false);

        // 4) Ahora sí se apaga el 14_1
        if (videoPlayerNode14_1 != null)
            videoPlayerNode14_1.Stop();

        GO_CanvaNode14_1.SetActive(false);
        GO_Node14_1.SetActive(false);
    }

    // Se mantiene el nombre original para no romper la llamada desde OpenCanvasOnClick
    public void TriggerEndPanel()
    {
        TriggerFadeOut();
    }

    // Llamado desde OpenCanvasOnClick al elegir una opcion
    public void TriggerFadeOut()
    {
        if (isFadingOut) return;
        isFadingOut = true;
        StartCoroutine(FadeOutAndLoadScene());
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

    IEnumerator FadeOutAndLoadScene()
    {
        if (fadePanel != null)
        {
            // Reactiva el panel negro (estaba desactivado tras el fade in)
            fadePanel.gameObject.SetActive(true);
            fadePanel.transform.SetAsLastSibling(); // asegura que quede por encima de todo

            float elapsed = 0f;
            Color c = fadePanel.color;
            c.a = 0f;
            fadePanel.color = c;

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

        SceneManager.LoadScene(nextSceneName);
    }
}