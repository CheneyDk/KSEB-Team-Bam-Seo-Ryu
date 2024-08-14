using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class RE_SaveManager : MonoBehaviour
{
    public static RE_SaveManager instance;

    private ShopData shopData;
    private GameDataList gameDataList;
    private RecordDatA recordData;

    public GameRecord gameRecord;

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

            shopData = RE_SaveLoader.Load<ShopData>("ShopData", "RecordData");
            shopData ??= new ShopData("ShopData", "SaveFile");

            gameDataList = RE_SaveLoader.Load<GameDataList>("GameData", "RecordData");
            gameDataList ??= new GameDataList("GameData", "SaveFile");

            recordData = RE_SaveLoader.Load<RecordDatA>("RecordData", "RecordData");
            recordData ??= new RecordDatA("RecordData", "SaveFile");
            
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
        recordData.totalDamage += CalcTotal(gameRecord, "damage");
        recordData.totalEnemiesDeafeated += CalcTotal(gameRecord, "enemiesDeafeated");
        recordData.meleeEnemyDefeated += enemiesDeafeatedDict["MeleeEnemyDefeated"];
        recordData.meleeEnemyDefeated += enemiesDeafeatedDict["RangeEnemyDefeated"];
        recordData.meleeEnemyDefeated += enemiesDeafeatedDict["HeavyEnemyDefeated"];
        recordData.meleeEnemyDefeated += enemiesDeafeatedDict["RuntimeErrorDeafeated"];
        recordData.meleeEnemyDefeated += enemiesDeafeatedDict["LogicErrorDeafeated"];

        recordData.bestSurvived = Math.Max(recordData.bestSurvived, gameRecord.survived);
        recordData.bestWaveReached = Math.Max(recordData.bestWaveReached, gameRecord.waveReached);
        recordData.bestLevelReached = Math.Max(recordData.bestLevelReached, gameRecord.levelReached);
        recordData.bestTotalDamage = Math.Max(recordData.bestTotalDamage, CalcTotal(gameRecord, "damage"));
        if (gameRecord.isClear)
        {
            recordData.leastClearTime = Math.Min(recordData.leastClearTime, gameRecord.survived);
        }
    }
    //getter
    private int CalcTotal(GameRecord gameRecord, string record)
    {
        int totalDamage = 0, totalEnemiesDeafeated = 0;
        foreach (var data in gameRecord.weaponDataList)
        {
            totalDamage += data.weaponDamage;
        }
        foreach (var data in gameRecord.enemiesDeafeatedList)
        {
            totalEnemiesDeafeated += data.deafeatedCount;
        }

        if (record == "damage")
        {
            return totalDamage;
        }
        else if (record == "enemiesDeafeated")
        {
            return totalEnemiesDeafeated;
        }
        else
        {
            return 0;
        }
    }

    private void ResetData()
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

        foreach (KeyValuePair<string, float> kvp in weaponDataDict)
        {
            gameRecord.weaponDataList.Add(new weaponData(kvp.Key, (int)kvp.Value));
        }
        gameDataList.gameRecordList.Add(gameRecord);

        RE_SaveLoader.Save(gameDataList);

        UpdateRecordData(gameRecord);
    }

    public void SaveAllData()
    {
        RE_SaveLoader.Save(shopData);
        RE_SaveLoader.Save(gameDataList);
        RE_SaveLoader.Save(recordData);
    }
}
