using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlickeringLight : MonoBehaviour
{
    public Light2D lightSource;
    public float minIntensity = 0.5f;
    public float maxIntensity = 4f;
    public float flickerSpeed = 1.5f;
    private float randomOffset;

    void Start()
    {
        if (lightSource == null)
        {
            lightSource = GetComponent<Light2D>();
        }
        randomOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        if (lightSource != null)
        {
            float noise = Mathf.PerlinNoise(Time.time * flickerSpeed + randomOffset, 0);
            lightSource.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
        }
    }
}