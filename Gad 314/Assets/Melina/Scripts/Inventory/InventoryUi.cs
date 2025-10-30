using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform slotContainer;
    public GameObject slotPrefab;

    void Start()
    {
        if (slotContainer == null || slotPrefab == null)
        {
            Debug.LogError("InventoryUI not set correctly in Inspector!");
            return;
        }

        gameObject.SetActive(true);
        Debug.Log("Inventory UI initialized and always visible.");
    }

    public void RefreshInventory(List<InventoryItem> items)
    {
        foreach (Transform child in slotContainer)
            Destroy(child.gameObject);

        foreach (var item in items)
        {
            GameObject slot = Instantiate(slotPrefab, slotContainer);
            InventorySlotUI slotUI = slot.GetComponent<InventorySlotUI>();
            slotUI.SetItem(item);
        }
    }
}