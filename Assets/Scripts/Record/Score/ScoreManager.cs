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

    public Dictionary<string, int> match = new Dictionary<string, int>
    {
        {"CD", 0 },
        {"W2", 1 },
        {"W3", 2 },
        {"P1", 3 },
        {"P2", 4 },
        {"P3", 5 }
    };

    private Dictionary<string, float> tempWeaponDamages = new Dictionary<string, float>();
    public IReadOnlyDictionary<string, float> WeaponDamages => tempWeaponDamages;

    private DateTime timer;

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
            return;
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

        timer = DateTime.Now;

        scoreData.character = SceneManager.GetActiveScene().name;
        // 씬 이름 최종결정 시 삭제
        if (scoreData.character == "UDD")
        {
            scoreData.character = "Python";
        }
        else if (scoreData.character == "MainScene 1")
        {
            scoreData.character = "C";
        }
    }

    public void UpdateEnemiesDeafeated()
    {
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

        return total;
    }

    public void SaveData(bool clear)
    {
        scoreData.survived = (int)(DateTime.Now - timer).TotalSeconds;
        scoreData.playDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        scoreData.totalDamage = GetTotalDamages();
        scoreData.isClear = clear;
        if (clear) { scoreData.waveReached--; }

        List<WeaponDamagesData> list = new List<WeaponDamagesData>();
        foreach (KeyValuePair<string, float> kvp in tempWeaponDamages)
        {
            list.Add(new WeaponDamagesData { weaponName = kvp.Key, damage = kvp.Value });
        }
        scoreData.weaponDamagesData = list;

        recordData.scoreDataList.Add(scoreData);

        SaveLoadHelper.Save(recordData);

        Debug.Log("Save Success!");
    }

    public int GerSurvived()
    {
        return scoreData.survived;
    }
}
