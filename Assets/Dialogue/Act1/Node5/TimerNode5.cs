using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;

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

    public int OptionTecnology{ set; get;}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(flagOneTime == false){

            StartTimerNode5();

            flagOneTime = true;
        }
    }

    public void StartTimerNode5(){
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

        yield return new WaitForSeconds(5f);

        myTextNode5a_1.gameObject.SetActive(false);
        myTextNode5a_2.gameObject.SetActive(true);
        textNode5a_2.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode5a_2.gameObject.SetActive(false);
        myTextNode5a_3.gameObject.SetActive(true);
        textNode5a_3.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode5a_3.gameObject.SetActive(false);
        myTextNode5a_4.gameObject.SetActive(true);
        textNode5a_4.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode5a_4.gameObject.SetActive(false);
        myTextNode5a_5.gameObject.SetActive(true);
        textNode5a_5.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode5a_5.gameObject.SetActive(false);
        myTextNode5a_6.gameObject.SetActive(true);
        textNode5a_6.StartTyping();

        yield return new WaitForSeconds(5f);

        myTextNode5a_6.gameObject.SetActive(false);
        myTextNode5a_7.gameObject.SetActive(true);
        textNode5a_7.StartTyping();

        yield return new WaitForSeconds(5f);

        GO_CanvaNode5a.SetActive(false);
        GO_CanvaNode5D1.SetActive(true);
    }

    public void OpcionTecnologia(int optTecnologia){
        if(optTecnologia == 0){
            OptionTecnology = 0;
            StartCoroutine(AdvancingTimerNode5_2());
        }else if(optTecnologia == 1){
            OptionTecnology = 1;
            StartCoroutine(AdvancingTimerNode5_2());
        }else{
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

        yield return new WaitForSeconds(5f);
    }
}
