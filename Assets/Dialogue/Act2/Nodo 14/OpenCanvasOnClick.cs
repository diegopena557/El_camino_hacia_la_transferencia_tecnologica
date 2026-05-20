using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;

public class OpenCanvasOnClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject targetCanvas;
    [SerializeField] private GameObject oldCanvas;

    public VideoPlayer videoPlayer;

    public void OnPointerClick(PointerEventData eventData)
    {
        targetCanvas.SetActive(true);
        oldCanvas.SetActive(false);

        videoPlayer.Stop();
        videoPlayer.time = 0;        
        videoPlayer.frame = 0;
        videoPlayer.Play();
    }
}
