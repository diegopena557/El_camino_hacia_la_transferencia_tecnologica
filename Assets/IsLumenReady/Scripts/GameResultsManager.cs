using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameResultsManager : MonoBehaviour
{
    public static GameResultsManager Instance;

    [Header("Resultados (este minijuego)")]
    public int correctAnswers = 0;
    public int wrongAnswers = 0;

    [Header("Suma de minijuegos")]
    [Tooltip("Incluir los aciertos/errores registrados con RegisterAnswer en este manager. " +
             "Desactivar si RegisterAnswer ya se llama junto con GameStatsManager (evita doble conteo).")]
    public bool includeOwnAnswers = true;

    [Header("UI Final")]
    public GameObject resultsPanel;

    public TMP_Text correctText;
    public TMP_Text wrongText;
    public TMP_Text percentageText;
    public TMP_Text titleText;

    [Header("Medalla")]
    public Image medalImage;

    public Sprite bronzeMedal;
    public Sprite silverMedal;
    public Sprite goldMedal;

    [Header("Titulo y umbrales de medalla")]
    [Tooltip("Porcentaje minimo para medalla de oro")]
    public float goldThreshold = 80f;
    [Tooltip("Porcentaje minimo para medalla de plata")]
    public float silverThreshold = 50f;

    public string goldTitle = "¡MEDALLA DE ORO!";
    public string silverTitle = "¡MEDALLA DE PLATA!";
    public string bronzeTitle = "¡MEDALLA DE BRONCE!";

    [Header("Animacion")]
    public float popupDuration = 0.35f;
    public float staggerDelay = 0.15f;

    [Header("Continue Button")]
    public Button continueButton;

    [Header("Fade Out Settings")]
    public Image fadePanel;
    public float fadeOutDuration = 1.5f;

    [Header("Remix")]
    [Tooltip("Desactivado = comportamiento original (singleton estricto)")]
    public bool overrideInstance = false;

    [Tooltip("Escena a cargar al presionar Continuar")]
    public string nextSceneName = "Dialogue14";

    void Awake()
    {
        if (Instance == null || overrideInstance)
            Instance = this;
        else
            Destroy(gameObject);

        if (resultsPanel != null)
            resultsPanel.SetActive(false);

        // El panel empieza transparente
        if (fadePanel != null)
        {
            Color c = fadePanel.color;
            c.a = 0f;
            fadePanel.color = c;
            fadePanel.gameObject.SetActive(false);
        }

        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinuePressed);
    }

    public void RegisterAnswer(bool correct)
    {
        if (correct)
            correctAnswers++;
        else
            wrongAnswers++;
    }

    // ----------------------------------------------------
    // SUMA DE LOS MINIJUEGOS
    // ----------------------------------------------------

    int GetTotalCorrect()
    {
        int total = includeOwnAnswers ? correctAnswers : 0;

        // Minijuego 1
        if (GameStatsManager.Instance != null)
            total += GameStatsManager.Instance.GetCorrect();

        // Minijuego 2 (varias etapas)
        if (TokenResultsScreen.Instance != null)
            total += TokenResultsScreen.Instance.GetTotalCorrect();

        return total;
    }

    int GetTotalWrong()
    {
        int total = includeOwnAnswers ? wrongAnswers : 0;

        // Minijuego 1
        if (GameStatsManager.Instance != null)
            total += GameStatsManager.Instance.GetErrors();

        // Minijuego 2 (varias etapas)
        if (TokenResultsScreen.Instance != null)
            total += TokenResultsScreen.Instance.GetTotalWrong();

        return total;
    }

    public void ShowResults()
    {
        resultsPanel.SetActive(true);

        int totalCorrect = GetTotalCorrect();
        int totalWrong = GetTotalWrong();
        int total = totalCorrect + totalWrong;

        float percentage = 0f;

        if (total > 0)
            percentage = ((float)totalCorrect / total) * 100f;

        correctText.text = "Aciertos: " + totalCorrect;
        wrongText.text = "Errores: " + totalWrong;
        percentageText.text = "Precision: " + Mathf.RoundToInt(percentage) + "%";

        if (percentage >= goldThreshold)
        {
            medalImage.sprite = goldMedal;
            titleText.text = goldTitle;
        }
        else if (percentage >= silverThreshold)
        {
            medalImage.sprite = silverMedal;
            titleText.text = silverTitle;
        }
        else
        {
            medalImage.sprite = bronzeMedal;
            titleText.text = bronzeTitle;
        }

        titleText.transform.localScale = Vector3.zero;
        correctText.transform.localScale = Vector3.zero;
        wrongText.transform.localScale = Vector3.zero;
        percentageText.transform.localScale = Vector3.zero;
        medalImage.transform.localScale = Vector3.zero;

        StopAllCoroutines();
        StartCoroutine(AnimateResults());
    }

    public void OnContinuePressed()
    {
        continueButton.interactable = false; // Evita doble clic
        StartCoroutine(FadeOutAndLoad());
    }

    // ----------------------------------------------------
    // FADE OUT Y CARGA DE ESCENA
    // ----------------------------------------------------

    IEnumerator FadeOutAndLoad()
    {
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

        SceneManager.LoadScene(nextSceneName);
    }

    // ----------------------------------------------------
    // ANIMACION SECUENCIAL
    // ----------------------------------------------------

    IEnumerator AnimateResults()
    {
        yield return AnimatePopup(titleText.transform, 1.15f);

        yield return new WaitForSecondsRealtime(staggerDelay);

        yield return AnimatePopup(correctText.transform, 1.1f);

        yield return new WaitForSecondsRealtime(staggerDelay);

        yield return AnimatePopup(wrongText.transform, 1.1f);

        yield return new WaitForSecondsRealtime(staggerDelay);

        yield return AnimatePopup(percentageText.transform, 1.15f);

        yield return new WaitForSecondsRealtime(staggerDelay);

        yield return AnimatePopup(medalImage.transform, 1.35f);
    }

    // ----------------------------------------------------
    // POPUP
    // ----------------------------------------------------

    IEnumerator AnimatePopup(Transform target, float overshootMultiplier)
    {
        Vector3 startScale = Vector3.zero;
        Vector3 overshootScale = Vector3.one * overshootMultiplier;
        Vector3 finalScale = Vector3.one;

        float firstPhase = popupDuration * 0.7f;
        float secondPhase = popupDuration * 0.3f;

        float t = 0f;

        while (t < firstPhase)
        {
            t += Time.unscaledDeltaTime;
            float lerp = t / firstPhase;
            target.localScale = Vector3.Lerp(startScale, overshootScale, EaseOutBack(lerp));
            yield return null;
        }

        t = 0f;

        while (t < secondPhase)
        {
            t += Time.unscaledDeltaTime;
            float lerp = t / secondPhase;
            target.localScale = Vector3.Lerp(overshootScale, finalScale, lerp);
            yield return null;
        }

        target.localScale = finalScale;
    }

    // ----------------------------------------------------
    // EASING
    // ----------------------------------------------------

    float EaseOutBack(float x)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1f;
        return 1 + c3 * Mathf.Pow(x - 1, 3) + c1 * Mathf.Pow(x - 1, 2);
    }
}