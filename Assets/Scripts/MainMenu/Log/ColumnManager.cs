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

    private ScoreData data;

    public void setAll(ScoreData score)
    {
        data = score;
        wave.text = score.waveReached.ToString();
        level.text = score.levelReached.ToString();
        kills.text = score.enemiesDeafeated.ToString();
        damage.text = score.totalDamage.ToString();
        dateTime.text = score.playDateTime.ToString();
    }
}
