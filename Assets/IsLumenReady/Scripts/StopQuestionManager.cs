using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StopQuestionManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject stopQuestionUI;

    private CanvasGroup stopCanvasGroup;
    private CanvasGroup cgButtonA;
    private CanvasGroup cgButtonB;

    public Image characterImage;
    public TMP_Text questionText;

    public Button buttonA;
    public Button buttonB;

    public TMP_Text feedbackText;

    [Header("Duracion del feedback")]
    [Tooltip("Segundos que se muestra el feedback antes de reanudar")]
    public float feedbackDuration = 2f;

    [Header("Feedback colors")]
    public Color colorCorrect = Color.green;
    public Color colorIncorrect = Color.red;

    // -- Runtime ---------------------------------------------------------------

    private WaypointMover mover;

    private StopQuestionTrigger[] allTriggers;

    private StopQuestionTrigger activeTrigger;

    private bool waitingForAnswer = false;

    // -- Unity -----------------------------------------------------------------

    void Awake()
    {
        mover = FindObjectOfType<WaypointMover>();

        allTriggers = FindObjectsOfType<StopQuestionTrigger>();

        // CanvasGroup principal
        stopCanvasGroup = stopQuestionUI.GetComponent<CanvasGroup>();

        if (stopCanvasGroup == null)
            stopCanvasGroup = stopQuestionUI.AddComponent<CanvasGroup>();

        // CanvasGroup boton A
        if (buttonA != null)
        {
            cgButtonA = buttonA.GetComponent<CanvasGroup>();

            if (cgButtonA == null)
                cgButtonA = buttonA.gameObject.AddComponent<CanvasGroup>();
        }

        // CanvasGroup boton B
        if (buttonB != null)
        {
            cgButtonB = buttonB.GetComponent<CanvasGroup>();

            if (cgButtonB == null)
                cgButtonB = buttonB.gameObject.AddComponent<CanvasGroup>();
        }

        stopQuestionUI.SetActive(true);

        HideAll();
    }

    void Update()
    {
        if (waitingForAnswer) return;

        if (activeTrigger == null) return;

        if (activeTrigger.alreadyFired) return;

        if (mover.SegmentProgress >= activeTrigger.triggerAtProgress)
        {
            activeTrigger.alreadyFired = true;

            FireQuestion();
        }
    }

    // -- Segment management ----------------------------------------------------

    public void NotifySegmentStarted(int segmentIndex)
    {
        activeTrigger = FindTrigger(segmentIndex);

        if (activeTrigger != null)
            activeTrigger.Prepare();
    }

    StopQuestionTrigger FindTrigger(int segmentIndex)
    {
        foreach (StopQuestionTrigger t in allTriggers)
        {
            if (t != null && t.segmentIndex == segmentIndex)
                return t;
        }

        return null;
    }

    // -- Question --------------------------------------------------------------

    void FireQuestion()
    {
        StopQuestionData data = activeTrigger.PickQuestion();

        if (data == null) return;

        waitingForAnswer = true;

        // SOLO pausa el movimiento
        mover.SetPaused(true);

        // Imagen personaje
        if (characterImage != null)
        {
            characterImage.sprite = data.characterSprite;

            characterImage.enabled = data.characterSprite != null;
        }

        // Pregunta
        questionText.text = data.questionText;

        // Opciones
        WireButton(buttonA, cgButtonA, data.optionA);
        WireButton(buttonB, cgButtonB, data.optionB);

        // Feedback oculto inicialmente
        feedbackText.gameObject.SetActive(false);

        ShowPanel();
    }

    void WireButton(Button btn, CanvasGroup cg, StopQuestionData.StopOption option)
    {
        if (btn == null) return;

        // Mostrar boton
        if (cg != null)
        {
            cg.alpha = 1f;
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }

        // Texto
        TMP_Text lbl = btn.GetComponentInChildren<TMP_Text>();

        if (lbl != null)
            lbl.text = option.buttonLabel;

        // Eventos
        btn.onClick.RemoveAllListeners();

        btn.onClick.AddListener(delegate
        {
            OnOptionChosen(option);
        });
    }

    // -- Answer ----------------------------------------------------------------

    void OnOptionChosen(StopQuestionData.StopOption option)
    {
        if (!waitingForAnswer) return;

        waitingForAnswer = false;

        // Ocultar botones
        HideButton(cgButtonA);

        HideButton(cgButtonB);

        // Mostrar feedback
        feedbackText.text = option.feedbackText;

        feedbackText.color = option.isCorrect
            ? colorCorrect
            : colorIncorrect;

        feedbackText.gameObject.SetActive(true);

        StopAllCoroutines();

        StartCoroutine(ResumeAfterDelay(feedbackDuration));
    }

    IEnumerator ResumeAfterDelay(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);

        HideAll();

        // Reanudar movimiento
        mover.SetPaused(false);
    }

    // -- Helpers ---------------------------------------------------------------

    void ShowPanel()
    {
        stopCanvasGroup.alpha = 1f;
        stopCanvasGroup.interactable = true;
        stopCanvasGroup.blocksRaycasts = true;
    }

    void HideAll()
    {
        stopCanvasGroup.alpha = 0f;
        stopCanvasGroup.interactable = false;
        stopCanvasGroup.blocksRaycasts = false;

        feedbackText.gameObject.SetActive(false);
    }

    void HideButton(CanvasGroup cg)
    {
        if (cg == null) return;

        cg.alpha = 0f;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }
}