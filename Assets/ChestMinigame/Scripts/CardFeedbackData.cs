using UnityEngine;

[System.Serializable]
public class CardFeedbackData
{
    [Header("Información de la Carta")]
    [TextArea(2, 4)]
    public string cardTitle;

    [TextArea(3, 6)]
    public string feedbackMessage;

    [Header("Mensaje Breve (Opcional)")]
    [Tooltip("Mensaje corto que aparece primero")]
    public string shortMessage = "¡Correcto!";
}