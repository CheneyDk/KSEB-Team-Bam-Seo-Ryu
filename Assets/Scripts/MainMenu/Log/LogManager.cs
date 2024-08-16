using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviour
{
    public ColumnManager[] logs;
    public DetailPanel detailPanel;

    private void OnEnable()
    {
        SetStart();
    }

    public void SetStart()
    {
        int idx = 0;
        foreach (var data in SaveManager.instance.gameDataList.gameRecordList)
        {
            if (idx >= 10)
            {
                break;
            }

            logs[idx].gameObject.SetActive(true);
            logs[idx].setAll(data);

            idx++;
        }
    }

    public void SetDetails(int idx)
    {
        detailPanel.SetPanel(SaveManager.instance.gameDataList[idx]);
    }
}
