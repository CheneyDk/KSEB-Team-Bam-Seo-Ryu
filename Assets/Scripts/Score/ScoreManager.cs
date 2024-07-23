using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int waveCount = 1;
    public int enemiesDeafeated = 0;
    public int LV = 1;
    public Dictionary<string, float> weaponDamages = new Dictionary<string, float>
        {
            { "Basic", 0.0f }
        };

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("생성!");
        }
        else
        {
            return;
        }
    }

    public void SaveScoreData()
    {
        Debug.Log("저장 성공!");
        Debug.Log("도달 웨이브: " + waveCount.ToString());
        Debug.Log("죽인 적 수: " + enemiesDeafeated.ToString());
        Debug.Log("도달 레벨: " + LV.ToString());
        foreach (KeyValuePair<string, float> kvp in weaponDamages)
        {
            Debug.Log(kvp.Key + " " + kvp.Value);
        }
    }

    public void AddWeapon(string name)
    {
        weaponDamages.Add(name, 0);
    }

    public void UpdateDamage(string name, float damage)
    {
        weaponDamages[name] += damage;
    }

    public void UpdateLevel()
    {
        LV++;
    }

    public int GetLevel()
    {
        return LV;
    }

    public void UpdateWave()
    {
        waveCount++;
    }

    public int GetWave()
    {
        return waveCount;
    }

    public void ResetData()
    {
        waveCount = 1;
        enemiesDeafeated = 0;
        LV = 1;
        weaponDamages = new Dictionary<string, float>
        {
            { "Basic", 0.0f }
        };
    }

    public void UpdateEnemiesDeafeated()
    {
        enemiesDeafeated++;
    }

    public int GetEnemiesDeafeated()
    {
        return enemiesDeafeated;
    }

    public Dictionary<string, float> GetweaponDamages()
    {
        return weaponDamages;
    }
}
