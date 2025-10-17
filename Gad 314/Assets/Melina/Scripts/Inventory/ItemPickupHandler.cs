using UnityEngine;

public class ItemPickupHandler : MonoBehaviour
{
    public float pickupRadius = 2f;
    public LayerMask itemLayer;

    private Inventory inventory;

    void Start()
    {
        inventory = GetComponent<Inventory>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, pickupRadius, itemLayer);
            foreach (Collider hit in hits)
            {
                ItemPickup item = hit.GetComponent<ItemPickup>();
                if (item != null)
                {
                    inventory.AddItem(item.itemData, item.amount);
                    Destroy(item.gameObject);
                }
            }
        }
    }
}

