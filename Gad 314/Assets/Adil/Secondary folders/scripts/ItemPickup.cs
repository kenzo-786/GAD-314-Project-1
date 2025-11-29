using UnityEngine;


public class ItemPickup : MonoBehaviour
{
    [Header("Configuration")]
    public ItemData itemData;
    public int amount = 1;

    [Header("Interaction")]
    public KeyCode pickupKey = KeyCode.E;
    public float holdDuration = 0.5f;

    [Header("Feedback")]
    public AudioClip pickupSound;
    public bool rotate = true;

    private bool _inRange;
    private float _timer;

    private void Update()
    {
        if (rotate) transform.Rotate(Vector3.up * 50f * Time.deltaTime);

        if (_inRange)
        {
            if (Input.GetKey(pickupKey))
            {
                _timer += Time.deltaTime;

                if (InteractionHUD.Instance)
                    InteractionHUD.Instance.UpdateProgress(_timer / holdDuration);

                if (_timer >= holdDuration)
                {
                    Collect();
                }
            }
            else
            {
                if (_timer > 0)
                {
                    _timer = 0;
                    if (InteractionHUD.Instance) InteractionHUD.Instance.UpdateProgress(0);
                }
            }
        }
    }

    private void Collect()
    {
        if (InventoryManager.Instance != null && itemData != null)
        {
            InventoryManager.Instance.AddItem(itemData, amount);

            if (pickupSound) AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            if (InteractionHUD.Instance) InteractionHUD.Instance.Hide();

            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Pet"))
        {
            _inRange = true;
            if (InteractionHUD.Instance)
            {
                InteractionHUD.Instance.Show();
                InteractionHUD.Instance.UpdateProgress(0);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Pet"))
        {
            _inRange = false;
            _timer = 0;
            if (InteractionHUD.Instance) InteractionHUD.Instance.Hide();
        }
    }
}
