using UnityEngine;
using TMPro;

public class CollectableItem : MonoBehaviour
{
    public string itemName = "Item";
    private bool playerInRange = false;
    private PlayerInventory playerInventory;

    public TextMeshProUGUI pickupPrompt; // assign in Inspector

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
            HidePrompt();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerInventory = other.GetComponent<PlayerInventory>();
            ShowPrompt();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerInventory = null;
            HidePrompt();
        }
    }

    void ShowPrompt()
    {
        if (pickupPrompt != null)
            pickupPrompt.gameObject.SetActive(true);
    }

    void HidePrompt()
    {
        if (pickupPrompt != null)
            pickupPrompt.gameObject.SetActive(false);
    }
}