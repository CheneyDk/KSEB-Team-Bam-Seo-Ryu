using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class SaveData
{
    [SerializeField] private string fileName;
    [SerializeField] private string directory;

    [SerializeField] public int money;

    [SerializeField]
    public List<string> installedItems = new List<string>();

    [SerializeField]
    public List<(string, int)> enemiesDeafeatedData = new List<(string, int)>
    {
        ("meleeEnemyDefeated", 0 ),
        ("rangeEnemyDefeated", 0 ),
        ("heavyEnemyDefeated", 0 ),
        ("runtimeErrorDeafeated", 0 ),
        ("logicErrorDeafeated", 0 )
    };

    [SerializeField]
    public bool isPet; // 펫 구매 여부
    public bool isPetUpgrade; // 펫 업그레이드 여부
    public bool isCUpgrade; // C기본무기 업그레이드 여부
    public bool isPythonUpgrade; // Python기본무기 업그레이드 여부

    public SaveData(string _fileName, string _directory)
    {
        fileName = _fileName;
        directory = _directory;
    }

    public string GetFullPath() => GetDirectory() + "/" + fileName + ".json";
    public string GetDirectory() => Application.persistentDataPath + "/" + directory;

    public void plusMoney(int price)
    {
        money += price;
    }

    public int GetMoney() { return money; }
}

public class RecordData : SaveData
{
    public List<ScoreData> scoreDataList = new List<ScoreData>();

    public ScoreData this[int i] //indexer
    {
        get => scoreDataList[i];
        set => scoreDataList[i] = value;
    }

    public RecordData(string _fileName, string _directory) : base(_fileName, _directory)
    {

    }
}

[System.Serializable]
public class ScoreData
{
    public int survived;
    public int waveReached;
    public int levelReached;
    public int enemiesDeafeated;
    public float totalDamage;
    public List<WeaponDamagesData> weaponDamagesData;
    public string playDateTime;

    public bool isClear;
    public string character;
}

[System.Serializable]
public class WeaponDamagesData
{
    public string weaponName;
    public float damage;
}
