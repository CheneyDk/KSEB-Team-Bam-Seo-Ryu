using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class RE_SaveData
{
    [SerializeField] private string fileName;
    [SerializeField] private string directory;

    public RE_SaveData(string _fileName, string _directory)
    {
        fileName = _fileName;
        directory = _directory;
    }

    public string GetFullPath() => GetDirectory() + "/" + fileName + ".json";
    public string GetDirectory() => Application.persistentDataPath + "/" + directory;
}

public class ShopData : RE_SaveData
{
    public int money;

    public bool isCUpgrade;
    public bool isPythonUpgrade;
    public bool isPetInstalled;
    public bool isPetUpgrade;

    public List<string> installedItemList = new List<string>();

    public ShopData(string _fileName, string _directory) : base(_fileName, _directory)
    {

    }
}

public class GameDataList : RE_SaveData
{
    public List<GameRecord> gameRecordList = new List<GameRecord>();

    public GameRecord this[int i] //indexer
    {
        get => gameRecordList[i];
        set => gameRecordList[i] = value;
    }

    public GameDataList(string _fileName, string _directory) : base(_fileName, _directory)
    {

    }
}

public class RecordDatA : RE_SaveData
{
    public int totalSurvived = 0;
    public int totalDamage = 0;
    public int totalEnemiesDeafeated = 0;
    public int meleeEnemyDefeated = 0;
    public int rangeEnemyDefeated = 0;
    public int heavyEnemyDefeated = 0;
    public int runtimeErrorDeafeated = 0;
    public int logicErrorDeafeated = 0;
    public int bestSurvived = 0;
    public int bestWaveReached = 0;
    public int bestLevelReached = 0;
    public int bestTotalDamage = 0;
    public int bestEnemiesDeafeated = 0;
    public int leastClearTime = int.MaxValue;

    public RecordDatA(string _fileName, string _directory) : base(_fileName, _directory)
    {

    }
}

[System.Serializable]
public class GameRecord
{
    public string character;

    public int score;
    public int survived;
    public int waveReached;
    public int levelReached;
    public bool isClear;

    public List<weaponData> weaponDataList;
    public List<enemiesDeafeatedDatA> enemiesDeafeatedList;
    public int totalDamage;
    public int totalEnemiesDeafeated;

    public string playDateTime;

    public GameRecord(string character, string playDateTime)
    {
        this.character = character;
        score = 0;
        survived = 0;
        waveReached = 1;
        levelReached = 1;
        isClear = false;
        weaponDataList = new List<weaponData>();
        enemiesDeafeatedList = new List<enemiesDeafeatedDatA>();
        totalDamage = 0;
        totalEnemiesDeafeated = 0;
        this.playDateTime = playDateTime;
    }
}

[System.Serializable]
public class weaponData
{
    public string weaponName;
    public int weaponDamage;

    public weaponData(string weaponName, int weaponDamage)
    {
        this.weaponName = weaponName;
        this.weaponDamage = weaponDamage;
    }
}

[System.Serializable]
public class enemiesDeafeatedDatA
{
    public string enemyType;
    public int deafeatedCount;

    public enemiesDeafeatedDatA(string enemyType, int deafeatedCount)
    {
        this.enemyType = enemyType;
        this.deafeatedCount = deafeatedCount;
    }
}