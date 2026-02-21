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

    [Header("Sprites de las Barras (0-6)")]
    public Sprite[] esfuerzoSprites = new Sprite[7];
    public Sprite[] incertidumbreSprites = new Sprite[7];

    [Header("Valores Totales")]
    [SerializeField] private int totalEsfuerzo = 0;
    [SerializeField] private int totalIncertidumbre = 100; // Inicia en 100

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

    // Método para cambiar la categoría del slot (llamado por LevelManager)
    public void SetCategory(TokenCategory newCategory)
    {
        acceptedCategory = newCategory;
        Debug.Log($"[TokenSlot] Categoría cambiada a: {acceptedCategory}");
    }

    // Método para resetear el slot al inicio de un nivel
    public void ResetSlot()
    {
        ClearAllTokens();
        totalEsfuerzo = 0;
        totalIncertidumbre = 100; // Resetear a 100
        UpdateBars();
        Debug.Log($"[TokenSlot] Slot reseteado. Incertidumbre: {totalIncertidumbre}, Esfuerzo: {totalEsfuerzo}");
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

            // Mostrar feedback UI de slot lleno
            if (TokenFeedbackUI.Instance != null)
                TokenFeedbackUI.Instance.ShowSlotFullFeedback();

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

        // Verificar si la categoría es correcta para mostrar feedback apropiado
        bool isCorrectCategory = (token.GetCategory() == acceptedCategory);

        // Registrar estadística
        if (TokenStatsManager.Instance != null)
        {
            if (isCorrectCategory)
                TokenStatsManager.Instance.AddCorrectToken();
            else
                TokenStatsManager.Instance.AddWrongToken();
        }

        if (isCorrectCategory)
        {
            // Mostrar feedback personalizado de la ficha (correcto)
            if (TokenFeedbackUI.Instance != null)
            {
                string customMessage = token.GetCorrectFeedback();
                TokenFeedbackUI.Instance.ShowCustomFeedback(customMessage, TokenFeedbackUI.Instance.correctColor);
            }
        }
        else
        {
            // Mostrar feedback personalizado de la ficha (incorrecto)
            if (TokenFeedbackUI.Instance != null)
            {
                string customMessage = token.GetWrongFeedback();
                TokenFeedbackUI.Instance.ShowCustomFeedback(customMessage, TokenFeedbackUI.Instance.wrongColor);
            }
        }

        // SIEMPRE añadir el token (sin validar categoría para permitir balance)
        Debug.Log($"[TokenSlot]  Token '{token.GetTokenName()}' añadido! Categoría: {token.GetCategory()}");

        placedTokens.Add(token);
        token.SetPlaced(true);

        // Reproducir feedback visual/sonoro
        TokenFeedback feedback2 = token.GetComponent<TokenFeedback>();
        if (feedback2 != null)
        {
            if (isCorrectCategory)
                feedback2.PlayCorrectFeedback();
            else
                feedback2.PlayWrongFeedback();
        }

        // IMPORTANTE: Actualizar totales ANTES de posicionar
        RecalculateTotals();
        UpdateBars();

        // Posicionar el token
        if (autoArrange)
        {
            Debug.Log($"[TokenSlot] Auto-arrange activado. Reorganizando {placedTokens.Count} fichas...");
            ArrangeTokens();
        }
        else
        {
            Vector3 position = GetTokenPosition(placedTokens.Count - 1);
            token.transform.position = position;
            token.SetOriginalPosition(position);
            Debug.Log($"[TokenSlot] Token posicionado manualmente en {position}");
        }

        Debug.Log($"[TokenSlot] Total en slot: Esfuerzo={totalEsfuerzo}, Incertidumbre={totalIncertidumbre}");

        // VERIFICAR SI EL SLOT ESTÁ LLENO Y COMPLETAR NIVEL
        CheckLevelCompletion();
    }

    void CheckLevelCompletion()
    {
        // Solo verificar si el slot está lleno
        if (placedTokens.Count >= maxTokens)
        {
            Debug.Log($"[TokenSlot] Slot lleno! Verificando composición de fichas...");

            // Contar cuántas fichas hay de cada categoría
            int countEntender = 0;
            int countImaginar = 0;
            int countProbar = 0;

            foreach (ProjectToken token in placedTokens)
            {
                switch (token.GetCategory())
                {
                    case TokenCategory.Entender:
                        countEntender++;
                        break;
                    case TokenCategory.Imaginar:
                        countImaginar++;
                        break;
                    case TokenCategory.Probar:
                        countProbar++;
                        break;
                }
            }

            Debug.Log($"[TokenSlot] Composición - Entender: {countEntender}, Imaginar: {countImaginar}, Probar: {countProbar}");

            // Verificar si TODAS las fichas son de la categoría correcta
            bool allCorrect = true;
            foreach (ProjectToken token in placedTokens)
            {
                if (token.GetCategory() != acceptedCategory)
                {
                    allCorrect = false;
                    Debug.Log($"[TokenSlot] Token '{token.GetTokenName()}' es incorrecto ({token.GetCategory()} != {acceptedCategory})");
                    break;
                }
            }

            if (allCorrect)
            {
                Debug.Log($"[TokenSlot]  ¡Nivel completado! Las 3 fichas son de {acceptedCategory}");

                // Notificar al LevelManager
                if (LevelManager.Instance != null)
                {
                    LevelManager.Instance.CompleteLevel();
                }
                else
                {
                    Debug.LogWarning("[TokenSlot] No se encontró LevelManager en la escena");
                }
            }
            else
            {
                Debug.Log($"[TokenSlot] Slot lleno pero NO todas las fichas son de {acceptedCategory}. Reintenta!");
            }
        }
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
        // Esfuerzo: suma normal (0 a 100)
        totalEsfuerzo = 0;

        foreach (ProjectToken token in placedTokens)
        {
            totalEsfuerzo += token.GetEsfuerzo();
        }

        // Limitar esfuerzo a 100 máximo
        totalEsfuerzo = Mathf.Clamp(totalEsfuerzo, 0, 100);

        // Incertidumbre: inicia en 100 y RESTA los valores de las fichas
        totalIncertidumbre = 100;

        foreach (ProjectToken token in placedTokens)
        {
            totalIncertidumbre -= token.GetIncertidumbre();
        }

        // Limitar incertidumbre entre 0 y 100
        totalIncertidumbre = Mathf.Clamp(totalIncertidumbre, 0, 100);
    }

    void UpdateBars()
    {
        // Normalizar de 0-100 a 0-6 para el índice del sprite (7 sprites totales)
        int esfuerzoIndex = Mathf.RoundToInt((totalEsfuerzo / 100f) * 6f);
        int incertidumbreIndex = Mathf.RoundToInt((totalIncertidumbre / 100f) * 6f);

        esfuerzoIndex = Mathf.Clamp(esfuerzoIndex, 0, 6);
        incertidumbreIndex = Mathf.Clamp(incertidumbreIndex, 0, 6);

        if (esfuerzoBar != null && esfuerzoSprites.Length > esfuerzoIndex)
        {
            esfuerzoBar.sprite = esfuerzoSprites[esfuerzoIndex];
        }

        if (incertidumbreBar != null && incertidumbreSprites.Length > incertidumbreIndex)
        {
            incertidumbreBar.sprite = incertidumbreSprites[incertidumbreIndex];
        }

        Debug.Log($"[TokenSlot] Barras actualizadas - Esfuerzo: {totalEsfuerzo}/100 (sprite {esfuerzoIndex}/6), Incertidumbre: {totalIncertidumbre}/100 (sprite {incertidumbreIndex}/6)");
    }

    Vector3 GetTokenPosition(int index)
    {
        // Si solo hay 1 token, centrarlo
        if (maxTokens == 1)
        {
            return transform.position;
        }

        // Calcular posición distribuida
        float totalWidth = (maxTokens - 1) * tokenSpacing.x;
        float totalHeight = (maxTokens - 1) * tokenSpacing.y;

        float startX = -totalWidth / 2f;
        float startY = -totalHeight / 2f;

        Vector3 position = transform.position;
        position.x += startX + (index * tokenSpacing.x);
        position.y += startY + (index * tokenSpacing.y);

        return position;
    }

    void ArrangeTokens()
    {
        for (int i = 0; i < placedTokens.Count; i++)
        {
            if (placedTokens[i] == null) continue;

            Vector3 position = GetTokenPosition(i);
            placedTokens[i].transform.position = position;
            placedTokens[i].SetOriginalPosition(position);

            Debug.Log($"[TokenSlot] Posicionando ficha {i}: {placedTokens[i].GetTokenName()} en {position}");
        }
    }

    // Getters públicos
    public int GetTotalEsfuerzo() => totalEsfuerzo;
    public int GetTotalIncertidumbre() => totalIncertidumbre;
    public int GetTokenCount() => placedTokens.Count;
    public bool IsFull() => placedTokens.Count >= maxTokens;
    public List<ProjectToken> GetPlacedTokens() => new List<ProjectToken>(placedTokens);
}