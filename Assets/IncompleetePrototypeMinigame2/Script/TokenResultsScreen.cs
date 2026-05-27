using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TokenResultsScreen : MonoBehaviour
{
    public static TokenResultsScreen Instance;

    [Header("UI References")]
    public GameObject resultsPanel;
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI levelsCompletedText;
    public TextMeshProUGUI correctTokensText;
    public TextMeshProUGUI wrongTokensText;
    public TextMeshProUGUI accuracyText;
    public TextMeshProUGUI medalTitleText;

    [Header("Medallas")]
    public GameObject goldMedal;
    public GameObject silverMedal;
    public GameObject bronzeMedal;

    [Header("Umbrales de Medallas (% de precisin)")]
    public float goldThreshold = 90f;
    public float silverThreshold = 70f;

    [Header("Animacin")]
    public float fadeDuration = 0.5f;
    public float medalDelay = 0.8f;
    public float medalScaleAnimation = 1.2f;
    public float medalAnimationSpeed = 2f;

    [Header("Botones")]
    public Button restartButton;
    public Button continueButton; // Botn para continuar a Dialogue7

    [Header("Audio (Opcional)")]
    public AudioSource audioSource;
    public AudioClip medalSound;

    private int totalCorrect = 0;
    private int totalWrong = 0;
    private int levelsCompleted = 0;

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

        // Conectar botones
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);

        if (continueButton != null)
            continueButton.onClick.AddListener(ContinueToDialogue7);
    }

    // Llamar este mtodo cuando se complete un nivel
    public void OnLevelCompleted(int correctInLevel, int wrongInLevel)
    {
        levelsCompleted++;
        totalCorrect += correctInLevel;
        totalWrong += wrongInLevel;

        Debug.Log($"[TokenResultsScreen] Nivel completado. Total: {levelsCompleted} niveles, {totalCorrect} correctas, {totalWrong} incorrectas");
    }

    // Llamar cuando se completen todos los niveles
    public void ShowFinalResults()
    {
        if (resultsPanel != null)
            resultsPanel.SetActive(true);

        StartCoroutine(ShowResultsCoroutine());
    }

    // Mtodo alternativo con parmetros directos
    public void ShowResults(int correct, int wrong, int levels)
    {
        totalCorrect = correct;
        totalWrong = wrong;
        levelsCompleted = levels;

        if (resultsPanel != null)
            resultsPanel.SetActive(true);

        StartCoroutine(ShowResultsCoroutine());
    }

    IEnumerator ShowResultsCoroutine()
    {
        // Pausar el juego
        Time.timeScale = 0f;

        Debug.Log($"[TokenResultsScreen] Mostrando resultados: {totalCorrect} correctas, {totalWrong} incorrectas, {levelsCompleted} niveles");

        // Calcular estadsticas
        int total = totalCorrect + totalWrong;
        float accuracy = total > 0 ? (totalCorrect / (float)total) * 100f : 0f;

        // Configurar textos
        if (levelsCompletedText != null)
            levelsCompletedText.text = $"Niveles Completados: {levelsCompleted}";

        if (correctTokensText != null)
            correctTokensText.text = $"Fichas Correctas: {totalCorrect}";

        if (wrongTokensText != null)
            wrongTokensText.text = $"Fichas Incorrectas: {totalWrong}";

        if (accuracyText != null)
            accuracyText.text = $"Precisión: {accuracy:F1}%";

        // Ocultar medallas
        HideAllMedals();

        // Fade In
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            if (canvasGroup != null)
                canvasGroup.alpha = Mathf.Lerp(0, 1, elapsed / fadeDuration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        if (canvasGroup != null)
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
                medalTitle = "MEDALLA DE ORO!";
                break;
            case MedalType.Silver:
                medalObject = silverMedal;
                medalTitle = "MEDALLA DE PLATA!";
                break;
            case MedalType.Bronze:
                medalObject = bronzeMedal;
                medalTitle = "MEDALLA DE BRONCE!";
                break;
        }

        if (medalTitleText != null)
            medalTitleText.text = medalTitle;

        if (medalObject != null)
        {
            medalObject.SetActive(true);

            // Reproducir sonido de medalla
            if (audioSource != null && medalSound != null)
            {
                audioSource.PlayOneShot(medalSound);
            }

            // Animacin de escala (rebote)
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

        Debug.Log($"[TokenResultsScreen] Medalla mostrada: {medal}");
    }

    void HideAllMedals()
    {
        if (goldMedal != null) goldMedal.SetActive(false);
        if (silverMedal != null) silverMedal.SetActive(false);
        if (bronzeMedal != null) bronzeMedal.SetActive(false);
    }

    // NUEVO: Mtodo para continuar a Dialogue7 con fade
    public void ContinueToDialogue7()
    {
        StartCoroutine(ContinueToDialogue7Coroutine());
    }

    IEnumerator ContinueToDialogue7Coroutine()
    {
        Debug.Log("[TokenResultsScreen] Continuando a Dialogue7...");

        Time.timeScale = 1f;

        if (SceneFadeIn.Instance != null)
        {
            // FadeOutAndLoadScene hace el fade y carga la escena automaticamente
            SceneFadeIn.Instance.FadeOutAndLoadScene("Dialogue7");
        }
        else
        {
            Debug.LogWarning("[TokenResultsScreen] SceneFadeIn no encontrado, cargando sin fade.");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Dialogue7");
        }

        yield break;
    }

    public void RestartGame()
    {
        Debug.Log("[TokenResultsScreen] Reiniciando juego...");

        // Resetear estadsticas
        totalCorrect = 0;
        totalWrong = 0;
        levelsCompleted = 0;

        // Reanudar tiempo
        Time.timeScale = 1f;

        // Recargar escena
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    // Mtodo para resetear estadsticas sin reiniciar escena
    public void ResetStats()
    {
        totalCorrect = 0;
        totalWrong = 0;
        levelsCompleted = 0;
        Debug.Log("[TokenResultsScreen] Estadsticas reseteadas");
    }

    // Getters para consultar estadsticas
    public int GetTotalCorrect() => totalCorrect;
    public int GetTotalWrong() => totalWrong;
    public int GetLevelsCompleted() => levelsCompleted;
    public float GetAccuracy()
    {
        int total = totalCorrect + totalWrong;
        return total > 0 ? (totalCorrect / (float)total) * 100f : 0f;
    }

    void OnDestroy()
    {
        // Asegurar que el juego se reanude
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }

        // Desconectar botones
        if (restartButton != null)
            restartButton.onClick.RemoveListener(RestartGame);

        if (continueButton != null)
            continueButton.onClick.RemoveListener(ContinueToDialogue7);
    }
}