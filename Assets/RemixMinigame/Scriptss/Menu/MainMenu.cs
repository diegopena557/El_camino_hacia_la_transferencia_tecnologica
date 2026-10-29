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
}