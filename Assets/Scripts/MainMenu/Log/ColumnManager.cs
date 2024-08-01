using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class ColumnManager : MonoBehaviour
{
    public TextMeshProUGUI wave;
    public TextMeshProUGUI level;
    public TextMeshProUGUI kills;
    public TextMeshProUGUI damage;
    public TextMeshProUGUI dateTime;

    public void setAll(ScoreData score)
    {
        wave.text = score.waveReached.ToString();
        level.text = score.levelReached.ToString();
        kills.text = score.enemiesDeafeated.ToString();
        damage.text = score.totalDamage.ToString();
        dateTime.text = score.playDateTime.ToString();
    }
}
