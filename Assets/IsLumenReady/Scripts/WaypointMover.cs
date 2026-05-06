using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 3f;

    private int currentIndex = 0;
    private bool isMoving = true;

    private float baseSpeed;

    private Animator animator;

    public System.Action OnReachPoint;

    void Start()
    {
        baseSpeed = speed;
        animator = GetComponent<Animator>();

        SetWalking(true); // empieza caminando
    }

    void Update()
    {
        if (!isMoving || waypoints.Length == 0) return;

        Transform target = waypoints[currentIndex];

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            isMoving = false;

            SetWalking(false); // deja de caminar

            OnReachPoint?.Invoke();
        }
    }

    public void ContinueToNextPoint()
    {
        currentIndex++;

        if (currentIndex >= waypoints.Length)
        {
            Debug.Log("Recorrido terminado");
            SetWalking(false);
            return;
        }

        isMoving = true;

        SetWalking(true); // vuelve a caminar
    }

    public void ModifySpeed(float multiplier)
    {
        speed = baseSpeed * multiplier;
    }

    void SetWalking(bool value)
    {
        if (animator != null)
        {
            animator.SetBool("isWalking", value);
        }
    }
}