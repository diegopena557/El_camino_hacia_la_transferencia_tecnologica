using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CategoryTokens
{
    public List<GameObject> entenderTokens = new List<GameObject>();
    public List<GameObject> imaginarTokens = new List<GameObject>();
    public List<GameObject> probarTokens = new List<GameObject>();

    // Obtener lista de tokens por categoría
    public List<GameObject> GetTokensByCategory(TokenCategory category)
    {
        return category switch
        {
            TokenCategory.Entender => entenderTokens,
            TokenCategory.Imaginar => imaginarTokens,
            TokenCategory.Probar => probarTokens,
            _ => entenderTokens
        };
    }

    // Obtener un token aleatorio de una categoría
    public GameObject GetRandomToken(TokenCategory category)
    {
        List<GameObject> tokens = GetTokensByCategory(category);
        if (tokens == null || tokens.Count == 0) return null;
        return tokens[Random.Range(0, tokens.Count)];
    }

    // Obtener N tokens aleatorios sin repetir de una categoría
    public List<GameObject> GetRandomTokens(TokenCategory category, int amount)
    {
        List<GameObject> sourceTokens = GetTokensByCategory(category);
        List<GameObject> result = new List<GameObject>();

        if (sourceTokens == null || sourceTokens.Count == 0) return result;

        // Crear copia temporal para no modificar la original
        List<GameObject> tempPool = new List<GameObject>(sourceTokens);

        for (int i = 0; i < amount && tempPool.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, tempPool.Count);
            result.Add(tempPool[randomIndex]);
            tempPool.RemoveAt(randomIndex);
        }

        return result;
    }

    // Contar total de tokens
    public int GetTotalTokens()
    {
        return entenderTokens.Count + imaginarTokens.Count + probarTokens.Count;
    }

    // Verificar si hay suficientes tokens por categoría
    public bool HasEnoughTokens(int requiredPerCategory)
    {
        return entenderTokens.Count >= requiredPerCategory &&
               imaginarTokens.Count >= requiredPerCategory &&
               probarTokens.Count >= requiredPerCategory;
    }
}

[System.Serializable]
public class LevelData
{
    [Header("Información del Nivel")]
    public int levelNumber = 1;
    public string levelName = "Nivel 1 - Entender";
    public TokenCategory focusCategory = TokenCategory.Entender;

    [Header("Configuración de Tokens")]
    [Tooltip("Cuántos tokens de la categoría principal spawn")]
    public int focusTokenCount = 3;
    [Tooltip("Cuántos tokens de otras categorías spawn")]
    public int otherTokensCount = 3;

    [Header("Objetivos del Nivel")]
    [TextArea(2, 4)]
    public string levelDescription = "Coloca los tokens correctos en el slot de Entender";
    public int tokensToComplete = 3;

    [Header("Pool de Tokens para este Nivel")]
    public CategoryTokens tokenPool;

    // Verificar si el nivel tiene suficientes tokens configurados
    public bool IsValid()
    {
        int focusAvailable = tokenPool.GetTokensByCategory(focusCategory).Count;

        if (focusAvailable < focusTokenCount)
        {
            Debug.LogWarning($"Nivel {levelNumber}: No hay suficientes tokens de {focusCategory}. Necesita {focusTokenCount}, tiene {focusAvailable}");
            return false;
        }

        return true;
    }
}