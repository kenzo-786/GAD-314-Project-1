using UnityEngine;

public class PlayerThrower : MonoBehaviour
{
    [Header("Requirements")]
    public ItemData stoneItem;

    [Header("Settings")]
    public GameObject stonePrefab;
    public Transform throwPoint;
    public float throwForce = 20f;
    public float throwUpwardForce = 2f;

    [Header("Input")]
    public KeyCode throwKey = KeyCode.Mouse0;


    private void Update()
    {
        if (GameManager.Instance && !GameManager.Instance.CanMove()) return;

        if (Input.GetKeyDown(throwKey))
        {
            AttemptThrow();
        }
    }

    private void AttemptThrow()
    {
        if (HotbarManager.Instance == null) return;

        ItemData heldItem = HotbarManager.Instance.GetSelectedItem();

        if (heldItem == stoneItem)
        {
            if (InventoryManager.Instance.HasItem(stoneItem, 1))
            {
                ThrowStone();

                InventoryManager.Instance.RemoveItem(stoneItem, 1);
            }
            else
            {
                Debug.Log("Out of stones!");
            }
        }
    }

    private void ThrowStone()
    {
        if (stonePrefab == null || throwPoint == null) return;

        GameObject stone = Instantiate(stonePrefab, throwPoint.position, Quaternion.identity);


        Rigidbody rb = stone.GetComponent<Rigidbody>();

        if (rb)
        {
            Vector3 force = Camera.main.transform.forward * throwForce + Vector3.up * throwUpwardForce;
            rb.AddForce(force, ForceMode.Impulse);
        }
    }

}
