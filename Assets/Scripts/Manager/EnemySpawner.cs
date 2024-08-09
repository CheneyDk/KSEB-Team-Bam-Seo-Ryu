using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using VInspector;

public class EnemySpawner : MonoBehaviour
{
    private bool GameOver = false;
    private bool Spawning = false;
    private WaveManager waveManager;

    [Foldout("Prefab")]
    public GameObject MeleeEnemyPrefab;
    public GameObject RangeEnemyPrefab;
    public GameObject HeavyEnemyPrefab;
    public GameObject midBossPrefab;
    public GameObject finalBossPrefab;
    public GameObject WarningPrefab;
    public GameObject bossWarningPrefab;
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
    [SerializeField]
    private Transform warningMarkPoolZone;
    private List<GameObject> pooledMeleeEnemies = new List<GameObject>();
    private List<GameObject> pooledRangeEnemies = new List<GameObject>();
    private List<GameObject> pooledHeavyEnemies = new List<GameObject>();
    private List<GameObject> pooledMark = new List<GameObject>();
    [EndFoldout]

    private int MaxMeleeEnemy = 1;
    private int MaxRangeEnemy = 1;
    private int MaxHeavyEnemy = 1;

    public float powerEnemyRate = 1f;

    private void Awake()
    {
        waveManager = GetComponent<WaveManager>();
        nowWave = waveManager.curWave;

        MaxMeleeEnemy = MESpawnNumber;
        MaxRangeEnemy = RESpawnNumber;
        MaxHeavyEnemy = HESpawnNumber;

        //Pooling
        PutEnemiesToPool(pooledMeleeEnemies, MeleeEnemyPrefab);
        PutEnemiesToPool(pooledRangeEnemies, RangeEnemyPrefab);
        PutEnemiesToPool(pooledHeavyEnemies, HeavyEnemyPrefab);
        PutMarkToPool(pooledMark, WarningPrefab);
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



            if (waveManager.curWave == 10)
            {
                StartCoroutine(BossSpawn(midBossPrefab));
            }
            else if (waveManager.curWave == 20)
            {
                StartCoroutine(BossSpawn(finalBossPrefab));
            }
        }
    }

    public void PowerEnemy()
    {
        if (waveManager.curWave % 3 == 0)
        {
            if (MaxMeleeEnemy < 7)
            {
                MaxMeleeEnemy += 1;
            }
            if (MaxRangeEnemy < 5)
            {
                MaxRangeEnemy += 1;
            }
            if (MaxHeavyEnemy < 3)
            {
                MaxHeavyEnemy += 1;
            }

            powerEnemyRate += 0.5f;
        }
    }

    // Stop spawn
    public void StopSpawning()
    {
        Spawning = false;
        PowerEnemy();
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

                var warnMark = GetPooledMark(pooledMark);
                if (warnMark != null)
                {
                    warnMark.transform.position = limitSpawnPosition;
                    warnMark.transform.rotation = Quaternion.identity;
                    warnMark.SetActive(true);
                }
                StartCoroutine(DeActiveWarn(warnMark));
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

    IEnumerator DeActiveWarn(GameObject warn)
    {
        yield return new WaitForSeconds(warningTime);
        warn.SetActive(false);
    }

    private IEnumerator BossSpawn(GameObject boss)
    {
        Vector2 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

        Vector2 spawnPosition;
        do
        {
            spawnPosition = playerPosition + (Vector2)Random.insideUnitCircle * EnemySpawnRange;
        }
        while (Vector2.Distance(spawnPosition, playerPosition) < noSpawnRange);

        Vector2 finalSpawnPosition = spawnPosition;
        finalSpawnPosition.x = Mathf.Clamp(finalSpawnPosition.x, -25f, 25f);
        finalSpawnPosition.y = Mathf.Clamp(finalSpawnPosition.y, -25f, 25f);
        Vector2 limitSpawnPosition = new Vector2(finalSpawnPosition.x, finalSpawnPosition.y);

        GameObject warning = Instantiate(bossWarningPrefab, limitSpawnPosition, Quaternion.identity);
        Destroy(warning, warningTime);

        yield return new WaitForSeconds(warningTime);

        Instantiate(boss, limitSpawnPosition, Quaternion.identity);
    }

    private void PutMarkToPool(List<GameObject> poolList, GameObject enemyPrefab)
    {
        for (int i = 0; i < 10; i++)
        {
            var tmp = Instantiate(enemyPrefab);
            tmp.transform.parent = warningMarkPoolZone;
            tmp.SetActive(false);
            poolList.Add(tmp);
        }
    }

    // Get bullet from pooling list
    public GameObject GetPooledMark(List<GameObject> poolList)
    {
        for (int i = 0; i < poolList.Count; i++)
        {
            if (!poolList[i].activeInHierarchy)
            {
                return poolList[i];
            }
        }
        return null;
    }

    [Button]
    public void SpawnMidBoss()
    {
        StartCoroutine(BossSpawn(midBossPrefab));
    }

    [Button]
    public void SpawnFinalBoss()
    {
        StartCoroutine(BossSpawn(finalBossPrefab));
    }
}
