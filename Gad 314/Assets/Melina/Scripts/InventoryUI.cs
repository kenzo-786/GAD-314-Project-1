using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Transform slotContainer;
    public GameObject slotPrefab;

    private bool isOpen = false;

    void Start()
    {
        inventoryPanel.SetActive(false);
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

            Image icon = slot.transform.Find("ItemIcon").GetComponent<Image>();
            TextMeshProUGUI quantityText = slot.transform.Find("QuantityText").GetComponent<TextMeshProUGUI>();

            // Remove any default white box
            icon.sprite = item.icon;
            icon.color = Color.white;
            icon.type = Image.Type.Simple;
            icon.preserveAspect = true;
            icon.enabled = true;

            quantityText.text = item.quantity > 1 ? item.quantity.ToString() : "";
        }
    }
}