using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class SaveData
{
    [SerializeField] private string fileName;
    [SerializeField] private string directory;

    public SaveData(string _fileName, string _directory)
    {
        fileName = _fileName;
        directory = _directory;
    }

    public string GetFullPath() => GetDirectory() + "/" + fileName + ".json";
    public string GetDirectory() => Application.persistentDataPath + "/" + directory;
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
}

[System.Serializable]
public class WeaponDamagesData
{
    public string weaponName;
    public float damage;
}