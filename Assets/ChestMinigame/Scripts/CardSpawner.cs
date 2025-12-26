using UnityEngine;
using System.Collections.Generic;

public class CardSpawner : MonoBehaviour
{
    [Header("Spawn Area")]
    public Collider2D spawnArea;

    [Header("Cards por categoría (FÁCIL)")]
    public List<GameObject> cienciaCards;
    public List<GameObject> tecnologiaCards;
    public List<GameObject> innovacionCards;

    [Header("Cards por categoría (DIFÍCIL)")]
    public List<GameObject> cienciaDificil;
    public List<GameObject> tecnologiaDificil;
    public List<GameObject> innovacionDificil;

    private List<GameObject> activeCards = new List<GameObject>();
    private bool hardMode = false;

    void Start()
    {
        SpawnSet();
    }

    // CREA 6 CARTAS (2 POR CATEGORÍA)
    public void SpawnSet()
    {
        DespawnActiveCards();

        SpawnFromPool(GetPool("Ciencia"), 2);
        SpawnFromPool(GetPool("Tecnologia"), 2);
        SpawnFromPool(GetPool("Innovacion"), 2);
    }

    void SpawnFromPool(List<GameObject> pool, int amount)
    {
        List<GameObject> tempPool = new List<GameObject>(pool);

        for (int i = 0; i < amount; i++)
        {
            if (tempPool.Count == 0) return;

            GameObject card = tempPool[Random.Range(0, tempPool.Count)];
            tempPool.Remove(card);

            card.SetActive(true);
            card.transform.position = GetRandomPosition();

            activeCards.Add(card);
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

    // LLÁMALO CUANDO UNA CARTA SE COLOCA BIEN
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
                hardMode = true;

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
