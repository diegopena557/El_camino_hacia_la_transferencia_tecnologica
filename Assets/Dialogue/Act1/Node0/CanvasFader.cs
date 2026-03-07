using UnityEngine;
using System.Collections;

public class CanvasFader : MonoBehaviour
{
    public CanvasGroup canvasA;
    public CanvasGroup canvasB;
    public float fadeDuration = 1f;

    public void FadeAToB()
    {
        StartCoroutine(Fade(canvasA, canvasB));
    }

    IEnumerator Fade(CanvasGroup from, CanvasGroup to)
    {
        to.gameObject.SetActive(true);

        float t = 0f;

        from.interactable = false;
        from.blocksRaycasts = false;

        to.interactable = false;
        to.blocksRaycasts = false;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float normalized = t / fadeDuration;

            from.alpha = Mathf.Lerp(1f, 0f, normalized);
            to.alpha   = Mathf.Lerp(0f, 1f, normalized);

            yield return null;
        }

        from.alpha = 0f;
        to.alpha = 1f;

        to.interactable = true;
        to.blocksRaycasts = true;
        from.gameObject.SetActive(false);
    }
}
