using UnityEngine;
using UnityEngine.UI;

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Instance;

    [Header("Modo de Juego")]
    public int currentMode = 1; // 1 o 2

    [Header("Indicador Visual")]
    public Image modeIndicator;
    public Sprite mode1Sprite;
    public Sprite mode2Sprite;

    [Header("Colores del Indicador (Opcional)")]
    public bool useColorCoding = true;
    public Color mode1Color = new Color(0.3f, 0.7f, 1f); // Azul
    public Color mode2Color = new Color(1f, 0.5f, 0.3f); // Naranja

    [Header("Configuración")]
    public bool allowModeChange = true;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Lee el modo guardado por TimerNode5 vía PlayerPrefs
        if (PlayerPrefs.HasKey(TimerNode5.GAME_MODE_KEY))
        {
            int savedMode = PlayerPrefs.GetInt(TimerNode5.GAME_MODE_KEY);
            SetMode(savedMode);
            Debug.Log($"[GameModeManager] Modo cargado desde selección anterior: {savedMode}");
        }
        else
        {
            // Fallback: usa el valor por defecto asignado en el Inspector
            UpdateModeIndicator();
            Debug.Log($"[GameModeManager] No se encontró modo guardado, usando modo por defecto: {currentMode}");
        }
    }

    void Update()
    {
        if (!allowModeChange) return;

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            SetMode(1);
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            SetMode(2);
    }

    public void SetMode(int mode)
    {
        if (mode != 1 && mode != 2)
        {
            Debug.LogWarning($"[GameModeManager] Modo inválido: {mode}. Usa 1 o 2.");
            return;
        }

        currentMode = mode;
        UpdateModeIndicator();

        Debug.Log($"[GameModeManager] Modo cambiado a: {currentMode}");
    }

    void UpdateModeIndicator()
    {
        if (modeIndicator == null)
        {
            Debug.LogWarning("[GameModeManager] No hay indicador visual asignado");
            return;
        }

        if (currentMode == 1 && mode1Sprite != null)
            modeIndicator.sprite = mode1Sprite;
        else if (currentMode == 2 && mode2Sprite != null)
            modeIndicator.sprite = mode2Sprite;

        if (useColorCoding)
            modeIndicator.color = currentMode == 1 ? mode1Color : mode2Color;

        Debug.Log($"[GameModeManager] Indicador actualizado para modo {currentMode}");
    }

    public int GetCurrentMode() => currentMode;
    public bool IsMode1() => currentMode == 1;
    public bool IsMode2() => currentMode == 2;
}