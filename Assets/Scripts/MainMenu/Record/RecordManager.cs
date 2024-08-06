using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class RecordManager : MonoBehaviour
{
    public static RecordManager instance;

    public TextMeshProUGUI[] texts;

    private void Awake()
    {
        instance = this;
    }

    public void Calc()
    {
        Dictionary<string, int> data = new Dictionary<string, int>
        {
            {"TotalSurvived", 0 },
            {"TotalEnemiesDeafeated", 0 },
            {"TotalDamage", 0 },
            {"MeleeEnemyDefeated", 0 },
            {"RangeEnemyDefeated", 0 },
            {"HeavyEnemyDefeated", 0 },
            {"RuntimeErrorDeafeated", 0 },
            {"LogicErrorDeafeated", 0 },
            {"BestSurvived", 0 },
            {"BestWaveReached", 0 },
            {"BestLevelReached", 0 },
            {"BestEnemiesDeafeated", 0 },
            {"BestTotalDamage", 0 },
            {"LeastClearTime", int.MaxValue }
        };

        foreach (ScoreData score in ScoreManager.instance.recordData.scoreDataList)
        {
            data["TotalSurvived"] += score.survived;
            data["TotalEnemiesDeafeated"] += score.enemiesDeafeated;
            data["TotalDamage"] += (int)score.totalDamage;

            data["BestSurvived"] = Math.Max(data["BestSurvived"], score.survived);
            data["BestWaveReached"] = Math.Max(data["BestWaveReached"], score.waveReached);
            data["BestLevelReached"] = Math.Max(data["BestLevelReached"], score.levelReached);
            data["BestEnemiesDeafeated"] = Math.Max(data["BestEnemiesDeafeated"], score.enemiesDeafeated);
            data["BestTotalDamage"] = (int)Math.Max(data["BestTotalDamage"], score.totalDamage);

            if (score.isClear)
            {
                data["leastClearTime"] = Math.Min(data["leastClearTime"], score.survived);
            }
        }

        foreach (var value in ScoreManager.instance.recordData.enemiesDeafeatedData)
        {
            data[value.type] = value.kills;
        }

        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].text = data[texts[i].name] != int.MaxValue ? data[texts[i].name].ToString() : "-";
        }
    }
}
