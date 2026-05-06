using UnityEngine;

public class AnswerButton : MonoBehaviour
{
    public int optionIndex;

    public void OnClick()
    {
        FindObjectOfType<QuestionManager>().Answer(optionIndex);
    }
}