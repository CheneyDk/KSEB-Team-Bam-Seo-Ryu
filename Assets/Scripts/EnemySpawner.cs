﻿using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using VInspector;

public class EnemySpawner : MonoBehaviour
{
    private bool GameOver = false;
    private bool Spawning = false;

    public GameObject MeleeEnemyPrefab;
    public GameObject RangeEnemyPrefab;
    public GameObject HeavyEnemyPrefab;
    public GameObject WarningPrefab;
    private WaveManager waveManager;

    [Foldout("Max Enemy Number")]
    public int MaxMeleeEnemy = 3;
    public int MaxRangeEnemy = 3;
    public int MaxHeavyEnemy = 3;
    [EndFoldout]

    [Tab("Melee Enemy")]
    public float MESpawnRate = 1f;
    public float MESpawnStartTime = 1f;
    public int MESpawnNumber = 3;
    public float MESpawnGroupRadius = 1f;

    [Tab("Range Enemy")]
    public float RESpawnRate = 1f;
    public float RESpawnStartTime = 1f;
    public int RESpawnNumber = 3;
    public float RESpawnGroupRadius = 1f;

    [Tab("Heavy Enemy")]
    public float HESpawnRate = 1f;
    public float HESpawnStartTime = 1f;
    public int HESpawnNumber = 3;
    public float HESpawnGroupRadius = 1f;
    [EndTab]

    [Foldout("Spawn Range")]
    public float noSpawnRange = 5f;
    public float EnemySpawnRange = 10f;

    [Foldout("Warning Settings"), SerializeField]
    private float warningTime = 1f;

    private int nowWave;

    [Foldout("Object Pooling")]
    [SerializeField]
    private int numberToPool;
    [SerializeField]
    private Transform enemiesPoolingZone;
    private List<GameObject> pooledMeleeEnemies = new List<GameObject>();
    private List<GameObject> pooledRangeEnemies = new List<GameObject>();
    private List<GameObject> pooledHeavyEnemies = new List<GameObject>();
    [EndFoldout]

    private void Awake()
    {
        waveManager = GetComponent<WaveManager>();
        nowWave = waveManager.curWave;

        //Pooling
        PutEnemiesToPool(pooledMeleeEnemies, MeleeEnemyPrefab);
        PutEnemiesToPool(pooledRangeEnemies, RangeEnemyPrefab);
        PutEnemiesToPool(pooledHeavyEnemies, HeavyEnemyPrefab);
    }

    // Put enemies in pooling list
    private void PutEnemiesToPool(List<GameObject>  poolList, GameObject enemyPrefab)
    {
        for (int i = 0; i < numberToPool; i++)
        {
            var tmp = Instantiate(enemyPrefab);
            tmp.transform.parent = enemiesPoolingZone;
            tmp.SetActive(false);
            poolList.Add(tmp);
        }
    }

    // Get enemies from pooling list
    public GameObject GetPooledEnemies(List<GameObject> poolList)
    {
        for (int i = 0; i < numberToPool; i++)
        {
            if (!poolList[i].activeInHierarchy)
            {
                poolList[i].GetComponent<Enemy>().ResetEnemy();
                return poolList[i];
            }
        }
        return null;
    }

    // Start spawn enemies
    public void StartSpawning()
    {
        if (!Spawning)
        {
            Spawning = true;
            if(waveManager.curWave != nowWave)
            {
                if (MESpawnNumber < MaxMeleeEnemy)
                {
                    MESpawnNumber++;
                }
                if (RESpawnNumber < MaxRangeEnemy)
                {
                    RESpawnNumber++;
                }
                if (HESpawnNumber < MaxHeavyEnemy)
                {
                    HESpawnNumber++;
                }
                nowWave = waveManager.curWave;
            }
            StartCoroutine(EnemiesSpawn(pooledMeleeEnemies, MESpawnStartTime, MESpawnRate, MESpawnNumber, MESpawnGroupRadius));
            StartCoroutine(EnemiesSpawn(pooledRangeEnemies, RESpawnStartTime, RESpawnRate, RESpawnNumber, RESpawnGroupRadius));
            StartCoroutine(EnemiesSpawn(pooledHeavyEnemies, HESpawnStartTime, HESpawnRate, HESpawnNumber, HESpawnGroupRadius));
        }
    }

    // Stop spawn
    public void StopSpawning()
    {
        Spawning = false;
        StopAllCoroutines();
    }

    // Spawn enemies and warning mark
    private IEnumerator EnemiesSpawn(List<GameObject> enemyPoolList, float spawnStartTime, float spawnRate, int spawnNumber, float spawnGroupRadius)
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
                finalSpawnPosition.x = Mathf.Clamp(finalSpawnPosition.x, -25f, 25f);
                finalSpawnPosition.y = Mathf.Clamp(finalSpawnPosition.y, -25f, 25f);
                Vector2 limitSpawnPosition = new Vector2(finalSpawnPosition.x, finalSpawnPosition.y);
                spawnPositions.Add(limitSpawnPosition);

                GameObject warning = Instantiate(WarningPrefab, limitSpawnPosition, Quaternion.identity);
                Destroy(warning, warningTime);
            }

            yield return new WaitForSeconds(warningTime);

            foreach (var position in spawnPositions)
            {
                GameObject enemies = GetPooledEnemies(enemyPoolList);
                if (enemies != null)
                {
                    enemies.transform.position = position;
                    enemies.transform.rotation = Quaternion.identity;
                    enemies.SetActive(true);
                }
            }

            yield return new WaitForSeconds(spawnRate);
        }
    }
}
