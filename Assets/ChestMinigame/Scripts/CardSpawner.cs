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

    [Header("Cards por categoría (Fácil)")]
    public List<GameObject> cienciaCards;
    public List<GameObject> tecnologiaCards;
    public List<GameObject> innovacionCards;

    [Header("Cards por categoría (Difícil)")]
    public List<GameObject> cienciaDificil;
    public List<GameObject> tecnologiaDificil;
    public List<GameObject> innovacionDificil;

    [Header("UI de Nivel")]
    public TextMeshProUGUI levelText;
    public GameObject hardModeNotification;

    [Header("Configuración de Dificultad")]
    public float hardModeNotificationDuration = 2f;

    private List<GameObject> activeCards = new List<GameObject>();
    private bool hardMode = false;
    private int completedSets = 0;

    // 🔹 NUEVO: evento para notificar al MusicManager cuando se entra en modo difícil
    public event System.Action OnHardModeEntered;

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

        UpdateLevelUI();
        SpawnSet();
    }

    // CREA 6 CARTAS (2 POR CATEGORÍA)
    public void SpawnSet()
    {
        DespawnActiveCards();
        StartCoroutine(SpawnSequence());
    }

    IEnumerator SpawnSequence()
    {
        List<GameObject> spawnList = new List<GameObject>();

        AddRandomFromPool(GetPool("Ciencia"), 2, spawnList);
        AddRandomFromPool(GetPool("Tecnologia"), 2, spawnList);
        AddRandomFromPool(GetPool("Innovacion"), 2, spawnList);

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
            Debug.LogWarning($"Pool vacío o nulo. Asegúrate de asignar cartas en el Inspector.");
            return;
        }

        List<GameObject> temp = new List<GameObject>(pool);

        for (int i = 0; i < amount; i++)
        {
            if (temp.Count == 0) return;

            GameObject card = temp[Random.Range(0, temp.Count)];
            temp.Remove(card);

            result.Add(card);
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

    List<GameObject> GetPool(string category)
    {
        if (!hardMode)
        {
            if (category == "Ciencia") return cienciaCards;
            if (category == "Tecnologia") return tecnologiaCards;
            return innovacionCards;
        }
        else
        {
            if (category == "Ciencia") return cienciaDificil;
            if (category == "Tecnologia") return tecnologiaDificil;
            return innovacionDificil;
        }
    }

    // LLAMADO CUANDO UNA CARTA SE COLOCA BIEN
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

    void ShowFinalResults()
    {
        if (ResultsScreen.Instance != null && GameStatsManager.Instance != null)
        {
            int correct = GameStatsManager.Instance.GetCorrect();
            int errors = GameStatsManager.Instance.GetErrors();

            ResultsScreen.Instance.ShowResults(correct, errors);
        }
    }

    IEnumerator ActivateHardMode()
    {
        hardMode = true;
        UpdateLevelUI();

        // 🔹 NUEVO: notificar al MusicManager que entramos en modo difícil
        OnHardModeEntered?.Invoke();

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

    void UpdateLevelUI()
    {
        if (levelText != null)
        {
            levelText.text = hardMode ? "NIVEL: DIFÍCIL" : "NIVEL: FÁCIL";
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

    public bool IsHardMode() => hardMode;
    public int GetCompletedSets() => completedSets;
}