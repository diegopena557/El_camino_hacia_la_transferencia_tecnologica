using UnityEngine;
using UnityEngine.UI;
using TMPro;

// SETUP UI:
//   questionUI       -> root panel of the question UI (disabled by default)
//   questionText     -> TMP_Text showing the question
//   answerButtons[5] -> the 5 Button references assigned in the Inspector
//                       each Button must have a TMP_Text child for the label
//   feedbackText     -> TMP_Text for timing/result feedback
//   feedbackDuration -> seconds to show feedback before hiding
//
// TIMING:
//   Each segment has an AnswerZone in the scene with two GameObjects (zoneStart,
//   zoneEnd) placed visually on the spline. When the player answers, the current
//   spline-t is compared against the cached t values of that zone.
//   No colliders, no percentage sliders.
//
// FEEDBACK:
//   Each option has its own feedbackText. For correct answers it is combined
//   with the timing suffix. Example: "Correcto! en el momento justo!"
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
    public Color colorLate = new Color(1f, 0.5f, 0f);
    public Color colorWrong = Color.red;

    // -- Runtime state ---------------------------------------------------------
    private bool questionActive = false;
    private SegmentData activeSegmentData = null;

    private WaypointMover mover;
    private AnswerZone[] allZones;   // all AnswerZone components in the scene

    // -- Unity -----------------------------------------------------------------

    void Awake()
    {
        mover = FindObjectOfType<WaypointMover>();
        allZones = FindObjectsOfType<AnswerZone>();

        mover.OnReachStop += HandleReachStop;
        HideQuestion();
    }

    void OnDestroy()
    {
        if (mover != null) mover.OnReachStop -= HandleReachStop;
    }

    // -- Mover events ----------------------------------------------------------

    void HandleReachStop(int stopIndex)
    {
        if (questionActive)
        {
            ResolveAnswer(optionIndex: -1, autoFail: true);
            return;
        }

        if (stopIndex >= mover.stopPoints.Length - 1)
        {
            Debug.Log("Journey complete.");
            return;
        }

        // Capture the SegmentData for THIS leg BEFORE advancing the mover
        activeSegmentData = stopIndex < mover.segmentData.Length
            ? mover.segmentData[stopIndex]
            : null;

        mover.AdvanceToNextStop();
        ShowQuestion();
    }

    // -- Show question ---------------------------------------------------------

    void ShowQuestion()
    {
        SegmentData data = activeSegmentData;
        if (data == null) return;

        questionActive = true;
        questionText.text = data.question;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (answerButtons[i] == null) continue;

            bool hasOption = i < data.options.Length;
            answerButtons[i].gameObject.SetActive(hasOption);

            if (!hasOption) continue;

            TMP_Text lbl = answerButtons[i].GetComponentInChildren<TMP_Text>();
            if (lbl != null) lbl.text = data.options[i].text;

            answerButtons[i].onClick.RemoveAllListeners();
            int capturedIndex = i;
            answerButtons[i].onClick.AddListener(() => OnButtonClicked(capturedIndex));
        }

        questionUI.SetActive(true);
        SetFeedbackVisible(false);
    }

    void HideQuestion()
    {
        questionUI.SetActive(false);
        SetFeedbackVisible(false);
        activeSegmentData = null;
        foreach (Button btn in answerButtons)
            if (btn != null) btn.gameObject.SetActive(true);
    }

    // -- Answer logic ----------------------------------------------------------

    void OnButtonClicked(int index)
    {
        if (!questionActive) return;
        ResolveAnswer(optionIndex: index, autoFail: false);
    }

    // Public overload for AnswerButton compatibility
    public void Answer(int optionIndex)
    {
        if (!questionActive) return;
        ResolveAnswer(optionIndex: optionIndex, autoFail: false);
    }

    void ResolveAnswer(int optionIndex, bool autoFail)
    {
        questionActive = false;

        SegmentData data = activeSegmentData;
        float currentT = mover.SegmentProgress;   // [0..1] within segment

        // Find the AnswerZone for the current segment
        int segIdx = mover.CurrentSegmentIndex;
        AnswerZone zone = FindZone(segIdx);

        HideQuestion();

        float speedMult;
        string msg;
        Color col;

        if (autoFail)
        {
            speedMult = 0.55f;
            msg = data != null ? data.feedbackNoAnswer : "Sin respuesta.";
            col = colorWrong;
        }
        else
        {
            string optionFeedback = "";
            bool isCorrect = false;

            if (data != null && optionIndex >= 0 && optionIndex < data.options.Length)
            {
                optionFeedback = data.options[optionIndex].feedbackText;
                isCorrect = data.options[optionIndex].isCorrect;
            }

            if (!isCorrect)
            {
                speedMult = 0.65f;
                msg = optionFeedback;
                col = colorWrong;
            }
            else
            {
                // Evaluate timing using AnswerZone if available,
                // otherwise fall back to SegmentProgress midpoint
                int timing = EvaluateTiming(zone, currentT);

                string timingSuffix;
                if (timing < 0)
                {
                    speedMult = 0.85f;
                    timingSuffix = data != null ? data.feedbackTimingEarly : "muy temprano.";
                    col = colorEarly;
                }
                else if (timing == 0)
                {
                    speedMult = 1.4f;
                    timingSuffix = data != null ? data.feedbackTimingPerfect : "en el momento justo!";
                    col = colorPerfect;
                }
                else
                {
                    speedMult = 0.75f;
                    timingSuffix = data != null ? data.feedbackTimingLate : "muy tarde.";
                    col = colorLate;
                }

                msg = string.IsNullOrWhiteSpace(optionFeedback)
                    ? timingSuffix
                    : optionFeedback + " " + timingSuffix;
            }
        }

        mover.ModifySpeed(speedMult);
        ShowFeedback(msg, col);
    }

    // -- Timing helpers --------------------------------------------------------

    // Find the AnswerZone whose segmentIndex matches the given segment
    AnswerZone FindZone(int segmentIndex)
    {
        if (allZones == null) return null;
        foreach (AnswerZone z in allZones)
            if (z != null && z.segmentIndex == segmentIndex) return z;
        return null;
    }

    // Evaluate timing: -1 Early, 0 Perfect, 1 Late
    // Uses the AnswerZone's cached spline-t values directly.
    // currentT here is WaypointMover.SegmentProgress [0..1], but AnswerZone
    // uses raw spline-t, so we convert back using the segment's t range.
    int EvaluateTiming(AnswerZone zone, float segmentProgress)
    {
        if (zone == null)
        {
            // No zone assigned: use simple thirds as fallback
            if (segmentProgress < 0.4f) return -1;
            if (segmentProgress < 0.7f) return 0;
            return 1;
        }

        // Convert segment progress [0..1] back to raw spline-t for comparison
        // SegmentProgress = (currentT - segmentStartT) / (segmentEndT - segmentStartT)
        // We need to pass raw spline-t to zone.EvaluateTiming
        float rawT = mover.GetRawT(segmentProgress);
        return zone.EvaluateTiming(rawT);
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
    void SetFeedbackVisible(bool v) { if (feedbackText != null) feedbackText.gameObject.SetActive(v); }
}