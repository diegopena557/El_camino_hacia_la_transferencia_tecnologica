using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CategoryCards
{
    public List<GameObject> cienciaCards = new List<GameObject>();
    public List<GameObject> tecnologiaCards = new List<GameObject>();
    public List<GameObject> innovacionCards = new List<GameObject>();

    // Obtener lista de cartas por tipo
    public List<GameObject> GetCardsByType(CardType type)
    {
        return type switch
        {
            CardType.Ciencia => cienciaCards,
            CardType.Tecnologia => tecnologiaCards,
            CardType.Innovacion => innovacionCards,
            _ => cienciaCards
        };
    }

    // Obtener una carta aleatoria de una categoría
    public GameObject GetRandomCard(CardType type)
    {
        List<GameObject> cards = GetCardsByType(type);
        if (cards == null || cards.Count == 0) return null;
        return cards[Random.Range(0, cards.Count)];
    }

    // Contar total de cartas
    public int GetTotalCards()
    {
        return cienciaCards.Count + tecnologiaCards.Count + innovacionCards.Count;
    }
}

[System.Serializable]
public class SchoolData
{
    [Header("Información de la Escuela")]
    public string schoolName;
    public Sprite schoolIcon;

    [TextArea(2, 3)]
    public string schoolDescription;

    [Header("Cartas por Categoría")]
    public CategoryCards cards;

    // Verificar si la escuela tiene suficientes cartas
    public bool HasEnoughCards(int requiredPerCategory)
    {
        return cards.cienciaCards.Count >= requiredPerCategory &&
               cards.tecnologiaCards.Count >= requiredPerCategory &&
               cards.innovacionCards.Count >= requiredPerCategory;
    }

    public int GetTotalCards() => cards.GetTotalCards();
}

// 
//  VERSIÓN PARA CARTAS EN ESCENA (NO ScriptableObject)
// 
public class SchoolCardManager : MonoBehaviour
{
    public static SchoolCardManager Instance;

    [Header("Todas las Escuelas Disponibles")]
    public List<SchoolData> schools = new List<SchoolData>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Obtener escuela por nombre
    public SchoolData GetSchoolByName(string name)
    {
        return schools.Find(s => s.schoolName == name);
    }

    // Obtener escuela por índice
    public SchoolData GetSchoolByIndex(int index)
    {
        if (index >= 0 && index < schools.Count)
            return schools[index];
        return null;
    }

    // Obtener todas las escuelas excepto una
    public List<SchoolData> GetOtherSchools(string excludeSchoolName)
    {
        return schools.FindAll(s => s.schoolName != excludeSchoolName);
    }

    // Obtener pool combinado de múltiples escuelas
    public CategoryCards GetCombinedPool(List<SchoolData> schoolsList)
    {
        CategoryCards combined = new CategoryCards();

        foreach (SchoolData school in schoolsList)
        {
            if (school == null) continue;

            combined.cienciaCards.AddRange(school.cards.cienciaCards);
            combined.tecnologiaCards.AddRange(school.cards.tecnologiaCards);
            combined.innovacionCards.AddRange(school.cards.innovacionCards);
        }

        return combined;
    }
}