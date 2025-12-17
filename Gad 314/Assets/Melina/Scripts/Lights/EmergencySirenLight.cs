using UnityEngine;

public class EmergencySirenLight : MonoBehaviour
{
    public Light sirenLight;

    [Header("Light Settings")]
    public float rotationSpeed = 120f;
    public float minIntensity = 0f;
    public float maxIntensity = 3f;
    public float flickerSpeed = 5f;

    [Header("Audio Settings")]
    public AudioSource sirenAudio;
    public Transform player;
    public float maxHearingDistance = 15f;
    public float fadeSpeed = 3f;

    void Start()
    {
        if (sirenLight == null)
            sirenLight = GetComponent<Light>();

        if (sirenAudio == null)
            sirenAudio = GetComponent<AudioSource>();

        sirenLight.color = Color.red;

        sirenAudio.loop = true;
        sirenAudio.volume = 0f;
        sirenAudio.spatialBlend = 1f;
        sirenAudio.Play();
    }

    void Update()
    {
        RotateLight();
        FlickerLight();
        UpdateSirenAudio();
    }

    void RotateLight()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    void FlickerLight()
    {
        float pulse = (Mathf.Sin(Time.time * flickerSpeed) + 1f) * 0.5f;
        sirenLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, pulse);
    }

    void UpdateSirenAudio()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        float targetVolume = distance <= maxHearingDistance
            ? 1f - (distance / maxHearingDistance)
            : 0f;

        sirenAudio.volume = Mathf.Lerp(
            sirenAudio.volume,
            targetVolume,
            Time.deltaTime * fadeSpeed
        );
    }
}