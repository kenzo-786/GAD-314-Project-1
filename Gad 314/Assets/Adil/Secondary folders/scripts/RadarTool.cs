using UnityEngine;

public class RadarTool : MonoBehaviour
{
    [Header("Requirements")]
    public ItemData radarItem;
    public KeyCode scanKey = KeyCode.Q;

    [Header("Settings")]
    public float scanRadius = 25f;
    public float cooldown = 5f;
    public LayerMask itemLayer;

    [Header("Visuals")]
    public GameObject iconPrefab;
    public AudioClip scanSound;
    public Color blipColor = new Color(0f, 1f, 0f, 0.8f);

    private float _lastScanTime;

    private void Update()
    {
        if (Input.GetKeyDown(scanKey))
        {
            if (Time.time < _lastScanTime + cooldown) return;

            if (HasRadar())
            {
                PerformScan();
            }
        }
       
        else
        {
            Debug.Log("Cannot Scan: You do not have the Radar item.");
        }
    }

    private bool HasRadar()
    {
        if (InventoryManager.Instance == null) return false;

        return InventoryManager.Instance.HasItem(radarItem, 1);
    }

    private void PerformScan()
    {
        _lastScanTime = Time.time;
        if (scanSound) AudioSource.PlayClipAtPoint(scanSound, transform.position);

        Collider[] hits = Physics.OverlapSphere(transform.position, scanRadius, itemLayer);

        foreach (var hit in hits)
        {
            ItemPickup pickup = hit.GetComponent<ItemPickup>();

            if (pickup != null && pickup.gameObject.activeInHierarchy)
            {
                CreateBlip(pickup);
            }
        }
    }

    private void CreateBlip(ItemPickup target)
    {
        if (iconPrefab == null) return;

        GameObject blip = Instantiate(iconPrefab, target.transform.position, Quaternion.identity);
        WorldIcon iconScript = blip.GetComponent<WorldIcon>();

        if (iconScript)
        {
            Sprite spriteToShow = target.itemData != null ? target.itemData.icon : null;

            iconScript.Setup(target.transform, spriteToShow, blipColor);

        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, scanRadius);
    }
}
