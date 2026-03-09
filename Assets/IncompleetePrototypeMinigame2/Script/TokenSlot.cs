using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TokenSlot : MonoBehaviour
{
    [Header("Categora Aceptada")]
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

    [Header("Indicador de Capacidad")]
    public SpriteRenderer capacityIndicator;
    public Sprite[] capacitySprites = new Sprite[4]; // ndices 0, 1, 2, 3 fichas
    public TextMeshProUGUI capacityText; // Alternativa con texto
    public bool showCapacityIndicator = true;
    public bool useTextIndicator = true; // Usar texto en lugar de sprites

    [Header("Sprites de las Barras (0-6)")]
    public Sprite[] esfuerzoSprites = new Sprite[7];
    public Sprite[] incertidumbreSprites = new Sprite[7];

    [Header("Efectos de Feedback de Barras")]
    public float barShakeDuration = 0.3f;
    public float barShakeMagnitude = 0.15f;
    public float barScalePulse = 1.15f;
    public bool enableBarFeedback = true;
    [Tooltip("Escala base de las barras (debe coincidir con la escala en el editor)")]
    public float barBaseScale = 0.77019f;

    [Header("Valores Totales")]
    [SerializeField] private int totalEsfuerzo = 0;
    [SerializeField] private int totalIncertidumbre = 100; // Inicia en 100

    private List<ProjectToken> placedTokens = new List<ProjectToken>();

    // Para el shake de las barras
    private Vector3 esfuerzoBarOriginalPos;
    private Vector3 incertidumbreBarOriginalPos;
    private Vector3 esfuerzoBarOriginalScale;
    private Vector3 incertidumbreBarOriginalScale;
    private Coroutine esfuerzoShakeCoroutine;
    private Coroutine incertidumbreShakeCoroutine;

    // Para detectar cambios
    private int previousEsfuerzo = 0;
    private int previousIncertidumbre = 100;

    void Start()
    {
        UpdateBars();

        // Verificar configuracin
        Collider2D col = GetComponent<Collider2D>();
        if (col == null)
        {
            Debug.LogError($"[TokenSlot] {gameObject.name} NO TIENE COLLIDER2D! Agrgalo para que funcione.", gameObject);
        }
        else if (!col.isTrigger)
        {
            Debug.LogWarning($"[TokenSlot] {gameObject.name} - El Collider2D debe estar marcado como 'Is Trigger'", gameObject);
        }
        else
        {
            Debug.Log($"[TokenSlot] {gameObject.name} configurado correctamente. Categora aceptada: {acceptedCategory}", gameObject);
        }
    }

    // Mtodo para cambiar la categora del slot (llamado por LevelManager)
    public void SetCategory(TokenCategory newCategory)
    {
        acceptedCategory = newCategory;
        Debug.Log($"[TokenSlot] Categora cambiada a: {acceptedCategory}");
    }

    // Mtodo para resetear el slot al inicio de un nivel
    public void ResetSlot()
    {
        ClearAllTokens();
        totalEsfuerzo = 0;
        totalIncertidumbre = 100; // Resetear a 100
        UpdateBars(false); // NO hacer shake al resetear
        Debug.Log($"[TokenSlot] Slot reseteado. Incertidumbre: {totalIncertidumbre}, Esfuerzo: {totalEsfuerzo}");
    }

    public bool CanAcceptToken()
    {
        return placedTokens.Count < maxTokens;
    }

    public void AddToken(ProjectToken token)
    {
        Debug.Log($"[TokenSlot] Intentando aadir token '{token.GetTokenName()}' al slot '{gameObject.name}'");

        if (!CanAcceptToken())
        {
            Debug.LogWarning($"[TokenSlot] {gameObject.name} est lleno ({placedTokens.Count}/{maxTokens})");

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
            Debug.LogWarning($"[TokenSlot] Esta ficha ya est en el slot");
            return;
        }

        // Verificar si la categora es correcta para mostrar feedback apropiado
        bool isCorrectCategory = (token.GetCategory() == acceptedCategory);

        // Registrar estadstica
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
                TokenFeedbackUI.Instance.ShowCorrectCustomFeedback(customMessage);
            }
        }
        else
        {
            // Mostrar feedback personalizado de la ficha (incorrecto)
            if (TokenFeedbackUI.Instance != null)
            {
                string customMessage = token.GetWrongFeedback();
                TokenFeedbackUI.Instance.ShowWrongCustomFeedback(customMessage);
            }
        }

        // SIEMPRE aadir el token (sin validar categora para permitir balance)
        Debug.Log($"[TokenSlot] Token '{token.GetTokenName()}' aadido! Categora: {token.GetCategory()}");

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

        // Reproducir sonido de slot en el propio token
        token.PlaySlotSound(isCorrectCategory);

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

        // VERIFICAR SI EL SLOT EST LLENO Y COMPLETAR NIVEL
        CheckLevelCompletion();
    }

    void CheckLevelCompletion()
    {
        // Solo verificar si el slot est lleno
        if (placedTokens.Count >= maxTokens)
        {
            Debug.Log($"[TokenSlot] Slot lleno! Verificando composicin de fichas...");

            // Contar cuntas fichas hay de cada categora
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

            Debug.Log($"[TokenSlot] Composicin - Entender: {countEntender}, Imaginar: {countImaginar}, Probar: {countProbar}");

            // Verificar si TODAS las fichas son de la categora correcta
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
                Debug.Log($"[TokenSlot] Nivel completado! Las 3 fichas son de {acceptedCategory}");

                // Notificar al LevelManager
                if (LevelManager.Instance != null)
                {
                    LevelManager.Instance.CompleteLevel();
                }
                else
                {
                    Debug.LogWarning("[TokenSlot] No se encontr LevelManager en la escena");
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
        UpdateBars(false); // NO hacer shake al limpiar
    }

    void RecalculateTotals()
    {
        // Esfuerzo: suma normal (0 a 100)
        totalEsfuerzo = 0;

        foreach (ProjectToken token in placedTokens)
        {
            totalEsfuerzo += token.GetEsfuerzo();
        }

        // Limitar esfuerzo a 100 mximo
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

    void UpdateBars(bool doShake = true)
    {
        // Detectar si hubo cambios
        bool esfuerzoChanged = totalEsfuerzo != previousEsfuerzo;
        bool incertidumbreChanged = totalIncertidumbre != previousIncertidumbre;

        // Normalizar de 0-100 a 0-6 para el ndice del sprite (7 sprites totales)
        int esfuerzoIndex = Mathf.RoundToInt((totalEsfuerzo / 100f) * 6f);
        int incertidumbreIndex = Mathf.RoundToInt((totalIncertidumbre / 100f) * 6f);

        esfuerzoIndex = Mathf.Clamp(esfuerzoIndex, 0, 6);
        incertidumbreIndex = Mathf.Clamp(incertidumbreIndex, 0, 6);

        if (esfuerzoBar != null && esfuerzoSprites.Length > esfuerzoIndex)
        {
            esfuerzoBar.sprite = esfuerzoSprites[esfuerzoIndex];

            // Aplicar shake solo si est habilitado Y se pidi hacer shake Y cambi el valor
            if (enableBarFeedback && doShake && esfuerzoChanged)
            {
                if (esfuerzoShakeCoroutine != null)
                    StopCoroutine(esfuerzoShakeCoroutine);
                esfuerzoShakeCoroutine = StartCoroutine(ShakeBar(esfuerzoBar, esfuerzoBarOriginalPos, esfuerzoBarOriginalScale));
            }
        }

        if (incertidumbreBar != null && incertidumbreSprites.Length > incertidumbreIndex)
        {
            incertidumbreBar.sprite = incertidumbreSprites[incertidumbreIndex];

            // Aplicar shake solo si est habilitado Y se pidi hacer shake Y cambi el valor
            if (enableBarFeedback && doShake && incertidumbreChanged)
            {
                if (incertidumbreShakeCoroutine != null)
                    StopCoroutine(incertidumbreShakeCoroutine);
                incertidumbreShakeCoroutine = StartCoroutine(ShakeBar(incertidumbreBar, incertidumbreBarOriginalPos, incertidumbreBarOriginalScale));
            }
        }

        // Actualizar valores previos
        previousEsfuerzo = totalEsfuerzo;
        previousIncertidumbre = totalIncertidumbre;

        Debug.Log($"[TokenSlot] Barras actualizadas - Esfuerzo: {totalEsfuerzo}/100 (sprite {esfuerzoIndex}/6), Incertidumbre: {totalIncertidumbre}/100 (sprite {incertidumbreIndex}/6)");

        // Actualizar indicador de capacidad
        UpdateCapacityIndicator();
    }

    void UpdateCapacityIndicator()
    {
        if (!showCapacityIndicator)
            return;

        int tokenCount = placedTokens.Count;

        // Opcin 1: Usar texto (TextMeshPro)
        if (useTextIndicator && capacityText != null)
        {
            capacityText.text = $"{tokenCount}/{maxTokens}";



            Debug.Log($"[TokenSlot] Indicador de capacidad (texto): {tokenCount}/{maxTokens}");
        }
        // Opcin 2: Usar sprites
        else if (!useTextIndicator && capacityIndicator != null)
        {
            tokenCount = Mathf.Clamp(tokenCount, 0, capacitySprites.Length - 1);

            if (capacitySprites.Length > tokenCount && capacitySprites[tokenCount] != null)
            {
                capacityIndicator.sprite = capacitySprites[tokenCount];
                Debug.Log($"[TokenSlot] Indicador de capacidad (sprite): {tokenCount}/{maxTokens}");
            }
        }
    }

    System.Collections.IEnumerator ShakeBar(SpriteRenderer bar, Vector3 originalPos, Vector3 originalScale)
    {
        if (bar == null)
        {
            Debug.LogWarning("[TokenSlot] Barra es null, no se puede hacer shake");
            yield break;
        }

        // Usar la escala configurada como respaldo
        Vector3 safeScale = originalScale.magnitude < 0.01f ? Vector3.one * barBaseScale : originalScale;

        Debug.Log($"[TokenSlot] Iniciando shake con escala: {safeScale}");

        // SOLO HACER PULSE (escala), NO mover la posicin
        float elapsed = 0f;
        float pulseDuration = barShakeDuration;

        while (elapsed < pulseDuration)
        {
            float t = elapsed / pulseDuration;
            // Ping-pong: agranda y luego vuelve a normal
            float scaleFactor = Mathf.Lerp(1f, barScalePulse, Mathf.Sin(t * Mathf.PI));
            Vector3 targetScale = safeScale * scaleFactor;

            bar.transform.localScale = targetScale;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Asegurar que vuelve a la escala original
        bar.transform.localScale = safeScale;
        Debug.Log($"[TokenSlot] Shake completado. Escala final: {bar.transform.localScale}");
    }

    Vector3 GetTokenPosition(int index)
    {
        // Si solo hay 1 token, centrarlo
        if (maxTokens == 1)
        {
            return transform.position;
        }

        // Calcular posicin distribuida
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

    // Getters pblicos
    public int GetTotalEsfuerzo() => totalEsfuerzo;
    public int GetTotalIncertidumbre() => totalIncertidumbre;
    public int GetTokenCount() => placedTokens.Count;
    public bool IsFull() => placedTokens.Count >= maxTokens;
    public List<ProjectToken> GetPlacedTokens() => new List<ProjectToken>(placedTokens);
}