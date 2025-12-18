using UnityEngine;

public class Chest : MonoBehaviour
{
    public CardType acceptedType;

    public void TryAcceptCard(CardDrag card)
    {
        if (card.cardType == acceptedType)
        {
            CorrectCard(card);
        }
        else
        {
            
        }
    }

    void CorrectCard(CardDrag card)
    {
        Debug.Log("Tarjeta correcta");
        Destroy(card.gameObject);
    }


}
