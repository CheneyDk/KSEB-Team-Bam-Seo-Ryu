using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreData
{
    public int waveCount;
    public int enemyKills;
    public int LV;
    public Dictionary<string, int> weaponDamages = new Dictionary<string, int>();
}
