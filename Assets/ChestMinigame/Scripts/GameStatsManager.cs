using UnityEngine;
using TMPro;

public class GameStatsManager : MonoBehaviour
{
    public static GameStatsManager Instance;

    [Header("UI")]
    public TextMeshProUGUI correctText;
    public TextMeshProUGUI errorText;

    private int correctCount = 0;
    private int errorCount = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
    }

    //  LLAMAR CUANDO UNA CARTA ES CORRECTA
    public void AddCorrect()
    {
        correctCount++;
        UpdateUI();
    }

    //  LLAMAR CUANDO UNA CARTA ES INCORRECTA
    public void AddError()
    {
        errorCount++;
        UpdateUI();
    }

    void UpdateUI()
    {
        correctText.text = $"Aciertos: {correctCount}";
        errorText.text = $"Errores: {errorCount}";
    }

    //  ÚTIL PARA EL FUTURO (puntaje, resumen, etc.)
    public int GetCorrect() => correctCount;
    public int GetErrors() => errorCount;
}
