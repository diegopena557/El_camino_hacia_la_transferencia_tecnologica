using UnityEngine;
using UnityEngine.EventSystems;

public class HoverScaleRawImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float hoverScale = 1.15f;
    [SerializeField] private float speed = 8f;

    private Vector3 initialScale;
    private Vector3 targetScale;

    private void Start()
    {
        initialScale = transform.localScale;
        targetScale = initialScale;
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.deltaTime * speed
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = initialScale * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = initialScale;
    }
}