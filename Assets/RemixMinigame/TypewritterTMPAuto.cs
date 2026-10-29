using UnityEngine;
using TMPro;
using System.Collections;

public class TypewriterTMPAuto : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    public float delay = 0.05f;

    string fullText;

    // Es true cuando termino de escribir todo el texto
    public bool IsFinished { get; private set; }

    void Awake()
    {
        if (textUI == null)
            textUI = GetComponent<TextMeshProUGUI>();

        fullText = textUI.text;
        textUI.text = "";

        StartTyping();
    }

    public void StartTyping()
    {
        StopAllCoroutines();
        IsFinished = false;
        textUI.text = "";
        StartCoroutine(TypeText());
    }

    public void RestartText()
    {
        StopAllCoroutines();
        IsFinished = false;
        textUI.text = "";
    }

    IEnumerator TypeText()
    {
        foreach (char c in fullText)
        {
            textUI.text += c;
            yield return new WaitForSeconds(delay);
        }

        IsFinished = true;
    }
}