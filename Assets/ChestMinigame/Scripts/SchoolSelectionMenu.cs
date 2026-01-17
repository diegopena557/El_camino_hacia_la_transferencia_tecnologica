using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SchoolSelectionMenu : MonoBehaviour
{
    public static SchoolSelectionMenu Instance;

    [Header("Referencias")]
    public SchoolCardManager cardManager; // Cambiado de database
    public GameObject selectionPanel;
    public GameObject gameplayPanel;

    [Header("UI Elements")]
    public Transform schoolButtonsContainer;
    public GameObject schoolButtonPrefab;
    public TextMeshProUGUI selectedSchoolText;
    public Button startGameButton;

    [Header("Preview Panel (Opcional)")]
    public TextMeshProUGUI schoolNameText;
    public TextMeshProUGUI schoolDescriptionText;
    public Image schoolIconImage;
    public TextMeshProUGUI cienciaCountText;
    public TextMeshProUGUI tecnologiaCountText;
    public TextMeshProUGUI innovacionCountText;

    private SchoolData selectedSchool;
    private List<GameObject> spawnedButtons = new List<GameObject>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (gameplayPanel != null)
            gameplayPanel.SetActive(false);

        if (startGameButton != null)
            startGameButton.interactable = false;

        // Verificar si hay IntroManager
        bool hasIntroManager = IntroManager.Instance != null;

        if (hasIntroManager)
        {
            // Si hay IntroManager, NO tocar el selectionPanel
            // IntroManager ya lo tiene activo desde la jerarquía
            Debug.Log("IntroManager detectado - esperando activación");
        }
        else
        {
            // Si NO hay IntroManager, activar directamente
            if (selectionPanel != null)
                selectionPanel.SetActive(true);

            GenerateSchoolButtons();
            Debug.Log("Sin IntroManager - activando SchoolSelection directamente");
        }
    }

    // Llamado por IntroManager cuando se activa esta pantalla
    public void OnScreenActivated()
    {
        if (selectionPanel != null)
            selectionPanel.SetActive(true);

        GenerateSchoolButtons();

        Debug.Log("SchoolSelectionMenu activado por IntroManager");
    }

    void GenerateSchoolButtons()
    {
        if (cardManager == null || schoolButtonsContainer == null || schoolButtonPrefab == null)
        {
            Debug.LogError("Faltan referencias en SchoolSelectionMenu");
            Debug.LogError($"CardManager: {(cardManager != null ? "OK" : "NULL")}");
            Debug.LogError($"Container: {(schoolButtonsContainer != null ? "OK" : "NULL")}");
            Debug.LogError($"Prefab: {(schoolButtonPrefab != null ? "OK" : "NULL")}");
            return;
        }

        // Limpiar botones anteriores
        foreach (GameObject btn in spawnedButtons)
        {
            Destroy(btn);
        }
        spawnedButtons.Clear();

        Debug.Log($"Generando {cardManager.schools.Count} botones de escuelas...");

        // Crear un botón por cada escuela
        for (int i = 0; i < cardManager.schools.Count; i++)
        {
            SchoolData school = cardManager.schools[i];

            if (school == null)
            {
                Debug.LogWarning($"Escuela en índice {i} es null");
                continue;
            }

            GameObject buttonObj = Instantiate(schoolButtonPrefab, schoolButtonsContainer);
            buttonObj.SetActive(true); // Asegurar que esté activo
            spawnedButtons.Add(buttonObj);

            // Configurar texto del botón
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = school.schoolName;
                Debug.Log($"Botón creado: {school.schoolName}");
            }
            else
            {
                Debug.LogWarning($"No se encontró TextMeshProUGUI en el botón de {school.schoolName}");
            }

            // Configurar icono (opcional)
            Transform iconTransform = buttonObj.transform.Find("Icon");
            if (iconTransform != null)
            {
                Image buttonIcon = iconTransform.GetComponent<Image>();
                if (buttonIcon != null && school.schoolIcon != null)
                    buttonIcon.sprite = school.schoolIcon;
            }

            // Asignar evento de click
            Button button = buttonObj.GetComponent<Button>();
            if (button != null)
            {
                // IMPORTANTE: Capturar el índice para evitar problemas de closure
                int index = i;
                button.onClick.RemoveAllListeners(); // Limpiar listeners anteriores
                button.onClick.AddListener(() => OnSchoolSelected(cardManager.schools[index]));

                Debug.Log($"Evento onClick asignado a {school.schoolName}");
            }
            else
            {
                Debug.LogError($"No se encontró componente Button en el prefab para {school.schoolName}");
            }
        }

        Debug.Log($"Se generaron {spawnedButtons.Count} botones correctamente");
    }

    public void OnSchoolSelected(SchoolData school)
    {
        if (school == null)
        {
            Debug.LogError("Escuela seleccionada es null!");
            return;
        }

        selectedSchool = school;

        Debug.Log($"Escuela seleccionada: {school.schoolName}");

        if (selectedSchoolText != null)
            selectedSchoolText.text = $"Escuela seleccionada: {school.schoolName}";

        UpdatePreviewPanel(school);

        if (startGameButton != null)
        {
            startGameButton.interactable = true;
            Debug.Log("Botón de inicio activado");
        }
    }

    void UpdatePreviewPanel(SchoolData school)
    {
        if (schoolNameText != null)
            schoolNameText.text = school.schoolName;

        if (schoolDescriptionText != null)
            schoolDescriptionText.text = school.schoolDescription;

        if (schoolIconImage != null && school.schoolIcon != null)
            schoolIconImage.sprite = school.schoolIcon;

        if (cienciaCountText != null)
            cienciaCountText.text = $"Ciencia: {school.cards.cienciaCards.Count} cartas";

        if (tecnologiaCountText != null)
            tecnologiaCountText.text = $"Tecnología: {school.cards.tecnologiaCards.Count} cartas";

        if (innovacionCountText != null)
            innovacionCountText.text = $"Innovación: {school.cards.innovacionCards.Count} cartas";
    }

    public void StartGame()
    {
        if (selectedSchool == null)
        {
            Debug.LogWarning("No se ha seleccionado ninguna escuela");
            return;
        }

        // Notificar a IntroManager que se seleccionó la escuela
        if (IntroManager.Instance != null)
        {
            IntroManager.Instance.OnSchoolSelected(selectedSchool);
        }
        else
        {
            // Fallback: Si no hay IntroManager, iniciar directamente
            Debug.LogWarning("IntroManager no encontrado, iniciando juego directamente");
            StartGameDirectly();
        }
    }

    // Método de respaldo por si no se usa IntroManager
    void StartGameDirectly()
    {
        if (cardManager != null)
        {
            CardSpawner.Instance?.SetupSchools(selectedSchool, cardManager);
        }

        if (selectionPanel != null)
            selectionPanel.SetActive(false);

        if (gameplayPanel != null)
            gameplayPanel.SetActive(true);

        

        Debug.Log($"Juego iniciado directamente con escuela: {selectedSchool.schoolName}");
    }

    public SchoolData GetSelectedSchool() => selectedSchool;
}