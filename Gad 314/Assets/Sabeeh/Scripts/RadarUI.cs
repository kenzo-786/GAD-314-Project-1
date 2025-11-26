using System.Collections.Generic;
using UnityEngine;

public class RadarUI : MonoBehaviour
{

    public Transform player;
    public RectTransform radarPanel;
    public RectTransform playerDot;        // Green dot
    public GameObject enemyDotPrefab;      // Red dot prefab

    [Header("Settings")]
    public float radarRange = 30f;

    private bool radarEnabled = false;
    private Dictionary<Transform, GameObject> enemyDots = new Dictionary<Transform, GameObject>();

    void Start()
    {
        radarPanel.gameObject.SetActive(false);
        playerDot.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!radarEnabled) return;

        // Update enemy dots positions relative to player
        UpdateEnemyDots();
    }

    public void EnableRadar()
    {
        radarEnabled = true;
        radarPanel.gameObject.SetActive(true);
        playerDot.gameObject.SetActive(true);
    }

    // Called when pressing R
    public void Scan()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            Transform enemyTransform = enemy.transform;
            float distance = Vector3.Distance(player.position, enemyTransform.position);

            if (distance <= radarRange)
            {
                if (!enemyDots.ContainsKey(enemyTransform))
                {
                    GameObject dot = Instantiate(enemyDotPrefab, radarPanel);
                    dot.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    enemyDots.Add(enemyTransform, dot);
                }
            }
        }

        // Remove enemies out of range
        List<Transform> toRemove = new List<Transform>();
        foreach (var pair in enemyDots)
        {
            float dist = Vector3.Distance(player.position, pair.Key.position);
            if (dist > radarRange)
            {
                Destroy(pair.Value);
                toRemove.Add(pair.Key);
            }
        }
        foreach (Transform t in toRemove)
            enemyDots.Remove(t);
    }

    void UpdateEnemyDots()
    {
        foreach (var pair in enemyDots)
        {
            Transform enemy = pair.Key;
            GameObject dot = pair.Value;

            // Calculate offset relative to player
            Vector3 offset = enemy.position - player.position;

            // Rotate offset so it matches radar orientation (player forward is up)
            Vector3 rotatedOffset = Quaternion.Inverse(Quaternion.Euler(0, player.eulerAngles.y, 0)) * offset;

            // Scale for radar panel
            float x = (rotatedOffset.x / radarRange) * (radarPanel.sizeDelta.x / 2);
            float y = (rotatedOffset.z / radarRange) * (radarPanel.sizeDelta.y / 2);

            dot.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
        }

        // Player dot stays at center
        playerDot.anchoredPosition = Vector2.zero;
    }
}