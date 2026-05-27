using UnityEngine;

// SETUP EN ESCENA:
// 1. Crea un GameObject por segmento, ej: "StopTrigger_0", "StopTrigger_1"...
// 2. Agrega este componente y asigna segmentIndex.
// 3. Arrastra uno o varios StopQuestionData al array questions.
//    En runtime se escoge uno al azar.
// 4. Ajusta randomWindow: el trigger se activa en un momento aleatorio
//    dentro de ese rango del progreso del segmento [0..1].
//    Ejemplo: min=0.2 max=0.8 significa que puede ocurrir entre
//    el 20% y el 80% del recorrido del tramo.
//
// StopQuestionManager busca todos los StopQuestionTrigger de la escena
// en Awake y se suscribe automaticamente al WaypointMover.
public class StopQuestionTrigger : MonoBehaviour
{
    [Header("Segmento al que pertenece")]
    [Tooltip("0 = tramo entre stopPoints[0] y stopPoints[1], etc.")]
    public int segmentIndex = 0;

    [Header("Preguntas disponibles para este segmento")]
    [Tooltip("Se escoge una al azar cada vez que se activa el trigger")]
    public StopQuestionData[] questions;

    [Header("Ventana de activacion (progreso del segmento 0=inicio 1=fin)")]
    [Range(0f, 1f)] public float windowMin = 0.2f;
    [Range(0f, 1f)] public float windowMax = 0.8f;

    // Set by StopQuestionManager when the segment starts
    [HideInInspector] public float triggerAtProgress = -1f;
    [HideInInspector] public bool alreadyFired = false;

    // Picks a random trigger point within the window and resets state
    public void Prepare()
    {
        triggerAtProgress = Random.Range(windowMin, windowMax);
        alreadyFired = false;
    }

    // Returns a random question from the list, or null if empty
    public StopQuestionData PickQuestion()
    {
        if (questions == null || questions.Length == 0) return null;
        return questions[Random.Range(0, questions.Length)];
    }
}