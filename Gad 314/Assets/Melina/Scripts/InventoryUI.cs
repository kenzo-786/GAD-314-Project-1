using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Transform slotContainer;
    public GameObject slotPrefab;
    private bool isOpen = false;

    private PlayerInventory playerInventory;

    void Start()
    {
        inventoryPanel.SetActive(false);
        playerInventory = FindObjectOfType<PlayerInventory>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);
    }

    public void RefreshInventory(List<InventoryItem> items)
    {
        foreach (Transform child in slotContainer)
            Destroy(child.gameObject);

        foreach (var item in items)
        {
            GameObject slot = Instantiate(slotPrefab, slotContainer);
            InventorySlotUI slotScript = slot.GetComponent<InventorySlotUI>();
            slotScript.Setup(item, playerInventory);
        }
    }
}