using UnityEngine;

public class CardDrag : MonoBehaviour
{
    public CardType cardType;

    private Camera mainCamera;
    private bool isDragging;

    private Vector3 spawnPosition;
    private Vector3 lastValidPosition;

    void Start()
    {
        mainCamera = Camera.main;

        // Posición inicial al aparecer
        spawnPosition = transform.position;
        lastValidPosition = transform.position;
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

        // Si NO cayó en un cofre:
        lastValidPosition = transform.position;
    }

    public void ResetPosition()
    {
        // Solo se usa cuando el cofre es incorrecto
        transform.position = lastValidPosition;
    }

    public void ResetToSpawn()
    {
        transform.position = spawnPosition;
        lastValidPosition = spawnPosition;
    }
}
