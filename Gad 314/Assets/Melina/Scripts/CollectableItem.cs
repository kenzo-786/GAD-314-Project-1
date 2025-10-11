using UnityEngine;
using TMPro;

public class CollectableItem : MonoBehaviour
{
    [Header("Item Settings")]
    public string itemName;         
    public Sprite itemIcon;         

    [Header("UI Prompt")]
    public TextMeshProUGUI pickupPrompt;

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
            playerInventory.AddItem(itemName, itemIcon);
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