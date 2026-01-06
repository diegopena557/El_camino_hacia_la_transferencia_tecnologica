using UnityEngine;

public class Chest : MonoBehaviour
{
    public CardType acceptedType;
    public ChestFeedback feedback;

    public void TryAcceptCard(CardDrag card)
    {
        if (card.cardType == acceptedType)
        {
            feedback?.PlayCorrectFeedback();

            // NUEVA LÍNEA: Mostrar retroalimentación de texto
            if (FeedbackTextManager.Instance != null)
                FeedbackTextManager.Instance.ShowFeedback(card.cardType);

            Destroy(card.gameObject);
            GameStatsManager.Instance.AddCorrect();
        }
        else
        {
            feedback?.PlayWrongFeedback();
            card.Respawn();
            GameStatsManager.Instance.AddError();
        }
    }
}