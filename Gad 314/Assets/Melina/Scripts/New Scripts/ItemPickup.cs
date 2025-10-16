using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemSO itemData;
    public int amount = 1;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            Inventory inventory = other.GetComponent<Inventory>();
            if (inventory == null)
            {
                inventory = GameObject.FindObjectOfType<Inventory>();
            }

            if (inventory != null)
            {
                inventory.AddItem(itemData, amount);
                Destroy(gameObject);
            }
        }
    }
}