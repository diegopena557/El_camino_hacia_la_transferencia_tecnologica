using UnityEngine;

public enum TokenCategory
{
    Entender,
    Imaginar,
    Probar
}

public class ProjectToken : MonoBehaviour
{
    [Header("Categoría del Token")]
    public TokenCategory tokenCategory;

    [Header("Valores de la Ficha")]
    [Range(0, 100)]
    public int esfuerzo = 5;
    [Range(0, 100)]
    public int incertidumbre = 5;

    [Header("Información del Token")]
    public string tokenName = "Proyecto";
    [TextArea(2, 4)]
    public string description = "";

    [Header("Feedback Personalizado")]
    [TextArea(3, 5)]
    public string correctFeedback = "¡Correcto!\nEsta ficha pertenece aquí.";

    [Space(10)]
    [Header("Feedback Incorrecto - Modo 1")]
    [TextArea(3, 5)]
    public string wrongFeedbackMode1 = "Esta ficha no pertenece aquí.\nIntenta con otra categoría.";

    [Header("Feedback Incorrecto - Modo 2")]
    [TextArea(3, 5)]
    public string wrongFeedbackMode2 = "Incorrecto.\n[Explicación más detallada para modo 2]";

    [Header("Visual Feedback")]
    public SpriteRenderer tokenRenderer;
    public float hoverScale = 1.1f;
    public float hoverLiftHeight = 0.3f;
    public float hoverSpeed = 5f;

    [Header("Efectos de Selección")]
    public float selectedScale = 1.2f;
    public float selectedBrightness = 1.5f;
    public float transitionSpeed = 8f;

    [Header("Drag Settings")]
    public float dragZPosition = -1f;

    private Vector3 originalPosition;
    private Vector3 originalScale;
    private Vector3 targetPosition;
    private Vector3 targetScale;
    private Color targetColor;
    private bool isHovering = false;
    private bool isDragging = false;
    private bool isPlaced = false;
    private Camera mainCamera;
    private TokenSlot currentSlot;

    void Start()
    {
        if (tokenRenderer == null)
            tokenRenderer = GetComponent<SpriteRenderer>();

        originalScale = transform.localScale;
        originalPosition = transform.position;
        targetPosition = originalPosition;
        targetScale = originalScale;



        mainCamera = Camera.main;

        // Verificar configuración
        Collider2D col = GetComponent<Collider2D>();
        if (col == null)
        {
            Debug.LogError($"[ProjectToken] {gameObject.name} NO TIENE COLLIDER2D! Agrégalo para poder arrastrarlo.", gameObject);
        }
        else
        {
            Debug.Log($"[ProjectToken] '{tokenName}' configurado. Categoría: {tokenCategory}, Esfuerzo: {esfuerzo}, Incertidumbre: {incertidumbre}", gameObject);
        }
    }

    void Update()
    {
        // Suavizar la transición de escala y color
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * transitionSpeed);

        if (tokenRenderer != null)
        {
            tokenRenderer.color = Color.Lerp(tokenRenderer.color, targetColor, Time.deltaTime * transitionSpeed);
        }

