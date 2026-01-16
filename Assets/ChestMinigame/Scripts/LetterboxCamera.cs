using UnityEngine;

public class LetterboxCamera : MonoBehaviour
{
    void Start()
    {
        float targetAspect = 16f / 9f;
        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        Camera cam = GetComponent<Camera>();

        if (scaleHeight < 1f)
        {
            cam.rect = new Rect(0, (1 - scaleHeight) / 2, 1, scaleHeight);
        }
        else
        {
            float scaleWidth = 1f / scaleHeight;
            cam.rect = new Rect((1 - scaleWidth) / 2, 0, scaleWidth, 1);
        }
    }
}

