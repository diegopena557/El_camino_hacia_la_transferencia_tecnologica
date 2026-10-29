using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SceneNameOverride
{
    [Tooltip("Nombre EXACTO de la escena (como aparece en Build Settings).")]
    public string sceneName;

    [Tooltip("Texto que se mostrará en el botón.")]
    public string displayName;
}

public class SceneSelectorPanel : MonoBehaviour
{
    [Header("Referencias")]
    public SceneTransition sceneTransition;
    public GameObject panel;              // El panel completo (se activa/desactiva)
    public Transform contentParent;       // Content del ScrollView
    public Button sceneButtonPrefab;      // Prefab de botón con un TMP_Text hijo

    [Tooltip("Opcional: el panel/objeto con los botones del menú principal. Se oculta al abrir el selector y reaparece al cerrarlo.")]
    public GameObject mainMenuPanel;

    [Header("Origen de las escenas")]
    [Tooltip("Si está activo, lee todas las escenas del Build Settings.")]
    public bool loadFromBuildSettings = true;

    [Tooltip("Se usa si loadFromBuildSettings está desactivado.")]
    public List<string> manualScenes = new List<string>();

    [Tooltip("Escenas que NO deben aparecer en la lista (ej. el propio menú).")]
    public List<string> excludedScenes = new List<string> { "MainMenu" };

    [Header("Nombres personalizados (opcional)")]
    [Tooltip("Si una escena no está aquí, se muestra su nombre normal.")]
    public List<SceneNameOverride> nameOverrides = new List<SceneNameOverride>();

    bool built;

    void Start()
    {
        if (panel != null) panel.SetActive(false);
    }

    public void Open()
    {
        if (!built) BuildButtons();
        panel.SetActive(true);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
    }

    public void Close()
    {
        panel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }

    void BuildButtons()
    {
        built = true;

        foreach (string sceneName in GetSceneNames())
        {
            if (excludedScenes.Contains(sceneName)) continue;

            Button btn = Instantiate(sceneButtonPrefab, contentParent);
            btn.gameObject.SetActive(true);

            TMP_Text label = btn.GetComponentInChildren<TMP_Text>();
            if (label != null) label.text = GetDisplayName(sceneName);

            string target = sceneName; // copia local para el closure
            btn.onClick.AddListener(() => sceneTransition.LoadScene(target));
        }
    }

    List<string> GetSceneNames()
    {
        if (!loadFromBuildSettings) return manualScenes;

        var names = new List<string>();
        int count = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < count; i++)
        {
            string path = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
            names.Add(Path.GetFileNameWithoutExtension(path));
        }
        return names;
    }

    string GetDisplayName(string sceneName)
    {
        foreach (var entry in nameOverrides)
        {
            if (entry.sceneName == sceneName && !string.IsNullOrWhiteSpace(entry.displayName))
                return entry.displayName;
        }
        return Prettify(sceneName);
    }

    // "Nivel_01" -> "Nivel 01"
    string Prettify(string sceneName)
    {
        return sceneName.Replace("_", " ");
    }
}