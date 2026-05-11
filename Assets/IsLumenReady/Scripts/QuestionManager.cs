using UnityEngine;
using UnityEngine.UI;
using TMPro;

// SETUP UI:
//   questionUI       -> root panel of the question UI (disabled by default)
//   questionText     -> TMP_Text showing the question
//   answerButtons[5] -> assign the 5 Button references directly in the Inspector
//                       each Button must have a TMP_Text child for the label
//   feedbackText     -> TMP_Text for timing/result feedback
//   feedbackDuration -> seconds to show feedback before hiding
//
// HOW IT WORKS:
//   - SegmentData.options maps 1-to-1 to answerButtons[0..4].
//   - If a SegmentData has fewer than 5 options the extra buttons are hidden.
//   - Any option flagged isCorrect = true counts as a valid answer.
//   - Pressing an incorrect button registers as a wrong answer.
//   - Timing is tracked via TimingZone colliders in the scene.
//   - After answering, speed is modified and the mover continues.
public class QuestionManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject questionUI;
    public TMP_Text questionText;
    [Tooltip("Assign the 5 answer Button GameObjects here in order")]
    public Button[] answerButtons = new Button[5];
    public TMP_Text feedbackText;
    public float feedbackDuration = 1.8f;

    [Header("Feedback colors")]
    public Color colorPerfect = Color.green;
    public Color colorEarly = Color.yellow;
    public Color colorLate = new Color(1f, 0.5f, 0f);   // orange
    public Color colorWrong = Color.red;

    // -- Timing state ----------------------------------------------------------
    private enum TimingState { Early, Perfect, Late }
    private TimingState timing = TimingState.Early;

    private bool questionActive = false;

    private WaypointMover mover;

    // -- Unity -----------------------------------------------------------------

    void Awake()
    {
        mover = FindObjectOfType<WaypointMover>();
        mover.OnReachStop += HandleReachStop;

        TimingZone.OnPlayerEnter += HandleZoneEnter;
        TimingZone.OnPlayerExit += HandleZoneExit;

        HideQuestion();
    }

    void OnDestroy()
    {
        if (mover != null) mover.OnReachStop -= HandleReachStop;
        TimingZone.OnPlayerEnter -= HandleZoneEnter;
        TimingZone.OnPlayerExit -= HandleZoneExit;
    }

    // -- Mover events ----------------------------------------------------------

    void HandleReachStop(int stopIndex)
    {
        // Character arrived without the player answering -> auto-fail
        if (questionActive)
        {
            ResolveAnswer(correct: false, autoFail: true);
            return;
        }

        // Last stop reached: journey over
        if (stopIndex >= mover.stopPoints.Length - 1)
        {
            Debug.Log("Journey complete.");
            return;
        }

        // Start moving to next stop and show the question for that segment
        mover.AdvanceToNextStop();
        ShowQuestion();
    }

    // -- Timing zone events ----------------------------------------------------

    void HandleZoneEnter(int zoneSegment)
    {
        if (zoneSegment == mover.CurrentSegmentIndex)
            timing = TimingState.Perfect;
    }

    void HandleZoneExit(int zoneSegment)
    {
        if (zoneSegment == mover.CurrentSegmentIndex && timing == TimingState.Perfect)
            timing = TimingState.Late;
    }

    // -- Show question ---------------------------------------------------------

    void ShowQuestion()
    {
        SegmentData data = mover.CurrentSegmentData;
        if (data == null) return;

        timing = TimingState.Early;
        questionActive = true;

        questionText.text = data.question;

        // Wire each button to its option; hide buttons beyond the option count
        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (answerButtons[i] == null) continue;

            bool hasOption = i < data.options.Length;
            answerButtons[i].gameObject.SetActive(hasOption);

            if (!hasOption) continue;

            // Set label text
            TMP_Text lbl = answerButtons[i].GetComponentInChildren<TMP_Text>();
            if (lbl != null) lbl.text = data.options[i].text;

            // Remove old listeners then add the new one
            answerButtons[i].onClick.RemoveAllListeners();
            bool isCorrect = data.options[i].isCorrect;
            answerButtons[i].onClick.AddListener(() => OnButtonClicked(isCorrect));
        }

        questionUI.SetActive(true);
        SetFeedbackVisible(false);
    }

    void HideQuestion()
    {
        questionUI.SetActive(false);
        SetFeedbackVisible(false);

        // Re-show all buttons so they are ready for the next question
        foreach (Button btn in answerButtons)
            if (btn != null) btn.gameObject.SetActive(true);
    }

    // -- Answer logic ----------------------------------------------------------

    void OnButtonClicked(bool isCorrect)
    {
        if (!questionActive) return;
        ResolveAnswer(correct: isCorrect, autoFail: false);
    }

    // Public overload kept for AnswerButton compatibility
    public void Answer(int optionIndex)
    {
        if (!questionActive) return;
        SegmentData data = mover.CurrentSegmentData;
        bool isCorrect = data != null && optionIndex < data.options.Length
                           && data.options[optionIndex].isCorrect;
        ResolveAnswer(correct: isCorrect, autoFail: false);
    }

    void ResolveAnswer(bool correct, bool autoFail)
    {
        questionActive = false;
        HideQuestion();

        SegmentData data = mover.CurrentSegmentData;

        float speedMult;
        string msg;
        Color col;

        if (autoFail)
        {
            speedMult = 0.55f;
            msg = data != null ? data.feedbackNoAnswer : "Sin respuesta";
            col = colorWrong;
        }
        else if (!correct)
        {
            speedMult = 0.65f;
            msg = data != null ? data.feedbackWrong : "Incorrecto";
            col = colorWrong;
        }
        else
        {
            switch (timing)
            {
                case TimingState.Perfect:
                    speedMult = 1.4f;
                    msg = data != null ? data.feedbackCorrectPerfect : "Perfecto!";
                    col = colorPerfect;
                    break;
                case TimingState.Late:
                    speedMult = 0.75f;
                    msg = data != null ? data.feedbackCorrectLate : "Correcto, tarde";
                    col = colorLate;
                    break;
                default: // Early
                    speedMult = 0.85f;
                    msg = data != null ? data.feedbackCorrectEarly : "Correcto, temprano";
                    col = colorEarly;
                    break;
            }
        }

        mover.ModifySpeed(speedMult);
        ShowFeedback(msg, col);
    }

    // -- Feedback --------------------------------------------------------------

    void ShowFeedback(string msg, Color col)
    {
        if (feedbackText == null) return;
        feedbackText.text = msg;
        feedbackText.color = col;
        SetFeedbackVisible(true);
        CancelInvoke(nameof(HideFeedback));
        Invoke(nameof(HideFeedback), feedbackDuration);
    }

    void HideFeedback() { SetFeedbackVisible(false); }

    void SetFeedbackVisible(bool value)
    {
        if (feedbackText != null) feedbackText.gameObject.SetActive(value);
    }
}