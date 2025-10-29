using UnityEngine;

public class ItemFloat : MonoBehaviour
{
    public InventoryItem itemData;
    public float floatSpeed = 2f;
    public float floatHeight = 0.2f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.position = startPos + Vector3.up * Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.Rotate(Vector3.up * 60f * Time.deltaTime, Space.World);
    }

    public void Initialize(InventoryItem data)
    {
        itemData = data;
    }
}