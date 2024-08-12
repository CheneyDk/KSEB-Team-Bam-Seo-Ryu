using System.Collections;
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

    public static bool waveIsStarted = false;

    private EnemySpawner enemySpawner;

    private AudioSource audioSource;
    public AudioClip waveEndClip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        time = waveTime;
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
        if (time > 0)
        {
            time -= Time.deltaTime;
        }
    }

    private IEnumerator StartWave()
    {
        while (curWave <= waveNumber)
        {
            GameInfoManager.Instance.DisplayGameInfo($"Wave {curWave} Start!");
            enemySpawner.StartSpawning();
            waveIsStarted = true;
            yield return new WaitForSeconds(waveTime);
            waveIsStarted = false;
            enemySpawner.StopSpawning();
            DestroyAllEnemies();

            GameManager.Instance.player.playerMagneticRange = 100f;
            yield return new WaitForSeconds(magneticTime);
            GameManager.Instance.player.playerMagneticRange = 5f;

            GameManager.Instance.WaveEnd();

            curWave++;
            if (curWave <= waveNumber)
            {
                GameInfoManager.Instance.DisplayGameInfo($"Wave {curWave} Loading...");
                yield return new WaitForSeconds(waveInterval);
                time = waveTime;
            }
        }
        GameInfoManager.Instance.DisplayGameInfo("All Wave Clear");
        ScoreManager.instance.SaveData(true);
    }

    private void DestroyAllEnemies()
    {
        audioSource.PlayOneShot(waveEndClip);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        GameObject[] warning = GameObject.FindGameObjectsWithTag("Warning Mark");
        foreach (GameObject enemy in enemies)
        {
            enemy.SetActive(false);
        }
        foreach (GameObject bullet in bullets)
        {
            bullet.SetActive(false);
        }
        foreach (GameObject warn in warning)
        {
            warn.SetActive(false);
        }
    }
}
