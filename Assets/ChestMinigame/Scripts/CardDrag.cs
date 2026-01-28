using UnityEngine;
using UnityEngine.EventSystems;

public class CardDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public CardType cardType;
    public Collider2D spawnArea;

    [Header("Informaci�n de la Carta")]
    public CardFeedbackData cardInfo;

    [Header("Efectos Visuales")]
    public float selectedScale = 1.2f;
    public float selectedBrightness = 1.5f;
    public float transitionSpeed = 8f;

    private Camera mainCamera;
    private Canvas parentCanvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private bool isDragging;
    private Vector3 normalScale;
    private SpriteRenderer spriteRenderer;
    private UnityEngine.UI.Image imageComponent;
    private Color normalColor;
    private Vector3 targetScale;
    private Color targetColor;
    private int originalSortingOrder;
    private Transform originalParent;
    private int originalSiblingIndex;
    private Vector3 dragStartPosition;
    private Rigidbody2D rb;

    void Start()
    {
        mainCamera = Camera.main;
        rectTransform = GetComponent<RectTransform>();
        rb = GetComponent<Rigidbody2D>();

        // Buscar el Canvas padre
        parentCanvas = GetComponentInParent<Canvas>();

        // Intentar obtener SpriteRenderer o Image
        spriteRenderer = GetComponent<SpriteRenderer>();
        imageComponent = GetComponent<UnityEngine.UI.Image>();

        normalScale = transform.localScale;
        targetScale = normalScale;

        // Configurar color inicial
        if (spriteRenderer != null)
        {
            normalColor = spriteRenderer.color;
            targetColor = normalColor;
        }
        else if (imageComponent != null)
        {
            normalColor = imageComponent.color;
            targetColor = normalColor;
        }

        // Agregar CanvasGroup si no existe (para control de raycast)
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();
    }

    void Update()
    {
        // Suavizar la transici�n de escala y brillo
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * transitionSpeed);

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, targetColor, Time.deltaTime * transitionSpeed);
        }
        else if (imageComponent != null)
        {
            imageComponent.color = Color.Lerp(imageComponent.color, targetColor, Time.deltaTime * transitionSpeed);
        }
    }

    // ========================================
    // IMPLEMENTACI�N DE INTERFACES UI
    // ========================================

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        dragStartPosition = transform.position;

        // DESACTIVAR F�SICA mientras arrastra
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.gravityScale = 0f; // Desactivar gravedad
            rb.isKinematic = true; // Hacer kinematic para que no aplique f�sica
        }

        // Hacer la carta m�s grande y brillante
        targetScale = normalScale * selectedScale;

        if (spriteRenderer != null)
        {
            targetColor = normalColor * selectedBrightness;
        }
        else if (imageComponent != null)
        {
            targetColor = normalColor * selectedBrightness;
        }

        // Mover al final de la jerarqu�a para renderizar encima
        transform.SetAsLastSibling();

        // Permitir que raycast pase a trav�s durante drag
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false;
        }

        Debug.Log($"Comenz� a arrastrar: {gameObject.name} desde posici�n {dragStartPosition}");
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        if (rectTransform != null && parentCanvas != null)
        {
            // Para World Space Canvas
            if (parentCanvas.renderMode == RenderMode.WorldSpace)
            {
                // Convertir posici�n del mouse a posici�n del mundo
                Vector3 worldPos;
                if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
                    parentCanvas.transform as RectTransform,
                    eventData.position,
                    eventData.pressEventCamera != null ? eventData.pressEventCamera : mainCamera,
                    out worldPos))
                {
                    // Mantener la Z original
                    worldPos.z = transform.position.z;
                    transform.position = worldPos;
                }
            }
            else
            {
                // Para Screen Space Canvas
                Vector2 localPoint;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    parentCanvas.transform as RectTransform,
                    eventData.position,
                    parentCanvas.worldCamera,
                    out localPoint))
                {
                    rectTransform.localPosition = localPoint;
                }
            }
        }
        else
        {
            // Fallback: usar posici�n de mouse directamente
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
            worldPos.z = transform.position.z;
            transform.position = worldPos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        // REACTIVAR F�SICA y RESETEAR velocidad completamente
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;        // Resetear velocidad lineal
            rb.angularVelocity = 0f;           // Resetear velocidad angular
            rb.isKinematic = false;            // Volver a dynamic
            rb.gravityScale = 0.5f;              // Restaurar gravedad
        }

        // Volver al tama�o y brillo normal
        targetScale = normalScale;
        targetColor = normalColor;

        // Restaurar raycast
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
        }

        Debug.Log($"=== OnEndDrag: {gameObject.name} ===");
        Debug.Log($"Posici�n inicial: {dragStartPosition}");
        Debug.Log($"Posici�n final: {transform.position}");

        // Calcular distancia movida
        float distanceMoved = Vector3.Distance(dragStartPosition, transform.position);
        Debug.Log($"Distancia movida: {distanceMoved}");

        // Si NO se movi� (click sin arrastrar), no hacer nada
        if (distanceMoved < 0.1f)
        {
            Debug.Log("No se movi� - ignorando");
            return;
        }

        // Verificar si cay� en un cofre usando Raycast desde la posici�n de la carta
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.zero, 0.1f);

        Debug.Log($"Raycast hits: {hits.Length}");

        Chest foundChest = null;

        foreach (RaycastHit2D hit in hits)
        {
            // Ignorar si el hit es la carta misma
            if (hit.collider.gameObject == gameObject)
            {
                Debug.Log($"Hit ignorado (es la carta misma): {hit.collider.gameObject.name}");
                continue;
            }

            Debug.Log($"Hit detectado: {hit.collider.gameObject.name}");

            Chest chest = hit.collider.GetComponent<Chest>();
            if (chest != null)
            {
                foundChest = chest;
                Debug.Log($" Encontr� cofre v�lido: {chest.gameObject.name}");
                break;
            }
        }

        if (foundChest != null)
        {
            Debug.Log($"Enviando carta a cofre");
            foundChest.TryAcceptCard(this);
        }
        else
        {
            Debug.Log("No cay� en ning�n cofre - carta se queda en posici�n actual");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isDragging)
        {
            // Efecto sutil al pasar el mouse
            targetScale = normalScale * 1.05f;

            if (spriteRenderer != null)
            {
                targetColor = normalColor * 1.2f;
            }
            else if (imageComponent != null)
            {
                targetColor = normalColor * 1.2f;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isDragging)
        {
            // Volver a normal
            targetScale = normalScale;
            targetColor = normalColor;
        }
    }

    public void Respawn()
    {
        if (spawnArea == null)
        {
            Debug.LogWarning("SpawnArea no asignada");
            return;
        }

        // Detener cualquier drag en progreso
        isDragging = false;

        // Restablecer f�sica
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.isKinematic = false;
            rb.gravityScale = 1f;
        }

        // Restablecer escala y color
        targetScale = normalScale;
        transform.localScale = normalScale;

        if (spriteRenderer != null)
        {
            targetColor = normalColor;
            spriteRenderer.color = normalColor;
        }
        else if (imageComponent != null)
        {
            targetColor = normalColor;
            imageComponent.color = normalColor;
        }

        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
        }

        // Volver a la posici�n original en jerarqu�a
        if (originalParent != null)
        {
            transform.SetParent(originalParent);
            transform.SetSiblingIndex(originalSiblingIndex);
        }

        Bounds bounds = spawnArea.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = bounds.max.y;

        // Si el Canvas es World Space, usar posici�n directamente
        if (parentCanvas != null && parentCanvas.renderMode == RenderMode.WorldSpace)
        {
            transform.position = new Vector2(x, y);
        }
        else
        {
            // Para Screen Space, necesitamos convertir
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(parentCanvas.worldCamera, new Vector2(x, y));
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentCanvas.transform as RectTransform,
                screenPos,
                parentCanvas.worldCamera,
                out localPoint
            );
            rectTransform.localPosition = localPoint;
        }
    }
}