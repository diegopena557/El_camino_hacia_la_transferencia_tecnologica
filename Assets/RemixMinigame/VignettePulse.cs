using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignettePulse : MonoBehaviour
{
    public Volume volume;

    [Header("Animación")]
    public float velocidad = 1f;
    public float intensidadMin = 0.462f;
    public float intensidadMax = 0.537f;

    private Vignette vignette;

    void Start()
    {
        if (volume.profile.TryGet(out vignette))
        {
            vignette.intensity.overrideState = true;
        }
    }

    void Update()
    {
        if (vignette == null)
            return;

        float t = (Mathf.Sin(Time.time * velocidad) + 1f) / 2f;

        vignette.intensity.value = Mathf.Lerp(
            intensidadMin,
            intensidadMax,
            t
        );
    }
}