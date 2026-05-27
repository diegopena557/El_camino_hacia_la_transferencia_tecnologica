using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class GlassBridgeManager : MonoBehaviour
{
    public static GlassBridgeManager Instance;

    [Header("Player")]
    public Transform player;
    public Animator playerAnimator;
    public float moveSpeed = 2f;

    [Header("Platforms")]
    public List<PlatformLevel> platformLevels;

    [Header("Game Settings")]
    public int totalLevels = 10;
    public float fadeInDuration = 0.5f;

    [Header("UI")]
    public TextMeshProUGUI levelText;
    public GameObject winPanel;

    [Header("Victory UI")]
    public TextMeshProUGUI correctText;
    public TextMeshProUGUI errorText;
    public TextMeshProUGUI percentageText;
    public TextMeshProUGUI medalTitleText;

    public Image medalImage;
    public Sprite bronzeMedal;
    public Sprite silverMedal;
    public Sprite goldMedal;

    [Header("Stats")]
    public int correctCount = 0;
    public int errorCount = 0;

    private int currentLevel = 0;
    private bool isMoving = false;
    private bool gameOver = false;
    private bool gameStarted = false; 
    private Vector3 initialPlayerPosition;

    [Header("End Game")]
    public Transform endPoint;

    private Vector3 correctScale;
    private Vector3 errorScale;
    private Vector3 percentageScale;
    private Vector3 medalTitleScale;
    private Vector3 medalImageScale;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (player)
            initialPlayerPosition = player.position;

        correctScale = correctText.transform.localScale;
        errorScale = errorText.transform.localScale;
        percentageScale = percentageText.transform.localScale;
        medalTitleScale = medalTitleText.transform.localScale;
        medalImageScale = medalImage.transform.localScale;

        // NO iniciar el juego automáticamente
        // El panel de instrucciones llamará a StartGame() cuando el jugador esté listo
    }

    void RandomizeLevel(PlatformLevel level)
    {
        List<GlassPlatform> platforms = level.GetPlatforms();

        // Recopilar datos originales
        List<string> texts = new List<string>();
        List<bool> corrects = new List<bool>();
        List<string> feedbackTexts = new List<string>();

        foreach (GlassPlatform p in platforms)
        {
            texts.Add(p.platformText);
            corrects.Add(p.isCorrect);
            feedbackTexts.Add(p.feedbackText);
        }

        // Fisher-Yates shuffle — todos los datos se mueven juntos
        for (int i = texts.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);

            string tmpText = texts[i];
            texts[i] = texts[j];
            texts[j] = tmpText;

            bool tmpCorrect = corrects[i];
            corrects[i] = corrects[j];
            corrects[j] = tmpCorrect;

            string tmpFeedback = feedbackTexts[i];
            feedbackTexts[i] = feedbackTexts[j];
            feedbackTexts[j] = tmpFeedback;
        }

        // Reasignar datos a cada plataforma (posición en escena no cambia)
        for (int i = 0; i < platforms.Count; i++)
        {
            platforms[i].platformText = texts[i];
            platforms[i].isCorrect = corrects[i];
            platforms[i].feedbackText = feedbackTexts[i];

            if (platforms[i].textDisplayUI != null)
                platforms[i].textDisplayUI.text = texts[i];
        }
    }

  
    void RandomizeAllLevels()
    {
        foreach (PlatformLevel level in platformLevels)
            RandomizeLevel(level);
    }

 

    public void OnPlatformClicked(GlassPlatform platform)
    {
        if (isMoving || gameOver) return;
        if (currentLevel >= platformLevels.Count) return;

        PlatformLevel level = platformLevels[currentLevel];
        if (!level.ContainsPlatform(platform)) return;

        platform.ShowResult();

        if (platform.isCorrect)
        {
            if (playerAnimator)
            {
                playerAnimator.SetBool("IsJumping", true);
                playerAnimator.SetBool("IsFalling", false);
            }
            StartCoroutine(MovePlayerToPlatform(platform.transform.position, true, platform));
        }
        else
        {
            if (playerAnimator)
            {
                playerAnimator.SetBool("IsFalling", true);
                playerAnimator.SetBool("IsJumping", false);
            }
            StartCoroutine(MovePlayerToPlatform(platform.transform.position, false, platform));
        }
    }

    IEnumerator MovePlayerToPlatform(Vector3 targetPosition, bool isCorrect, GlassPlatform clickedPlatform)
    {
        isMoving = true;

        Vector3 startPos = player.position;
        Vector3 endPos = new Vector3(targetPosition.x, targetPosition.y + 0.5f, startPos.z);

        float elapsed = 0f;
        float duration = Vector3.Distance(startPos, endPos) / moveSpeed;

        while (elapsed < duration)
        {
            player.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        player.position = endPos;

        if (playerAnimator)
        {
            playerAnimator.SetBool("IsJumping", false);
            playerAnimator.SetBool("IsFalling", false);
        }

        if (isCorrect)
        {
            correctCount++;
            currentLevel++;
            UpdateUI();

            if (currentLevel < platformLevels.Count)
                yield return StartCoroutine(FadeInLevel(currentLevel));

            isMoving = false;

            if (currentLevel >= platformLevels.Count)
                WinGame();
        }
        else
        {
            errorCount++;

            yield return new WaitForSeconds(1f);

            if (clickedPlatform != null)
                clickedPlatform.BreakPlatformNow();

            currentLevel++;
            UpdateUI();

            if (currentLevel < platformLevels.Count)
                yield return StartCoroutine(FadeInLevel(currentLevel));

            isMoving = false;

            if (currentLevel >= platformLevels.Count)
                WinGame();
        }
    }

    IEnumerator FadeInLevel(int levelIndex)
    {
        if (levelIndex >= platformLevels.Count) yield break;

        PlatformLevel level = platformLevels[levelIndex];

        float elapsed = 0f;
        while (elapsed < fadeInDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeInDuration);
            level.SetAlphaAll(alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        level.SetAlphaAll(1f);
    }

    void WinGame()
    {
        StartCoroutine(MoveToEndAndWin());
    }

    IEnumerator MoveToEndAndWin()
    {
        gameOver = true;
        isMoving = true;

        if (playerAnimator)
        {
            playerAnimator.SetBool("IsJumping", false);
            playerAnimator.SetBool("IsFalling", false);
        }

        Vector3 startPos = player.position;
        Vector3 endPos = new Vector3(endPoint.position.x, endPoint.position.y, startPos.z);

        float elapsed = 0f;
        float duration = Vector3.Distance(startPos, endPos) / moveSpeed;

        while (elapsed < duration)
        {
            player.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        player.position = endPos;
        isMoving = false;

        ShowVictoryUI();
    }

    void ShowVictoryUI()
    {
        if (winPanel) winPanel.SetActive(true);

        float percentage = GetSuccessPercentage();

        correctText.text = "Aciertos: " + correctCount;
        errorText.text = "Errores: " + errorCount;
        percentageText.text = "Precisión: " + percentage.ToString("F1") + "%";

        if (percentage >= 80f)
        {
            medalImage.sprite = goldMedal;
            medalTitleText.text = "MEDALLA DE ORO";
        }
        else if (percentage >= 50f)
        {
            medalImage.sprite = silverMedal;
            medalTitleText.text = "MEDALLA DE PLATA";
        }
        else
        {
            medalImage.sprite = bronzeMedal;
            medalTitleText.text = "MEDALLA DE BRONCE";
        }

        correctText.transform.localScale = Vector3.zero;
        errorText.transform.localScale = Vector3.zero;
        percentageText.transform.localScale = Vector3.zero;
        medalTitleText.transform.localScale = Vector3.zero;
        medalImage.transform.localScale = Vector3.zero;

        StartCoroutine(AnimateVictoryUI());
    }

    IEnumerator AnimateVictoryUI()
    {
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(PopUpEffect(correctText.transform, correctScale));
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(PopUpEffect(errorText.transform, errorScale));
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(PopUpEffect(percentageText.transform, percentageScale));
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(PopUpEffect(medalTitleText.transform, medalTitleScale, 1.3f));
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(PopUpEffect(medalImage.transform, medalImageScale, 1.4f));
    }

    IEnumerator PopUpEffect(Transform target, Vector3 originalScale, float scaleMultiplier = 1.2f)
    {
        Vector3 targetScale = originalScale * scaleMultiplier;
        float duration = 0.3f;
        float elapsed = 0f;

        while (elapsed < duration / 2f)
        {
            target.localScale = Vector3.Lerp(Vector3.zero, targetScale, elapsed / (duration / 2f));
            elapsed += Time.deltaTime;
            yield return null;
        }
        target.localScale = targetScale;

        elapsed = 0f;
        while (elapsed < duration / 2f)
        {
            target.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / (duration / 2f));
            elapsed += Time.deltaTime;
            yield return null;
        }
        target.localScale = originalScale;
    }

    public float GetSuccessPercentage()
    {
        int total = correctCount + errorCount;
        if (total == 0) return 0;
        return (float)correctCount / total * 100f;
    }

    public void ResetGame()
    {
        currentLevel = 0;
        gameOver = false;
        isMoving = false;
        correctCount = 0;
        errorCount = 0;

        if (winPanel) winPanel.SetActive(false);

        foreach (PlatformLevel level in platformLevels)
            level.ResetAllPlatforms();

        // Aleatorizar DESPUÉS del reset para que los textos originales
        // estén disponibles antes del shuffle
        RandomizeAllLevels();

        if (player) player.position = initialPlayerPosition;

        if (platformLevels.Count > 0 && gameStarted)
            StartCoroutine(FadeInLevel(0));

        UpdateUI();
    }

    public void StartGame()
    {
        if (gameStarted) return;

        gameStarted = true;
        ResetGame();
    }

    void UpdateUI()
    {
        if (levelText)
            levelText.text = $"Nivel: {currentLevel + 1} / {platformLevels.Count}";
    }

    public bool IsGameOver() => gameOver;
    public bool IsMoving() => isMoving;
}



[System.Serializable]
public class PlatformLevel
{
    public GlassPlatform platform1;
    public GlassPlatform platform2;
    public GlassPlatform platform3;
    public GlassPlatform platform4;

    
    public List<GlassPlatform> GetPlatforms()
    {
        var list = new List<GlassPlatform>();
        if (platform1) list.Add(platform1);
        if (platform2) list.Add(platform2);
        if (platform3) list.Add(platform3);
        if (platform4) list.Add(platform4);
        return list;
    }

    public bool ContainsPlatform(GlassPlatform platform)
    {
        return platform == platform1 || platform == platform2 ||
               platform == platform3 || platform == platform4;
    }

    public void SetAlphaAll(float alpha)
    {
        if (platform1) platform1.SetAlpha(alpha);
        if (platform2) platform2.SetAlpha(alpha);
        if (platform3) platform3.SetAlpha(alpha);
        if (platform4) platform4.SetAlpha(alpha);
    }

    public void ResetAllPlatforms()
    {
        if (platform1) platform1.ResetPlatform();
        if (platform2) platform2.ResetPlatform();
        if (platform3) platform3.ResetPlatform();
        if (platform4) platform4.ResetPlatform();
    }
}