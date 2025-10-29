using UnityEngine;
using TMPro;

public class CollectibleItem : MonoBehaviour
{
    public InventoryItem item;
    public TMP_Text collectText;

    void Start()
    {
        if (collectText != null)
            collectText.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && collectText != null)
            collectText.enabled = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && collectText != null)
            collectText.enabled = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.Q))
        {
            InventoryManager.Instance.AddItem(item);
            if (collectText != null)
                collectText.enabled = false;
            Destroy(gameObject);
        }
    }
}