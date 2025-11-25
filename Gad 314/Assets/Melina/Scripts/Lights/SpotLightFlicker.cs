using UnityEngine;

public class SpotLightFlicker : MonoBehaviour
{
    public Light lightSource;
    public float minIntensity = 0.5f;
    public float maxIntensity = 2f;
    public float flickerSpeed = 0.1f;

    void Start()
    {
        if (lightSource == null)
            lightSource = GetComponent<Light>();
    }

    void Update()
    {
        float noise = Mathf.PerlinNoise(Time.time / flickerSpeed, 0f);
        lightSource.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
    }
}