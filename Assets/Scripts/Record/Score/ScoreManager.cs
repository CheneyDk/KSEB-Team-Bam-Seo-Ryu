using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    private ScoreData scoreData;

    private Dictionary<string, float> tempWeaponDamages = new Dictionary<string, float>();
    public IReadOnlyDictionary<string, float> WeaponDamages => tempWeaponDamages;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            ResetData();
        }
        else
        {
            return;
        }
    }

    public void SaveScoreData()
    {
        Debug.Log("저장 성공!");
        Debug.Log("도달 웨이브: " + scoreData.waveReached.ToString());
        Debug.Log("죽인 적 수: " + scoreData.enemiesDeafeated.ToString());
        Debug.Log("도달 레벨: " + scoreData.levelReached.ToString());
        //foreach (KeyValuePair<string, float> kvp in scoreData.weaponDamages)
        //{
        //    Debug.Log(kvp.Key + " " + kvp.Value);
        //}
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
        scoreData.waveReached = 1;
        scoreData.enemiesDeafeated = 0;
        scoreData.levelReached = 1;
        tempWeaponDamages = new Dictionary<string, float>
        {
            { "Basic", 0.0f }
        };
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

    public void SaveData()
    {
        RecordData data = SaveLoadHelper.Load("GameData", "Test/Bootcamp");

        data ??= new RecordData("GameData", "Test/Bootcamp");

        scoreData.playDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        scoreData.totalDamage = GetTotalDamages();

        List<WeaponDamagesData> list = new List<WeaponDamagesData>();
        foreach (KeyValuePair<string, float> kvp in tempWeaponDamages)
        {
            list.Add(new WeaponDamagesData { weaponName = kvp.Key, damage = kvp.Value });
        }
        scoreData.weaponDamagesData = list;

        data.scoreDataList.Add(scoreData);
        Debug.Log(data.scoreDataList.Count);

        SaveLoadHelper.Save(data);

        Debug.Log("Save Success!");
    }
}
