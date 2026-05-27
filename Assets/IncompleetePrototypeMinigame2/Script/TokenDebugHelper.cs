using UnityEngine;

public class TokenDebugHelper : MonoBehaviour
{
    [Header("Teclas de Debug")]
    [Tooltip("Presiona esta tecla para desbloquear TODOS los tokens")]
    public KeyCode resetAllTokensKey = KeyCode.R;

    [Tooltip("Presiona esta tecla para ver el estado de todos los tokens")]
    public KeyCode showStatusKey = KeyCode.T;

    void Update()
    {
        // Resetear todos los tokens
        if (Input.GetKeyDown(resetAllTokensKey))
        {
            ResetAllTokens();
        }

        // Mostrar estado de todos los tokens
        if (Input.GetKeyDown(showStatusKey))
        {
            ShowAllTokensStatus();
        }
    }

    void ResetAllTokens()
    {
        Debug.Log("=======================================");
        Debug.Log("[TokenDebugHelper] RESETEANDO TODOS LOS TOKENS");
        Debug.Log("=======================================");

        ProjectToken[] allTokens = FindObjectsOfType<ProjectToken>();

        if (allTokens.Length == 0)
        {
            Debug.LogWarning("[TokenDebugHelper] No se encontraron tokens en la escena");
            return;
        }

        int resettedCount = 0;
        foreach (ProjectToken token in allTokens)
        {
            if (token != null)
            {
                token.ForceReset();
                resettedCount++;
            }
        }

        Debug.Log($"[TokenDebugHelper] OK - {resettedCount} tokens reseteados exitosamente");
        Debug.Log("=======================================");
    }

    void ShowAllTokensStatus()
    {
        Debug.Log("=======================================");
        Debug.Log("[TokenDebugHelper] ESTADO DE TODOS LOS TOKENS");
        Debug.Log("=======================================");

        ProjectToken[] allTokens = FindObjectsOfType<ProjectToken>();

        if (allTokens.Length == 0)
        {
            Debug.LogWarning("[TokenDebugHelper] No se encontraron tokens en la escena");
            return;
        }

        foreach (ProjectToken token in allTokens)
        {
            if (token != null)
            {
                // Usar reflection para acceder a variables privadas
                var type = token.GetType();
                var isDraggingField = type.GetField("isDragging", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var isPlacedField = type.GetField("isPlaced", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var currentSlotField = type.GetField("currentSlot", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                bool isDragging = isDraggingField != null ? (bool)isDraggingField.GetValue(token) : false;
                bool isPlaced = isPlacedField != null ? (bool)isPlacedField.GetValue(token) : false;
                TokenSlot currentSlot = currentSlotField != null ? (TokenSlot)currentSlotField.GetValue(token) : null;

                string slotName = currentSlot != null ? currentSlot.gameObject.name : "null";
                string status = (isDragging || (isPlaced && currentSlot == null)) ? "WARNING POSIBLE BLOQUEO" : "OK";

                Debug.Log($"{status} | {token.GetTokenName()} | isDragging:{isDragging} | isPlaced:{isPlaced} | slot:{slotName}");
            }
        }

        Debug.Log("=======================================");
    }
}