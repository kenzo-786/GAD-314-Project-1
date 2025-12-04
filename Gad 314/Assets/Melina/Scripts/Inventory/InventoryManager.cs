using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public Dictionary<ItemData, int> inventory = new Dictionary<ItemData, int>();

    public event Action OnInventoryChanged;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(ItemData item, int amount = 1)
    {
        if (item == null) return;

        if (inventory.ContainsKey(item))
        {
            inventory[item] += amount;
        }
        else
        {
            inventory.Add(item, amount);
        }

        Debug.Log($"Added {amount}x {item.displayName}. Total: {inventory[item]}");

        OnInventoryChanged?.Invoke();
    }

    public bool HasItem(ItemData item, int requiredAmount)
    {
        if (inventory.ContainsKey(item))
        {
            return inventory[item] >= requiredAmount;
        }
        return false;
    }

    public void RemoveItem(ItemData item, int amount = 1)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item] -= amount;
            if (inventory[item] <= 0)
            {
                inventory.Remove(item);
            }
            OnInventoryChanged?.Invoke();
        }
    }
}