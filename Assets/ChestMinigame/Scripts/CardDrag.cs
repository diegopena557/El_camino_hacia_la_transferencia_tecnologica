using UnityEngine;

public class CardDrag : MonoBehaviour
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
    private bool isDragging;
    private Vector3 normalScale;
    private SpriteRenderer spriteRenderer;
    private Color normalColor;
    private Vector3 targetScale;
    private Color targetColor;

    void Start()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();

        normalScale = transform.localScale;
        targetScale = normalScale;

        if (spriteRenderer != null)
        {
            normalColor = spriteRenderer.color;
            targetColor = normalColor;
        }
    }

    void Update()
    {
        // Suavizar la transición de escala y brillo
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * transitionSpeed);

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, targetColor, Time.deltaTime * transitionSpeed);
        }
    }

    void OnMouseDown()
    {
        isDragging = true;

        // Hacer la carta más grande y brillante
        targetScale = normalScale * selectedScale;

        if (spriteRenderer != null)
        {
            targetColor = normalColor * selectedBrightness;
        }
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        transform.position = mainCamera.ScreenToWorldPoint(mousePos);
    }

    void OnMouseUp()
    {
        isDragging = false;

        // Volver al tamaño y brillo normal
        targetScale = normalScale;
        targetColor = normalColor;

        Collider2D hit = Physics2D.OverlapPoint(transform.position);

        if (hit != null)
        {
            Chest chest = hit.GetComponent<Chest>();
            if (chest != null)
            {
                chest.TryAcceptCard(this);
                return;
            }
        }
        // Si no cayó en un cofre, se queda donde se soltó
    }

    void OnMouseEnter()
    {
        if (!isDragging)
        {
            // Efecto sutil al pasar el mouse
            targetScale = normalScale * 1.05f;

            if (spriteRenderer != null)
            {
                targetColor = normalColor * 1.2f;
            }
        }
    }

    void OnMouseExit()
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

        // Restablecer escala y color
        targetScale = normalScale;
        transform.localScale = normalScale;

        if (spriteRenderer != null)
        {
            targetColor = normalColor;
            spriteRenderer.color = normalColor;
        }

        Bounds bounds = spawnArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = bounds.max.y;

        transform.position = new Vector2(x, y);
    }
}