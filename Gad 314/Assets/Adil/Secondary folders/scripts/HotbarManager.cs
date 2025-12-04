using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class HotbarManager : MonoBehaviour
{
    public static HotbarManager Instance;

    [Header("UI References")]
    public Transform hotbarPanel;
    public GameObject slotPrefab;

    [Header("Selection Visuals")]
    public Color selectedColor = Color.yellow;
    public Color normalColor = new Color(1f, 1f, 1f, 0.5f);

    private List<HotbarSlot> _uiSlots = new List<HotbarSlot>();
    private int _selectedIndex = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        foreach (Transform child in hotbarPanel)
        {
            HotbarSlot slot = child.GetComponent<HotbarSlot>();
            if (slot == null) slot = child.gameObject.AddComponent<HotbarSlot>();
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectSlot(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SelectSlot(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) SelectSlot(5);

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            _selectedIndex--;
            if (_selectedIndex < 0) _selectedIndex = _uiSlots.Count - 1;
            SelectSlot(_selectedIndex);
        }
        else if (scroll < 0f)
        {
            _selectedIndex++;
            if (_selectedIndex >= _uiSlots.Count) _selectedIndex = 0;
            SelectSlot(_selectedIndex);
        }
    }

    private void SelectSlot(int index)
    {
        if (index < 0 || index >= _uiSlots.Count) return;

        _selectedIndex = index;
        RefreshUI();
    }

    public ItemData GetSelectedItem()
    {
        var inventory = InventoryManager.Instance.inventory;
        int i = 0;
        foreach (var itemPair in inventory)
        {
            if (i == _selectedIndex) return itemPair.Key;
            i++;
        }
        return null;
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

        for (int i = 0; i < _uiSlots.Count; i++)
        {
            Image bg = _uiSlots[i].GetComponent<Image>();
            if (bg)
            {
                bg.color = (i == _selectedIndex) ? selectedColor : normalColor;
            }
        }
    }
}
