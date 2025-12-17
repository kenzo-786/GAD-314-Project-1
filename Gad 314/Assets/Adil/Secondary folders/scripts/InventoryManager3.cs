using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager3 : MonoBehaviour
{
    [Header("Configuration")]
    public int slotCount = 6;
    public Color selectedColor = Color.cyan;
    public Color normalColor = Color.white;

    [Header("Item Definitions ")]
    public ItemDefinition[] allItems;

    [Header("UI References")]
    public Transform hotbarPanel;
    public Image[] slotImages;
    public TextMeshProUGUI[] countTexts;

    private InventorySlot[] slots;
    private int selectedSlotIndex = 0;

    private void Start()
    {
        slots = new InventorySlot[slotCount];
        for (int i = 0; i < slotCount; i++) slots[i] = new InventorySlot();

        if (hotbarPanel != null)
        {
            if (slotImages == null || slotImages.Length == 0)
            {
                slotImages = new Image[slotCount];
                countTexts = new TextMeshProUGUI[slotCount];
                for (int i = 0; i < slotCount; i++)
                {
                    if (i < hotbarPanel.childCount)
                    {
                        Transform child = hotbarPanel.GetChild(i);
                        slotImages[i] = child.GetComponent<Image>();
                        countTexts[i] = child.GetComponentInChildren<TextMeshProUGUI>();
                    }
                }
            }
        }

        UpdateUI();
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
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
            selectedSlotIndex--;
            if (selectedSlotIndex < 0) selectedSlotIndex = slotCount - 1;
            SelectSlot(selectedSlotIndex);
        }
        else if (scroll < 0f)
        {
            selectedSlotIndex++;
            if (selectedSlotIndex >= slotCount) selectedSlotIndex = 0;
            SelectSlot(selectedSlotIndex);
        }
    }

    public void AddItem(ItemType type, int amount)
    {
        for (int i = 0; i < slotCount; i++)
        {
            if (slots[i].type == type && slots[i].count > 0)
            {
                slots[i].count += amount;
                UpdateUI();
                return;
            }
        }

        for (int i = 0; i < slotCount; i++)
        {
            if (slots[i].count == 0)
            {
                slots[i].type = type;
                slots[i].count = amount;
                UpdateUI();
                return;
            }
        }

        Debug.Log("Inventory Full!");
    }

    public ItemType ConsumeSelectedItem()
    {
        InventorySlot current = slots[selectedSlotIndex];

        if (current.count > 0)
        {
            current.count--;
            ItemType usedType = current.type;

            if (current.count <= 0)
            {
                current.type = ItemType.None; // Clear
            }

            UpdateUI();
            return usedType;
        }
        return ItemType.None;
    }

    public ItemType GetSelectedType()
    {
        if (slots[selectedSlotIndex].count > 0) return slots[selectedSlotIndex].type;
        return ItemType.None;
    }

    private void SelectSlot(int index)
    {
        selectedSlotIndex = index;
        UpdateUI();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < slotCount; i++)
        {
            if (i >= slotImages.Length) break;

            if (i == selectedSlotIndex)
                slotImages[i].color = selectedColor;
            else
                slotImages[i].color = normalColor;

            if (slots[i].count > 0)
            {
                Sprite icon = GetIconForType(slots[i].type);
                slotImages[i].sprite = icon;
                slotImages[i].enabled = true;

                slotImages[i].preserveAspect = true;

                if (countTexts[i] != null)
                    countTexts[i].text = slots[i].count.ToString();
            }
            else
            {
                slotImages[i].sprite = null;
                slotImages[i].enabled = false;

                if (countTexts[i] != null)
                    countTexts[i].text = "";
            }
        }
    }

    private Sprite GetIconForType(ItemType type)
    {
        foreach (var item in allItems)
        {
            if (item.type == type) return item.icon;
        }
        return null;
    }

    [System.Serializable]
    public class ItemDefinition
    {
        public string name;
        public ItemType type;
        public Sprite icon;
    }

    public class InventorySlot
    {
        public ItemType type;
        public int count;
    }

    public enum ItemType
    {
        None,
        Egg,
        Crystal,
        Volcanic,
        Flower,
        Stone,
        Radar
    }


}
    

    
