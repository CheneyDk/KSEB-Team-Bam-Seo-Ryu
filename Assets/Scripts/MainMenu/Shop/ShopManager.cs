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
    public AudioClip buyClip;

    private List<(string, int)> itemList = new List<(string, int)>
    {
        ("Mouse", 500),
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

        for (int i = 0; i < 7; i++)
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

        itemSetter[7].Set(itemList[7].Item1, itemList[7].Item2);
        if (ScoreManager.instance.recordData.isCUpgrade) { itemSetter[7].Installed(); }
        itemSetter[8].Set(itemList[8].Item1, itemList[8].Item2);
        if (ScoreManager.instance.recordData.isPythonUpgrade) { itemSetter[8].Installed(); }
        itemSetter[9].Set(itemList[9].Item1, itemList[9].Item2);
        if (ScoreManager.instance.recordData.isPet)
        { 
            itemSetter[9].ChangeUpdatePage();
            itemList[9] = ("PowerPet", 1000);
        }
        if (ScoreManager.instance.recordData.isPetUpgrade) { itemSetter[9].Installed(); }
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
            Alert("success");
        }
        else
        {
            Alert("failure");
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
            Alert("success");
        }
        else
        {
            Alert("failure");
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
                Alert("success");
            }
            else
            {
                Alert("failure");
            }
        }
        else
        {
            if (ScoreManager.instance.recordData.money >= itemList[idx].Item2)
            {
                ScoreManager.instance.recordData.money -= itemList[idx].Item2;
                itemSetter[idx].Installed();
                ScoreManager.instance.recordData.isPetUpgrade = true;
                Alert("success");
            }
            else
            {
                Alert("failure");
            }
        }
    }

    private void Alert(string result)
    {
        if (result == "failure")
        {
            audioSource.PlayOneShot(noMoneyClip);
            Debug.Log("Insufficient funds");
        }
        else if (result == "success")
        {
            audioSource.PlayOneShot(buyClip);
        }
    }
}
