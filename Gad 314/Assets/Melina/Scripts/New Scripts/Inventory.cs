using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemStack
{
    public ItemSO item;
    public int quantity;

    public ItemStack(ItemSO item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}

public class Inventory : MonoBehaviour
{
    public List<ItemStack> items = new List<ItemStack>();
    public event Action<ItemStack> OnItemAdded;

    public void AddItem(ItemSO item, int amount = 1)
    {
        bool added = false;

        if (item.stackable)
        {
            foreach (ItemStack stack in items)
            {
                if (stack.item == item && stack.quantity < item.maxStack)
                {
                    int spaceLeft = item.maxStack - stack.quantity;
                    int toAdd = Mathf.Min(spaceLeft, amount);
                    stack.quantity += toAdd;
                    amount -= toAdd;

                    OnItemAdded?.Invoke(new ItemStack(item, toAdd));

                    if (amount <= 0)
                    {
                        added = true;
                        break;
                    }
                }
            }
        }

        while (amount > 0)
        {
            int toAdd = item.stackable ? Mathf.Min(amount, item.maxStack) : 1;
            ItemStack newStack = new ItemStack(item, toAdd);
            items.Add(newStack);
            amount -= toAdd;

            OnItemAdded?.Invoke(newStack);
            added = true;
        }
    }
}