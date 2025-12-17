using UnityEngine;
using System.Collections;

public class RespawnFromTop : MonoBehaviour
{
    [Header("Respawn Settings")]
    public float respawnDelay = 0.5f;

    [Header("Spawn Area")]
    public Collider2D spawnArea; // Collider que define el rango de aparición

    private SpriteRenderer spriteRenderer;
    private Collider2D objectCollider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        objectCollider = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // SOLO cuando toca el suelo
        if (!collision.CompareTag("Floor")) return;

        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        // Desaparece
        spriteRenderer.enabled = false;
        objectCollider.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        // Posición aleatoria dentro del collider
        Vector2 randomPosition = GetRandomPointInCollider(spawnArea);
        transform.position = randomPosition;

        // Vuelve a aparecer
        spriteRenderer.enabled = true;
        objectCollider.enabled = true;
    }

    Vector2 GetRandomPointInCollider(Collider2D col)
    {
        Bounds bounds = col.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector2(x, y);
    }
}