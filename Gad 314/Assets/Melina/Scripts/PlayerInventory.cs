using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string name;
    public Sprite icon;
    public int quantity = 1;
    public GameObject worldPrefab;
}

public class PlayerInventory : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();
    public InventoryUI inventoryUI;
    public Transform dropPoint;

    void Start()
    {
        if (inventoryUI != null)
            inventoryUI.RefreshInventory(items);
    }

    public void AddItem(string itemName, Sprite itemIcon, GameObject worldPrefab = null)
    {
        InventoryItem existingItem = items.Find(i => i.name == itemName);

        if (existingItem != null)
        {
            existingItem.quantity++;
        }
        else
        {
            InventoryItem newItem = new InventoryItem
            {
                name = itemName,
                icon = itemIcon,
                quantity = 1,
                worldPrefab = worldPrefab
            };
            items.Add(newItem);
        }

        if (inventoryUI != null)
            inventoryUI.RefreshInventory(items);
    }

    public void DropItem(InventoryItem item)
    {
        if (item == null) return;

        item.quantity--;
        if (item.quantity <= 0)
            items.Remove(item);

        if (item.worldPrefab != null && dropPoint != null)
        {
            Vector3 dropPos = dropPoint.position + dropPoint.forward * 0.8f + Vector3.up * 0.5f;
            GameObject dropped = Instantiate(item.worldPrefab, dropPos, Quaternion.identity);

            Rigidbody rb = dropped.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddForce(dropPoint.forward * 2f, ForceMode.Impulse);
        }

        if (inventoryUI != null)
            inventoryUI.RefreshInventory(items);
    }
}