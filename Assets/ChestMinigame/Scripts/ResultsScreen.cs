using UnityEngine;
using TMPro;
using System.Collections;

public class ResultsScreen : MonoBehaviour
{
    public static ResultsScreen Instance;

    [Header("UI References")]
    public GameObject resultsPanel;
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI correctText;
    public TextMeshProUGUI errorsText;
    public TextMeshProUGUI accuracyText;
    public TextMeshProUGUI medalTitleText;

    [Header("Medallas")]
    public GameObject goldMedal;
    public GameObject silverMedal;
    public GameObject bronzeMedal;

    [Header("Umbrales de Medallas (% de precisión)")]
    public float goldThreshold = 90f;
    public float silverThreshold = 70f;

    [Header("Animación")]
    public float fadeDuration = 0.5f;
    public float medalDelay = 0.8f;
    public float medalScaleAnimation = 1.2f;
    public float medalAnimationSpeed = 2f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (resultsPanel != null)
            resultsPanel.SetActive(false);

        if (canvasGroup != null)
            canvasGroup.alpha = 0;

        HideAllMedals();
    }

    public void ShowResults(int correct, int errors)
    {
        // Activar el panel ANTES de iniciar la coroutine
        if (resultsPanel != null)
            resultsPanel.SetActive(true);

        StartCoroutine(ShowResultsCoroutine(correct, errors));
    }

    IEnumerator ShowResultsCoroutine(int correct, int errors)
    {
        // Pausar el juego
        Time.timeScale = 0f;

        // Calcular estadísticas
        int total = correct + errors;
        float accuracy = total > 0 ? (correct / (float)total) * 100f : 0f;

        // Configurar textos
        if (correctText != null)
            correctText.text = $"Aciertos: {correct}";

        if (errorsText != null)
            errorsText.text = $"Errores: {errors}";

        if (accuracyText != null)
            accuracyText.text = $"Precisión: {accuracy:F1}%";

        // Panel ya está activo, solo ocultar medallas
        HideAllMedals();

        // Fade In
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, elapsed / fadeDuration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1;

        // Esperar antes de mostrar medalla
        yield return new WaitForSecondsRealtime(medalDelay);

        // Determinar y mostrar medalla
        MedalType medal = DetermineMedal(accuracy);
        yield return StartCoroutine(ShowMedalAnimation(medal));
    }

    MedalType DetermineMedal(float accuracy)
    {
        if (accuracy >= goldThreshold)
            return MedalType.Gold;
        else if (accuracy >= silverThreshold)
            return MedalType.Silver;
        else
            return MedalType.Bronze;
    }

    IEnumerator ShowMedalAnimation(MedalType medal)
    {
        GameObject medalObject = null;
        string medalTitle = "";

        switch (medal)
        {
            case MedalType.Gold:
                medalObject = goldMedal;
                medalTitle = "¡MEDALLA DE ORO!";
                break;
            case MedalType.Silver:
                medalObject = silverMedal;
                medalTitle = "¡MEDALLA DE PLATA!";
                break;
            case MedalType.Bronze:
                medalObject = bronzeMedal;
                medalTitle = "¡MEDALLA DE BRONCE!";
                break;
        }

        if (medalTitleText != null)
            medalTitleText.text = medalTitle;

        if (medalObject != null)
        {
            medalObject.SetActive(true);

            // Animación de escala (rebote)
            Vector3 originalScale = medalObject.transform.localScale;
            Vector3 targetScale = originalScale * medalScaleAnimation;

            float elapsed = 0f;
            float duration = 0.5f;

            while (elapsed < duration)
            {
                float t = elapsed / duration;
                float scale = Mathf.Lerp(1, medalScaleAnimation, Mathf.Sin(t * Mathf.PI));
                medalObject.transform.localScale = originalScale * scale;

                elapsed += Time.unscaledDeltaTime * medalAnimationSpeed;
                yield return null;
            }

            medalObject.transform.localScale = originalScale;
        }
    }

    void HideAllMedals()
    {
        if (goldMedal != null) goldMedal.SetActive(false);
        if (silverMedal != null) silverMedal.SetActive(false);
        if (bronzeMedal != null) bronzeMedal.SetActive(false);
    }

    public void CloseResults()
    {
        StartCoroutine(CloseResultsCoroutine());
    }

    IEnumerator CloseResultsCoroutine()
    {
        // Fade Out
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, elapsed / fadeDuration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0;

        resultsPanel.SetActive(false);

        // Reanudar el juego
        Time.timeScale = 1f;

        // Opcional: Reiniciar el juego
        if (GameStatsManager.Instance != null)
        {
            // Aquí podrías resetear las estadísticas si quieres
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}

public enum MedalType
{
    Gold,
    Silver,
    Bronze
}