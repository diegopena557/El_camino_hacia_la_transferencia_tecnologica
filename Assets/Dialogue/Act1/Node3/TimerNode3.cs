using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections.Generic; //Necesario para los audios

public class TimerNode3 : MonoBehaviour
{
    public GameObject GO_CanvaNode3_0;
    public GameObject GO_CanvaNode3a;
    public GameObject GO_CanvaNode3b;
    public GameObject GO_CanvaNode3c;
    public GameObject GO_CanvaNode3D1;
    public GameObject GO_CanvaNode3d;
    public GameObject GO_PanelPreguntaCiencia;
    public GameObject GO_CanvaNode3e;
    public GameObject GO_PanelPreguntaTecnologia;
    public GameObject GO_CanvaNode3f;
    public GameObject GO_CanvaNode3g;
    public GameObject GO_CanvaNode3h0;
    public GameObject GO_CanvaNode3h;
    public GameObject GO_CanvaNode3i;
    public GameObject GO_CanvaNode3j;

    public VideoPlayer videoPlayerNode3_0;
    public VideoPlayer videoPlayerNode3a;
    public VideoPlayer videoPlayerNode3b;
    public VideoPlayer videoPlayerNode3c;
    public VideoPlayer videoPlayerNode3d;
    public VideoPlayer videoPlayerNode3e;
    public VideoPlayer videoPlayerNode3f;
    public VideoPlayer videoPlayerNode3g;
    public VideoPlayer videoPlayerNode3h0;
    public VideoPlayer videoPlayerNode3h;
    public VideoPlayer videoPlayerNode3i;
    public VideoPlayer videoPlayerNode3j;

    public TextMeshProUGUI myTextNode3a_1;
    public TextMeshProUGUI myTextNode3a_2;
    public TextMeshProUGUI myTextNode3a_3;

    public TypewriterTMP textNode3a_1;
    public TypewriterTMP textNode3a_2;
    public TypewriterTMP textNode3a_3;

    public TextMeshProUGUI myTextNode3b_1;
    public TextMeshProUGUI myTextNode3b_2;
    public TextMeshProUGUI myTextNode3b_3;

    public TypewriterTMP textNode3b_1;
    public TypewriterTMP textNode3b_2;
    public TypewriterTMP textNode3b_3;

    public TextMeshProUGUI myTextNode3c_1;
    public TextMeshProUGUI myTextNode3c_2;
    public TextMeshProUGUI myTextNode3c_3;
    public TextMeshProUGUI myTextNode3c_4;

    public TypewriterTMP textNode3c_1;
    public TypewriterTMP textNode3c_2;
    public TypewriterTMP textNode3c_3;
    public TypewriterTMP textNode3c_4;

    public TextMeshProUGUI myTextNode3d_1;
    public TextMeshProUGUI myTextNode3d_2;
    public TextMeshProUGUI myTextNode3d_3;

    public TypewriterTMP textNode3d_1;
    public TypewriterTMP textNode3d_2;
    public TypewriterTMP textNode3d_3;

    public TextMeshProUGUI myTextNode3d_I1;
    public TextMeshProUGUI myTextNode3d_I2;
    public TextMeshProUGUI myTextNode3d_I3;

    public TypewriterTMP textNode3d_I1;
    public TypewriterTMP textNode3d_I2;
    public TypewriterTMP textNode3d_I3;

    public TextMeshProUGUI myTextNode3d_C1;
    public TextMeshProUGUI myTextNode3d_C2;
    public TextMeshProUGUI myTextNode3d_C3;

    public TypewriterTMP textNode3d_C1;
    public TypewriterTMP textNode3d_C2;
    public TypewriterTMP textNode3d_C3;

    public TextMeshProUGUI myTextNode3e_1;
    public TextMeshProUGUI myTextNode3e_2;
    public TextMeshProUGUI myTextNode3e_3;

    public TypewriterTMP textNode3e_1;
    public TypewriterTMP textNode3e_2;
    public TypewriterTMP textNode3e_3;

    public TextMeshProUGUI myTextNode3f_1;
    public TextMeshProUGUI myTextNode3f_2;

    public TypewriterTMP textNode3f_1;
    public TypewriterTMP textNode3f_2;

