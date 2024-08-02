using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public TextMeshProUGUI[] curMoney;
    public ItemSetter[] itemSetter;

    private List<(string, int)> itemList = new List<(string, int)>
    {
        ("CD", 100),
        ("Internet", 0),
        ("MySQL", 0),
        ("Pytorch", 0),
        ("React", 0),
        ("Swift", 0)
    };

    private void Awake()
    {
        curMoney[0].text = ScoreManager.instance.recordData.money.ToString();
        curMoney[1].text = ScoreManager.instance.recordData.money.ToString();

        for (int i = 0; i < itemList.Count; i++)
        {
            itemSetter[i].Set(itemList[i].Item1, itemList[i].Item2);
            foreach(string name in ScoreManager.instance.recordData.installedItems)
            {
                if (itemList[i].Item1 == name)
                {
                    itemSetter[i].Installed();
                }
            }
        }
    }

    public void Install(int idx)
    {
        itemSetter[idx].Installed();
        ScoreManager.instance.recordData.installedItems.Add(itemList[idx].Item1);
    }
}
