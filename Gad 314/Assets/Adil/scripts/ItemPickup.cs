using UnityEngine;
using static InventoryManager3;

public class ItemPickup : MonoBehaviour
{
    [Header("Settings")]
    public ItemType itemType;
    public int amount = 1;

    [Header("Visuals & Audio")]
    public AudioClip pickupSound;
    public bool rotateObject = true;
    public float rotationSpeed = 50f;

    private void Update()
    {
        if (rotateObject)
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InventoryManager3 inventory = other.GetComponent<InventoryManager3>();

            if (inventory != null)
            {
                inventory.AddItem(itemType, amount);

                if (pickupSound != null)
                {
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                }

                Destroy(gameObject);
            }
        }
    }
}
