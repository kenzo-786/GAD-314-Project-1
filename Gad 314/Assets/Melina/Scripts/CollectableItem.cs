using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public string itemName = "Item";
    private bool playerInRange = false;
    private PlayerInventory playerInventory;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Q))
        {
            CollectItem();
        }
    }

    void CollectItem()
    {
        if (playerInventory != null)
        {
            playerInventory.AddItem(itemName);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerInventory = other.GetComponent<PlayerInventory>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerInventory = null;
        }
    }
}