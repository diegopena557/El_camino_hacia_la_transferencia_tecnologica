using UnityEngine;

public class PlayerLookAtMouse : MonoBehaviour
{
    void LateUpdate()
    {
        // BLOQUEAR ROTACIÓN CUANDO NO DEBE
        if (GlassBridgeManager.Instance != null)
        {
            if (GlassBridgeManager.Instance.IsMoving() ||
                GlassBridgeManager.Instance.IsGameOver())
                return;
        }

        RotateTowardsMouse();
    }

    void RotateTowardsMouse()
    {
        // Obtener posición del mouse en mundo
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        // Dirección hacia el mouse
        Vector2 direction = mouseWorldPos - transform.position;

        // Calcular ángulo
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Aplicar rotación con offset (-90)
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }
}