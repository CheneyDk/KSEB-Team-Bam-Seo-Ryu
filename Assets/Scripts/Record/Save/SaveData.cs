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
    public List<item> items = new List<item>
    {
        new item("CD", false),
        new item("W2", false),
        new item("W3", false),
        new item("P1", false),
        new item("P2", false),
        new item("P3", false)
    };

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

[System.Serializable]
public class item
{
    public string itemName;
    public bool isBought;

    public item(string itemName, bool isBought)
    {
        this.itemName = itemName;
        this.isBought = isBought;
    }
}