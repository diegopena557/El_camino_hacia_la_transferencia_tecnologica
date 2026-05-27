using UnityEngine;

// Create assets: right-click in Project -> Create -> MiniGame -> Stop Question
// Create one asset per question. Assign multiple to a segment in StopQuestionTrigger.
//
// Each asset represents one question that can pause the character mid-segment.
// Multiple assets per segment allow random variation - one is picked at runtime.
[CreateAssetMenu(menuName = "MiniGame/Stop Question", fileName = "StopQuestion")]
public class StopQuestionData : ScriptableObject
{
    [Header("Identificacion")]
    public string questionLabel = "Pregunta";

    [Header("Personaje o imagen de la interfaz")]
    [Tooltip("Sprite que aparece en la UI al mostrar esta pregunta (personaje, ilustracion, etc.)")]
    public Sprite characterSprite;

    [Header("Pregunta")]
    [TextArea(2, 5)]
    public string questionText = "Escribe la pregunta aqui";

    [Header("Opciones (exactamente 2)")]
    public StopOption optionA;
    public StopOption optionB;

    [System.Serializable]
    public struct StopOption
    {
        public string buttonLabel;

        [Tooltip("Si es true, esta opcion es correcta")]
        public bool isCorrect;

        [TextArea(1, 3)]
        public string feedbackText;
    }
}