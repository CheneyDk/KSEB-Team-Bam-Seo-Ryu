using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class DetailPanel : MonoBehaviour
{
    public TextMeshProUGUI score;
    public TextMeshProUGUI survived;
    public TextMeshProUGUI wave;
    public TextMeshProUGUI level;
    public TextMeshProUGUI kills;
    public TextMeshProUGUI damage;
    public TextMeshProUGUI dateTime;

    public WeaponSetter[] setters;

    public void SetPanel(GameRecord data)
    {
        score.text = data.score.ToString();
        survived.text = data.survived.ToString();
        wave.text = data.waveReached.ToString();
        level.text = data.levelReached.ToString();
        kills.text = data.totalEnemiesDeafeated.ToString();
        damage.text = data.totalDamage.ToString();
        dateTime.text = data.playDateTime.ToString();
        if (data.isClear)
        {
            dateTime.text += "(clear)";
        }

        int idx = setters.Length - 1;
        while (idx != 0)
        {
            setters[idx--].ResetAll();
        }

        foreach(weaponData damageData in data.weaponDataList)
        {
            setters[idx++].SetAll(damageData, data.totalDamage, data.character);
        }
    }
}
