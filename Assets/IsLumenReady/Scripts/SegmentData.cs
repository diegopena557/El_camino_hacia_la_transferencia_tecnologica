using UnityEngine;

// Create assets: right-click in Project -> Create -> MiniGame -> Segment Data
[CreateAssetMenu(menuName = "MiniGame/Segment Data", fileName = "SegmentData_0")]
public class SegmentData : ScriptableObject
{
    [Header("Identificacion del segmento")]
    [Tooltip("Nombre descriptivo, solo para identificar el asset en el proyecto")]
    public string segmentLabel = "Segmento 0";

    [Header("Pregunta")]
    [TextArea(2, 5)]
    public string question = "Escribe la pregunta aqui";

    [Header("Opciones de respuesta")]
    [Tooltip("Agrega hasta 5 opciones. Marca cuales son correctas con isCorrect.\n" +
             "feedbackText se muestra al jugador cuando presiona ese boton,\n" +
             "independientemente del timing (correcto/incorrecto).")]
    public AnswerOption[] options;

    [Header("Feedback por timing (se combina con el feedback de la opcion)")]
    [TextArea(1, 2)] public string feedbackTimingPerfect = "en el momento justo!";
    [TextArea(1, 2)] public string feedbackTimingEarly = "muy temprano.";
    [TextArea(1, 2)] public string feedbackTimingLate = "muy tarde.";
    [TextArea(1, 2)] public string feedbackNoAnswer = "Sin respuesta.";

    [System.Serializable]
    public struct AnswerOption
    {
        [TextArea(1, 2)]
        public string text;

        [Tooltip("Si es true, esta opcion se considera una respuesta valida")]
        public bool isCorrect;

        [Tooltip("Texto que aparece cuando el jugador presiona este boton.\n" +
                 "El feedback de timing se agrega automaticamente a continuacion.")]
        [TextArea(1, 3)]
        public string feedbackText;
    }
}