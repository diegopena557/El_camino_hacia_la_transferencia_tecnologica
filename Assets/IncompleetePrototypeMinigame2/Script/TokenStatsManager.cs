using UnityEngine;

public class TokenStatsManager : MonoBehaviour
{
    public static TokenStatsManager Instance;

    [Header("Estadísticas del Nivel Actual")]
    [SerializeField] private int currentLevelCorrect = 0;
    [SerializeField] private int currentLevelWrong = 0;

    [Header("Estadísticas Totales")]
    [SerializeField] private int totalCorrect = 0;
    [SerializeField] private int totalWrong = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Llamar cuando se coloca una ficha correcta
    public void AddCorrectToken()
    {
        currentLevelCorrect++;
        totalCorrect++;
        Debug.Log($"[TokenStatsManager] Ficha correcta! Nivel: {currentLevelCorrect}, Total: {totalCorrect}");
    }

    // Llamar cuando se coloca una ficha incorrecta
    public void AddWrongToken()
    {
        currentLevelWrong++;
        totalWrong++;
        Debug.Log($"[TokenStatsManager] Ficha incorrecta! Nivel: {currentLevelWrong}, Total: {totalWrong}");
    }

    // Obtener estadísticas del nivel actual
    public int GetCurrentLevelCorrect() => currentLevelCorrect;
    public int GetCurrentLevelWrong() => currentLevelWrong;

    // Obtener estadísticas totales
    public int GetTotalCorrect() => totalCorrect;
    public int GetTotalWrong() => totalWrong;

    // Resetear el contador del nivel actual (llamar al inicio de cada nivel)
    public void ResetCurrentLevel()
    {
        currentLevelCorrect = 0;
        currentLevelWrong = 0;
        Debug.Log("[TokenStatsManager] Estadísticas del nivel reseteadas");
    }

    // Resetear todo (llamar al reiniciar el juego)
    public void ResetAll()
    {
        currentLevelCorrect = 0;
        currentLevelWrong = 0;
        totalCorrect = 0;
        totalWrong = 0;
        Debug.Log("[TokenStatsManager] Todas las estadísticas reseteadas");
    }
}