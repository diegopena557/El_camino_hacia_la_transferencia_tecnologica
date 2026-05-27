using UnityEngine;
using TMPro;
using System.Collections;

public class TypewriterTMP : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    public float delay;

    string fullText;

    void Awake()
    {
        fullText = textUI.text;
        textUI.text = "";
    }

    public void StartTyping()
    {
        StartCoroutine(TypeText());
    }

    public void RestartText()
    {
        fullText = textUI.text;
        textUI.text = "";
    }

    IEnumerator TypeText()
    {
        foreach (char c in fullText)
        {
            textUI.text += c;
            yield return new WaitForSeconds(delay);
        }
    }
}