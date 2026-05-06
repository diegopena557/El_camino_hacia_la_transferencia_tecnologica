using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    public GameObject questionUI;
    public Image timeBar;

    public float maxTime = 5f;

    [Header("Colores de timing")]
    public Color earlyColor = Color.red;
    public Color perfectColor = Color.green;
    public Color lateColor = Color.red;

    private float currentTime;
    private bool isAnswering = false;

    private WaypointMover mover;

    void Start()
    {
        mover = FindObjectOfType<WaypointMover>();
        mover.OnReachPoint += ShowQuestion;
    }

    void Update()
    {
        if (!isAnswering) return;

        currentTime += Time.deltaTime;

        float normalized = currentTime / maxTime;

        // Actualizar barra
        timeBar.fillAmount = normalized;

        // Cambiar color según zona
        float perfectStart = maxTime * 0.4f;
        float perfectEnd = maxTime * 0.6f;

        if (currentTime < perfectStart)
        {
            timeBar.color = earlyColor;
        }
        else if (currentTime <= perfectEnd)
        {
            timeBar.color = perfectColor;
        }
        else
        {
            timeBar.color = lateColor;
        }

        // Tiempo agotado
        if (currentTime >= maxTime)
        {
            Answer(-1);
        }
    }

    void ShowQuestion()
    {
        questionUI.SetActive(true);
        currentTime = 0f;
        isAnswering = true;

        // Reset visual
        timeBar.fillAmount = 0f;
        timeBar.color = earlyColor;
    }

    public void Answer(int option)
    {
        if (!isAnswering) return;

        isAnswering = false;
        questionUI.SetActive(false);

        float t = currentTime;

        float perfectStart = maxTime * 0.4f;
        float perfectEnd = maxTime * 0.6f;

        // Evaluación de timing
        if (t >= perfectStart && t <= perfectEnd)
        {
            mover.ModifySpeed(1.4f);
            Debug.Log("Perfect timing");
        }
        else
        {
            mover.ModifySpeed(0.7f);
            Debug.Log("Mal timing");
        }

        mover.ContinueToNextPoint();
    }
}