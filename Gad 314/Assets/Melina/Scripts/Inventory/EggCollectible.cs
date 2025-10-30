using UnityEngine;

public class EggCollectible : MonoBehaviour
{
    private bool playerInRange = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Press Q to collect the egg");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Q))
        {
            CollectEgg();
        }
    }

    private void CollectEgg()
    {
        Debug.Log("Egg collected!");
        Destroy(gameObject);
    }
}