    public TextMeshProUGUI myTextNode3f_I1;
    public TextMeshProUGUI myTextNode3f_I2;
    public TextMeshProUGUI myTextNode3f_I3;

    public TypewriterTMP textNode3f_I1;
    public TypewriterTMP textNode3f_I2;
    public TypewriterTMP textNode3f_I3;

    public TextMeshProUGUI myTextNode3f_C1;
    public TextMeshProUGUI myTextNode3f_C2;
    public TextMeshProUGUI myTextNode3f_C3;
    public TextMeshProUGUI myTextNode3f_C4;

    public TypewriterTMP textNode3f_C1;
    public TypewriterTMP textNode3f_C2;
    public TypewriterTMP textNode3f_C3;
    public TypewriterTMP textNode3f_C4;

    public TextMeshProUGUI myTextNode3g_1;
    public TextMeshProUGUI myTextNode3g_2;
    public TextMeshProUGUI myTextNode3g_3;

    public TypewriterTMP textNode3g_1;
    public TypewriterTMP textNode3g_2;
    public TypewriterTMP textNode3g_3;

    public TextMeshProUGUI myTextNode3h_1;
    public TextMeshProUGUI myTextNode3h_2;

    public TypewriterTMP textNode3h_1;
    public TypewriterTMP textNode3h_2;

    public TextMeshProUGUI myTextNode3i_1;
    public TextMeshProUGUI myTextNode3i_2;
    public TextMeshProUGUI myTextNode3i_3;

    public TypewriterTMP textNode3i_1;
    public TypewriterTMP textNode3i_2;
    public TypewriterTMP textNode3i_3;

    public TextMeshProUGUI myTextNode3j_1;
    public TextMeshProUGUI myTextNode3j_2;
    public TextMeshProUGUI myTextNode3j_3;

    public TypewriterTMP textNode3j_1;
    public TypewriterTMP textNode3j_2;
    public TypewriterTMP textNode3j_3;

    public TimerNode4 timerNode4;
    public GameObject GO_Node4;
    public GameObject GO_Node3;

    public bool flagOneTime;

    public bool flagRepeticionCiencia;
    public bool flagRepeticionCienciaCorrecta;
    public bool flagRepeticionCienciaIncorrecta1;

    public bool flagRepeticionTecnologia;
    public bool flagRepeticionTecnologiaCorrecta;
    public bool flagRepeticionTecnologiaIncorrecta1;

    //////////////////// AUDIOS ////////////////////////
    /// SFX Anim Sec ///
    [Header("SFX Anim Sec")]
    public AudioSource Anim1;
    public AudioSource Anim2;
    /// Dialogos ///
    [Header("Audio Sources")]
    public AudioSource[] sources;

    private Dictionary<string, AudioSource> audioDict;

    void Awake()
    {
        audioDict = new Dictionary<string, AudioSource>();
        foreach (AudioSource src in sources)
        {
            if (src != null)
            {
                audioDict[src.gameObject.name] = src;
            }
        }

        flagRepeticionCiencia = false;
        flagRepeticionCienciaCorrecta = false;
        flagRepeticionCienciaIncorrecta1 = false;

        flagRepeticionTecnologia = false;
        flagRepeticionTecnologiaCorrecta = false;
        flagRepeticionTecnologiaIncorrecta1 = false;
    }

    public void PlayByName(string name)
    {
        if (audioDict.ContainsKey(name))
        {
            audioDict[name].Play();
        }
        else
        {
            Debug.LogWarning("No AudioSource found with name: " + name);
        }
    }

    public void StopByName(string name)
    {
        if (audioDict.ContainsKey(name))
        {
            audioDict[name].Stop();
        }
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (flagOneTime == false)
        {

            StartTimerNode3();

            flagOneTime = true;
        }
    }

    public void StartTimerNode3()
    {
        StartCoroutine(AdvancingTimerNode3());
    }

    IEnumerator AdvancingTimerNode3()
    {
        GO_CanvaNode3_0.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayerNode3_0);
        Anim1.Play(); // sfx animacion secundaria

        yield return new WaitForSeconds(4.8f);

