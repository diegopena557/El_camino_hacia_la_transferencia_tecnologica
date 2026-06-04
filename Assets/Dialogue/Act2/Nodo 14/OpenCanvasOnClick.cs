using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;
using System.Collections;

public class OpenCanvasOnClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject targetCanvas;
    [SerializeField] private GameObject oldCanvas;

    public VideoPlayer videoPlayer;

    [Header("End Panel (solo en la decisión final)")]
    [SerializeField] private TimerTextNode14 timerNode14;

    private bool videoFinished = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        targetCanvas.SetActive(true);
        oldCanvas.SetActive(false);

        videoPlayer.Stop();
        videoPlayer.time = 0;
        videoPlayer.frame = 0;

        if (timerNode14 != null)
        {
            videoFinished = false;
            videoPlayer.loopPointReached += OnVideoFinished;
        }

        videoPlayer.Play();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        if (videoFinished) return; // Evita doble disparo
        videoFinished = true;

        videoPlayer.loopPointReached -= OnVideoFinished; // Desuscribe el evento
        timerNode14.TriggerEndPanel();
    }
}