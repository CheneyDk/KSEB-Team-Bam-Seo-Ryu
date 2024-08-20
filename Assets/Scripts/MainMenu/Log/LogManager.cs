using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviour
{
    public ColumnManager[] logs;
    public DetailPanel detailPanel;

    private List<GameRecord> gameRecordList;
    
    private void OnEnable()
    {
        gameRecordList = SaveManager.instance.gameDataList.gameRecordList;
        SetStart();
    }

    public void SetStart()
    {
        for (int idx = 0; idx < Math.Min(logs.Length, gameRecordList.Count); idx++)
        {
            logs[idx].gameObject.SetActive(true);
            logs[idx].setAll(gameRecordList[gameRecordList.Count - idx - 1]);
        }
    }

    public void SetDetails(int idx)
    {
        detailPanel.SetPanel(gameRecordList[gameRecordList.Count - idx - 1]);
    }
}
