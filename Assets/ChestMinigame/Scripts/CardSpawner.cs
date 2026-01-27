using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardSpawner : MonoBehaviour
{
    public static CardSpawner Instance;

    [Header("Spawn Area")]
    public Collider2D spawnArea;

    [Header("Spawn Timing")]
    public float spawnDelay = 2f;

    [Header("Sistema de Escuelas")]
    public SchoolCardManager cardManager; // Cambiado de database
    private SchoolData playerSchool;          // Escuela del jugador (nivel fácil)
    private CategoryCards easyPool;           // Pool nivel fácil
    private CategoryCards hardPool;           // Pool nivel difícil (otras escuelas)

    [Header("UI de Nivel (Opcional)")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI schoolText;
    public GameObject hardModeNotification;

    [Header("Configuración de Dificultad")]
    public float hardModeNotificationDuration = 2f;

    private List<GameObject> activeCards = new List<GameObject>();
    private bool hardMode = false;
    private int completedSets = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (hardModeNotification != null)
            hardModeNotification.SetActive(false);

        // No iniciar automáticamente - esperar selección de escuela
        // SpawnSet();
    }

    // 
    //  CONFIGURACIÓN DE ESCUELAS
    //

    /// <summary>
    /// Configura las escuelas y pools de cartas según la selección del jugador
    /// </summary>
    public void SetupSchools(SchoolData selectedSchool, SchoolCardManager manager)
    {
        cardManager = manager;
        playerSchool = selectedSchool;

        // Pool FÁCIL: Solo cartas de la escuela del jugador
        easyPool = new CategoryCards
        {
            cienciaCards = new List<GameObject>(playerSchool.cards.cienciaCards),
            tecnologiaCards = new List<GameObject>(playerSchool.cards.tecnologiaCards),
            innovacionCards = new List<GameObject>(playerSchool.cards.innovacionCards)
        };

        // Pool DIFÍCIL: Cartas de todas las otras escuelas
        List<SchoolData> otherSchools = cardManager.GetOtherSchools(playerSchool.schoolName);
        hardPool = cardManager.GetCombinedPool(otherSchools);

        UpdateLevelUI();

        // Iniciar el juego
        SpawnSet();

        Debug.Log($"Pools configurados:");
        Debug.Log($"Fácil - Ciencia: {easyPool.cienciaCards.Count}, " +
                  $"Tecnología: {easyPool.tecnologiaCards.Count}, " +
                  $"Innovación: {easyPool.innovacionCards.Count}");
        Debug.Log($"Difícil - Ciencia: {hardPool.cienciaCards.Count}, " +
                  $"Tecnología: {hardPool.tecnologiaCards.Count}, " +
                  $"Innovación: {hardPool.innovacionCards.Count}");
    }

    //
    //  GENERACIÓN DE CARTAS
    // 

    public void SpawnSet()
    {
        DespawnActiveCards();
        StartCoroutine(SpawnSequence());
    }

    IEnumerator SpawnSequence()
    {
        List<GameObject> spawnList = new List<GameObject>();

        CategoryCards currentPool = hardMode ? hardPool : easyPool;

        // Obtener 2 cartas de cada categoría
        AddRandomFromPool(currentPool.cienciaCards, 2, spawnList);
        AddRandomFromPool(currentPool.tecnologiaCards, 2, spawnList);
        AddRandomFromPool(currentPool.innovacionCards, 2, spawnList);

        Shuffle(spawnList);

        foreach (GameObject card in spawnList)
        {
            yield return new WaitForSeconds(spawnDelay);

            card.transform.position = GetRandomPosition();
            card.SetActive(true);

            Rigidbody2D rb = card.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.simulated = true;
            }

            activeCards.Add(card);
        }
    }

    void AddRandomFromPool(List<GameObject> pool, int amount, List<GameObject> result)
    {
        if (pool == null || pool.Count == 0)
        {
            Debug.LogWarning($"Pool vacío. No se pueden agregar cartas.");
            return;
        }

        // Si el pool tiene menos cartas de las solicitadas, ajustar
        int actualAmount = Mathf.Min(amount, pool.Count);

        List<GameObject> temp = new List<GameObject>(pool);

        for (int i = 0; i < actualAmount; i++)
        {
            if (temp.Count == 0) break;

            GameObject card = temp[Random.Range(0, temp.Count)];
            temp.Remove(card);

            result.Add(card);
        }

        if (actualAmount < amount)
        {
            Debug.LogWarning($"Pool insuficiente: se solicitaron {amount} cartas pero solo hay {pool.Count}");
        }
    }

    void Shuffle(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            GameObject temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    Vector2 GetRandomPosition()
    {
        Bounds b = spawnArea.bounds;
        return new Vector2(
            Random.Range(b.min.x, b.max.x),
            b.max.y
        );
    }

    // 
    //  LÓGICA DE PROGRESIÓN
    //

    public void OnCardCompleted(GameObject card)
    {
        if (activeCards.Contains(card))
        {
            activeCards.Remove(card);
        }

        if (activeCards.Count == 0)
        {
            completedSets++;

            if (!hardMode)
            {
                StartCoroutine(ActivateHardMode());
            }
            else
            {
                ShowFinalResults();
            }
        }
    }

    IEnumerator ActivateHardMode()
    {
        hardMode = true;
        UpdateLevelUI();

       

        if (hardModeNotification != null)
        {
            hardModeNotification.SetActive(true);
            yield return new WaitForSeconds(hardModeNotificationDuration);
            hardModeNotification.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }

        SpawnSet();
    }

    void ShowFinalResults()
    {
        if (ResultsScreen.Instance != null && GameStatsManager.Instance != null)
        {
            int correct = GameStatsManager.Instance.GetCorrect();
            int errors = GameStatsManager.Instance.GetErrors();

            ResultsScreen.Instance.ShowResults(correct, errors);
        }
    }

    void UpdateLevelUI()
    {
        if (levelText != null)
        {
            levelText.text = hardMode ? "NIVEL: DIFÍCIL" : "NIVEL: FÁCIL";
        }

        if (schoolText != null && playerSchool != null)
        {
            if (hardMode)
                schoolText.text = "Cartas: Otras Escuelas";
            else
                schoolText.text = $"Escuela: {playerSchool.schoolName}";
        }
    }

    void DespawnActiveCards()
    {
        foreach (GameObject card in activeCards)
        {
            if (card != null)
                card.SetActive(false);
        }

        activeCards.Clear();
    }

    // 
    // MÉTODOS PÚBLICOS
    //

    public bool IsHardMode() => hardMode;
    public int GetCompletedSets() => completedSets;
    public SchoolData GetPlayerSchool() => playerSchool;

    public void SetHardMode(bool enabled)
    {
        hardMode = enabled;
        UpdateLevelUI();
    }

    public void ResetLevelSystem()
    {
        hardMode = false;
        completedSets = 0;
        UpdateLevelUI();
    }
}