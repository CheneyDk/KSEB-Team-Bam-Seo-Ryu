using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int waveCount;
    public int enemyKills;
    public int LV;
    public Dictionary<string, float> weaponDamages = new Dictionary<string, float>();

    bool flag = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("생성!");
        }
        else
        {
            return;
        }
    }

    void Update()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "UDD_Scene")
        {
            WaveManager waveManager = FindObjectOfType<WaveManager>();
            GameManager gameManager = FindObjectOfType<GameManager>();
            Player player = FindObjectOfType<Player>();

            waveCount = waveManager.curWave;
            enemyKills = gameManager.enemyKills;
            LV = player.playerLevel;
            //weaponDamages["Sword"] = 150.0f;
            //weaponDamages["Bow"] = 120.5f;

            if (player.playerCurHp <= 0 && flag)
            {
                SaveScoreData();
                flag = false;
            }
            if (gameManager.isGameContinue)
            {
                flag = true;
            }
        }
    }

    public void SaveScoreData()
    {
        Debug.Log("저장 성공!");
        Debug.Log("도달 웨이브: " + waveCount.ToString());
        Debug.Log("죽인 적 수: " + enemyKills.ToString());
        Debug.Log("도달 레벨: " + LV.ToString());
    }
}
