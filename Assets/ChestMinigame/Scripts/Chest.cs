using UnityEngine;

public class Chest : MonoBehaviour
{
    public CardType acceptedType;
    public ChestFeedback feedback;

    public void TryAcceptCard(CardDrag card)
    {
        if (card.cardType == acceptedType)
        {
            //  ACIERTO
            feedback?.PlayCorrectFeedback();

            // Mostrar solo retroalimentación de texto corta
            if (FeedbackTextManager.Instance != null)
                FeedbackTextManager.Instance.ShowFeedback(card.cardType);

            // Notificar al spawner que una carta fue completada
            GameObject cardObject = card.gameObject;

            if (CardSpawner.Instance != null)
                CardSpawner.Instance.OnCardCompleted(cardObject);

            Destroy(cardObject);
            GameStatsManager.Instance.AddCorrect();
        }
        else
        {
            //  ERROR
            feedback?.PlayWrongFeedback();

            // Mostrar información detallada de la carta para educar al usuario
            if (CardInfoDisplay.Instance != null && card.cardInfo != null)
                CardInfoDisplay.Instance.ShowCardInfoOnError(card.cardInfo);

            card.Respawn();
            GameStatsManager.Instance.AddError();
        }
    }
}