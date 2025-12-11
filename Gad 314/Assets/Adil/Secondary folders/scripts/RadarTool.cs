using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class RadarTool : MonoBehaviour
{
    [Header("Requirements")]
    public ItemData radarItem;
    public KeyCode scanKey = KeyCode.Q;

    [Header("Settings")]
    public float scanRadius = 50f;
    public float cooldown = 5f;
    public LayerMask itemLayer;

    [Header("Audio Settings")]
    public float pingDelay = 0.2f;
    public float clusterDistance = 4.0f;

    [Header("Visuals")]
    public GameObject iconPrefab;
    public Color blipColor = new Color(0f, 1f, 1f, 1f);

    [Header("Audio")]
    public GameObject soundBlipPrefab;
    public AudioClip scanStartSound;

    private float _lastScanTime;

    private void Update()
    {
        if (Input.GetKeyDown(scanKey))
        {
            if (Time.time < _lastScanTime + cooldown)
            {
                return;
            }

            if (HasRadar())
            {
                PerformScan();
            }
            else
            {
                Debug.Log("Radar Missing!");
            }
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

        if (scanStartSound) AudioSource.PlayClipAtPoint(scanStartSound, transform.position);

        Collider[] hits = Physics.OverlapSphere(transform.position, scanRadius, itemLayer);

        List<ItemPickup> foundItems = new List<ItemPickup>();

        foreach (var hit in hits)
        {
            ItemPickup pickup = hit.GetComponent<ItemPickup>();
            if (pickup != null && pickup.gameObject.activeInHierarchy)
            {
                foundItems.Add(pickup);
            }
        }

        foundItems.Sort((a, b) =>
        {
            float distA = Vector3.Distance(transform.position, a.transform.position);
            float distB = Vector3.Distance(transform.position, b.transform.position);
            return distA.CompareTo(distB);
        });

        if (foundItems.Count > 0)
        {
            Debug.Log($"Radar: Found {foundItems.Count} items. Starting sequence...");
            StartCoroutine(PlayPingSequence(foundItems));
        }
    }

    private IEnumerator PlayPingSequence(List<ItemPickup> sortedItems)
    {
        List<Vector3> pingedLocations = new List<Vector3>();

        foreach (ItemPickup item in sortedItems)
        {
            CreateVisualBlip(item);

            bool isCluster = false;
            foreach (Vector3 loc in pingedLocations)
            {
                if (Vector3.Distance(item.transform.position, loc) < clusterDistance)
                {
                    isCluster = true;
                    break;
                }
            }

            if (!isCluster)
            {
                CreateAudioBlip(item.transform.position);
                pingedLocations.Add(item.transform.position);

                yield return new WaitForSeconds(pingDelay);
            }
        }
    }

    private void CreateVisualBlip(ItemPickup target)
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

    private void CreateAudioBlip(Vector3 targetPos)
    {
        if (soundBlipPrefab == null) return;

        GameObject audioObj = Instantiate(soundBlipPrefab, targetPos, Quaternion.identity);

        AudioSource src = audioObj.GetComponent<AudioSource>();
        if (src != null)
        {
            src.pitch = Random.Range(0.95f, 1.05f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, scanRadius);
    }

}
