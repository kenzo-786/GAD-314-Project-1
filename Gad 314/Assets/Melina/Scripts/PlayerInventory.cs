using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Dictionary<string, int> inventory = new Dictionary<string, int>();
    private InventoryUI inventoryUI;

    void Start()
    {
        inventoryUI = FindObjectOfType<InventoryUI>();
    }

    public void AddItem(string itemName)
    {
        if (inventory.ContainsKey(itemName))
            inventory[itemName]++;
        else
            inventory[itemName] = 1;

        if (inventoryUI != null)
            inventoryUI.UpdateInventoryDisplay(inventory);
    }
}