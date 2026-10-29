using UnityEngine;
using System.Collections;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VideoMenuController : MonoBehaviour
{
    [Header("Video")]
    public GameObject GO_Node;
    public GameObject GO_Canva;
    public VideoPlayer videoPlayer;

    [Header("Text")]
    [Tooltip("Objeto del texto. Dejalo inactivo en la escena: se activa despues del fade in y ahi empieza a escribirse.")]
    public GameObject GO_Text;
    [Tooltip("Typewriter que se espera antes de mostrar los botones.")]
    public TypewriterTMPAuto typewriter;
    [Tooltip("Pausa extra despues de terminar el texto, antes de mostrar los botones.")]
    public float buttonsDelay = 0.5f;

    [Header("Buttons")]
    [Tooltip("Contenedor de los botones. Se muestra cuando termina el texto.")]
    public GameObject GO_Buttons;
    public Button playButton;
    public Button quitButton;
    public float buttonsFadeInDuration = 1f;

    private CanvasGroup buttonsGroup;

    [Header("Fade Settings")]
    public Image fadePanel;
    public float fadeInDuration = 1.5f;
    public float fadeOutDuration = 1.5f;

    [Header("Scene")]
    public string nextSceneName = "";

    private bool isTransitioning;

    void Awake()
    {
        // El panel de fade empieza completamente opaco (negro)
        if (fadePanel != null)
        {
            Color c = fadePanel.color;
            c.a = 1f;
            fadePanel.color = c;
            fadePanel.gameObject.SetActive(true);
        }

        // Los botones quedan ocultos (transparentes) hasta que termine el texto
        if (GO_Buttons != null)
        {
            buttonsGroup = GO_Buttons.GetComponent<CanvasGroup>();
            if (buttonsGroup == null)
                buttonsGroup = GO_Buttons.AddComponent<CanvasGroup>();

            buttonsGroup.alpha = 0f;
            buttonsGroup.interactable = false;
            buttonsGroup.blocksRaycasts = false;
            GO_Buttons.SetActive(false);
        }

        // Conecta los botones por codigo (tambien puedes usar los metodos publicos desde el Inspector)
        if (playButton != null)
            playButton.onClick.AddListener(OnPlayPressed);

        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitPressed);
    }

    void Start()
    {
        StartCoroutine(StartSequence());
    }

    IEnumerator StartSequence()
    {
        if (GO_Node != null)
            GO_Node.SetActive(true);
        if (GO_Canva != null)
            GO_Canva.SetActive(true);

        yield return PlayVideoWhenReady(videoPlayer);

        // Fade in de entrada
        if (fadePanel != null)
            yield return StartCoroutine(FadeInEntrance());

        // Activa el texto (su Awake inicia la escritura)
        if (GO_Text != null)
            GO_Text.SetActive(true);

        // Espera a que el texto termine de escribirse
        if (typewriter != null)
        {
            while (!typewriter.IsFinished)
                yield return null;
        }

        yield return new WaitForSeconds(buttonsDelay);

        if (GO_Buttons != null)
            yield return StartCoroutine(FadeInButtons());
    }

    // Fade in de los botones: transparente a visible
    IEnumerator FadeInButtons()
    {
        GO_Buttons.SetActive(true);

        float elapsed = 0f;
        while (elapsed < buttonsFadeInDuration)
        {
            elapsed += Time.deltaTime;
            buttonsGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / buttonsFadeInDuration);
            yield return null;
        }

        buttonsGroup.alpha = 1f;
        buttonsGroup.interactable = true;
        buttonsGroup.blocksRaycasts = true;
    }

    // Prepara el video (si hace falta), espera su primer cuadro y lo reproduce.
    // Asi nunca se ve la pantalla vacia (flash azul) antes del fade in.
    private IEnumerator PlayVideoWhenReady(VideoPlayer vp)
    {
        if (vp == null)
            yield break;

        if (!vp.isPrepared)
        {
            vp.Prepare();
            while (!vp.isPrepared)
                yield return null;
        }
        else
        {
            vp.time = 0;
        }

        vp.Play();

        // Espera a que el primer cuadro llegue a la textura (con limite de seguridad)
        float timeout = 2f;
        while (vp.frame < 1 && timeout > 0f)
        {
            timeout -= Time.deltaTime;
            yield return null;
        }
        yield return new WaitForEndOfFrame();
    }

    //////////////////// BOTONES ////////////////////////

    public void OnPlayPressed()
    {
        if (isTransitioning)
            return;

        isTransitioning = true;
        StartCoroutine(FadeOutAndLoadScene());
    }

    public void OnQuitPressed()
    {
        if (isTransitioning)
            return;

        isTransitioning = true;
        StartCoroutine(FadeOutAndQuit());
    }

    IEnumerator FadeOutAndLoadScene()
    {
        SetButtonsInteractable(false);
        yield return StartCoroutine(FadeOutToBlack());

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("nextSceneName esta vacio, no se carga ninguna escena.");
            isTransitioning = false;
        }
    }

    IEnumerator FadeOutAndQuit()
    {
        SetButtonsInteractable(false);
        yield return StartCoroutine(FadeOutToBlack());

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void SetButtonsInteractable(bool value)
    {
        if (playButton != null)
            playButton.interactable = value;
        if (quitButton != null)
            quitButton.interactable = value;
    }

    //////////////////// FADES ////////////////////////

    // Fade in de entrada: negro a transparente
    IEnumerator FadeInEntrance()
    {
        float elapsed = 0f;
        Color c = fadePanel.color;

        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, elapsed / fadeInDuration);
            fadePanel.color = c;
            yield return null;
        }

        c.a = 0f;
        fadePanel.color = c;
        fadePanel.gameObject.SetActive(false);
    }

    // Fade out de salida: transparente a negro
    IEnumerator FadeOutToBlack()
    {
        if (fadePanel == null)
            yield break;

        fadePanel.gameObject.SetActive(true);
        Color c = fadePanel.color;
        c.a = 0f;
        fadePanel.color = c;

        float elapsed = 0f;

        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, elapsed / fadeOutDuration);
            fadePanel.color = c;
            yield return null;
        }

        c.a = 1f;
        fadePanel.color = c;
    }
}