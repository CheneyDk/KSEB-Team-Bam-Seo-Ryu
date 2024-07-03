using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool GameOver = false;

    public GameObject MeleeEnemyPrefab;
    public GameObject RangeEnemyPrefab;

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

    [Header("Spawn Range")]
    public float noSpawnRange = 5f;
    public float EnemySpawnRange = 10f;

    private void Start()
    {
        StartCoroutine(EnemiesSpawn(MeleeEnemyPrefab, MESpawnStartTime, MESpawnRate, MESpawnNumber, MESpawnGroupRadius));
        StartCoroutine(EnemiesSpawn(RangeEnemyPrefab, RESpawnStartTime, RESpawnRate, RESpawnNumber, RESpawnGroupRadius));
    }


    // 적 랜덤 위치 생성
    private IEnumerator EnemiesSpawn(GameObject EnemyPrefab, float SpawnTime, float SpawnRate, int SpawnNumber, float SpawnGroupRadius)
    {
        yield return new WaitForSeconds(SpawnTime);
        while (!GameOver)
        {
            Vector2 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
            Vector2 spawnPosition;

            for (int i = 0; i < SpawnNumber; i++)
            {
                do
                {
                    spawnPosition = playerPosition + (Vector2)Random.insideUnitCircle * EnemySpawnRange;
                }
                while (Vector2.Distance(spawnPosition, playerPosition) < noSpawnRange);

                Vector2 groupOffset = Random.insideUnitCircle * SpawnGroupRadius;
                Instantiate(EnemyPrefab, spawnPosition + groupOffset, Quaternion.identity);
            }
            yield return new WaitForSeconds(SpawnRate);
        }

    }
}
