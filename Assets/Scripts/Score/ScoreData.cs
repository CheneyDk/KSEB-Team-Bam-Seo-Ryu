using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreData
{
    public int waveCount;
    public int enemyKills;
    public int LV;
    public Dictionary<string, float> weaponDamages = new Dictionary<string, float>();
}
