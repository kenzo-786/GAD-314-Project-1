using UnityEngine;

public class PressurePlateTrigger : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public int numberOfEnemies = 5;
    public TrailGuideManager trailGuide;
    private bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;
        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            SpawnEnemies();
            if (trailGuide != null)
                trailGuide.HideTrail();
        }
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemy = Instantiate(enemyPrefab, randomPoint.position, randomPoint.rotation);

            EnemyFollow follow = enemy.GetComponent<EnemyFollow>();
            if (follow != null)
                follow.target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
}