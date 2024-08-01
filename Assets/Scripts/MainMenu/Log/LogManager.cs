using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviour
{
    public ColumnManager[] logs;
    public DetailPanel detailPanel;

    public void SetStart()
    {
        int idx = 0;
        foreach(ScoreData score in ScoreManager.instance.recordData.scoreDataList)
        {
            if (idx >= 10)
            {
                break;
            }

            logs[idx].gameObject.SetActive(true);
            logs[idx].setAll(score);
            
            idx++;
        }
    }

    public void SetDetails(int idx)
    {
        detailPanel.SetPanel(ScoreManager.instance.recordData[idx]);
    }
}
