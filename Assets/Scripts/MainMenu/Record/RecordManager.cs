using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class RecordManager : MonoBehaviour
{
    public static RecordManager instance;

    public TextMeshProUGUI[] texts;

    private int totalSurvived = 0;
    private float totalDamageSum = 0;
    private int totalEnemiesDeafeated = 0;

    //private int runtimeErrorDeafeated = 0;
    //private int logicErrorDeafeated = 0;

    private int bestSurvived = 0;
    private int bestWaveReached = 0;
    private int bestLevelReached = 0;
    private int bestEnemiesDeafeated = 0;
    private float bestTotalDamage = 0;

    private int leastClearTime = int.MaxValue;

    private int[] values = new int[14];
    //private List<int> values = new List<int>(14);

    private void Awake()
    {
        instance = this;
    }

    public void Calc()
    {
        Reset();

        foreach (ScoreData score in ScoreManager.instance.recordData.scoreDataList)
        {
            totalSurvived += score.survived;
            totalEnemiesDeafeated += score.enemiesDeafeated;
            totalDamageSum += score.totalDamage;

            bestSurvived = Math.Max(bestSurvived, score.survived);
            bestWaveReached = Math.Max(bestWaveReached, score.waveReached);
            bestLevelReached = Math.Max(bestLevelReached, score.levelReached);
            bestEnemiesDeafeated = Math.Max(bestEnemiesDeafeated, score.enemiesDeafeated);
            bestTotalDamage = Math.Max(bestTotalDamage, score.totalDamage);

            if (score.isClear)
            {
                leastClearTime = Math.Min(leastClearTime, score.survived);
            }
        }

        ToList();

        for (int i = 0; i < 14; i++)
        {
            if (3 <= i && i <= 7)
            {
                continue;
            }

            texts[i].text = values[i].ToString();
            texts[i + 14].text = values[i].ToString();

            // least clear time
            if (i == 13)
            {
                if (values[i] == int.MaxValue)
                {
                    texts[i].text = "-";
                    texts[i + 14].text = "-";
                }
            }
        }
    }

    private void Reset()
    {
        totalSurvived = 0;
        totalDamageSum = 0;
        totalEnemiesDeafeated = 0;

        //runtimeErrorDeafeated = 0;
        //logicErrorDeafeated = 0;

        bestSurvived = 0;
        bestWaveReached = 0;
        bestLevelReached = 0;
        bestEnemiesDeafeated = 0;
        bestTotalDamage = 0;

        leastClearTime = int.MaxValue;
    }

    private void ToList()
    {
        values[0] = totalSurvived;
        values[1] = (int)totalDamageSum;
        values[2] = totalEnemiesDeafeated;
        //values[3] = totalSurvived;
        //values[4] = totalSurvived;
        //values[5] = totalSurvived;
        //values[6] = totalSurvived;
        //values[7] = totalSurvived;
        values[8] = bestSurvived;
        values[9] = bestWaveReached;
        values[10] = bestLevelReached;
        values[11] = bestEnemiesDeafeated;
        values[12] = (int)bestTotalDamage;
        values[13] = leastClearTime;
    }
}
