using UnityEngine;

public class EmergencySirenLight : MonoBehaviour
{
    public Light sirenLight;
    public float rotationSpeed = 120f;
    public float minIntensity = 0f;
    public float maxIntensity = 3f;
    public float flickerSpeed = 5f;

    void Start()
    {
        if (sirenLight == null)
            sirenLight = GetComponent<Light>();

        sirenLight.color = Color.red;
    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        float pulse = (Mathf.Sin(Time.time * flickerSpeed) + 1f) * 0.5f;
        sirenLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, pulse);
    }
}