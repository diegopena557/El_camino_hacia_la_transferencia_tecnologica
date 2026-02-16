using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;

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

    public bool flagOneTime;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(flagOneTime == false){

            StartTimerNode3();

            flagOneTime = true;
        }
    }

    public void StartTimerNode3(){
        StartCoroutine(AdvancingTimerNode3());
    }

    IEnumerator AdvancingTimerNode3()
    {
        GO_CanvaNode3_0.SetActive(true);

        videoPlayerNode3_0.Stop();
        videoPlayerNode3_0.time = 0;        
        videoPlayerNode3_0.frame = 0;
        videoPlayerNode3_0.Play();

        yield return new WaitForSeconds(5f);

        GO_CanvaNode3_0.SetActive(false);
        GO_CanvaNode3a.SetActive(true);

        videoPlayerNode3a.Stop();
        videoPlayerNode3a.time = 0;        
        videoPlayerNode3a.frame = 0;
        videoPlayerNode3a.Play();

        videoPlayerNode3a.playbackSpeed = 1f;

        myTextNode3a_1.gameObject.SetActive(true);
        textNode3a_1.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode3a_1.gameObject.SetActive(false);
        myTextNode3a_2.gameObject.SetActive(true);
        textNode3a_2.StartTyping();

        yield return new WaitForSeconds(3f);

        myTextNode3a_2.gameObject.SetActive(false);
        myTextNode3a_3.gameObject.SetActive(true);
        textNode3a_3.StartTyping();

        yield return new WaitForSeconds(5f);

        GO_CanvaNode3a.SetActive(false);
        GO_CanvaNode3b.SetActive(true);

        videoPlayerNode3b.Stop();
        videoPlayerNode3b.time = 0;        
        videoPlayerNode3b.frame = 0;
        videoPlayerNode3b.Play();

        videoPlayerNode3b.playbackSpeed = 1f;

        myTextNode3b_1.gameObject.SetActive(true);
        textNode3b_1.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode3b_1.gameObject.SetActive(false);
        myTextNode3b_2.gameObject.SetActive(true);
        textNode3b_2.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode3b_2.gameObject.SetActive(false);
        myTextNode3b_3.gameObject.SetActive(true);
        textNode3b_3.StartTyping();

        yield return new WaitForSeconds(5f);

        GO_CanvaNode3b.SetActive(false);
        GO_CanvaNode3c.SetActive(true);

        videoPlayerNode3c.Stop();
        videoPlayerNode3c.time = 0;        
        videoPlayerNode3c.frame = 0;
        videoPlayerNode3c.Play();

        videoPlayerNode3c.playbackSpeed = 1f;

        myTextNode3c_1.gameObject.SetActive(true);
        textNode3c_1.StartTyping();

        yield return new WaitForSeconds(3f);

        myTextNode3c_1.gameObject.SetActive(false);
        myTextNode3c_2.gameObject.SetActive(true);
        textNode3c_2.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode3c_2.gameObject.SetActive(false);
        myTextNode3c_3.gameObject.SetActive(true);
        textNode3c_3.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode3c_3.gameObject.SetActive(false);
        myTextNode3c_4.gameObject.SetActive(true);
        textNode3c_4.StartTyping();

        yield return new WaitForSeconds(5f);

        GO_CanvaNode3c.SetActive(false);
        GO_CanvaNode3D1.SetActive(true);
    }

    //0 Ciencia 1 Tecnologia 2 Innovacion
    public void ElegirOpcion(int intOption){
        if(intOption == 0){
            StartCoroutine(AdvancingTimerCiencia());
        }else if(intOption == 1){
            StartCoroutine(AdvancingTimerTecnologia());
        }else if(intOption == 2){
            StartCoroutine(AdvancingTimerInnovacion());
        }else{
            Debug.Log("Esta opción no existe");
        }

    }

    IEnumerator AdvancingTimerCiencia()
    {
        GO_CanvaNode3D1.SetActive(false);
        GO_CanvaNode3d.SetActive(true);

        videoPlayerNode3d.Stop();
        videoPlayerNode3d.time = 0;        
        videoPlayerNode3d.frame = 0;
        videoPlayerNode3d.Play();

        videoPlayerNode3d.playbackSpeed = 1f;

        myTextNode3d_1.gameObject.SetActive(true);
        textNode3d_1.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode3d_1.gameObject.SetActive(false);
        myTextNode3d_2.gameObject.SetActive(true);
        textNode3d_2.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode3d_2.gameObject.SetActive(false);
        myTextNode3d_3.gameObject.SetActive(true);
        textNode3d_3.StartTyping();

        yield return new WaitForSeconds(5f);
        myTextNode3d_3.gameObject.SetActive(false);

        GO_PanelPreguntaCiencia.SetActive(true);
    }

    //Opcion 1 - Correcta
    //Opcion 2 - Incorrecta
    //Opcion 3 - Incorrecta
    public void ElegirPreguntaCiencia(int intCiencia){
        if(intCiencia == 0){
            StartCoroutine(AdvancingTimerOpcionCorrecta1());
        }else if(intCiencia == 1){
            StartCoroutine(AdvancingTimerOpcionIncorrecta1());
        }else if(intCiencia == 2){
            StartCoroutine(AdvancingTimerOpcionIncorrecta1());
        }else{
            Debug.Log("Esta opción no existe");
        }
    }

    IEnumerator AdvancingTimerOpcionIncorrecta1()
    {
        GO_PanelPreguntaCiencia.SetActive(false);
        
        GO_CanvaNode3d.SetActive(true);

        videoPlayerNode3d.Stop();
        videoPlayerNode3d.time = 0;        
        videoPlayerNode3d.frame = 0;
        videoPlayerNode3d.Play();

        videoPlayerNode3d.playbackSpeed = 1f;

        myTextNode3d_I1.gameObject.SetActive(true);
        textNode3d_I1.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode3d_I1.gameObject.SetActive(false);
        myTextNode3d_I2.gameObject.SetActive(true);
        textNode3d_I2.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode3d_I2.gameObject.SetActive(false);
        myTextNode3d_I3.gameObject.SetActive(true);
        textNode3d_I3.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode3d_I3.gameObject.SetActive(false);

        GO_PanelPreguntaCiencia.SetActive(true);
    }

    IEnumerator AdvancingTimerOpcionCorrecta1()
    {
        GO_PanelPreguntaCiencia.SetActive(false);
        GO_CanvaNode3d.SetActive(true);

        videoPlayerNode3d.Stop();
        videoPlayerNode3d.time = 0;        
        videoPlayerNode3d.frame = 0;
        videoPlayerNode3d.Play();

        videoPlayerNode3d.playbackSpeed = 1f;

        myTextNode3d_C1.gameObject.SetActive(true);
        textNode3d_C1.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode3d_C1.gameObject.SetActive(false);
        myTextNode3d_C2.gameObject.SetActive(true);
        textNode3d_C2.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode3d_C2.gameObject.SetActive(false);
        myTextNode3d_C3.gameObject.SetActive(true);
        textNode3d_C3.StartTyping();

        yield return new WaitForSeconds(5f);

        GO_CanvaNode3d.SetActive(false);
        GO_CanvaNode3e.SetActive(true);

        videoPlayerNode3e.Stop();
        videoPlayerNode3e.time = 0;        
        videoPlayerNode3e.frame = 0;
        videoPlayerNode3e.Play();

        videoPlayerNode3e.playbackSpeed = 1f;

        myTextNode3e_1.gameObject.SetActive(true);
        textNode3e_1.StartTyping();

        yield return new WaitForSeconds(3f);

        myTextNode3e_1.gameObject.SetActive(false);
        myTextNode3e_2.gameObject.SetActive(true);
        textNode3e_2.StartTyping();

        yield return new WaitForSeconds(3f);

        myTextNode3e_2.gameObject.SetActive(false);
        myTextNode3e_3.gameObject.SetActive(true);
        textNode3e_3.StartTyping();

        yield return new WaitForSeconds(3f);

        myTextNode3e_3.gameObject.SetActive(false);
    }


    //Opcion 1 - Incorrecta
    //Opcion 2 - Correcta
    //Opcion 3 - Incorrecta
    public void ElegirPreguntaTecnologia(int intTecnologia){
        if(intTecnologia == 0){
            StartCoroutine(AdvancingTimerOpcionIncorrecta2());
        }else if(intTecnologia == 1){
            StartCoroutine(AdvancingTimerOpcionCorrecta2());
        }else if(intTecnologia == 2){
            StartCoroutine(AdvancingTimerOpcionIncorrecta2());
        }else{
            Debug.Log("Esta opción no existe");
        }
    }

    IEnumerator AdvancingTimerTecnologia()
    {
        GO_CanvaNode3D1.SetActive(false);
        GO_CanvaNode3f.SetActive(true);

        videoPlayerNode3f.Stop();
        videoPlayerNode3f.time = 0;        
        videoPlayerNode3f.frame = 0;
        videoPlayerNode3f.Play();

        videoPlayerNode3f.playbackSpeed = 1f;

        myTextNode3f_1.gameObject.SetActive(true);
        textNode3f_1.StartTyping();

        yield return new WaitForSeconds(3f);

        myTextNode3f_1.gameObject.SetActive(false);
        myTextNode3f_2.gameObject.SetActive(true);
        textNode3f_2.StartTyping();

        yield return new WaitForSeconds(3f);

        myTextNode3f_2.gameObject.SetActive(false);

        GO_PanelPreguntaTecnologia.SetActive(true);
    }

    IEnumerator AdvancingTimerOpcionIncorrecta2()
    {
        GO_PanelPreguntaTecnologia.SetActive(false);
        GO_CanvaNode3f.SetActive(true);

        videoPlayerNode3f.Stop();
        videoPlayerNode3f.time = 0;        
        videoPlayerNode3f.frame = 0;
        videoPlayerNode3f.Play();

        videoPlayerNode3f.playbackSpeed = 1f;

        myTextNode3f_I1.gameObject.SetActive(true);
        textNode3f_I1.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode3f_I1.gameObject.SetActive(false);
        myTextNode3f_I2.gameObject.SetActive(true);
        textNode3f_I2.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode3f_I2.gameObject.SetActive(false);
        myTextNode3f_I3.gameObject.SetActive(true);
        textNode3f_I3.StartTyping();

        yield return new WaitForSeconds(3f);

        myTextNode3f_I3.gameObject.SetActive(false);

        GO_PanelPreguntaTecnologia.SetActive(true);
    }

    IEnumerator AdvancingTimerOpcionCorrecta2()
    {
        GO_PanelPreguntaTecnologia.SetActive(false);
        GO_CanvaNode3f.SetActive(true);

        videoPlayerNode3f.Stop();
        videoPlayerNode3f.time = 0;        
        videoPlayerNode3f.frame = 0;
        videoPlayerNode3f.Play();

        videoPlayerNode3f.playbackSpeed = 1f;

        myTextNode3f_C1.gameObject.SetActive(true);
        textNode3f_C1.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode3f_C1.gameObject.SetActive(false);
        myTextNode3f_C2.gameObject.SetActive(true);
        textNode3f_C2.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode3f_C2.gameObject.SetActive(false);
        myTextNode3f_C3.gameObject.SetActive(true);
        textNode3f_C3.StartTyping();

        yield return new WaitForSeconds(5f);

        GO_CanvaNode3f.SetActive(false);
        GO_CanvaNode3g.SetActive(true);

        videoPlayerNode3g.Stop();
        videoPlayerNode3g.time = 0;        
        videoPlayerNode3g.frame = 0;
        videoPlayerNode3g.Play();

        videoPlayerNode3g.playbackSpeed = 1f;

        myTextNode3g_1.gameObject.SetActive(true);
        textNode3g_1.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode3g_1.gameObject.SetActive(false);
        myTextNode3g_2.gameObject.SetActive(true);
        textNode3g_2.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode3g_2.gameObject.SetActive(false);
        myTextNode3g_3.gameObject.SetActive(true);
        textNode3g_3.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode3g_3.gameObject.SetActive(false);
    }


    IEnumerator AdvancingTimerInnovacion()
    {
        GO_CanvaNode3D1.SetActive(false);
        GO_CanvaNode3h0.SetActive(true);

        videoPlayerNode3h0.Stop();
        videoPlayerNode3h0.time = 0;        
        videoPlayerNode3h0.frame = 0;
        videoPlayerNode3h0.Play();

        videoPlayerNode3h0.playbackSpeed = 1f;

        yield return new WaitForSeconds(5f);

        GO_CanvaNode3h0.SetActive(false);
        GO_CanvaNode3h.SetActive(true);

        videoPlayerNode3h.Stop();
        videoPlayerNode3h.time = 0;        
        videoPlayerNode3h.frame = 0;
        videoPlayerNode3h.Play();

        videoPlayerNode3h.playbackSpeed = 1f;

        myTextNode3h_1.gameObject.SetActive(true);
        textNode3h_1.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode3h_1.gameObject.SetActive(false);
        myTextNode3h_2.gameObject.SetActive(true);
        textNode3h_2.StartTyping();

        yield return new WaitForSeconds(5f);

        GO_CanvaNode3h.SetActive(false);
        GO_CanvaNode3i.SetActive(true);

        videoPlayerNode3i.Stop();
        videoPlayerNode3i.time = 0;        
        videoPlayerNode3i.frame = 0;
        videoPlayerNode3i.Play();

        videoPlayerNode3i.playbackSpeed = 1f;

        myTextNode3i_1.gameObject.SetActive(true);
        textNode3i_1.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode3i_1.gameObject.SetActive(false);
        myTextNode3i_2.gameObject.SetActive(true);
        textNode3i_2.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode3i_2.gameObject.SetActive(false);
        myTextNode3i_3.gameObject.SetActive(true);
        textNode3i_3.StartTyping();

        yield return new WaitForSeconds(5f);

        GO_CanvaNode3i.SetActive(false);
        GO_CanvaNode3j.SetActive(true);

        videoPlayerNode3j.Stop();
        videoPlayerNode3j.time = 0;        
        videoPlayerNode3j.frame = 0;
        videoPlayerNode3j.Play();

        videoPlayerNode3j.playbackSpeed = 1f;

        myTextNode3j_1.gameObject.SetActive(true);
        textNode3j_1.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode3j_1.gameObject.SetActive(false);
        myTextNode3j_2.gameObject.SetActive(true);
        textNode3j_2.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode3j_2.gameObject.SetActive(false);
        myTextNode3j_3.gameObject.SetActive(true);
        textNode3j_3.StartTyping();

    }
}
