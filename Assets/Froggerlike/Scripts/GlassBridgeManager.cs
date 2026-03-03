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
    public List<PlatformPair> platformPairs; // Lista de pares de plataformas

    [Header("Game Settings")]
    public int totalLevels = 10; // Número de pares a cruzar

    [Header("UI")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI errorText; // Mostrar errores cometidos
    public GameObject winPanel;

    private int currentLevel = 0;
    private int errorCount = 0; // Contador de errores
    private bool isMoving = false;
    private bool gameOver = false;
    private bool isPaused = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        ResetGame();
    }

    public void OnPlatformClicked(GlassPlatform platform)
    {
        if (isMoving || gameOver || isPaused) return;

        // Verificar que sea del nivel actual
        if (currentLevel >= platformPairs.Count) return;

        PlatformPair currentPair = platformPairs[currentLevel];

        // Verificar que sea una de las plataformas del par actual
        if (platform != currentPair.leftPlatform && platform != currentPair.rightPlatform)
            return;

        platform.ShowResult();

        if (platform.isCorrect)
        {
            // Mover al jugador a la plataforma correcta
            StartCoroutine(MovePlayerToPlatform(platform.transform.position));
        }
        else
        {
            // Plataforma incorrecta - pausar y perder
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
        isMoving = false;

        // Avanzar al siguiente nivel
        currentLevel++;
        UpdateUI();

        // Verificar si ganó
        if (currentLevel >= totalLevels)
        {
            WinGame();
        }
    }

    IEnumerator HandleIncorrectChoice()
    {
        isPaused = true;
        errorCount++; // Incrementar errores
        UpdateUI();

        // Pausar por 1 segundo
        yield return new WaitForSeconds(pauseDuration);

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

        // Reiniciar plataformas
        foreach (PlatformPair pair in platformPairs)
        {
            if (pair.leftPlatform)
                pair.leftPlatform.ResetPlatform();
            if (pair.rightPlatform)
                pair.rightPlatform.ResetPlatform();
        }

        // Randomizar qué plataforma es correcta en cada par
        RandomizePlatforms();

        // Posicionar jugador al inicio
        if (player && platformPairs.Count > 0)
        {
            Vector3 startPos = new Vector3(0, platformPairs[0].leftPlatform.transform.position.y - 2f, 0);
            player.position = startPos;
        }

        UpdateUI();
    }

    void RandomizePlatforms()
    {
        foreach (PlatformPair pair in platformPairs)
        {
            bool leftIsCorrect = Random.value > 0.5f;

            if (pair.leftPlatform)
                pair.leftPlatform.isCorrect = leftIsCorrect;
            if (pair.rightPlatform)
                pair.rightPlatform.isCorrect = !leftIsCorrect;
        }
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
}

[System.Serializable]
public class PlatformPair
{
    public GlassPlatform leftPlatform;
    public GlassPlatform rightPlatform;
}