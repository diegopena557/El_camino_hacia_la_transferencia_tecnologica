using UnityEngine;

public class GlassBridgePlayer : MonoBehaviour
{
    [Header("Visual")]
    public SpriteRenderer spriteRenderer;
    public Sprite idleSprite;
    public Sprite walkSprite;

    [Header("Animation")]
    public float animationSpeed = 0.2f;

    private bool isWalking = false;
    private float animTimer = 0f;

    void Update()
    {
        // Detectar si se está moviendo
        Vector3 velocity = GetComponent<Rigidbody2D>() ?
            GetComponent<Rigidbody2D>().linearVelocity : Vector3.zero;

        isWalking = velocity.magnitude > 0.1f;

        if (isWalking && walkSprite && idleSprite)
        {
            animTimer += Time.deltaTime;

            if (animTimer >= animationSpeed)
            {
                animTimer = 0f;
                spriteRenderer.sprite = spriteRenderer.sprite == idleSprite ? walkSprite : idleSprite;
            }
        }
        else if (idleSprite)
        {
            spriteRenderer.sprite = idleSprite;
        }
    }
}
