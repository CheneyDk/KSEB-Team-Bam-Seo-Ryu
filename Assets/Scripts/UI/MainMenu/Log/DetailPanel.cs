using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class DetailPanel : MonoBehaviour
{
    public TextMeshProUGUI survived;
    public TextMeshProUGUI wave;
    public TextMeshProUGUI level;
    public TextMeshProUGUI kills;
    public TextMeshProUGUI damage;
    public TextMeshProUGUI dateTime;

    public WeaponSetter[] setters;

    public void SetPanel(ScoreData data)
    {
        wave.text = data.waveReached.ToString();
        level.text = data.levelReached.ToString();
        kills.text = data.enemiesDeafeated.ToString();
        damage.text = data.totalDamage.ToString();
        dateTime.text = data.playDateTime.ToString();

        int idx = 4;
        while (idx != 0)
        {
            setters[idx--].ResetAll();
        }

        foreach(WeaponDamagesData damageData in data.weaponDamagesData)
        {
            setters[idx++].setAll(damageData, data.totalDamage);
        }
    }
}
