using UnityEngine;
using UnityEngine.UI;

public class InventoryUi : MonoBehaviour
{
    public Inventory inventory;
    public Transform slotParent;
    public GameObject slotPrefab;

    private void Start()
    {
        if (inventory == null || slotParent == null || slotPrefab == null) return;
        inventory.OnItemAdded += AddSlotForItem;
    }

    private void OnDestroy()
    {
        if (inventory != null)
            inventory.OnItemAdded -= AddSlotForItem;
    }

    private void AddSlotForItem(ItemStack stack)
    {
        GameObject slot = Instantiate(slotPrefab, slotParent);
        Transform iconTransform = slot.transform.Find("Icon");
        Transform countTransform = slot.transform.Find("CountText");

        if (iconTransform != null)
        {
            Image icon = iconTransform.GetComponent<Image>();
            icon.sprite = stack.item.icon;
            icon.enabled = true;
        }

        if (countTransform != null)
        {
            Text countText = countTransform.GetComponent<Text>();
            countText.text = stack.quantity > 1 ? stack.quantity.ToString() : "";
        }
    }
}