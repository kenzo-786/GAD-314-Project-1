using UnityEngine;
using TMPro;

public class CollectableItem : MonoBehaviour
{
    public InventoryItem itemData;
    public TextMeshProUGUI pickupPrompt; 

    private bool playerInRange = false;
    private PlayerInventory playerInventory;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Q))
            CollectItem();
    }

    void CollectItem()
    {
        if (playerInventory != null && itemData != null)
        {
            playerInventory.AddItem(itemData);
            if (pickupPrompt != null) pickupPrompt.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerInventory = other.GetComponent<PlayerInventory>();
            if (pickupPrompt != null) pickupPrompt.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerInventory = null;
            if (pickupPrompt != null) pickupPrompt.gameObject.SetActive(false);
        }
    }
}