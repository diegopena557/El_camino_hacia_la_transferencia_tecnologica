using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    public void ShowResults(int correct, int errors, float percentage)
    {
        correctText.text = "Aciertos: " + correct;
        errorText.text = "Errores: " + errors;
        percentageText.text = "Precisión: " + percentage.ToString("F1") + "%";

        // Elegir medalla
        if (percentage >= 80)
            medalImage.sprite = goldMedal;
        else if (percentage >= 50)
            medalImage.sprite = silverMedal;
        else
            medalImage.sprite = bronzeMedal;
    }
}