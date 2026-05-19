using UnityEngine;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class VideoHoverPlayer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public VideoPlayer videoPlayer;

    // Called when mouse enters the RawImage
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (videoPlayer != null)
        {
            videoPlayer.Play();
        }
    }

    // Optional: stop or pause when mouse exits
    public void OnPointerExit(PointerEventData eventData)
    {
        if (videoPlayer != null)
        {
            videoPlayer.Pause();
            // or videoPlayer.Stop();
        }
    }
}
