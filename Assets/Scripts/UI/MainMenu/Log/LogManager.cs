using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviour
{
    public ColumnManager[] logs;
    public Button[] buttons;
    public GameObject detailPage;
    // public DetailPanel detailPanel;

    int idx = 0;
    public void SetStart()
    {
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

    public void ShowDetails(int idx)
    {
        detailPage.SetActive(true);
        // detailPanel.ShowPanel(ScoreManager.instance.recordData[idx]);
    }

    public void CloseDetails()
    {
        detailPage.SetActive(false);
    }
}
