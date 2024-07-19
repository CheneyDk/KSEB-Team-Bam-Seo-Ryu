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
        Debug.Log("积己!");

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
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
            //enemyKills = 20;
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
        Debug.Log("历厘 己傍!");
    }
}
