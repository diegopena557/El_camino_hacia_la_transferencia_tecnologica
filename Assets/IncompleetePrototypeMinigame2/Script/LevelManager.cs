using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Niveles del Juego")]
    public List<LevelData> levels = new List<LevelData>();

    [Header("Referencias")]
    public TokenSlot mainTokenSlot; // Referencia al slot que cambiará de categoría
    public LevelUIDisplay levelUI; // Referencia al UI display (opcional)

    [Header("Configuración de Spawn")]
    public Transform spawnContainer;
    public Vector2 spawnAreaMin = new Vector2(-5f, 3f);
    public Vector2 spawnAreaMax = new Vector2(5f, 5f);
    public float spawnDelay = 0.3f;

    [Header("Estado Actual")]
    [SerializeField] private int currentLevelIndex = 0;
    [SerializeField] private LevelData currentLevel;

    private List<GameObject> spawnedTokens = new List<GameObject>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (levels.Count > 0)
        {
            LoadLevel(0);
        }
        else
        {
            Debug.LogError("[LevelManager] No hay niveles configurados!");
        }
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levels.Count)
        {
            Debug.LogError($"[LevelManager] Índice de nivel inválido: {levelIndex}");
            return;
        }

        currentLevelIndex = levelIndex;
        currentLevel = levels[levelIndex];

        if (!currentLevel.IsValid())
        {
            Debug.LogError($"[LevelManager] El nivel {levelIndex} no es válido!");
            return;
        }

        Debug.Log($"[LevelManager] Cargando {currentLevel.levelName}");

        // Resetear estadísticas del nivel actual
        if (TokenStatsManager.Instance != null)
        {
            TokenStatsManager.Instance.ResetCurrentLevel();
        }

        // Actualizar UI si está conectada
        if (levelUI != null)
        {
            levelUI.UpdateDisplay();
        }

        // Cambiar la categoría del slot principal
        if (mainTokenSlot != null)
        {
            mainTokenSlot.SetCategory(currentLevel.focusCategory);
            mainTokenSlot.ResetSlot();
        }
        else
        {
            Debug.LogWarning("[LevelManager] No hay TokenSlot asignado en 'Main Token Slot'");
        }

        // Limpiar tokens anteriores
        ClearSpawnedTokens();

        // Spawn nuevos tokens
        StartCoroutine(SpawnLevelTokens());
    }

    IEnumerator SpawnLevelTokens()
    {
        List<GameObject> tokensToSpawn = new List<GameObject>();

        // 1. Agregar todos los tokens de la categoría principal
        List<GameObject> focusTokens = currentLevel.tokenPool.GetRandomTokens(
            currentLevel.focusCategory,
            currentLevel.focusTokenCount
        );
        tokensToSpawn.AddRange(focusTokens);

        Debug.Log($"[LevelManager] Agregados {focusTokens.Count} tokens de {currentLevel.focusCategory}");

        // 2. Agregar tokens aleatorios de las otras 2 categorías
        List<TokenCategory> otherCategories = GetOtherCategories(currentLevel.focusCategory);

        int tokensPerOtherCategory = currentLevel.otherTokensCount / 2;
        int remainder = currentLevel.otherTokensCount % 2;

        foreach (TokenCategory category in otherCategories)
        {
            int amountToGet = tokensPerOtherCategory;

            // Agregar el resto al primero
            if (remainder > 0)
            {
                amountToGet++;
                remainder--;
            }

            List<GameObject> otherTokens = currentLevel.tokenPool.GetRandomTokens(category, amountToGet);
            tokensToSpawn.AddRange(otherTokens);

            Debug.Log($"[LevelManager] Agregados {otherTokens.Count} tokens de {category}");
        }

        // 3. Mezclar la lista
        ShuffleList(tokensToSpawn);

        // 4. Spawn con delay
        foreach (GameObject tokenPrefab in tokensToSpawn)
        {
            yield return new WaitForSeconds(spawnDelay);

            if (tokenPrefab == null)
            {
                Debug.LogWarning("[LevelManager] Token prefab es null, saltando...");
                continue;
            }

            Vector2 spawnPos = GetRandomSpawnPosition();
            GameObject spawnedToken = Instantiate(tokenPrefab, spawnPos, Quaternion.identity);

            if (spawnContainer != null)
                spawnedToken.transform.SetParent(spawnContainer);

            spawnedTokens.Add(spawnedToken);

            // Log para verificación
            ProjectToken tokenScript = spawnedToken.GetComponent<ProjectToken>();
            if (tokenScript != null)
            {
                Debug.Log($"[LevelManager] Spawned: {tokenScript.GetTokenName()} ({tokenScript.GetCategory()})");
            }
        }

        Debug.Log($"[LevelManager] Nivel cargado! Total tokens: {spawnedTokens.Count}");
    }

    Vector2 GetRandomSpawnPosition()
    {
        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        return new Vector2(x, y);
    }

    List<TokenCategory> GetOtherCategories(TokenCategory exclude)
    {
        List<TokenCategory> all = new List<TokenCategory>
        {
            TokenCategory.Entender,
            TokenCategory.Imaginar,
            TokenCategory.Probar
        };

        all.Remove(exclude);
        return all;
    }

    void ShuffleList(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            GameObject temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    void ClearSpawnedTokens()
    {
        foreach (GameObject token in spawnedTokens)
        {
            if (token != null)
                Destroy(token);
        }

        spawnedTokens.Clear();
        Debug.Log("[LevelManager] Tokens anteriores limpiados");
    }

    // Llamar cuando el jugador complete el nivel
    public void CompleteLevel()
    {
        Debug.Log($"[LevelManager] ¡Nivel {currentLevelIndex + 1} completado!");

        // Obtener estadísticas del nivel actual
        int levelCorrect = 0;
        int levelWrong = 0;

        if (TokenStatsManager.Instance != null)
        {
            levelCorrect = TokenStatsManager.Instance.GetCurrentLevelCorrect();
            levelWrong = TokenStatsManager.Instance.GetCurrentLevelWrong();
            Debug.Log($"[LevelManager] Estadísticas del nivel: {levelCorrect} correctas, {levelWrong} incorrectas");
        }

        // Notificar a la pantalla de resultados
        if (TokenResultsScreen.Instance != null)
        {
            TokenResultsScreen.Instance.OnLevelCompleted(levelCorrect, levelWrong);
        }

        if (currentLevelIndex + 1 < levels.Count)
        {
            StartCoroutine(LoadNextLevelWithDelay(2f));
        }
        else
        {
            Debug.Log("[LevelManager] ¡Juego completado! Mostrando resultados finales...");

            // Mostrar pantalla de resultados final
            if (TokenResultsScreen.Instance != null)
            {
                StartCoroutine(ShowResultsWithDelay(2f));
            }
        }
    }

    IEnumerator ShowResultsWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (TokenResultsScreen.Instance != null)
        {
            TokenResultsScreen.Instance.ShowFinalResults();
        }
    }

    IEnumerator LoadNextLevelWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadLevel(currentLevelIndex + 1);
    }

    // Getters
    public LevelData GetCurrentLevel() => currentLevel;
    public int GetCurrentLevelIndex() => currentLevelIndex;
    public int GetTotalLevels() => levels.Count;

    // Debug: Visualizar área de spawn
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
        Vector3 center = new Vector3(
            (spawnAreaMin.x + spawnAreaMax.x) / 2f,
            (spawnAreaMin.y + spawnAreaMax.y) / 2f,
            0f
        );
        Vector3 size = new Vector3(
            spawnAreaMax.x - spawnAreaMin.x,
            spawnAreaMax.y - spawnAreaMin.y,
            0f
        );
        Gizmos.DrawCube(center, size);
    }
}