using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InstructionsPanel : MonoBehaviour
{
    [Header("Referencias")]
    public CanvasGroup canvasGroup;
    public Button continueButton;
    public GameObject instructionsPanel;

    [Header("Configuración de Fade")]
    public float fadeDuration = 1f;
    public bool pauseGameDuringInstructions = true;

    [Header("Audio (Opcional)")]
    public AudioSource audioSource;
    public AudioClip buttonClickSound;

    private bool isFading = false;

    void Start()
    {
        // Asegurar que el panel esté visible al inicio
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(true);
        }

        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }

        // Asegurar que esté completamente visible
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        // Pausar el juego si está configurado
        if (pauseGameDuringInstructions)
        {
            Time.timeScale = 0f;
            Debug.Log("[InstructionsPanel] Juego pausado durante instrucciones");
        }

        // Conectar el botón
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(OnContinueClicked);
            Debug.Log("[InstructionsPanel] Botón de continuar conectado");
        }
        else
        {
            Debug.LogWarning("[InstructionsPanel] No hay botón de continuar asignado!");
        }
    }

    public void OnContinueClicked()
    {
        if (isFading) return;

        Debug.Log("[InstructionsPanel] Botón continuar presionado");

        // Reproducir sonido si hay
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }

        StartCoroutine(FadeOutAndClose());
    }

    IEnumerator FadeOutAndClose()
    {
        isFading = true;

        // Deshabilitar interacción inmediatamente
        canvasGroup.interactable = false;
        if (continueButton != null)
        {
            continueButton.interactable = false;
        }

        Debug.Log("[InstructionsPanel] Iniciando fade out...");

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            // Usar unscaledDeltaTime porque el juego podría estar en pausa
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / fadeDuration;

            // Fade out suave
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

            yield return null;
        }

        // Asegurar que esté completamente transparente
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;

        Debug.Log("[InstructionsPanel] Fade out completado");

        // Desactivar el panel
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }

        // Reanudar el juego si estaba pausado
        if (pauseGameDuringInstructions)
        {
            Time.timeScale = 1f;
            Debug.Log("[InstructionsPanel] Juego reanudado");
        }

        isFading = false;
    }

    // Método público para cerrar las instrucciones desde otro script
    public void CloseInstructions()
    {
        if (!isFading)
        {
            StartCoroutine(FadeOutAndClose());
        }
    }

    // Método para mostrar las instrucciones de nuevo (útil para debug o menú)
    public void ShowInstructions()
    {
        if (isFading)
        {
            StopAllCoroutines();
            isFading = false;
        }

        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(true);
        }
        else
        {
            gameObject.SetActive(true);
        }

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        if (continueButton != null)
        {
            continueButton.interactable = true;
        }

        if (pauseGameDuringInstructions)
        {
            Time.timeScale = 0f;
        }

        Debug.Log("[InstructionsPanel] Instrucciones mostradas");
    }

    void OnDestroy()
    {
        // Asegurar que el juego se reanude si se destruye el objeto
        if (pauseGameDuringInstructions && Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }

        // Desconectar el botón
        if (continueButton != null)
        {
            continueButton.onClick.RemoveListener(OnContinueClicked);
        }
    }
}