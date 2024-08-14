using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class RE_ColumnManager : MonoBehaviour
{
    public TextMeshProUGUI wave;
    public TextMeshProUGUI level;
    public TextMeshProUGUI kills;
    public TextMeshProUGUI damage;
    public TextMeshProUGUI dateTime;

    public void setAll(GameRecord record)
    {
        wave.text = record.waveReached.ToString();
        level.text = record.levelReached.ToString();
        kills.text = record.totalEnemiesDeafeated.ToString();
        damage.text = record.totalDamage.ToString();
        dateTime.text = record.playDateTime.ToString();
    }
}
