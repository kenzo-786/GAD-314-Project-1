using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string itemName;
    public Sprite icon;
    public int quantity;

    public InventoryItem(string name, Sprite icon)
    {
        this.itemName = name;
        this.icon = icon;
        this.quantity = 1;
    }
}

public class PlayerInventory : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();
    private InventoryUI inventoryUI;

    void Start()
    {
        inventoryUI = FindObjectOfType<InventoryUI>();
    }

    public void AddItem(string itemName, Sprite itemIcon)
    {
        InventoryItem existingItem = items.Find(i => i.itemName == itemName);

        if (existingItem != null)
            existingItem.quantity++;
        else
            items.Add(new InventoryItem(itemName, itemIcon));

        if (inventoryUI != null)
            inventoryUI.RefreshInventory(items);
    }
}