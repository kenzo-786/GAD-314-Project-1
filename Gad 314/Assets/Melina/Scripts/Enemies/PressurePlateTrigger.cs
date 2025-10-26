using UnityEngine;
using System.Collections;

public class PressurePlateTrigger : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public TrailGuideManager trailGuide;
    public Transform player;
    public float triggerDistance = 1.5f;

    public int waves = 4;
    public int enemiesPerWave = 4;
    public float delayBetweenWaves = 3f;

    public bool HasTriggered { get; private set; } = false;

    void Update()
    {
        if (HasTriggered || player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);
        if (distance <= triggerDistance)
        {
            HasTriggered = true;
            Debug.Log("Pressure plate triggered by: " + player.name);

            if (trailGuide != null)
                trailGuide.HideTrail();

            StartCoroutine(SpawnEnemyWaves(player));
        }
    }

    IEnumerator SpawnEnemyWaves(Transform player)
    {
        for (int w = 0; w < waves; w++)
        {
            for (int i = 0; i < enemiesPerWave && i < spawnPoints.Length; i++)
            {
                GameObject enemy = Instantiate(enemyPrefab, spawnPoints[i].position, spawnPoints[i].rotation);
                Debug.Log("Spawned enemy at: " + spawnPoints[i].position);

                EnemyFollow follow = enemy.GetComponent<EnemyFollow>();
                if (follow != null)
                    follow.target = player;
            }
            yield return new WaitForSeconds(delayBetweenWaves);
        }
    }
}
