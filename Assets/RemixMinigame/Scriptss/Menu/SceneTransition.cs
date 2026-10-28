using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public CanvasGroup fadeOverlay;
    public float fadeDuration = 0.6f;

    public void LoadScene(string sceneName)
    {
        StartCoroutine(FadeAndLoad(sceneName));
    }

    IEnumerator FadeAndLoad(string sceneName)
    {
        fadeOverlay.blocksRaycasts = true; // stop further clicks during transition

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeOverlay.alpha = Mathf.Clamp01(t / fadeDuration);
            yield return null;
        }

        fadeOverlay.alpha = 1f;
        SceneManager.LoadScene(sceneName);
    }
}