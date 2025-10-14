using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Image icon;
    public TextMeshProUGUI quantityText;

    private InventoryItem item;
    private PlayerInventory playerInventory;
    private Transform originalParent;
    private Canvas canvas;
    private bool isDragging = false;

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
    }

    public void Setup(InventoryItem newItem, PlayerInventory inventory)
    {
        item = newItem;
        playerInventory = inventory;

        if (icon != null)
        {
            icon.sprite = item.icon;
            icon.color = Color.white;
            icon.preserveAspect = true;
            icon.enabled = true;
        }

        if (quantityText != null)
            quantityText.text = item.quantity > 1 ? item.quantity.ToString() : "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Start dragging only with right mouse button
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            isDragging = true;
            originalParent = transform.parent;
            transform.SetParent(canvas.transform); 
        }
    }

    public void OnBeginDrag(PointerEventData eventData) { }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        
        if (!RectTransformUtility.RectangleContainsScreenPoint(originalParent.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera))
        {
            playerInventory.DropItem(item);
        }

        transform.SetParent(originalParent);
        transform.localPosition = Vector3.zero;
        isDragging = false;
    }
}
