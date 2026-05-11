using UnityEngine;

// Optional: attach to answer buttons if you prefer to wire them manually
// instead of using the dynamic prefab spawning in QuestionManager.
// Set optionIndex to match the position of the option in SegmentData.options.
public class AnswerButton : MonoBehaviour
{
    public int optionIndex;

    public void OnClick()
    {
        FindObjectOfType<QuestionManager>().Answer(optionIndex);
    }
}