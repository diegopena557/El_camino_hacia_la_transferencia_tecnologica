using UnityEngine;

// SETUP:
// 1. Crea un GameObject por segmento, ej: "Zone_0", "Zone_1"...
// 2. Agrega un BoxCollider2D y marca "Is Trigger".
// 3. Ajusta el tama~no para cubrir la zona perfecta del camino.
// 4. Asigna el segmentIndex correcto (0 = primer tramo).
// 5. El personaje necesita el tag "Player".
public class TimingZone : MonoBehaviour
{
    [Tooltip("Indice del segmento al que pertenece esta zona (0 = primer tramo)")]
    public int segmentIndex;

    public static System.Action<int> OnPlayerEnter;
    public static System.Action<int> OnPlayerExit;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            OnPlayerEnter?.Invoke(segmentIndex);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            OnPlayerExit?.Invoke(segmentIndex);
    }
}