using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerNode1 : MonoBehaviour
{
    public GameObject GO_CanvaNode1a;
    public GameObject GO_CanvaNode1b;
    public GameObject GO_CanvaNode1c;
    public GameObject GO_CanvaNode1d;
    public GameObject GO_CanvaNode1e;
    public GameObject GO_CanvaNode1f;


    public VideoPlayer videoPlayerNode1a;
    public VideoPlayer videoPlayerNode1b;
    public VideoPlayer videoPlayerNode1c;
    public VideoPlayer videoPlayerNode1d;
    public VideoPlayer videoPlayerNode1e;
    public VideoPlayer videoPlayerNode1f;

    public TextMeshProUGUI myTextNode1a_1;
    public TextMeshProUGUI myTextNode1a_2;

    public TypewriterTMP textNode1a_1;
    public TypewriterTMP textNode1a_2;

    public TextMeshProUGUI myTextNode1b_1;
    public TextMeshProUGUI myTextNode1b_2;
    public TextMeshProUGUI myTextNode1b_3;

    public TypewriterTMP textNode1b_1;
    public TypewriterTMP textNode1b_2;
    public TypewriterTMP textNode1b_3;

    public TextMeshProUGUI myTextNode1c_1;
    public TextMeshProUGUI myTextNode1c_2;
    public TextMeshProUGUI myTextNode1c_3;

    public TypewriterTMP textNode1c_1;
    public TypewriterTMP textNode1c_2;
    public TypewriterTMP textNode1c_3;

    public TextMeshProUGUI myTextNode1d_1;
    public TextMeshProUGUI myTextNode1d_2;
    public TextMeshProUGUI myTextNode1d_3;
    public TextMeshProUGUI myTextNode1d_4;

    public TypewriterTMP textNode1d_1;
    public TypewriterTMP textNode1d_2;
    public TypewriterTMP textNode1d_3;
    public TypewriterTMP textNode1d_4;

    public TextMeshProUGUI myTextNode1e_1;
    public TextMeshProUGUI myTextNode1e_2;
    public TextMeshProUGUI myTextNode1e_3;

    public TypewriterTMP textNode1e_1;
    public TypewriterTMP textNode1e_2;
    public TypewriterTMP textNode1e_3;

    public TextMeshProUGUI myTextNode1f_1;
    public TextMeshProUGUI myTextNode1f_2;
    public TextMeshProUGUI myTextNode1f_3;

    public TypewriterTMP textNode1f_1;
    public TypewriterTMP textNode1f_2;
    public TypewriterTMP textNode1f_3;


    //////////////////// AUDIOS ////////////////////////
    [Header("Audio Sources")]
    public AudioSource Dn1_11Orion;
    public AudioSource Dn1_12Lumen;
    public AudioSource Dn1_21Orion;
    public AudioSource Dn1_22Lumen;
    public AudioSource Dn1_31Aria;
    public AudioSource Dn1_32Orion;
    public AudioSource Dn1_61Aria;
    public AudioSource Dn1_62Orion;
    public AudioSource Music;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Fade Settings")]
    public Image fadePanel; // Asigna un Image negro en un Canvas que cubra toda la pantalla
    public float fadeDuration = 1f;


    void Start()
    {
        if (fadePanel != null)
        {
            Color c = fadePanel.color;
            c.a = 0f;
            fadePanel.color = c;
            fadePanel.gameObject.SetActive(false);
        }
    }

    void Update()
    {

    }

    public void StartTimerNode1()
    {
        StartCoroutine(AdvancingTimeNode1());
    }

    IEnumerator AdvancingTimeNode1()
    {
        videoPlayerNode1a.Stop();
        videoPlayerNode1a.time = 0;
        videoPlayerNode1a.frame = 0;
        videoPlayerNode1a.Play();
        Music.Play(); // Inicio Música de Fondo 

        videoPlayerNode1a.playbackSpeed = 1f;

        myTextNode1a_1.gameObject.SetActive(true);
        textNode1a_1.StartTyping();
        Dn1_11Orion.Play(); //dialogo

        yield return new WaitForSeconds(8.5f);

        myTextNode1a_1.gameObject.SetActive(false);
        myTextNode1a_2.gameObject.SetActive(true);
        textNode1a_2.StartTyping();
        Dn1_12Lumen.Play(); //dialogo

        yield return new WaitForSeconds(8f);

        videoPlayerNode1a.Stop();
        GO_CanvaNode1a.SetActive(false);
        GO_CanvaNode1b.SetActive(true);
        videoPlayerNode1b.Stop();
        videoPlayerNode1b.time = 0;
        videoPlayerNode1b.frame = 0;
        videoPlayerNode1b.Play();
        videoPlayerNode1b.playbackSpeed = 1f;

        myTextNode1b_1.gameObject.SetActive(true);
        textNode1b_1.StartTyping();
        Dn1_21Orion.Play(); //dialogo

        yield return new WaitForSeconds(2.5f);

        myTextNode1b_2.gameObject.SetActive(true);
        myTextNode1b_1.gameObject.SetActive(false);
        textNode1b_2.StartTyping();

        yield return new WaitForSeconds(15.5f);

        myTextNode1b_3.gameObject.SetActive(true);
        myTextNode1b_2.gameObject.SetActive(false);
        textNode1b_3.StartTyping();
        Dn1_22Lumen.Play(); //dialogo

        yield return new WaitForSeconds(3f);

        videoPlayerNode1b.Stop();
        GO_CanvaNode1b.SetActive(false);
        GO_CanvaNode1c.SetActive(true);
        videoPlayerNode1c.Stop();
        videoPlayerNode1c.time = 0;
        videoPlayerNode1c.frame = 0;
        videoPlayerNode1c.Play();
        videoPlayerNode1c.playbackSpeed = 1f;

        myTextNode1c_1.gameObject.SetActive(true);
        textNode1c_1.StartTyping();
        Dn1_31Aria.Play(); //dialogo

        yield return new WaitForSeconds(5f);

        myTextNode1c_2.gameObject.SetActive(true);
        myTextNode1c_1.gameObject.SetActive(false);
        textNode1c_2.StartTyping();
        Dn1_32Orion.Play(); //dialogo

        yield return new WaitForSeconds(1.5f);

        myTextNode1c_3.gameObject.SetActive(true);
        myTextNode1c_2.gameObject.SetActive(false);
        textNode1c_3.StartTyping();

        yield return new WaitForSeconds(17.7f);

        videoPlayerNode1c.Stop();
        GO_CanvaNode1d.SetActive(false);
        GO_CanvaNode1d.SetActive(true);
        videoPlayerNode1d.Stop();
        videoPlayerNode1d.time = 0;
        videoPlayerNode1d.frame = 0;
        videoPlayerNode1d.Play();
        videoPlayerNode1d.playbackSpeed = 1f;

        myTextNode1d_1.gameObject.SetActive(true);
        textNode1d_1.StartTyping();


        yield return new WaitForSeconds(11.4f);

        myTextNode1d_2.gameObject.SetActive(true);
        myTextNode1d_1.gameObject.SetActive(false);
        textNode1d_2.StartTyping();

        yield return new WaitForSeconds(12f);

        myTextNode1d_3.gameObject.SetActive(true);
        myTextNode1d_2.gameObject.SetActive(false);
        textNode1d_3.StartTyping();

        yield return new WaitForSeconds(11.1f);

        myTextNode1d_4.gameObject.SetActive(true);
        myTextNode1d_3.gameObject.SetActive(false);
        textNode1d_4.StartTyping();

        yield return new WaitForSeconds(16.3f);

        videoPlayerNode1d.Stop();
        GO_CanvaNode1e.SetActive(false);
        GO_CanvaNode1e.SetActive(true);
        videoPlayerNode1e.Stop();
        videoPlayerNode1e.time = 0;
        videoPlayerNode1e.frame = 0;
        videoPlayerNode1e.Play();
        videoPlayerNode1e.playbackSpeed = 1.0f;

        myTextNode1e_1.gameObject.SetActive(true);
        textNode1e_1.StartTyping();

        yield return new WaitForSeconds(12.3f);

        myTextNode1e_2.gameObject.SetActive(true);
        myTextNode1e_1.gameObject.SetActive(false);
        textNode1e_2.StartTyping();

        yield return new WaitForSeconds(13.3f);

        myTextNode1e_3.gameObject.SetActive(true);
        myTextNode1e_2.gameObject.SetActive(false);
        textNode1e_3.StartTyping();

        yield return new WaitForSeconds(16.4f);

        videoPlayerNode1e.Stop();
        GO_CanvaNode1f.SetActive(false);
        GO_CanvaNode1f.SetActive(true);
        videoPlayerNode1f.Stop();
        videoPlayerNode1f.time = 0;
        videoPlayerNode1f.frame = 0;
        videoPlayerNode1f.Play();
        videoPlayerNode1f.playbackSpeed = 1f;

        myTextNode1f_1.gameObject.SetActive(true);
        textNode1f_1.StartTyping();
        Dn1_61Aria.Play(); //dialogo

        yield return new WaitForSeconds(2f);

        myTextNode1f_2.gameObject.SetActive(true);
        myTextNode1f_1.gameObject.SetActive(false);
        textNode1f_2.StartTyping();
        Dn1_62Orion.Play(); //dialogo


        yield return new WaitForSeconds(8f);

        myTextNode1f_3.gameObject.SetActive(true);
        myTextNode1f_2.gameObject.SetActive(false);
        textNode1f_3.StartTyping();

        yield return new WaitForSeconds(5f);
        Music.Stop(); // Fin música de Fondo Música de Fondo 

        // --- FIN DE LA SECUENCIA ---
        // Espera 3 segundos antes del fade
        yield return new WaitForSeconds(3f);

        // Fade out hacia negro
        yield return StartCoroutine(FadeOut());

        // Carga la escena del minijuego
        SceneManager.LoadScene("ChestMinigameIngenieria");
    }

    IEnumerator FadeOut()
    {
        if (fadePanel == null) yield break;

        fadePanel.gameObject.SetActive(true);

        float elapsed = 0f;
        Color c = fadePanel.color;
        c.a = 0f;
        fadePanel.color = c;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsed / fadeDuration);
            fadePanel.color = c;
            yield return null;
        }

        c.a = 1f;
        fadePanel.color = c;

    }
}