using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class GlassBridgeManager : MonoBehaviour
{
    public static GlassBridgeManager Instance;

    [Header("Player")]
    public Transform player;
    public float moveSpeed = 2f;
    public float pauseDuration = 1f; // Pausa cuando falla

    [Header("Platforms")]
    public List<PlatformLevel> platformLevels; // Lista de niveles con 4 plataformas cada uno

    [Header("Game Settings")]
    public int totalLevels = 10; // Número de pares a cruzar
    public float fadeInDuration = 0.5f; // Duración del fade in de nuevas plataformas

    [Header("UI")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI errorText; // Mostrar errores cometidos
    public GameObject winPanel;

    private int currentLevel = 0;
    private int errorCount = 0; // Contador de errores
    private bool isMoving = false;
    private bool gameOver = false;
    private bool isPaused = false;
    private Vector3 initialPlayerPosition; // Guardar posición inicial del editor

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Guardar la posición inicial del jugador desde el editor
        if (player)
            initialPlayerPosition = player.position;

        // Las plataformas ya inician ocultas desde su propio Start()
        // No necesitamos ocultarlas aquí

        ResetGame();
    }

    public void OnPlatformClicked(GlassPlatform platform)
    {
        if (isMoving || gameOver || isPaused) return;

        // Verificar que sea del nivel actual
        if (currentLevel >= platformLevels.Count) return;

        PlatformLevel currentLevel_Level = platformLevels[currentLevel];

        // Verificar que sea una de las plataformas del nivel actual
        if (!currentLevel_Level.ContainsPlatform(platform))
            return;

        // Mostrar resultado visual
        platform.ShowResult();

        if (platform.isCorrect)
        {
            // Mover al jugador a la plataforma correcta
            StartCoroutine(MovePlayerToPlatform(platform.transform.position));
        }
        else
        {
            // Plataforma incorrecta - pausar pero permitir reintentar
            StartCoroutine(HandleIncorrectChoice());
        }
    }

    IEnumerator MovePlayerToPlatform(Vector3 targetPosition)
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

        // Avanzar al siguiente nivel
        currentLevel++;
        UpdateUI();

        // Mostrar el siguiente nivel con fade in ANTES de permitir más interacción
        if (currentLevel < platformLevels.Count)
        {
            yield return StartCoroutine(FadeInLevel(currentLevel));
        }

        isMoving = false;

        // Verificar si ganó
        if (currentLevel >= totalLevels)
        {
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

        // Asegurar alpha completo al final
        level.SetAlphaAll(1f);
    }

    IEnumerator HandleIncorrectChoice()
    {
        isPaused = true;
        errorCount++; // Incrementar errores
        UpdateUI();

        // Pausar por 1 segundo
        yield return new WaitForSeconds(pauseDuration);

        // Resetear el color de las plataformas incorrectas del nivel actual
        if (currentLevel < platformLevels.Count)
        {
            PlatformLevel currentLevel_Level = platformLevels[currentLevel];
            currentLevel_Level.ResetIncorrectPlatformsColor();
        }

        // Después de la pausa, permitir que elija de nuevo
        isPaused = false;
    }

    void WinGame()
    {
        gameOver = true;
        if (winPanel)
            winPanel.SetActive(true);
    }

    public void ResetGame()
    {
        currentLevel = 0;
        errorCount = 0; // Reiniciar contador de errores
        gameOver = false;
        isMoving = false;
        isPaused = false;

        if (winPanel)
            winPanel.SetActive(false);

        // Reiniciar todas las plataformas (ya las oculta ResetPlatform)
        foreach (PlatformLevel level in platformLevels)
        {
            level.ResetAllPlatforms();
        }

        // Randomizar qué plataforma es correcta en cada nivel
        RandomizePlatforms();

        // Posicionar jugador en la posición inicial guardada del editor
        if (player)
        {
            player.position = initialPlayerPosition;
        }

        // Mostrar el primer nivel con fade in
        if (platformLevels.Count > 0)
        {
            StartCoroutine(FadeInLevel(0));
        }

        UpdateUI();
    }

    void RandomizePlatforms()
    {
        // Ya no randomizamos automáticamente
        // El usuario marca manualmente qué plataformas son correctas/incorrectas en el Inspector
        // Este método se mantiene por compatibilidad pero ya no hace nada

        // Si quieres randomización automática, descomenta esto:
        /*
        foreach (PlatformLevel level in platformLevels)
        {
            level.RandomizeCorrectPlatform();
        }
        */
    }

    void UpdateUI()
    {
        if (levelText)
            levelText.text = $"Nivel: {currentLevel + 1} / {totalLevels}";

        if (errorText)
            errorText.text = $"Errores: {errorCount}";
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public bool IsMoving()
    {
        return isMoving || isPaused;
    }
}

[System.Serializable]
public class PlatformLevel
{
    public GlassPlatform platform1;
    public GlassPlatform platform2;
    public GlassPlatform platform3;
    public GlassPlatform platform4;

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

    public void ResetIncorrectPlatformsColor()
    {
        if (platform1 && !platform1.isCorrect) platform1.ResetColor();
        if (platform2 && !platform2.isCorrect) platform2.ResetColor();
        if (platform3 && !platform3.isCorrect) platform3.ResetColor();
        if (platform4 && !platform4.isCorrect) platform4.ResetColor();
    }

    public void RandomizeCorrectPlatform()
    {
        // Desmarcar todas primero
        if (platform1) platform1.isCorrect = false;
        if (platform2) platform2.isCorrect = false;
        if (platform3) platform3.isCorrect = false;
        if (platform4) platform4.isCorrect = false;

        // Elegir una al azar para ser correcta
        int randomIndex = Random.Range(0, 4);

        switch (randomIndex)
        {
            case 0: if (platform1) platform1.isCorrect = true; break;
            case 1: if (platform2) platform2.isCorrect = true; break;
            case 2: if (platform3) platform3.isCorrect = true; break;
            case 3: if (platform4) platform4.isCorrect = true; break;
        }
    }
}