        GO_CanvaNode3a.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayerNode3a, GO_CanvaNode3_0);

        videoPlayerNode3a.playbackSpeed = 1f;
        PlayByName("3a_1");

        myTextNode3a_1.gameObject.SetActive(true);
        textNode3a_1.StartTyping();

        yield return new WaitForSeconds(3f);

        myTextNode3a_1.gameObject.SetActive(false);
        myTextNode3a_2.gameObject.SetActive(true);
        textNode3a_2.StartTyping();
        PlayByName("3a_2");

        yield return new WaitForSeconds(2f);

        myTextNode3a_2.gameObject.SetActive(false);
        myTextNode3a_3.gameObject.SetActive(true);
        textNode3a_3.StartTyping();
        PlayByName("3a_3");

        yield return new WaitForSeconds(2f);

        GO_CanvaNode3b.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayerNode3b, GO_CanvaNode3a);

        videoPlayerNode3b.playbackSpeed = 1f;

        myTextNode3b_1.gameObject.SetActive(true);
        textNode3b_1.StartTyping();
        PlayByName("3b_1");

        yield return new WaitForSeconds(7.5f);

        myTextNode3b_1.gameObject.SetActive(false);
        myTextNode3b_2.gameObject.SetActive(true);
        textNode3b_2.StartTyping();
        PlayByName("3b_2");

        yield return new WaitForSeconds(10f);

        myTextNode3b_2.gameObject.SetActive(false);
        myTextNode3b_3.gameObject.SetActive(true);
        textNode3b_3.StartTyping();
        PlayByName("3b_3");

        yield return new WaitForSeconds(4f);

        GO_CanvaNode3c.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayerNode3c, GO_CanvaNode3b);

        videoPlayerNode3c.playbackSpeed = 1f;

        myTextNode3c_1.gameObject.SetActive(true);
        textNode3c_1.StartTyping();
        PlayByName("3c_1");

        yield return new WaitForSeconds(6.4f);

        myTextNode3c_1.gameObject.SetActive(false);
        myTextNode3c_2.gameObject.SetActive(true);
        textNode3c_2.StartTyping();
        PlayByName("3c_2");

        yield return new WaitForSeconds(7f);

        myTextNode3c_2.gameObject.SetActive(false);
        myTextNode3c_3.gameObject.SetActive(true);
        textNode3c_3.StartTyping();
        PlayByName("3c_3");

        yield return new WaitForSeconds(4.4f);

        myTextNode3c_3.gameObject.SetActive(false);
        myTextNode3c_4.gameObject.SetActive(true);
        textNode3c_4.StartTyping();
        PlayByName("3c_4");

        yield return new WaitForSeconds(8f);

        GO_CanvaNode3c.SetActive(false);
        GO_CanvaNode3D1.SetActive(true);
    }

    //0 Ciencia 1 Tecnologia 2 Innovacion
    public void ElegirOpcion(int intOption)
    {
        if (intOption == 0)
        {
            StartCoroutine(AdvancingTimerCiencia());
        }
        else if (intOption == 1)
        {
            StartCoroutine(AdvancingTimerTecnologia());
        }
        else if (intOption == 2)
        {
            StartCoroutine(AdvancingTimerInnovacion());
        }
        else
        {
            Debug.Log("Esta opcion no existe");
        }

    }

    IEnumerator AdvancingTimerCiencia()
    {
        if (flagRepeticionCiencia == true)
        {
            textNode3d_1.RestartText();
            textNode3d_2.RestartText();
            textNode3d_3.RestartText();
        }

        GO_CanvaNode3d.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayerNode3d, GO_CanvaNode3D1);

        videoPlayerNode3d.playbackSpeed = 1f;

        myTextNode3d_1.gameObject.SetActive(true);
        textNode3d_1.StartTyping();
        PlayByName("3d_1");

        yield return new WaitForSeconds(2f);

        myTextNode3d_2.gameObject.SetActive(true);
        myTextNode3d_1.gameObject.SetActive(false);
        textNode3d_2.StartTyping();
        PlayByName("3d_2");

        yield return new WaitForSeconds(6.4f);

        myTextNode3d_3.gameObject.SetActive(true);
        myTextNode3d_2.gameObject.SetActive(false);
        textNode3d_3.StartTyping();
        PlayByName("3d_3");

        yield return new WaitForSeconds(3f);
        myTextNode3d_3.gameObject.SetActive(false);

        GO_PanelPreguntaCiencia.SetActive(true);

        flagRepeticionCiencia = true;
    }

    //Opcion 1 - Correcta
    //Opcion 2 - Incorrecta
    //Opcion 3 - Incorrecta
    public void ElegirPreguntaCiencia(int intCiencia)
    {
        if (intCiencia == 0)
        {
            StartCoroutine(AdvancingTimerOpcionCorrecta1());
        }
        else if (intCiencia == 1)
        {
            StartCoroutine(AdvancingTimerOpcionIncorrecta1());
        }
        else if (intCiencia == 2)
        {
            StartCoroutine(AdvancingTimerOpcionIncorrecta1());
        }
        else
        {
            Debug.Log("Esta opcion no existe");
        }
    }

    IEnumerator AdvancingTimerOpcionIncorrecta1()
    {
        if (flagRepeticionCienciaIncorrecta1 == true)
        {
            textNode3d_I1.RestartText();
            textNode3d_I2.RestartText();
            textNode3d_I3.RestartText();
        }

        GO_PanelPreguntaCiencia.SetActive(false);

        GO_CanvaNode3d.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayerNode3d);

        videoPlayerNode3d.playbackSpeed = 1f;

        myTextNode3d_I1.gameObject.SetActive(true);
        textNode3d_I1.StartTyping();
        PlayByName("3d_I1");

        yield return new WaitForSeconds(8f);

        myTextNode3d_I1.gameObject.SetActive(false);
        myTextNode3d_I2.gameObject.SetActive(true);
        textNode3d_I2.StartTyping();
        PlayByName("3d_I2");

        yield return new WaitForSeconds(5.5f);

        myTextNode3d_I2.gameObject.SetActive(false);
        myTextNode3d_I3.gameObject.SetActive(true);
        textNode3d_I3.StartTyping();
        PlayByName("3d_I3");

        yield return new WaitForSeconds(6f);

        myTextNode3d_I3.gameObject.SetActive(false);

        GO_PanelPreguntaCiencia.SetActive(true);

        flagRepeticionCienciaIncorrecta1 = true;
    }

    IEnumerator AdvancingTimerOpcionCorrecta1()
    {
        if (flagRepeticionCienciaCorrecta == true)
        {
            textNode3d_C1.RestartText();
            textNode3d_C2.RestartText();
            textNode3d_C3.RestartText();
            textNode3e_1.RestartText();
            textNode3e_2.RestartText();
            textNode3e_3.RestartText();
        }

        GO_PanelPreguntaCiencia.SetActive(false);
        GO_CanvaNode3d.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayerNode3d);

        videoPlayerNode3d.playbackSpeed = 1f;

        myTextNode3d_C1.gameObject.SetActive(true);
        textNode3d_C1.StartTyping();
        PlayByName("3d_C1");

        yield return new WaitForSeconds(7.5f);

        myTextNode3d_C1.gameObject.SetActive(false);
        myTextNode3d_C2.gameObject.SetActive(true);
        textNode3d_C2.StartTyping();
        PlayByName("3d_C2");

        yield return new WaitForSeconds(9.5f);

        myTextNode3d_C2.gameObject.SetActive(false);
        myTextNode3d_C3.gameObject.SetActive(true);
        textNode3d_C3.StartTyping();
        PlayByName("3d_C3");

        yield return new WaitForSeconds(5.5f);

        myTextNode3d_C3.gameObject.SetActive(false);
        GO_CanvaNode3e.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayerNode3e, GO_CanvaNode3d);

        videoPlayerNode3e.playbackSpeed = 1f;

        myTextNode3e_1.gameObject.SetActive(true);
        textNode3e_1.StartTyping();
        PlayByName("3e_1");

        yield return new WaitForSeconds(2.5f);

        myTextNode3e_1.gameObject.SetActive(false);
        myTextNode3e_2.gameObject.SetActive(true);
        textNode3e_2.StartTyping();
        PlayByName("3e_2");

        yield return new WaitForSeconds(6.3f);

        myTextNode3e_2.gameObject.SetActive(false);
        myTextNode3e_3.gameObject.SetActive(true);
        textNode3e_3.StartTyping();
        PlayByName("3e_3");

        yield return new WaitForSeconds(5.4f);

        myTextNode3e_3.gameObject.SetActive(false);
        GO_CanvaNode3D1.SetActive(true);
        GO_CanvaNode3e.SetActive(false);

        flagRepeticionCienciaCorrecta = true;
    }


    //Opcion 1 - Incorrecta
    //Opcion 2 - Correcta
    //Opcion 3 - Incorrecta
    public void ElegirPreguntaTecnologia(int intTecnologia)
    {
        if (intTecnologia == 0)
        {
            StartCoroutine(AdvancingTimerOpcionIncorrecta2());
        }
        else if (intTecnologia == 1)
        {
            StartCoroutine(AdvancingTimerOpcionCorrecta2());
        }
        else if (intTecnologia == 2)
        {
            StartCoroutine(AdvancingTimerOpcionIncorrecta2());
        }
        else
        {
            Debug.Log("Esta opcion no existe");
        }
    }

    IEnumerator AdvancingTimerTecnologia()
    {
        if (flagRepeticionTecnologia == true)
        {
            textNode3f_1.RestartText();
            textNode3f_2.RestartText();
        }

        GO_CanvaNode3f.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayerNode3f, GO_CanvaNode3D1);

        videoPlayerNode3f.playbackSpeed = 1f;

        myTextNode3f_1.gameObject.SetActive(true);
        textNode3f_1.StartTyping();
        PlayByName("3f_1");

        yield return new WaitForSeconds(6.9f);

        myTextNode3f_1.gameObject.SetActive(false);
        myTextNode3f_2.gameObject.SetActive(true);
        textNode3f_2.StartTyping();
        PlayByName("3f_2");

        yield return new WaitForSeconds(4.3f);

        myTextNode3f_2.gameObject.SetActive(false);

        GO_PanelPreguntaTecnologia.SetActive(true);

        flagRepeticionTecnologia = true;
    }

    IEnumerator AdvancingTimerOpcionIncorrecta2()
    {
        if (flagRepeticionTecnologiaIncorrecta1 == true)
        {
            textNode3f_I1.RestartText();
            textNode3f_I2.RestartText();
            textNode3f_I3.RestartText();
        }

        GO_CanvaNode3f.SetActive(true);
        GO_PanelPreguntaTecnologia.SetActive(false);

        yield return PlayVideoWhenReady(videoPlayerNode3f);

        videoPlayerNode3f.playbackSpeed = 1f;

        myTextNode3f_I1.gameObject.SetActive(true);
        textNode3f_I1.StartTyping();
        PlayByName("3f_I1");

        yield return new WaitForSeconds(4f);

        myTextNode3f_I1.gameObject.SetActive(false);
        myTextNode3f_I2.gameObject.SetActive(true);
        textNode3f_I2.StartTyping();
        PlayByName("3f_I2");

        yield return new WaitForSeconds(12.5f);

        myTextNode3f_I2.gameObject.SetActive(false);
        myTextNode3f_I3.gameObject.SetActive(true);
        textNode3f_I3.StartTyping();
        PlayByName("3f_I3");

        yield return new WaitForSeconds(7.2f);

        myTextNode3f_I3.gameObject.SetActive(false);

        GO_PanelPreguntaTecnologia.SetActive(true);

        flagRepeticionTecnologiaIncorrecta1 = true;
    }

    IEnumerator AdvancingTimerOpcionCorrecta2()
    {
        if (flagRepeticionTecnologiaCorrecta == true)
        {
            textNode3f_C1.RestartText();
            textNode3f_C2.RestartText();
            textNode3f_C3.RestartText();
            textNode3g_1.RestartText();
            textNode3g_2.RestartText();
            textNode3g_3.RestartText();
        }

        GO_CanvaNode3f.SetActive(true);
        GO_PanelPreguntaTecnologia.SetActive(false);

        yield return PlayVideoWhenReady(videoPlayerNode3f);

        videoPlayerNode3f.playbackSpeed = 1f;

        myTextNode3f_C1.gameObject.SetActive(true);
        textNode3f_C1.StartTyping();
        PlayByName("3f_C1");

        yield return new WaitForSeconds(6.8f);

        myTextNode3f_C1.gameObject.SetActive(false);
        myTextNode3f_C2.gameObject.SetActive(true);
        textNode3f_C2.StartTyping();
        PlayByName("3f_C2");

        yield return new WaitForSeconds(7.8f);

        myTextNode3f_C2.gameObject.SetActive(false);
        myTextNode3f_C3.gameObject.SetActive(true);
        textNode3f_C3.StartTyping();
        PlayByName("3f_C3");

        yield return new WaitForSeconds(6.8f);
        myTextNode3f_C3.gameObject.SetActive(false);

        GO_CanvaNode3g.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayerNode3g, GO_CanvaNode3f);

        videoPlayerNode3g.playbackSpeed = 1f;

        myTextNode3g_1.gameObject.SetActive(true);
        textNode3g_1.StartTyping();
        PlayByName("3g_1");

        yield return new WaitForSeconds(3f);

        myTextNode3g_1.gameObject.SetActive(false);
        myTextNode3g_2.gameObject.SetActive(true);
        textNode3g_2.StartTyping();
        PlayByName("3g_2");

        yield return new WaitForSeconds(4f);

        myTextNode3g_2.gameObject.SetActive(false);
        myTextNode3g_3.gameObject.SetActive(true);
        textNode3g_3.StartTyping();
        PlayByName("3g_3");

        yield return new WaitForSeconds(4f);

        myTextNode3g_3.gameObject.SetActive(false);
        GO_CanvaNode3D1.SetActive(true);
        GO_CanvaNode3g.SetActive(false);

        flagRepeticionTecnologiaCorrecta = true;
    }


    IEnumerator AdvancingTimerInnovacion()
    {
        GO_CanvaNode3h0.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayerNode3h0, GO_CanvaNode3D1);
        Anim2.Play(); // sfx animacion secundaria


        videoPlayerNode3h0.playbackSpeed = 1f;

        yield return new WaitForSeconds(5f);

        GO_CanvaNode3h.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayerNode3h, GO_CanvaNode3h0);

        videoPlayerNode3h.playbackSpeed = 1f;

        myTextNode3h_1.gameObject.SetActive(true);
        textNode3h_1.StartTyping();
        PlayByName("3h_1");

        yield return new WaitForSeconds(7.7f);

        myTextNode3h_1.gameObject.SetActive(false);
        myTextNode3h_2.gameObject.SetActive(true);
        textNode3h_2.StartTyping();
        PlayByName("3h_2");

        yield return new WaitForSeconds(5.5f);

        GO_CanvaNode3i.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayerNode3i, GO_CanvaNode3h);

        videoPlayerNode3i.playbackSpeed = 1f;

        myTextNode3i_1.gameObject.SetActive(true);
        textNode3i_1.StartTyping();
        PlayByName("3i_1");

        yield return new WaitForSeconds(2.4f);

        myTextNode3i_1.gameObject.SetActive(false);
        myTextNode3i_2.gameObject.SetActive(true);
        textNode3i_2.StartTyping();
        PlayByName("3i_2");

        yield return new WaitForSeconds(8.4f);

        myTextNode3i_2.gameObject.SetActive(false);
        myTextNode3i_3.gameObject.SetActive(true);
        textNode3i_3.StartTyping();
        PlayByName("3i_3");

        yield return new WaitForSeconds(4.4f);

        GO_CanvaNode3j.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayerNode3j, GO_CanvaNode3i);

        videoPlayerNode3j.playbackSpeed = 1f;

        myTextNode3j_1.gameObject.SetActive(true);
        textNode3j_1.StartTyping();
        PlayByName("3j_1");

        yield return new WaitForSeconds(6f);

        myTextNode3j_1.gameObject.SetActive(false);
        myTextNode3j_2.gameObject.SetActive(true);
        textNode3j_2.StartTyping();
        PlayByName("3j_2");

        yield return new WaitForSeconds(7.6f);

        myTextNode3j_2.gameObject.SetActive(false);
        myTextNode3j_3.gameObject.SetActive(true);
        textNode3j_3.StartTyping();
        PlayByName("3j_3");

        yield return new WaitForSeconds(6f);

        GO_Node4.SetActive(true);
        timerNode4.StartTimerNode4();
        GO_Node3.SetActive(false);
    }
}