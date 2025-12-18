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
            Destroy(card.gameObject);
        }
        else
        {
            feedback?.PlayWrongFeedback();
            card.Respawn();
        }
    }
}
