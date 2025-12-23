using UnityEngine;
using TMPro;

public class JustificationPanel : MonoBehaviour
{
    [Header("UI Root")]
    public GameObject panelRoot;

    [Header("Text")]
    public TextMeshProUGUI justificationText;

    void Awake()
    {
        panelRoot.SetActive(false);
    }

    public void Show(string justification)
    {
        // Pausa el juego
        Time.timeScale = 0f;

        justificationText.text = justification;

        // Activa TODO el panel (fondo + texto + botón)
        panelRoot.SetActive(true);
    }

    public void Hide()
    {
        // Oculta TODO
        panelRoot.SetActive(false);

        // Reanuda el juego
        Time.timeScale = 1f;
    }
}
