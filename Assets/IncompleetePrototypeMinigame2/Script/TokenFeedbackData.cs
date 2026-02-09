using UnityEngine;

[CreateAssetMenu(fileName = "TokenFeedback", menuName = "Game/Token Feedback Data")]
public class TokenFeedbackData : ScriptableObject
{
    [Header("Mensajes de Feedback")]
    [TextArea(3, 5)]
    public string correctFeedback = "¡Correcto!\nEsta ficha pertenece a esta categoría.";

    [TextArea(3, 5)]
    public string wrongFeedback = "Esta ficha no pertenece aquí.\nIntenta con otra categoría.";

    [Header("Información Adicional (Opcional)")]
    [TextArea(2, 4)]
    public string hintText = "";
}