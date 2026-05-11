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
    [Tooltip("Agrega todas las opciones. Marca cuales son correctas con el bool.")]
    public AnswerOption[] options;

    [Header("Feedback por timing")]
    [TextArea(1, 2)] public string feedbackCorrectPerfect = "Perfecto!";
    [TextArea(1, 2)] public string feedbackCorrectEarly = "Correcto, pero muy temprano";
    [TextArea(1, 2)] public string feedbackCorrectLate = "Correcto, pero muy tarde";
    [TextArea(1, 2)] public string feedbackWrong = "Respuesta incorrecta";
    [TextArea(1, 2)] public string feedbackNoAnswer = "Sin respuesta";

    [System.Serializable]
    public struct AnswerOption
    {
        [TextArea(1, 2)]
        public string text;
        public bool isCorrect;
    }
}