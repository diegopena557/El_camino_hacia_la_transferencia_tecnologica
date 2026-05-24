using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class QuestionManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject questionUI;
    public TMP_Text questionText;
    [Tooltip("Assign the 5 answer Button GameObjects here in order")]
    public Button[] answerButtons = new Button[5];
    public TMP_Text feedbackText;
    [Tooltip("TMP_Text separado que muestra solo el resultado del timing (Perfecto / Temprano / Tarde)")]
    public TMP_Text timingText;
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

            // Remove ALL listeners including any persistent ones set in the Inspector
            answerButtons[i].onClick.RemoveAllListeners();

            // Store index in a local copy so the lambda captures the right value
            int idx = i;
            answerButtons[i].onClick.AddListener(delegate { OnButtonClicked(idx); });
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

        // Capture everything BEFORE HideQuestion nulls activeSegmentData
        SegmentData data = activeSegmentData;
        float rawT = mover.CurrentRawT;
        int segIdx = mover.CurrentSegmentIndex;
        AnswerZone zone = FindZone(segIdx);

        HideQuestion();

        float speedMult = 0.65f;
        string msg = "";
        Color col = colorWrong;

        string timingSuffix = "";
        string optionFeedback = "";
        bool isCorrect = false;

        if (autoFail)
        {
            speedMult = 0.55f;
            msg = data != null ? data.feedbackNoAnswer : "Sin respuesta.";
            col = colorWrong;
        }
        else
        {
            if (data != null && optionIndex >= 0 && optionIndex < data.options.Length)
            {
                optionFeedback = data.options[optionIndex].feedbackText;
                isCorrect = data.options[optionIndex].isCorrect;
            }


            // Evaluate timing for ALL answers (correct or not)
            // Returns: -1 Early, 0 Perfect, 1 Late, 2 past lateEnd (no answer window)
            int timing = EvaluateTiming(zone, rawT);

            switch (timing)
            {
                case -1:  // Early
                    timingSuffix = data != null ? data.feedbackTimingEarly : "muy temprano.";
                    col = isCorrect ? colorEarly : colorWrong;
                    speedMult = isCorrect ? 0.85f : 0.65f;
                    break;
                case 0:   // Perfect
                    timingSuffix = data != null ? data.feedbackTimingPerfect : "en el momento justo!";
                    col = isCorrect ? colorPerfect : colorWrong;
                    speedMult = isCorrect ? 1.4f : 0.65f;
                    break;
                case 1:   // Late
                    timingSuffix = data != null ? data.feedbackTimingLate : "muy tarde.";
                    col = isCorrect ? colorLate : colorWrong;
                    speedMult = isCorrect ? 0.75f : 0.65f;
                    break;
                default:  // Past lateEnd - treat as no answer
                    timingSuffix = data != null ? data.feedbackNoAnswer : "sin respuesta.";
                    col = colorWrong;
                    speedMult = 0.55f;
                    break;
            }

            msg = optionFeedback;
        }

        mover.ModifySpeed(speedMult);

        // feedbackText -> option-specific text ("La integracion permite...")
        ShowFeedback(msg, col);

        // timingText -> timing result ("en el momento justo!" / "muy temprano." / "muy tarde.")
        if (!autoFail)
            ShowTimingFeedback(timingSuffix ?? "", col);
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
    // rawT is mover.CurrentRawT - the raw spline-t with no conversion.
    // AnswerZone.cachedStartT / cachedEndT are also raw spline-t values,
    // so the comparison is direct and accurate.
    int EvaluateTiming(AnswerZone zone, float rawT)
    {
        if (zone == null)
        {
            // No AnswerZone in scene for this segment: fallback to segment thirds
            float progress = mover.SegmentProgress;
            if (progress < 0.33f) return -1;  // Early
            if (progress < 0.66f) return 0;  // Perfect
            if (progress < 1.0f) return 1;  // Late
            return 2;                          // Past end
        }

        // rawT is the raw spline-t value - passed directly, no conversion needed
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

    void HideFeedback()
    {
        SetFeedbackVisible(false);
        if (timingText != null) timingText.gameObject.SetActive(false);
    }

    void ShowTimingFeedback(string msg, Color col)
    {
        if (timingText == null || string.IsNullOrWhiteSpace(msg)) return;
        timingText.text = msg;
        timingText.color = col;
        timingText.gameObject.SetActive(true);
    }

    void SetFeedbackVisible(bool v) { if (feedbackText != null) feedbackText.gameObject.SetActive(v); }
}