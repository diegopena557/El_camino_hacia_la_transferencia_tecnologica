// AnswerButton.cs
//
// IMPORTANTE: NO conectes este script a los botones en el Inspector si
// estas usando el sistema de listeners dinamicos de QuestionManager.
// QuestionManager asigna sus propios listeners con el indice correcto.
// Tener ambos activos causaria que siempre se dispare el optionIndex fijo
// de este script, ignorando cual boton se presiono realmente.
//
// Este archivo se conserva solo por compatibilidad. Puedes borrarlo si
// no lo usas en ningun otro lugar del proyecto.

using UnityEngine;

public class AnswerButton : MonoBehaviour
{
    [Tooltip("NO usar si QuestionManager gestiona los botones dinamicamente")]
    public int optionIndex;

    public void OnClick()
    {
        FindObjectOfType<QuestionManager>().Answer(optionIndex);
    }
}