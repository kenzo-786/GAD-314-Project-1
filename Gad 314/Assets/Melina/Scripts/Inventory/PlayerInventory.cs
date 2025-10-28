using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int maxSlots = 5;
    public List<InventoryItem> items = new List<InventoryItem>();
    public Transform dropPoint;
    public GameObject droppedPrefabTemplate;
    public InventoryUI inventoryUI;

    void Start()
    {
        if (inventoryUI != null)
            inventoryUI.RefreshInventory(items);
    }

    public bool AddItem(InventoryItem item)
    {
        if (items.Count >= maxSlots) return false;
        items.Add(item);
        Debug.Log($"Added item: {item.itemName}");
        if (inventoryUI != null)
            inventoryUI.RefreshInventory(items);
        return true;
    }

    public void RemoveItem(InventoryItem item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            if (inventoryUI != null)
                inventoryUI.RefreshInventory(items);
        }
    }

    public void DropItem(InventoryItem item)
    {
        if (!items.Contains(item)) return;
        RemoveItem(item);

        GameObject prefabToSpawn = droppedPrefabTemplate;
        if (prefabToSpawn == null) return;

        Vector3 pos = dropPoint != null
            ? dropPoint.position
            : transform.position + transform.forward * 1.5f + Vector3.up * 0.5f;

        GameObject dropped = Instantiate(prefabToSpawn, pos, Quaternion.identity);
        ItemFloat floatScript = dropped.GetComponent<ItemFloat>();
        if (floatScript != null) floatScript.Initialize(item);
        Rigidbody rb = dropped.GetComponent<Rigidbody>();
        if (rb != null) rb.AddForce(transform.forward * 2f + Vector3.up * 2f, ForceMode.Impulse);
    }
}