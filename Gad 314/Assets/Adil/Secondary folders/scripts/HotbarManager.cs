using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class HotbarManager : MonoBehaviour
{
    [Header("UI References")]
    public Transform hotbarPanel;
    public GameObject slotPrefab;

    private List<HotbarSlot> _uiSlots = new List<HotbarSlot>();

    private void Start()
    {
        foreach (Transform child in hotbarPanel)
        {
            HotbarSlot slot = child.GetComponent<HotbarSlot>();
            if (slot == null)
            {
                slot = child.gameObject.AddComponent<HotbarSlot>();
            }
            _uiSlots.Add(slot);
        }

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged += RefreshUI;
            RefreshUI();
        }
    }

    private void OnDestroy()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged -= RefreshUI;
        }
    }

    private void RefreshUI()
    {
        foreach (var slot in _uiSlots) slot.Clear();

        var inventory = InventoryManager.Instance.inventory;
        int slotIndex = 0;

        foreach (var itemPair in inventory)
        {
            if (slotIndex >= _uiSlots.Count) break;

            ItemData data = itemPair.Key;
            int count = itemPair.Value;

            _uiSlots[slotIndex].Setup(data, count);
            slotIndex++;
        }
    }
}
