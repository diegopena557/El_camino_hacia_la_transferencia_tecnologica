using UnityEngine;

public class CardCompletionNotifier : MonoBehaviour
{
    void OnDestroy()
    {
        if (CardSpawnerRemix.Instance != null)
        {
            CardSpawnerRemix.Instance.OnCardCompleted(gameObject);
        }
    }
}