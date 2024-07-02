using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject MeleeEnemyPrefab;
    public float MeleeEnemySpawnRange = 10f;
    public float MeleeEnemySpawnRate = 1f;
    public float MeleeEnemySpawnStartTime = 1f;
    public float noSpawnRange = 2f;
    public int MeleeEnemySpawnNumber = 3;
    public float MeleeEnemySpawnGroupRadius = 1f;

    private void Start()
    {
        InvokeRepeating(nameof(MeleeEnemiesSpawn), MeleeEnemySpawnStartTime, MeleeEnemySpawnRate);
    }

    //private void SpawnEnemy()
    //{
    //    Vector2 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
    //    Vector2 spawnPosition = playerPosition + (Vector2)Random.insideUnitCircle * MeleeEnemySpawnRange;
    //    Instantiate(MeleeEnemyPrefab, spawnPosition, Quaternion.identity);
    //}


    // 적 랜덤 위치 생성
    private void MeleeEnemiesSpawn()
    {
        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector3 spawnPosition;

        for (int i = 0; i < MeleeEnemySpawnNumber; i++)
        {
            do
            {
                spawnPosition = playerPosition + (Vector3)Random.insideUnitCircle * MeleeEnemySpawnRange;
            }
            while (Vector3.Distance(spawnPosition, playerPosition) < noSpawnRange);

            Vector3 groupOffset = Random.insideUnitCircle * MeleeEnemySpawnGroupRadius;
            Instantiate(MeleeEnemyPrefab, spawnPosition + groupOffset, Quaternion.identity);
        }
    }
}
