using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
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

    private List<GameObject> activeCards = new List<GameObject>();
    private bool hardMode = false;

    // NUEVO: evento público que como MusicManager pueden escuchar
    public event System.Action OnHardModeEntered;


    void Start()
    {
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
            yield return new WaitForSecondsRealtime(spawnDelay);

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
            card.SetActive(false);
        }

        if (activeCards.Count == 0)
        {
            if (!hardMode)
            {
                hardMode = true;

                // NUEVO: dispara el evento para avisar a MusicManager que entramos en modo difícil
                OnHardModeEntered?.Invoke();
            }

            SpawnSet();
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
}