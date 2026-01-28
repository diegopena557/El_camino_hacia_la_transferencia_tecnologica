using UnityEngine;
using UnityEngine.EventSystems;

public class CardDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public CardType cardType;
    public Collider2D spawnArea;

    [Header("Información de la Carta")]
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

    void Start()
    {
        mainCamera = Camera.main;
        rectTransform = GetComponent<RectTransform>();

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
        // Suavizar la transición de escala y brillo
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
    // IMPLEMENTACIÓN DE INTERFACES UI
    // ========================================

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        dragStartPosition = transform.position;

        // Hacer la carta más grande y brillante
        targetScale = normalScale * selectedScale;

        if (spriteRenderer != null)
        {
            targetColor = normalColor * selectedBrightness;
        }
        else if (imageComponent != null)
        {
            targetColor = normalColor * selectedBrightness;
        }

        // Mover al final de la jerarquía para renderizar encima
        transform.SetAsLastSibling();

        // Permitir que raycast pase a través durante drag
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false;
        }

        Debug.Log($"Comenzó a arrastrar: {gameObject.name} desde posición {dragStartPosition}");
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        if (rectTransform != null && parentCanvas != null)
        {
            // Para World Space Canvas
            if (parentCanvas.renderMode == RenderMode.WorldSpace)
            {
                // Convertir posición del mouse a posición del mundo
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
            // Fallback: usar posición de mouse directamente
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

        // Volver al tamaño y brillo normal
        targetScale = normalScale;
        targetColor = normalColor;

        // Restaurar raycast
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
        }

        Debug.Log($"=== OnEndDrag: {gameObject.name} ===");
        Debug.Log($"Posición inicial: {dragStartPosition}");
        Debug.Log($"Posición final: {transform.position}");

        // Calcular distancia movida
        float distanceMoved = Vector3.Distance(dragStartPosition, transform.position);
        Debug.Log($"Distancia movida: {distanceMoved}");

        // Si NO se movió (click sin arrastrar), no hacer nada
        if (distanceMoved < 0.1f)
        {
            Debug.Log("No se movió - ignorando");
            return;
        }

        // Verificar si cayó en un cofre usando Raycast desde la posición de la carta
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
                Debug.Log($" Encontró cofre válido: {chest.gameObject.name}");
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
            Debug.Log("No cayó en ningún cofre - carta se queda en posición actual");
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

        // Volver a la posición original en jerarquía
        if (originalParent != null)
        {
            transform.SetParent(originalParent);
            transform.SetSiblingIndex(originalSiblingIndex);
        }

        Bounds bounds = spawnArea.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = bounds.max.y;

        // Si el Canvas es World Space, usar posición directamente
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