using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrailGuideManager : MonoBehaviour
{
    public Transform player;
    public Transform pressurePlate;
    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = true;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

    void Update()
    {
        if (player != null && pressurePlate != null && lineRenderer.enabled)
        {
            Vector3 start = player.position + Vector3.up * 0.2f;
            Vector3 end = pressurePlate.position + Vector3.up * 0.2f;
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
        }
    }

    public void HideTrail()
    {
        if (lineRenderer != null)
            lineRenderer.enabled = false;
    }
}