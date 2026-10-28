using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public SceneTransition sceneTransition;

    public void StartGame()
    {
        sceneTransition.LoadScene("Dialogue");
    }
}
