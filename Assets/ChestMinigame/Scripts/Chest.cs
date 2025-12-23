using UnityEngine;

public class Chest : MonoBehaviour
{
    [Header("Chest Settings")]
    public CardType acceptedType;

    [Header("Feedback")]
    public ChestFeedback feedback;

    [Header("Justification")]
    public JustificationPanel justificationPanel;

    [TextArea(3, 6)]
    public string correctJustification;

    public void TryAcceptCard(CardDrag card)
    {
        if (card.cardType == acceptedType)
        {
            feedback?.PlayCorrectFeedback();

            //  AQUÍ se muestra la justificación
            justificationPanel.Show(correctJustification);

            Destroy(card.gameObject);
        }
        else
        {
            feedback?.PlayWrongFeedback();
            card.Respawn();
        }
    }
}
