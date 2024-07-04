using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private bool GameOver = false;
    private bool Spawning = false;

    public GameObject MeleeEnemyPrefab;
    public GameObject RangeEnemyPrefab;
    public GameObject HeavyEnemyPrefab;
    public GameObject WarningPrefab;

    [Header("Melee Enemy")]
    public float MESpawnRate = 1f;
    public float MESpawnStartTime = 1f;
    public int MESpawnNumber = 3;
    public float MESpawnGroupRadius = 1f;

    [Header("Range Enemy")]
    public float RESpawnRate = 1f;
    public float RESpawnStartTime = 1f;
    public int RESpawnNumber = 3;
    public float RESpawnGroupRadius = 1f;

    [Header("Heavy Enemy")]
    public float HESpawnRate = 1f;
    public float HESpawnStartTime = 1f;
    public int HESpawnNumber = 3;
    public float HESpawnGroupRadius = 1f;

    [Header("Spawn Range")]
    public float noSpawnRange = 5f;
    public float EnemySpawnRange = 10f;

    [Header("Warning Settings")]
    public float warningDuration = 1f;

    public void StartSpawning()
    {
        if (!Spawning)
        {
            Spawning = true;
            StartCoroutine(EnemiesSpawn(MeleeEnemyPrefab, MESpawnStartTime, MESpawnRate, MESpawnNumber, MESpawnGroupRadius));
            StartCoroutine(EnemiesSpawn(RangeEnemyPrefab, RESpawnStartTime, RESpawnRate, RESpawnNumber, RESpawnGroupRadius));
            StartCoroutine(EnemiesSpawn(HeavyEnemyPrefab, HESpawnStartTime, HESpawnRate, HESpawnNumber, HESpawnGroupRadius));
        }
    }

    public void StopSpawning()
    {
        Spawning = false;
        StopAllCoroutines();
    }

    private IEnumerator EnemiesSpawn(GameObject enemyPrefab, float spawnStartTime, float spawnRate, int spawnNumber, float spawnGroupRadius)
    {
        yield return new WaitForSeconds(spawnStartTime);
        while (Spawning && !GameOver)
        {
            List<Vector2> spawnPositions = new List<Vector2>();
            Vector2 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

            for (int i = 0; i < spawnNumber; i++)
            {
                Vector2 spawnPosition;
                do
                {
                    spawnPosition = playerPosition + (Vector2)Random.insideUnitCircle * EnemySpawnRange;
                }
                while (Vector2.Distance(spawnPosition, playerPosition) < noSpawnRange);

                Vector2 groupOffset = Random.insideUnitCircle * spawnGroupRadius;
                Vector2 finalSpawnPosition = spawnPosition + groupOffset;
                spawnPositions.Add(finalSpawnPosition);

                // 경고 표시 생성
                GameObject warning = Instantiate(WarningPrefab, finalSpawnPosition, Quaternion.identity);
                Destroy(warning, warningDuration);
            }

            yield return new WaitForSeconds(warningDuration);

            foreach (var position in spawnPositions)
            {
                Instantiate(enemyPrefab, position, Quaternion.identity);
            }

            yield return new WaitForSeconds(spawnRate);
        }
    }
}
