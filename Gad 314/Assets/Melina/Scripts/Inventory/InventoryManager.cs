using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<InventoryItem> items = new List<InventoryItem>();
    public InventoryUI inventoryUI;

    void Awake()
    {
        Instance = this;
    }

    public void AddItem(InventoryItem item)
    {
        items.Add(item);
        inventoryUI.RefreshInventory(items);
    }
}