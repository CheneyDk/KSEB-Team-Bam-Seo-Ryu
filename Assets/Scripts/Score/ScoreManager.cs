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

    //public WaveManager waveManager;
    //public GameManager gameManager;
    //public Player player;

    private void Awake()
    {
        Debug.Log("생성!");

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

    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "UDD_Scene")
        {
            WaveManager waveManager = FindObjectOfType<WaveManager>();
            //GameManager gameManager = (GameManager)GameObject.Find("GameManager");
            Player player = FindObjectOfType<Player>();

            waveCount = waveManager.curWave;
            //enemyKills = 20;
            LV = player.playerLevel;
            //weaponDamages["Sword"] = 150.0f;
            //weaponDamages["Bow"] = 120.5f;

            if (player.playerCurHp <= 0)
            {
                SaveScoreData();
            }
        }
    }

    public void SaveScoreData()
    {
        Debug.Log("저장 성공!");
    }
}
