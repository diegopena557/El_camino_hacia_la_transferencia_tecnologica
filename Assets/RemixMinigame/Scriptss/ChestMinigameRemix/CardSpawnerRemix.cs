using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawnerRemix : MonoBehaviour
{
    public static CardSpawnerRemix Instance;

    [Header("Spawn Area")]
    public Collider2D spawnArea;

    [Header("Spawn Timing")]
    public float spawnDelay = 2f;

    [Header("Sistema de Escuelas")]
    public SchoolCardManager cardManager; // Se asigna en el Inspector

    [Header("Transiciones")]
    public TransitionManager transitionManager; // Se asigna en el Inspector

    private CategoryCards allSchoolsPool; // Pool combinado: cartas de TODAS las escuelas

    private List<GameObject> activeCards = new List<GameObject>();
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
        SetupPool();
        SpawnSet();
    }

    //
    //  CONFIGURACION DEL POOL (todas las escuelas mezcladas)
    //

    void SetupPool()
    {
        if (cardManager == null)
        {
            Debug.LogError("CardSpawnerRemix: falta asignar 'cardManager' en el Inspector.");
            return;
        }

        // Pool UNICO: cartas de todas las escuelas combinadas
        allSchoolsPool = cardManager.GetCombinedPool(cardManager.schools);

        Debug.Log($"Pool combinado - Ciencia: {allSchoolsPool.cienciaCards.Count}, " +
                  $"Tecnologia: {allSchoolsPool.tecnologiaCards.Count}, " +
                  $"Innovacion: {allSchoolsPool.innovacionCards.Count}");
    }

    //
    //  GENERACION DE CARTAS
    //

    public void SpawnSet()
    {
        DespawnActiveCards();
        StartCoroutine(SpawnSequence());
    }

    IEnumerator SpawnSequence()
    {
        List<GameObject> spawnList = new List<GameObject>();

        // Obtener 2 cartas de cada categoria del pool combinado
        AddRandomFromPool(allSchoolsPool.cienciaCards, 2, spawnList);
        AddRandomFromPool(allSchoolsPool.tecnologiaCards, 2, spawnList);
        AddRandomFromPool(allSchoolsPool.innovacionCards, 2, spawnList);

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
            Debug.LogWarning("Pool vacio. No se pueden agregar cartas.");
            return;
        }

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
    //  LOGICA DE PROGRESION
    //

    public void OnCardCompleted(GameObject card)
    {
        Debug.Log($"CardSpawnerRemix: OnCardCompleted llamado para '{card.name}'. activeCards antes de quitar: {activeCards.Count}");

        if (activeCards.Contains(card))
        {
            activeCards.Remove(card);
        }
        else
        {
            Debug.LogWarning($"CardSpawnerRemix: la carta '{card.name}' no estaba en la lista activeCards (revisar quien la llama).");
        }

        Debug.Log($"CardSpawnerRemix: activeCards restantes: {activeCards.Count}");

        if (activeCards.Count == 0)
        {
            completedSets++;
            Debug.Log("CardSpawnerRemix: set completado. Intentando disparar transicion...");

            if (transitionManager != null)
            {
                transitionManager.TriggerTransition();
            }
            else
            {
                Debug.LogWarning("CardSpawnerRemix: no se asigno 'transitionManager' en el Inspector. No se disparara la transicion de cambio de etapa.");
            }
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
    // METODOS PUBLICOS
    //

    public int GetCompletedSets() => completedSets;

    public void ResetLevelSystem()
    {
        completedSets = 0;
    }
}