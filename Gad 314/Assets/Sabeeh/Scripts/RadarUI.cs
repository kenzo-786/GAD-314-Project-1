using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RadarUI : MonoBehaviour
{
    public Image radarBackground;  // The radar image
    public Image playerDot;
    public GameObject enemyDotPrefab;

    public float radarRange = 30f; // detection radius

    private List<Image> enemyDots = new List<Image>();
    private List<GameObject> enemies = new List<GameObject>();
    private bool radarActive = false;
    private Transform player;

    void Start()
    {
        radarBackground.gameObject.SetActive(false);
        playerDot.gameObject.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (!radarActive) return;

        UpdatePlayerDot();
        UpdateEnemyDots();
    }

    public void EnableRadar()
    {
        radarActive = true;
        radarBackground.gameObject.SetActive(true);
        playerDot.gameObject.SetActive(true);
    }

    // Called when player presses R
    public void ActivateEnemyDots()
    {
        // Find all enemies in the scene
        GameObject[] foundEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemies.Clear();
        enemies.AddRange(foundEnemies);

        // Destroy previous dots
        foreach (var dot in enemyDots)
            Destroy(dot.gameObject);
        enemyDots.Clear();

        // Create new enemy dots
        foreach (var enemy in enemies)
        {
            Image dot = Instantiate(enemyDotPrefab, radarBackground.transform).GetComponent<Image>();
            dot.color = Color.red;
            enemyDots.Add(dot);
        }
    }

    private void UpdatePlayerDot()
    {
        // Player always in center
        playerDot.rectTransform.anchoredPosition = Vector2.zero;
    }

    private void UpdateEnemyDots()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (i >= enemyDots.Count) continue;

            Vector3 offset = enemies[i].transform.position - player.position;

            // Convert to 2D radar space
            Vector2 pos = new Vector2(offset.x, offset.z);

            // Rotate relative to player forward
            float angle = Mathf.Atan2(pos.y, pos.x) - Mathf.Atan2(player.forward.z, player.forward.x);
            float distance = pos.magnitude;

            distance = Mathf.Min(distance, radarRange); // clamp to radar range

            float radarRadius = radarBackground.rectTransform.rect.width / 2f;

            float x = Mathf.Cos(angle) * (distance / radarRange) * radarRadius;
            float y = Mathf.Sin(angle) * (distance / radarRange) * radarRadius;

            enemyDots[i].rectTransform.anchoredPosition = new Vector2(x, y);
        }
    }
}
