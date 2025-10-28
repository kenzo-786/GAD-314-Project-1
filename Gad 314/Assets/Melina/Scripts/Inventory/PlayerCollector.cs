using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    private PlayerInventory inventory;

    void Start()
    {
        inventory = GetComponent<PlayerInventory>();
    }

    private void OnTriggerEnter(Collider other)
    {
        ItemFloat world = other.GetComponent<ItemFloat>();
        CollectableItem ci = other.GetComponent<CollectableItem>();

        InventoryItem data = null;
        if (ci != null) data = ci.itemData;
        else if (world != null) data = world.itemData;

        if (data != null)
        {
            bool added = inventory.AddItem(data);
            if (added) Destroy(other.gameObject);
        }
    }
}