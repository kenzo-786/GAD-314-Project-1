using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public TextMeshProUGUI inventoryText;

    void Start()
    {
        if (inventoryText != null)
            inventoryText.text = "Inventory:\n";
    }

    public void UpdateInventoryDisplay(Dictionary<string, int> inventory)
    {
        if (inventoryText == null) return;

        inventoryText.text = "Inventory:\n";

        foreach (var item in inventory)
        {
            inventoryText.text += $"{item.Key} x{item.Value}\n";
        }
    }
}