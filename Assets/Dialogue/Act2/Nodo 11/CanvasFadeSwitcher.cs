using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class CanvasFadeSwitcher : MonoBehaviour, IPointerClickHandler
{
    public CanvasGroup currentCanvas;
    public CanvasGroup nextCanvas;

    public float fadeDuration = 1f;

    private bool isTransitioning = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isTransitioning)
        {
            StartCoroutine(FadeTransition());
        }
    }

    IEnumerator FadeTransition()
    {
        isTransitioning = true;

        float time = 0;

        nextCanvas.gameObject.SetActive(true);

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            float t = time / fadeDuration;

            // Fade out current
            currentCanvas.alpha = Mathf.Lerp(1, 0, t);

            // Fade in next
            nextCanvas.alpha = Mathf.Lerp(0, 1, t);

            yield return null;
        }

        currentCanvas.alpha = 0;
        nextCanvas.alpha = 1;

        currentCanvas.interactable = false;
        currentCanvas.blocksRaycasts = false;

        nextCanvas.interactable = true;
        nextCanvas.blocksRaycasts = true;

        isTransitioning = false;
    }
}
