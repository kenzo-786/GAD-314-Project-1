using UnityEngine;
using static InventoryManager3;

public class ItemPickup : MonoBehaviour
{
    [Header("Settings")]
    public ItemType itemType;
    public int amount = 1;

    [Header("Interaction")]
    public KeyCode interactKey = KeyCode.E;
    public float holdDuration = 1.5f;

    [Header("Visuals & Audio")]
    public AudioClip pickupSound;
    public bool rotateObject = true;

    private bool _isPlayerInRange = false;
    private float _currentHoldTime = 0f;
    private InventoryManager3 _targetHotbar;

    private void Update()
    {
        if (rotateObject) transform.Rotate(Vector3.up * 50f * Time.deltaTime);

        if (_isPlayerInRange)
        {
            if (Input.GetKey(interactKey))
            {
                _currentHoldTime += Time.deltaTime;

                float progress = _currentHoldTime / holdDuration;
                InteractionHUD.Instance.UpdateProgress(progress);

                if (_currentHoldTime >= holdDuration)
                {
                    CollectItem();
                }
            }
            else
            {
                if (_currentHoldTime > 0)
                {
                    _currentHoldTime = 0;
                    InteractionHUD.Instance.UpdateProgress(0);
                }
            }
        }
    }

    private void CollectItem()
    {
        if (_targetHotbar != null)
        {
            _targetHotbar.AddItem(itemType, amount);

            if (pickupSound) AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            InteractionHUD.Instance.Hide();

            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = true;
            _targetHotbar = other.GetComponent<InventoryManager3>();

            InteractionHUD.Instance.Show();
            InteractionHUD.Instance.UpdateProgress(0);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = false;
            _targetHotbar = null;
            _currentHoldTime = 0;

            InteractionHUD.Instance.Hide();
        }
    }
}
