using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneFadeIn : MonoBehaviour
{
    public static SceneFadeIn Instance;

    [Header("Referencia")]
    public CanvasGroup canvasGroup;

    [Header("Configuracion")]
    public float fadeDuration = 1f;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        // Garantizar que este Canvas quede encima de todo
        Canvas myCanvas = GetComponent<Canvas>();
        if (myCanvas == null)
            myCanvas = gameObject.AddComponent<Canvas>();

        myCanvas.overrideSorting = true;
        myCanvas.sortingOrder = 999;

        if (GetComponent<GraphicRaycaster>() == null)
            gameObject.AddComponent<GraphicRaycaster>();

        // Completamente opaco desde el primer frame
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    void Start()
    {
        // Sin fade in al inicio, el panel empieza invisible
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }

    // Fade out antes de cambiar de escena (transparente -> negro) y luego carga la escena
    public void FadeOutAndLoadScene(string sceneName, float duration = -1f)
    {
        StartCoroutine(FadeOutCoroutine(sceneName, duration < 0f ? fadeDuration : duration));
    }

    IEnumerator FadeOutCoroutine(string sceneName, float duration)
    {
        gameObject.SetActive(true);
        canvasGroup.blocksRaycasts = true;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, Mathf.Clamp01(elapsed / duration));
            yield return null;
        }

        canvasGroup.alpha = 1f;

        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}