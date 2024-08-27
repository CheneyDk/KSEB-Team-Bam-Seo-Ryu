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
        ("Java", 700),
        ("Pet", 700)
    };

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        for (int i = 0; i < 7; i++)
        {
            itemSetter[i].Set(itemList[i].Item1, itemList[i].Item2);
            foreach(string name in SaveManager.instance.shopData.installedItemList)
            {
                if (itemList[i].Item1 == name)
                {
                    itemSetter[i].Installed();
                }
            }
        }

        itemSetter[7].Set(itemList[7].Item1, itemList[7].Item2);
        if (SaveManager.instance.shopData.isCUpgrade) { itemSetter[7].Installed(); }
        itemSetter[8].Set(itemList[8].Item1, itemList[8].Item2);
        if (SaveManager.instance.shopData.isPythonUpgrade) { itemSetter[8].Installed(); }
        itemSetter[9].Set(itemList[9].Item1, itemList[9].Item2);
        if (SaveManager.instance.shopData.isJavaUpgrade) { itemSetter[9].Installed(); }
        itemSetter[10].Set(itemList[10].Item1, itemList[10].Item2);
        if (SaveManager.instance.shopData.isPetInstalled)
        {
            itemSetter[10].ChangeUpdatePage();
            itemList[10] = ("PowerPet", 1000);
        }
        if (SaveManager.instance.shopData.isPetUpgrade) { itemSetter[10].Installed(); }
    }

    private void Update()
    {
        foreach (var text in curMoney)
        {
            text.text = SaveManager.instance.shopData.money.ToString("N0");
        }
    }

    public void InstallCommunalWeapon(int idx)
    {
        if (SaveManager.instance.shopData.money >= itemList[idx].Item2)
        {
            SaveManager.instance.shopData.money -= itemList[idx].Item2;
            itemSetter[idx].Installed();
            SaveManager.instance.shopData.installedItemList.Add(itemList[idx].Item1);
            Alert("success");
        }
        else
        {
            Alert("failure");
        }
    }

    public void InstallBasicWeaponUpgrade(int idx)
    {
        if (SaveManager.instance.shopData.money >= itemList[idx].Item2)
        {
            SaveManager.instance.shopData.money -= itemList[idx].Item2;
            itemSetter[idx].Installed();

            if (itemList[idx].Item1 == "C")
            {
                SaveManager.instance.shopData.isCUpgrade = true;
            }
            else if (itemList[idx].Item1 == "Python")
            {
                SaveManager.instance.shopData.isPythonUpgrade = true;
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
            if (SaveManager.instance.shopData.money >= itemList[idx].Item2)
            {
                SaveManager.instance.shopData.money -= itemList[idx].Item2;
                itemSetter[idx].ChangeUpdatePage();
                itemList[idx] = ("PowerPet", 1000);
                SaveManager.instance.shopData.isPetInstalled = true;
                Alert("success");
            }
            else
            {
                Alert("failure");
            }
        }
        else
        {
            if (SaveManager.instance.shopData.money >= itemList[idx].Item2)
            {
                SaveManager.instance.shopData.money -= itemList[idx].Item2;
                itemSetter[idx].Installed();
                SaveManager.instance.shopData.isPetUpgrade = true;
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
