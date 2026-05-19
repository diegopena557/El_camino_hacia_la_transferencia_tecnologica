using UnityEngine;

public class ShowCanvas : MonoBehaviour
{
    
    public CanvasGroup canvasGroup;

    public void Show()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}
