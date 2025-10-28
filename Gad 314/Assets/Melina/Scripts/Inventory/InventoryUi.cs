using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Transform slotContainer;
    public GameObject slotPrefab;

    private bool isOpen = false;
    private PlayerInventory playerInventory;

    void Start()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
        inventoryPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isOpen = !isOpen;
            inventoryPanel.SetActive(isOpen);
        }
    }

    public void RefreshInventory(List<InventoryItem> items)
    {
        foreach (Transform c in slotContainer) Destroy(c.gameObject);
        for (int i = 0; i < playerInventory.maxSlots; i++)
        {
            GameObject slot = Instantiate(slotPrefab, slotContainer);
            InventorySlotUI slotUI = slot.GetComponent<InventorySlotUI>();
            if (i < items.Count) slotUI.Setup(items[i], playerInventory);
            else slotUI.SetupEmpty(playerInventory);
        }
    }
}