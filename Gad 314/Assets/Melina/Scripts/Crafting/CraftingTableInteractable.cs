using UnityEngine;
using TMPro;

public class CraftingTableInteractable : MonoBehaviour
{
    public TextMeshProUGUI promptText;
    bool playerNear = false;

    void Start()
    {
        promptText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Crafting Table Interacted");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            promptText.text = "Press E to Interact";
            promptText.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            promptText.gameObject.SetActive(false);
        }
    }
}