﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    private int waveNumber = 5;
    public int curWave = 1;
    public float waveTime = 60f;
    public float waveInterval = 3f;
    public float time;
    public float magneticTime = 3f;

    private EnemySpawner enemySpawner;



    private int Test;
    public int numbertest
    {
        get => Test;
        set
        {

            Test = value;
        }
    }

    private void Start()
    {
        time = waveTime + magneticTime;
        enemySpawner = GetComponent<EnemySpawner>();
        if (enemySpawner == null)
        {
            Debug.Log("No enemy");
            return;
        }
        StartCoroutine(StartWave());
    }

    private void Update()
    {
        time -= Time.deltaTime;
        if (time < 0)
        {
            time = waveTime + waveInterval + magneticTime;
        }
    }

    private IEnumerator StartWave()
    {
        while (curWave <= waveNumber)
        {
            Debug.Log($"Wave {curWave} Start!");
            enemySpawner.StartSpawning();
            yield return new WaitForSeconds(waveTime);

            enemySpawner.StopSpawning();
            DestroyAllEnemies();

            GameManager.Instance.player.playerMagneticRange = 100f;
            yield return new WaitForSeconds(magneticTime);
            GameManager.Instance.player.playerMagneticRange = 5f;

            GameManager.Instance.WaveEnd();

            curWave++;
            if (curWave <= waveNumber)
            {
                Debug.Log($"Wave {curWave} Loading...");
                yield return new WaitForSeconds(waveInterval);
            }
        }
        Debug.Log("All Wave Clear!");
    }

    private void DestroyAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }
}
