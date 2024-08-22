using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    public ShopData shopData;
    public GameDataList gameDataList;
    private RecordData recordData;

    private GameRecord gameRecord;

    private Dictionary<string, float> weaponDataDict = new Dictionary<string, float>();
    public IReadOnlyDictionary<string, float> readOnlyWeaponDataDict => weaponDataDict;

    private Dictionary<string, int> enemiesDeafeatedDict = new Dictionary<string, int>
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

            shopData = SaveLoader.Load<ShopData>("ShopData", "SaveFile");
            shopData ??= new ShopData("ShopData", "SaveFile");

            gameDataList = SaveLoader.Load<GameDataList>("GameData", "SaveFile");
            gameDataList ??= new GameDataList("GameData", "SaveFile");

            recordData = SaveLoader.Load<RecordData>("RecordData", "SaveFile");
            recordData ??= new RecordData("RecordData", "SaveFile");

            ResetData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //setter
    public void EarnMoney()
    {
        shopData.money += 500;
    }
    public void Upgrade(string item)
    {
        if (item == "C")
        {
            shopData.isCUpgrade = true;
        }
        else if (item == "Python")
        {
            shopData.isPythonUpgrade = true;
        }
        else if (item == "pet")
        {
            shopData.isPetUpgrade = true;
        }
    }
    public void Install(string item)
    {
        if (item == "pet")
        {
            shopData.isPetInstalled = true;
        }
        else
        {
            shopData.installedItemList.Add(item);
        }
    }
    public void AddScore(int score)
    {
        gameRecord.score += score;
    }
    public void UpdateGameRecord(string record)
    {
        if (record == "wave")
        {
            gameRecord.waveReached++;
        }
        else if (record == "level")
        {
            gameRecord.levelReached++;
        }
    }
    public void AddWeapon(string weaponName)
    {
        weaponDataDict.Add(weaponName, 0);
    }
    public void UpdateDamage(string weaponName, float weaponDamage)
    {
        weaponDataDict[weaponName] += weaponDamage;
    }
    public void UpdatePetDamage(float dmg)
    {
        petDamage += dmg;
    }
    public void EnemyDeafeat(string type)
    {
        enemiesDeafeatedDict[type]++;
    }
    public void UpdateRecordData(GameRecord gameRecord)
    {
        recordData.totalSurvived += gameRecord.survived;
        recordData.totalDamage += gameRecord.totalDamage;
        recordData.totalEnemiesDeafeated += gameRecord.totalEnemiesDeafeated;
        recordData.meleeEnemyDefeated += enemiesDeafeatedDict["MeleeEnemyDefeated"];
        recordData.rangeEnemyDefeated += enemiesDeafeatedDict["RangeEnemyDefeated"];
        recordData.heavyEnemyDefeated += enemiesDeafeatedDict["HeavyEnemyDefeated"];
        recordData.runtimeErrorDeafeated += enemiesDeafeatedDict["RuntimeErrorDeafeated"];
        recordData.logicErrorDeafeated += enemiesDeafeatedDict["LogicErrorDeafeated"];

        recordData.bestSurvived = Math.Max(recordData.bestSurvived, gameRecord.survived);
        recordData.bestWaveReached = Math.Max(recordData.bestWaveReached, gameRecord.waveReached);
        recordData.bestLevelReached = Math.Max(recordData.bestLevelReached, gameRecord.levelReached);
        recordData.bestEnemiesDeafeated = Math.Max(recordData.bestEnemiesDeafeated, gameRecord.totalEnemiesDeafeated);
        recordData.bestTotalDamage = Math.Max(recordData.bestTotalDamage, gameRecord.totalDamage);
        if (gameRecord.isClear)
        {
            recordData.leastClearTime = Math.Min(recordData.leastClearTime, gameRecord.survived);
        }
    }
    //getter
    public GameRecord GetGameRecord()
    {
        return gameRecord;
    }
    public int GetPetDamage()
    {
        return (int)petDamage;
    }
    public Dictionary<string, int> GetRecordData()
    {
        Dictionary<string, int> recordDataDict = new Dictionary<string, int>
        {
            {"TotalSurvived", recordData.totalSurvived },
            {"TotalEnemiesDeafeated", recordData.totalEnemiesDeafeated },
            {"TotalDamage", recordData.totalDamage },
            {"MeleeEnemyDefeated", recordData.meleeEnemyDefeated },
            {"RangeEnemyDefeated", recordData.rangeEnemyDefeated },
            {"HeavyEnemyDefeated", recordData.heavyEnemyDefeated },
            {"RuntimeErrorDeafeated", recordData.runtimeErrorDeafeated },
            {"LogicErrorDeafeated", recordData.logicErrorDeafeated },
            {"BestSurvived", recordData.bestSurvived },
            {"BestWaveReached", recordData.bestWaveReached },
            {"BestLevelReached", recordData.bestLevelReached },
            {"BestEnemiesDeafeated", recordData.bestEnemiesDeafeated },
            {"BestTotalDamage", recordData.bestTotalDamage },
            {"LeastClearTime", recordData.leastClearTime }
        };

        return recordDataDict;
    }

    public void ResetData()
    {
        gameRecord = new GameRecord(SceneManager.GetActiveScene().name, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        weaponDataDict = new Dictionary<string, float>
        {
            { "Basic", 0.0f }
        };

        timer = Time.time;
        petDamage = 0;
    }

    public void SaveGameRecord(bool isClear)
    {
        gameRecord.survived = (int)(Time.time - timer);
        gameRecord.isClear = isClear;

        foreach (var data in weaponDataDict)
        {
            gameRecord.weaponDataList.Add(new weaponData(data.Key, (int)data.Value));
            gameRecord.totalDamage += (int)data.Value;
        }
        foreach (var data in enemiesDeafeatedDict)
        {
            gameRecord.totalEnemiesDeafeated += data.Value;
        }

        gameRecord.score += gameRecord.survived * 10;
        gameRecord.score += gameRecord.waveReached * 50;
        gameRecord.score += gameRecord.levelReached * 50;
        gameRecord.score += gameRecord.totalDamage;


        gameDataList.gameRecordList.Add(gameRecord);

        SaveLoader.Save(gameDataList);

        UpdateRecordData(gameRecord);
        shopData.money += gameRecord.score / 100;
    }
    public void SaveAllData()
    {
        SaveLoader.Save(shopData);
        SaveLoader.Save(gameDataList);
        SaveLoader.Save(recordData);
    }
}
