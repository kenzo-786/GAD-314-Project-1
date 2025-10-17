using UnityEngine;

public class ItemFloat : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float floatHeight = 0.25f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.position = startPos + Vector3.up * Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.Rotate(Vector3.up * 50 * Time.deltaTime);
    }
}