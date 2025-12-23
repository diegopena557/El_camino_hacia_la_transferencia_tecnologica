using UnityEngine;

public class CardDrag : MonoBehaviour
{
    public CardType cardType;
    public Collider2D spawnArea;

    private Camera mainCamera;
    private bool isDragging;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void OnMouseDown()
    {
        isDragging = true;
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

    public void Respawn()
    {
        if (spawnArea == null)
        {
            Debug.LogWarning("SpawnArea no asignada");
            return;
        }

        Bounds bounds = spawnArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = bounds.max.y;

        transform.position = new Vector2(x, y);
    }
}