        // Movimiento suave para el hover (solo cuando no está siendo arrastrado)
        if (!isDragging && isHovering)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                targetPosition,
                Time.deltaTime * hoverSpeed
            );
        }
    }

    void OnMouseEnter()
    {
        if (isDragging) return;

        isHovering = true;
        ApplyHoverEffect();
    }

    void OnMouseExit()
    {
        if (isDragging) return;

        isHovering = false;
        RemoveHoverEffect();
    }

    void OnMouseDown()
    {
        isDragging = true;
        isHovering = false;

        // Si estaba en un slot, sacarlo temporalmente
        if (isPlaced && currentSlot != null)
        {
            currentSlot.RemoveToken(this);
            isPlaced = false;
        }

        // Efecto visual de arrastre (más grande y brillante)
        targetScale = originalScale * selectedScale;

     
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(mainCamera.transform.position.z) + dragZPosition;
        transform.position = mainCamera.ScreenToWorldPoint(mousePos);
    }

    void OnMouseUp()
    {
        if (!isDragging) return;

        isDragging = false;

        // Volver a escala y color normal
        targetScale = originalScale;
        

        Debug.Log($"[ProjectToken] '{tokenName}' soltado en posición: {transform.position}");

        // Buscar TODOS los colliders en esta posición
        Collider2D[] hits = Physics2D.OverlapPointAll(transform.position);

        Debug.Log($"[ProjectToken] Se detectaron {hits.Length} colliders en esta posición");

        TokenSlot foundSlot = null;
        int slotPriority = -1;

        // Buscar el TokenSlot entre todos los colliders detectados
        foreach (Collider2D hit in hits)
        {
            Debug.Log($"[ProjectToken] - Revisando: {hit.gameObject.name} (Layer: {LayerMask.LayerToName(hit.gameObject.layer)})");

            // Ignorar el collider del propio token
            if (hit.gameObject == gameObject)
            {
                Debug.Log($"[ProjectToken]   (es el mismo token, ignorando)");
                continue;
            }

            // Ignorar otros tokens
            ProjectToken otherToken = hit.GetComponent<ProjectToken>();
            if (otherToken != null)
            {
                Debug.Log($"[ProjectToken]   (es otro token, ignorando)");
                continue;
            }

            // Buscar TokenSlot
            TokenSlot slot = hit.GetComponent<TokenSlot>();
            if (slot != null)
            {
                Debug.Log($"[ProjectToken]    Encontró TokenSlot!");

                // Priorizar slots que están como trigger
                if (hit.isTrigger && slotPriority < 1)
                {
                    foundSlot = slot;
                    slotPriority = 1;
                }
                else if (foundSlot == null)
                {
                    foundSlot = slot;
                    slotPriority = 0;
                }
            }
        }

        if (foundSlot != null)
        {
            Debug.Log($"[ProjectToken] Intentando añadir al slot '{foundSlot.gameObject.name}'");

            if (foundSlot.CanAcceptToken())
            {
                // Remover del slot anterior si tenía uno
                if (currentSlot != null && currentSlot != foundSlot)
                {
                    Debug.Log($"[ProjectToken] Removiendo del slot anterior '{currentSlot.gameObject.name}'");
                    currentSlot.RemoveToken(this);
                }

                foundSlot.AddToken(this);
                currentSlot = foundSlot;

                // NO establecer posición aquí - el slot lo hará
                RemoveHoverEffect();
                return;
            }
            else
            {
                Debug.LogWarning($"[ProjectToken] El slot no puede aceptar más tokens");
            }
        }
        else
        {
            Debug.Log($"[ProjectToken] No se encontró ningún TokenSlot en esta posición");
        }

        // Si no cayó en un slot, conservar la posición donde se soltó
        originalPosition = transform.position;
        targetPosition = originalPosition;
        RemoveHoverEffect();
    }

    void ApplyHoverEffect()
    {
        // Efecto sutil de hover
        targetScale = originalScale * 1.05f;

       

        // Levantar un poco la ficha
        targetPosition = originalPosition + Vector3.up * hoverLiftHeight;
    }

    void RemoveHoverEffect()
    {
        targetScale = originalScale;
        
        targetPosition = originalPosition;
    }

    void ReturnToOriginalPosition()
    {
        transform.position = originalPosition;
        targetPosition = originalPosition;
    }

    public void SetPlaced(bool placed)
    {
        isPlaced = placed;

        if (!placed)
        {
            currentSlot = null;
            RemoveHoverEffect();
        }
    }

    public void SetOriginalPosition(Vector3 position)
    {
        originalPosition = position;
        targetPosition = position;
    }

    // Getters
    public int GetEsfuerzo() => esfuerzo;
    public int GetIncertidumbre() => incertidumbre;
    public string GetTokenName() => tokenName;
    public string GetDescription() => description;
    public TokenCategory GetCategory() => tokenCategory;
    public string GetCorrectFeedback() => correctFeedback;

    public string GetWrongFeedback()
    {
        // Obtener el feedback según el modo actual
        if (GameModeManager.Instance != null)
        {
            return GameModeManager.Instance.GetCurrentMode() == 1
                ? wrongFeedbackMode1
                : wrongFeedbackMode2;
        }

        // Si no hay GameModeManager, usar modo 1 por defecto
        return wrongFeedbackMode1;
    }
}