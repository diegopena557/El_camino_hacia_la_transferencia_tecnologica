using UnityEngine;

public enum TokenCategory
{
    Entender,
    Imaginar,
    Probar
}

public class ProjectToken : MonoBehaviour
{
    [Header("Categoria del Token")]
    public TokenCategory tokenCategory;

    [Header("Valores de la Ficha")]
    [Range(0, 100)]
    public int esfuerzo = 5;
    [Range(0, 100)]
    public int incertidumbre = 5;

    [Header("Informacion del Token")]
    public string tokenName = "Proyecto";
    [TextArea(2, 4)]
    public string description = "";

    [Header("Feedback Personalizado")]
    [TextArea(3, 5)]
    public string correctFeedback = "!Correcto!\nEsta ficha pertenece aqui.";

    [Space(10)]
    [Header("Feedback Incorrecto - Modo 1")]
    [TextArea(3, 5)]
    public string wrongFeedbackMode1 = "Esta ficha no pertenece aqui.\nIntenta con otra categoria.";

    [Header("Feedback Incorrecto - Modo 2")]
    [TextArea(3, 5)]
    public string wrongFeedbackMode2 = "Incorrecto.\n[Explicacion mas detallada para modo 2]";

    [Header("Visual Feedback")]
    public SpriteRenderer tokenRenderer;

    public float hoverScale = 1.1f;
    public float hoverLiftHeight = 0.3f;
    public float hoverSpeed = 5f;

    [Header("Efectos de Seleccion")]
    public float selectedScale = 1.2f;
    public float selectedBrightness = 1.5f;
    public float transitionSpeed = 8f;

    [Header("Drag Settings")]
    public float dragZPosition = -1f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clickSound;       // Sonido al hacer clic sobre la ficha
    public AudioClip correctSlotSound; // Sonido al entrar en slot correcto
    public AudioClip wrongSlotSound;   // Sonido al entrar en slot incorrecto

    private Vector3 originalPosition;
    private Vector3 originalScale;
    private Vector3 targetPosition;
    private Vector3 targetScale;

    private bool isHovering = false;
    private bool isDragging = false;
    private bool isPlaced = false;
    private Camera mainCamera;
    private TokenSlot currentSlot;
    private TokenSlot slotToLeave = null; // slot del que hay que salir al comenzar a arrastrar

    void Start()
    {
        if (tokenRenderer == null)
            tokenRenderer = GetComponent<SpriteRenderer>();

        originalScale = transform.localScale;
        originalPosition = transform.position;
        targetPosition = originalPosition;
        targetScale = originalScale;


        mainCamera = Camera.main;

        // Verificar configuracion
        Collider2D col = GetComponent<Collider2D>();
        if (col == null)
        {
            Debug.LogError($"[ProjectToken] {gameObject.name} NO TIENE COLLIDER2D! Agregalo para poder arrastrarlo.", gameObject);
        }
        else
        {
            Debug.Log($"[ProjectToken] '{tokenName}' configurado. Categoria: {tokenCategory}, Esfuerzo: {esfuerzo}, Incertidumbre: {incertidumbre}", gameObject);
        }
    }

    void Update()
    {
        // Suavizar la transicion de escala y color
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * transitionSpeed);



        // Movimiento suave para el hover (solo cuando no esta siendo arrastrado)
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

        // Sonido de clic al agarrar la ficha
        if (audioSource != null && clickSound != null)
            audioSource.PlayOneShot(clickSound);

        // Guardar referencia al slot actual pero NO remover todavia.
        // RemoveToken dispara ArrangeTokens() que desplaza el objeto mientras Unity
        // procesa el clic; esto puede romper el tracking del mouse y hacer que
        // OnMouseUp nunca se llame, dejando isDragging=true y bloqueando el token.
        // La remocion real ocurre en el primer frame de OnMouseDrag.
        slotToLeave = isPlaced ? currentSlot : null;

        // Efecto visual de arrastre (mas grande y brillante)
        targetScale = originalScale * selectedScale;



        // Mostrar tooltip con la descripcion
        if (TokenTooltip.Instance != null && !string.IsNullOrEmpty(description))
        {
            TokenTooltip.Instance.ShowTooltip(description);
        }
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;

        // Remover del slot en el primer frame de movimiento (diferido desde OnMouseDown)
        if (slotToLeave != null)
        {
            slotToLeave.RemoveToken(this);
            isPlaced = false;
            slotToLeave = null;
        }

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


        // Ocultar tooltip
        if (TokenTooltip.Instance != null)
        {
            TokenTooltip.Instance.HideTooltip();
        }

        Debug.Log($"[ProjectToken] '{tokenName}' soltado en posicion: {transform.position}");

        // Buscar TODOS los colliders en esta posicion
        Collider2D[] hits = Physics2D.OverlapPointAll(transform.position);

        Debug.Log($"[ProjectToken] Se detectaron {hits.Length} colliders en esta posicion");

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
                Debug.Log($"[ProjectToken]   Encontro TokenSlot!");

                // Priorizar slots que estan como trigger
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
            Debug.Log($"[ProjectToken] Intentando anadir al slot '{foundSlot.gameObject.name}'");

            if (foundSlot.CanAcceptToken())
            {
                // Remover del slot anterior si tenia uno
                if (currentSlot != null && currentSlot != foundSlot)
                {
                    Debug.Log($"[ProjectToken] Removiendo del slot anterior '{currentSlot.gameObject.name}'");
                    currentSlot.RemoveToken(this);
                }

                foundSlot.AddToken(this);
                currentSlot = foundSlot;

                // NO establecer posicion aqui - el slot lo hara
                RemoveHoverEffect();
                return;
            }
            else
            {
                Debug.LogWarning($"[ProjectToken] El slot no puede aceptar mas tokens");
            }
        }
        else
        {
            Debug.Log($"[ProjectToken] No se encontro ningun TokenSlot en esta posicion");
        }

        // Si no cayo en un slot, conservar la posicion donde se solto
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

    // Metodo publico para forzar reset (puede ser llamado desde otro script)
    public void ForceReset()
    {
        Debug.Log($"[ProjectToken] FORCE RESET para {tokenName}");
        isDragging = false;
        isHovering = false;
        slotToLeave = null;

        // Si esta en un slot, sacarlo
        if (isPlaced && currentSlot != null)
        {
            currentSlot.RemoveToken(this);
        }

        isPlaced = false;
        currentSlot = null;

        targetScale = originalScale;
        transform.localScale = originalScale;

        RemoveHoverEffect();

        Debug.Log($"[ProjectToken] {tokenName} reseteado por ForceReset.");
    }

    // Llamado por TokenSlot para reproducir el sonido de colocacion
    public void PlaySlotSound(bool isCorrect)
    {
        if (audioSource == null) return;

        AudioClip clip = isCorrect ? correctSlotSound : wrongSlotSound;
        if (clip != null)
            audioSource.PlayOneShot(clip);
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
        // Obtener el feedback segun el modo actual
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