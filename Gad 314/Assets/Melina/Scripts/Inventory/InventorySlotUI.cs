using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Image itemIcon;
    public GameObject emptyGraphic;
    public Canvas canvas;

    private InventoryItem item;
    private PlayerInventory inventory;
    private Transform originalParent;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvas == null) canvas = GetComponentInParent<Canvas>();
    }

    public void Setup(InventoryItem newItem, PlayerInventory inv)
    {
        item = newItem;
        inventory = inv;
        itemIcon.sprite = item != null ? item.icon : null;
        itemIcon.enabled = item != null;
        emptyGraphic.SetActive(item == null);
    }

    public void SetupEmpty(PlayerInventory inv)
    {
        Setup(null, inv);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item == null) return;
        originalParent = transform.parent;
        transform.SetParent(canvas.transform);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item == null) return;
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (item == null) return;

        GameObject hit = eventData.pointerEnter;
        bool droppedToWorld = false;

        if (hit == null)
        {
            droppedToWorld = true;
        }
        else
        {
            InventorySlotUI targetSlot = hit.GetComponentInParent<InventorySlotUI>();
            if (targetSlot != null && targetSlot != this)
            {
                InventoryItem temp = targetSlot.item;
                targetSlot.Setup(this.item, inventory);
                this.Setup(temp, inventory);
            }
            else
            {
                RectTransform panelRect = inventory.inventoryUI.inventoryPanel.GetComponent<RectTransform>();
                if (!RectTransformUtility.RectangleContainsScreenPoint(panelRect, eventData.position, eventData.pressEventCamera))
                    droppedToWorld = true;
            }
        }

        if (droppedToWorld)
        {
            inventory.DropItem(item);
        }

        transform.SetParent(originalParent);
        transform.localPosition = Vector3.zero;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && item != null)
            inventory.DropItem(item);
    }
}
