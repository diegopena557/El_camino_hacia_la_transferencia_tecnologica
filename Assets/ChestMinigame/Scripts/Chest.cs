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

            // Mostrar retroalimentación de texto corta
            if (FeedbackTextManager.Instance != null)
                FeedbackTextManager.Instance.ShowFeedback(card.cardType);

            // Mostrar información detallada de la carta
            if (CardInfoDisplay.Instance != null && card.cardInfo != null)
                CardInfoDisplay.Instance.ShowCardInfo(card.cardInfo);

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