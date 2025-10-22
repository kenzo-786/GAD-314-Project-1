using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public int numberOfEnemies = 5;
    public float spawnDelay = 0.5f;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    public void SpawnEnemies()
    {
        StartCoroutine(SpawnEnemyRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            if (enemyPrefab == null || spawnPoints.Length == 0)
                yield break;

            Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject enemy = Instantiate(enemyPrefab, randomPoint.position, Quaternion.identity);

            EnemyFollow follow = enemy.GetComponent<EnemyFollow>();
            if (follow == null)
                follow = enemy.AddComponent<EnemyFollow>();

            follow.target = player;

            yield return new WaitForSeconds(spawnDelay);
        }
    }
}