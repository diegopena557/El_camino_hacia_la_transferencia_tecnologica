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
    public float feedbackDuration = 1.8f;

    [Header("Feedback colors")]
    public Color colorPerfect = Color.green;
    public Color colorEarly = Color.yellow;
    public Color colorLate = new Color(1f, 0.5f, 0f);
    public Color colorWrong = Color.red;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip buttonClickSound;
    public AudioClip correctAnswerSound;
    public AudioClip incorrectAnswerSound;

    // -- Runtime state ---------------------------------------------------------

    private bool questionActive = false;
    private bool forcedLateAnswer = false;

    private SegmentData activeSegmentData = null;

    private WaypointMover mover;
    private AnswerZone[] allZones;
    private StopQuestionManager stopManager;

    // -- Unity -----------------------------------------------------------------

    void Awake()
    {
        mover = FindObjectOfType<WaypointMover>();
        allZones = FindObjectsOfType<AnswerZone>();
        stopManager = FindObjectOfType<StopQuestionManager>();

        mover.OnReachStop += HandleReachStop;

        HideQuestion();
    }

    void OnDestroy()
    {
        if (mover != null)
            mover.OnReachStop -= HandleReachStop;
    }

    // -- Mover events ----------------------------------------------------------

    void HandleReachStop(int stopIndex)
    {
        // Si la pregunta sigue activa al llegar al final del tramo,
        // detener el personaje pero mantener la pregunta visible.
        if (questionActive)
        {
            forcedLateAnswer = true;
            mover.SetPaused(true);
            return;
        }

        // Fin del recorrido
        if (stopIndex >= mover.stopPoints.Length - 1)
        {
            Debug.Log("Journey complete.");
            GameResultsManager.Instance?.ShowResults();
            return;
        }

        // Capturar datos del siguiente segmento
        activeSegmentData = stopIndex < mover.segmentData.Length
            ? mover.segmentData[stopIndex]
            : null;

        mover.AdvanceToNextStop();

        // Notificar stop questions
        if (stopManager != null)
            stopManager.NotifySegmentStarted(mover.CurrentSegmentIndex);

        ShowQuestion();
    }

    // -- Show question ---------------------------------------------------------

    void ShowQuestion()
    {
        SegmentData data = activeSegmentData;

        if (data == null)
            return;

        questionActive = true;

        questionText.text = data.question;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (answerButtons[i] == null)
                continue;

            bool hasOption = i < data.options.Length;

            answerButtons[i].gameObject.SetActive(hasOption);

            if (!hasOption)
                continue;

            TMP_Text lbl = answerButtons[i].GetComponentInChildren<TMP_Text>();

            if (lbl != null)
                lbl.text = data.options[i].text;

            answerButtons[i].onClick.RemoveAllListeners();

            int idx = i;

            answerButtons[i].onClick.AddListener(delegate
            {
                OnButtonClicked(idx);
            });
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
        {
            if (btn != null)
                btn.gameObject.SetActive(true);
        }
    }

    // -- Answer logic ----------------------------------------------------------

    void OnButtonClicked(int index)
    {
        if (!questionActive)
            return;

        PlaySound(buttonClickSound);
        ResolveAnswer(optionIndex: index, autoFail: false);
    }

    public void Answer(int optionIndex)
    {
        if (!questionActive)
            return;

        PlaySound(buttonClickSound);
        ResolveAnswer(optionIndex: optionIndex, autoFail: false);
    }

    void ResolveAnswer(int optionIndex, bool autoFail)
    {
        questionActive = false;

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

            msg = data != null
                ? data.feedbackNoAnswer
                : "Sin respuesta.";

            col = colorWrong;
        }
        else
        {
            if (data != null &&
                optionIndex >= 0 &&
                optionIndex < data.options.Length)
            {
                optionFeedback = data.options[optionIndex].feedbackText;

                isCorrect = data.options[optionIndex].isCorrect;
            }

            int timing;

            // Si llego al final sin responder,
            // SIEMPRE cuenta como VERY LATE
            if (forcedLateAnswer)
            {
                timing = 1;
                forcedLateAnswer = false;
            }
            else
            {
                timing = EvaluateTiming(zone, rawT);
            }

            switch (timing)
            {
                case -1:

                    timingSuffix = data != null
                        ? data.feedbackTimingEarly
                        : "muy temprano.";

                    col = isCorrect
                        ? colorEarly
                        : colorWrong;

                    speedMult = isCorrect
                        ? 0.85f
                        : 0.65f;

                    break;

                case 0:

                    timingSuffix = data != null
                        ? data.feedbackTimingPerfect
                        : "en el momento justo!";

                    col = isCorrect
                        ? colorPerfect
                        : colorWrong;

                    speedMult = isCorrect
                        ? 1.4f
                        : 0.65f;

                    break;

                case 1:

                    timingSuffix = data != null
                        ? data.feedbackTimingLate
                        : "muy tarde.";

                    col = isCorrect
                        ? colorLate
                        : colorWrong;

                    speedMult = isCorrect
                        ? 0.75f
                        : 0.65f;

                    break;

                default:

                    timingSuffix = data != null
                        ? data.feedbackNoAnswer
                        : "sin respuesta.";

                    col = colorWrong;

                    speedMult = 0.55f;

                    break;
            }

            msg = optionFeedback;
        }

        mover.ModifySpeed(speedMult);

        // Reanudar movimiento si estaba pausado
        mover.SetPaused(false);

        // Reproducir sonido de resultado
        if (!autoFail)
            PlaySound(isCorrect ? correctAnswerSound : incorrectAnswerSound);
        else
            PlaySound(incorrectAnswerSound);

        // Registrar resultado
        if (autoFail)
            GameResultsManager.Instance?.RegisterAnswer(false);
        else
            GameResultsManager.Instance?.RegisterAnswer(isCorrect);

        // -------------------------------------------------------------------
        // FIX: mostrar feedback en todos los casos (correcto, incorrecto y
        // autoFail). Antes, el bloque "else if (isCorrect)" dejaba sin
        // feedback las respuestas incorrectas.
        // -------------------------------------------------------------------
        if (autoFail)
        {
            ShowFeedback(msg, col);
        }
        else
        {
            string combined = isCorrect
                ? (string.IsNullOrWhiteSpace(timingSuffix)
                    ? msg
                    : string.IsNullOrWhiteSpace(msg)
                        ? timingSuffix
                        : msg + "\n" + timingSuffix)
                : msg; // incorrecto: solo el texto de la opción, sin sufijo de timing

            ShowFeedback(combined, col);
        }
    }

    // -- Timing ----------------------------------------------------------------

    AnswerZone FindZone(int segmentIndex)
    {
        if (allZones == null)
            return null;

        foreach (AnswerZone z in allZones)
        {
            if (z != null && z.segmentIndex == segmentIndex)
                return z;
        }

        return null;
    }

    int EvaluateTiming(AnswerZone zone, float rawT)
    {
        if (zone == null)
        {
            float p = mover.SegmentProgress;

            if (p < 0.33f) return -1;
            if (p < 0.66f) return 0;
            if (p < 1.0f) return 1;

            return 2;
        }

        return zone.EvaluateTiming(rawT);
    }

    // -- Audio -----------------------------------------------------------------

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }

    // -- Feedback --------------------------------------------------------------

    void ShowFeedback(string msg, Color col)
    {
        if (feedbackText == null)
            return;

        feedbackText.text = msg;

        feedbackText.color = col;

        SetFeedbackVisible(true);

        CancelInvoke(nameof(HideFeedback));

        Invoke(nameof(HideFeedback), feedbackDuration);
    }

    void HideFeedback()
    {
        SetFeedbackVisible(false);
    }

    void SetFeedbackVisible(bool v)
    {
        if (feedbackText != null)
            feedbackText.gameObject.SetActive(v);
    }
}