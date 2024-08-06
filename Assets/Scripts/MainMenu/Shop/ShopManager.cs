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
        ("Internet", 50),
        ("MySQL", 30),
        ("Pytorch", 70),
        ("React", 50),
        ("Swift", 30),
        ("Loading", 30),
        ("C", 100),
        ("Python", 100),
        ("Pet", 300)
    };

    private void Awake()
    {
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

    private void Update()
    {
        curMoney[0].text = ScoreManager.instance.recordData.money.ToString();
        curMoney[1].text = ScoreManager.instance.recordData.money.ToString();
    }

    public void InstallCommunalWeapon(int idx)
    {
        if (ScoreManager.instance.recordData.money >= itemList[idx].Item2)
        {
            ScoreManager.instance.recordData.money -= itemList[idx].Item2;
            itemSetter[idx].Installed();
            ScoreManager.instance.recordData.installedItems.Add(itemList[idx].Item1);
        }
        else
        {
            Alert();
        }
    }

    public void InstallBasicWeaponUpgrade(int idx)
    {
        if (ScoreManager.instance.recordData.money >= itemList[idx].Item2)
        {
            ScoreManager.instance.recordData.money -= itemList[idx].Item2;
            itemSetter[idx].Installed();

            if (itemList[idx].Item1 == "C")
            {
                ScoreManager.instance.recordData.isCUpgrade = true;
            }
            else if (itemList[idx].Item1 == "Python")
            {
                ScoreManager.instance.recordData.isPythonUpgrade = true;
            }
        }
        else
        {
            Alert();
        }
    }

    public void InstallPet(int idx)
    {
        if (itemList[idx].Item2 == 300)
        {
            if (ScoreManager.instance.recordData.money >= itemList[idx].Item2)
            {
                ScoreManager.instance.recordData.money -= itemList[idx].Item2;
                itemSetter[idx].ChangeUpdatePage();
                itemList[idx] = ("PowerPet", 500);
                ScoreManager.instance.recordData.isPet = true;
            }
            else
            {
                Alert();
            }
        }
        else if (itemList[idx].Item2 == 500)
        {
            if (ScoreManager.instance.recordData.money >= itemList[idx].Item2)
            {
                ScoreManager.instance.recordData.money -= itemList[idx].Item2;
                itemSetter[idx].Installed();
                ScoreManager.instance.recordData.isPetUpgrade = true;
            }
            else
            {
                Alert();
            }
        }
    }

    private void Alert()
    {
        Debug.Log("Insufficient funds");
    }
}
