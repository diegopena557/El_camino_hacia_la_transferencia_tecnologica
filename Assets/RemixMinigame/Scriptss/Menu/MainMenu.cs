using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public SceneTransition sceneTransition;
    public SceneSelectorPanel sceneSelector;

    public void StartGame()
    {
        sceneTransition.LoadScene("Dialogue");
    }

    public void OpenSceneSelector()
    {
        sceneSelector.Open();
    }

    public void CloseSceneSelector()
    {
        sceneSelector.Close();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}