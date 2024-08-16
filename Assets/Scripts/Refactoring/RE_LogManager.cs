using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RE_LogManager : MonoBehaviour
{
    public RE_ColumnManager[] logs;
    public DetailPanel detailPanel;

    private void OnEnable()
    {
        SetStart();
    }

    public void SetStart()
    {
        int idx = 0;
        foreach (var data in RE_SaveManager.instance.gameDataList.gameRecordList)
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
        detailPanel.SetPanel(RE_SaveManager.instance.gameDataList[idx]);
    }
}
