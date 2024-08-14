using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public RecordData recordData { get; private set; }
    private ScoreData scoreData;

    private Dictionary<string, float> tempWeaponDamages = new Dictionary<string, float>();
    public IReadOnlyDictionary<string, float> WeaponDamages => tempWeaponDamages;

    private Dictionary<string, int> enemyKills = new Dictionary<string, int>
    {
        {"MeleeEnemyDefeated",0 },
        {"RangeEnemyDefeated",0 },
        {"HeavyEnemyDefeated",0 },
        {"RuntimeErrorDeafeated",0 },
        {"LogicErrorDeafeated",0 }
    };

    private float timer;
    private float petDamage;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            ResetData();

            recordData = SaveLoadHelper.Load("GameData", "Test/Bootcamp");
            recordData ??= new RecordData("GameData", "Test/Bootcamp");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCharacter(string character)
    {
        scoreData.character = character;
    }

    public string GetCharacter()
    {
        return scoreData.character;
    }

    public void AddWeapon(string name)
    {
        tempWeaponDamages.Add(name, 0);
    }

    public void UpdateDamage(string name, float damage)
    {
        tempWeaponDamages[name] += damage;
    }

    public void UpdateLevel()
    {
        scoreData.levelReached++;
    }

    public int GetLevel()
    {
        return scoreData.levelReached;
    }

    public void UpdateWave()
    {
        scoreData.waveReached++;
    }

    public int GetWave()
    {
        return scoreData.waveReached;
    }

    public void ResetData()
    {
        scoreData = new ScoreData();
        scoreData.survived = 0;
        scoreData.waveReached = 1;
        scoreData.enemiesDeafeated = 0;
        scoreData.levelReached = 1;
        tempWeaponDamages = new Dictionary<string, float>
        {
            { "Basic", 0.0f }
        };

        timer = Time.time;

        scoreData.character = SceneManager.GetActiveScene().name;
        
        petDamage = 0;
    }

    public void UpdateEnemiesDeafeated(string type)
    {
        enemyKills[type]++;
        scoreData.enemiesDeafeated++;
    }

    public int GetEnemiesDeafeated()
    {
        return scoreData.enemiesDeafeated;
    }

    public float GetTotalDamages()
    {
        float total = 0;
        foreach (KeyValuePair<String, float> kvp in tempWeaponDamages)
        {
            total += kvp.Value;
        }

        total += petDamage;

        return total;
    }

    public void SaveData(bool clear)
    {
        scoreData.survived = (int)(Time.time - timer);
        scoreData.playDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        scoreData.totalDamage = GetTotalDamages();
        scoreData.isClear = clear;
        if (clear) { scoreData.waveReached--; }

        List<WeaponDamagesData> list1 = new List<WeaponDamagesData>();
        foreach (KeyValuePair<string, float> kvp in tempWeaponDamages)
        {
            list1.Add(new WeaponDamagesData { weaponName = kvp.Key, damage = kvp.Value });
        }
        scoreData.weaponDamagesData = list1;
        recordData.scoreDataList.Add(scoreData);

        SaveLoadHelper.Save(recordData);

        foreach (KeyValuePair<string, int> kvp in enemyKills)
        {
            for (int i = 0; i < recordData.enemiesDeafeatedData.Count; i++)
            {
                if (recordData.enemiesDeafeatedData[i].type == kvp.Key)
                {
                    recordData.enemiesDeafeatedData[i] = new enemiesDeafeatedData(recordData.enemiesDeafeatedData[i].type, recordData.enemiesDeafeatedData[i].kills + kvp.Value);
                }
            }
        }

        recordData.money += scoreData.survived / 100 * 10;
        recordData.money += scoreData.waveReached * 10;
        recordData.money += (scoreData.levelReached / 10 + 1) * 10;
        recordData.money += (scoreData.enemiesDeafeated / 100) * 10;
        recordData.money += (int)scoreData.totalDamage / 100;

        Debug.Log("Save Success!");
    }

    public int GetSurvived()
    {
        return scoreData.survived;
    }

    public void GetMoney()
    {
        recordData.money += 500;
    }

    public void UpdatePetDamage(float dmg)
    {
        petDamage += dmg;
    }

    public float GetPetDamage()
    {
        return petDamage;
    }
}
