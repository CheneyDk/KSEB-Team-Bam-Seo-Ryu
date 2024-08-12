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
        survived.text = data.survived.ToString();
        wave.text = data.waveReached.ToString();
        level.text = data.levelReached.ToString();
        kills.text = data.enemiesDeafeated.ToString();
        damage.text = ((int)data.totalDamage).ToString();
        dateTime.text = data.playDateTime.ToString();

        int idx = setters.Length - 1;
        while (idx != 0)
        {
            setters[idx--].ResetAll();
        }

        foreach(WeaponDamagesData damageData in data.weaponDamagesData)
        {
            setters[idx++].SetAll(damageData, (int)data.totalDamage, data.character);
        }
    }
}
