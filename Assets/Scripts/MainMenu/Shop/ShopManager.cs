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

    private AudioSource audioSource;
    public AudioClip noMoneyClip;

    private List<(string, int)> itemList = new List<(string, int)>
    {
        ("Mouse", 0),
        ("Shield", 500),
        ("Elixir", 500),
        ("Pytorch", 500),
        ("React", 500),
        ("CD", 500),
        ("Internet", 500),
        ("C", 700),
        ("Python", 700),
        ("Pet", 700)
    };

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

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
        if (itemList[idx].Item1 == "Pet")
        {
            if (ScoreManager.instance.recordData.money >= itemList[idx].Item2)
            {
                ScoreManager.instance.recordData.money -= itemList[idx].Item2;
                itemSetter[idx].ChangeUpdatePage();
                itemList[idx] = ("PowerPet", 1000);
                ScoreManager.instance.recordData.isPet = true;
            }
            else
            {
                Alert();
            }
        }
        else
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
        audioSource.PlayOneShot(noMoneyClip);
        Debug.Log("Insufficient funds");
    }
}
