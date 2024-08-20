using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class RecordManager : MonoBehaviour
{
    public TextMeshProUGUI[] texts;

    private void OnEnable()
    {
        Dictionary<string, int> data = SaveManager.instance.GetRecordData();

        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].text = data[texts[i].name] != int.MaxValue ? data[texts[i].name].ToString() : "-";
        }
    }
}
