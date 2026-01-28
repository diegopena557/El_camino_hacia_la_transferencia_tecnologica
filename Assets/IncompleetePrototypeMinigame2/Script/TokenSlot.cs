using UnityEngine;
using System.Collections.Generic;

public class TokenSlot : MonoBehaviour
{
    [Header("Categoría Aceptada")]
    public TokenCategory acceptedCategory;
    public bool validateCategory = true;

    [Header("Capacidad")]
    public int maxTokens = 3;

    [Header("Posicionamiento")]
    public Vector2 tokenSpacing = new Vector2(1.2f, 0f);
    public bool autoArrange = true;

    [Header("Barras de Indicadores")]
    public SpriteRenderer esfuerzoBar;
    public SpriteRenderer incertidumbreBar;

    [Header("Sprites de las Barras (0-10)")]
    public Sprite[] esfuerzoSprites = new Sprite[11];
    public Sprite[] incertidumbreSprites = new Sprite[11];

    [Header("Valores Totales")]
    [SerializeField] private int totalEsfuerzo = 0;
    [SerializeField] private int totalIncertidumbre = 0;

    private List<ProjectToken> placedTokens = new List<ProjectToken>();

    void Start()
    {
        UpdateBars();

        // Verificar configuración
        Collider2D col = GetComponent<Collider2D>();
        if (col == null)
        {
            Debug.LogError($"[TokenSlot] {gameObject.name} NO TIENE COLLIDER2D! Agrégalo para que funcione.", gameObject);
        }
        else if (!col.isTrigger)
        {
            Debug.LogWarning($"[TokenSlot] {gameObject.name} - El Collider2D debe estar marcado como 'Is Trigger'", gameObject);
        }
        else
        {
            Debug.Log($"[TokenSlot] {gameObject.name} configurado correctamente. Categoría aceptada: {acceptedCategory}", gameObject);
        }
    }

    public bool CanAcceptToken()
    {
        return placedTokens.Count < maxTokens;
    }

    public void AddToken(ProjectToken token)
    {
        Debug.Log($"[TokenSlot] Intentando añadir token '{token.GetTokenName()}' al slot '{gameObject.name}'");

        if (!CanAcceptToken())
        {
            Debug.LogWarning($"[TokenSlot] {gameObject.name} está lleno ({placedTokens.Count}/{maxTokens})");

            // Reproducir feedback de error
            TokenFeedback feedback = token.GetComponent<TokenFeedback>();
            if (feedback != null)
                feedback.PlayWrongFeedback();

            return;
        }

        if (placedTokens.Contains(token))
        {
            Debug.LogWarning($"[TokenSlot] Esta ficha ya está en el slot");
            return;
        }

        // Validar categoría si está activado
        if (validateCategory && token.GetCategory() != acceptedCategory)
        {
            Debug.LogWarning($"[TokenSlot] Categoría INCORRECTA. Esperaba: {acceptedCategory}, Recibió: {token.GetCategory()}");

            // Reproducir feedback de error
            TokenFeedback feedback = token.GetComponent<TokenFeedback>();
            if (feedback != null)
                feedback.PlayWrongFeedback();

            return;
        }

        // Token aceptado correctamente
        Debug.Log($"[TokenSlot]  Token '{token.GetTokenName()}' aceptado!");

        placedTokens.Add(token);
        token.SetPlaced(true);

        // Reproducir feedback de éxito
        TokenFeedback feedback2 = token.GetComponent<TokenFeedback>();
        if (feedback2 != null)
            feedback2.PlayCorrectFeedback();

        // Posicionar el token
        if (autoArrange)
        {
            ArrangeTokens();
        }
        else
        {
            Vector3 position = GetTokenPosition(placedTokens.Count - 1);
            token.transform.position = position;
            token.SetOriginalPosition(position);
        }

        RecalculateTotals();
        UpdateBars();

        Debug.Log($"[TokenSlot] Total en slot: Esfuerzo={totalEsfuerzo}, Incertidumbre={totalIncertidumbre}");
    }

    public void RemoveToken(ProjectToken token)
    {
        if (placedTokens.Remove(token))
        {
            token.SetPlaced(false);

            // Reorganizar tokens restantes
            if (autoArrange)
            {
                ArrangeTokens();
            }

            RecalculateTotals();
            UpdateBars();

            Debug.Log($"Ficha removida: {token.GetTokenName()} | Total Esfuerzo: {totalEsfuerzo}, Total Incertidumbre: {totalIncertidumbre}");
        }
    }

    public void ClearAllTokens()
    {
        foreach (ProjectToken token in placedTokens)
        {
            token.SetPlaced(false);
        }

        placedTokens.Clear();
        RecalculateTotals();
        UpdateBars();
    }

    void RecalculateTotals()
    {
        totalEsfuerzo = 0;
        totalIncertidumbre = 0;

        foreach (ProjectToken token in placedTokens)
        {
            totalEsfuerzo += token.GetEsfuerzo();
            totalIncertidumbre += token.GetIncertidumbre();
        }

        // Limitar a 30 máximo (3 fichas  10 puntos)
        totalEsfuerzo = Mathf.Clamp(totalEsfuerzo, 0, 30);
        totalIncertidumbre = Mathf.Clamp(totalIncertidumbre, 0, 30);
    }

    void UpdateBars()
    {
        // Normalizar de 0 30 a 0 10 para el índice del sprite
        int esfuerzoIndex = Mathf.RoundToInt((totalEsfuerzo / 30f) * 10f);
        int incertidumbreIndex = Mathf.RoundToInt((totalIncertidumbre / 30f) * 10f);

        esfuerzoIndex = Mathf.Clamp(esfuerzoIndex, 0, 10);
        incertidumbreIndex = Mathf.Clamp(incertidumbreIndex, 0, 10);

        if (esfuerzoBar != null && esfuerzoSprites.Length > esfuerzoIndex)
        {
            esfuerzoBar.sprite = esfuerzoSprites[esfuerzoIndex];
        }

        if (incertidumbreBar != null && incertidumbreSprites.Length > incertidumbreIndex)
        {
            incertidumbreBar.sprite = incertidumbreSprites[incertidumbreIndex];
        }
    }

    Vector3 GetTokenPosition(int index)
    {
        // Calcular posición centrada
        float totalWidth = (maxTokens - 1) * tokenSpacing.x;
        float startX = -totalWidth / 2f;

        Vector3 position = transform.position;
        position.x += startX + (index * tokenSpacing.x);
        position.y += index * tokenSpacing.y;

        return position;
    }

    void ArrangeTokens()
    {
        for (int i = 0; i < placedTokens.Count; i++)
        {
            Vector3 position = GetTokenPosition(i);
            placedTokens[i].transform.position = position;
            placedTokens[i].SetOriginalPosition(position);
        }
    }

    
    public int GetTotalEsfuerzo() => totalEsfuerzo;
    public int GetTotalIncertidumbre() => totalIncertidumbre;
    public int GetTokenCount() => placedTokens.Count;
    public bool IsFull() => placedTokens.Count >= maxTokens;
    public List<ProjectToken> GetPlacedTokens() => new List<ProjectToken>(placedTokens);
}