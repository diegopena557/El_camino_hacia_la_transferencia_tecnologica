using UnityEngine;
using TMPro;

public class LevelUIDisplay : MonoBehaviour
{
    [Header("Referencias UI")]
    public TextMeshProUGUI levelNameText;
    public TextMeshProUGUI levelNumberText;
    public TextMeshProUGUI categoryText;
    public TextMeshProUGUI descriptionText;

    [Header("Configuración de Texto")]
    public string levelPrefix = "Nivel ";
    public string categoryPrefix = "Fase: ";

    [Header("Colores por Categoría (Opcional)")]
    public bool useColorCoding = true;
    public Color entenderColor = new Color(0.2f, 0.6f, 1f); // Azul
    public Color imaginarColor = new Color(1f, 0.6f, 0.2f); // Naranja
    public Color probarColor = new Color(0.4f, 1f, 0.4f);   // Verde

    void Start()
    {
        // Suscribirse a cambios de nivel si hay LevelManager
        if (LevelManager.Instance != null)
        {
            UpdateDisplay();
        }
    }

    void Update()
    {
        // Actualizar constantemente (puedes cambiar esto a eventos si prefieres)
        if (LevelManager.Instance != null)
        {
            UpdateDisplay();
        }
    }

    public void UpdateDisplay()
    {
        if (LevelManager.Instance == null) return;

        LevelData currentLevel = LevelManager.Instance.GetCurrentLevel();
        if (currentLevel == null) return;

        // Actualizar número de nivel
        if (levelNumberText != null)
        {
            levelNumberText.text = $"{levelPrefix}{currentLevel.levelNumber}";
        }

        // Actualizar nombre del nivel
        if (levelNameText != null)
        {
            levelNameText.text = currentLevel.levelName;
        }

        // Actualizar categoría
        if (categoryText != null)
        {
            string categoryName = GetCategoryDisplayName(currentLevel.focusCategory);
            categoryText.text = $"{categoryPrefix}{categoryName}";

            // Aplicar color según categoría
            if (useColorCoding)
            {
                categoryText.color = GetCategoryColor(currentLevel.focusCategory);
            }
        }

        // Actualizar descripción
        if (descriptionText != null)
        {
            descriptionText.text = currentLevel.levelDescription;
        }
    }

    string GetCategoryDisplayName(TokenCategory category)
    {
        return category switch
        {
            TokenCategory.Entender => "Entender",
            TokenCategory.Imaginar => "Imaginar",
            TokenCategory.Probar => "Probar",
            _ => "Desconocido"
        };
    }

    Color GetCategoryColor(TokenCategory category)
    {
        return category switch
        {
            TokenCategory.Entender => entenderColor,
            TokenCategory.Imaginar => imaginarColor,
            TokenCategory.Probar => probarColor,
            _ => Color.white
        };
    }

    // Método público para actualizar manualmente
    public void ForceUpdate()
    {
        UpdateDisplay();
    }
}