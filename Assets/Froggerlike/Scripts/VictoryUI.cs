using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class VictoryUI : MonoBehaviour
{
    [Header("Texts")]
    public TextMeshProUGUI correctText;
    public TextMeshProUGUI errorText;
    public TextMeshProUGUI percentageText;

    [Header("Medals")]
    public Image medalImage;
    public Sprite bronzeMedal;
    public Sprite silverMedal;
    public Sprite goldMedal;

    [Header("Continue Button")]
    public Button continueButton;

    [Header("Fade Out Settings")]
    public Image fadePanel;
    public float fadeOutDuration = 1.5f;

    void Awake()
    {
        // El panel empieza transparente (la victoria ya está visible)
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

    public void ShowResults(int correct, int errors, float percentage)
    {
        correctText.text = "Aciertos: " + correct;
        errorText.text = "Errores: " + errors;
        percentageText.text = "Precisión: " + percentage.ToString("F1") + "%";

        if (percentage >= 80)
            medalImage.sprite = goldMedal;
        else if (percentage >= 50)
            medalImage.sprite = silverMedal;
        else
            medalImage.sprite = bronzeMedal;
    }

    public void OnContinuePressed()
    {
        continueButton.interactable = false; // Evita doble clic
        StartCoroutine(FadeOutAndLoad());
    }

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

        SceneManager.LoadScene("Dialogue10");
    }
}