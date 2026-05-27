using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("References")]
    public Animator animator;

    [Header("Animation Settings")]
    public string idleAnimationName = "Idle";
    public string jumpAnimationName = "Jump";
    public string fallAnimationName = "Fall";

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    // Métodos públicos para controlar animaciones
    public void PlayIdle()
    {
        if (animator)
        {
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFalling", false);
        }
    }

    public void PlayJump()
    {
        if (animator)
        {
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsFalling", false);
        }
    }

    public void PlayFall()
    {
        if (animator)
        {
            animator.SetBool("IsFalling", true);
            animator.SetBool("IsJumping", false);
        }
    }

    public void StopAllAnimations()
    {
        if (animator)
        {
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFalling", false);
        }
    }

    // Para debug
    void Update()
    {
        // Opcional: Teclas para probar animaciones en modo juego
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha1))
            PlayIdle();
        if (Input.GetKeyDown(KeyCode.Alpha2))
            PlayJump();
        if (Input.GetKeyDown(KeyCode.Alpha3))
            PlayFall();
#endif
    }
}