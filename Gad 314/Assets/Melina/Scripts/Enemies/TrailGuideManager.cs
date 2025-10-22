using UnityEngine;

public class TrailGuideManager : MonoBehaviour
{
    public GameObject[] guideObjects;
    public float glowIntensity = 3f;
    public Color glowColor = Color.cyan;

    void Start()
    {
        foreach (var obj in guideObjects)
        {
            if (obj.TryGetComponent<Light>(out Light light))
            {
                light.color = glowColor;
                light.intensity = glowIntensity;
                light.range = 5f;
            }
        }
    }

    public void HideTrail()
    {
        foreach (var obj in guideObjects)
            obj.SetActive(false);
    }
}