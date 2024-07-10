using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class UpgradeManager : MonoBehaviour
{
    public GameObject UpgradeUI;
    public Button[] optionButtons;
    public TextMeshProUGUI[] optionNameTexts;
    public TextMeshProUGUI[] optionDescTexts;
    public Image[] optionImages;

    private List<WeaponData> weaponDataList;
    private List<WeaponData> itemDataList;

    private bool isLevelUp = false;

    public List<WeaponData> playerWeaponList = new List<WeaponData>();
    public List<WeaponData> playerItemList = new List<WeaponData>();

    public int maxItemNumber = 4;

    public void OnUpgrade(bool levelup)
    {
        UpgradeUI.SetActive(true);

        List<WeaponData> selectedItems = new List<WeaponData>();
        List<WeaponData> sourceList = null;
        List<WeaponData> itemList = null;

        if (levelup == true)
        {
            isLevelUp = true;
            sourceList = weaponDataList;
            itemList = playerWeaponList;
            if (itemList.Count != maxItemNumber)
            {
                while (selectedItems.Count < 3 && sourceList.Count > 0)
                {
                    WeaponData item = sourceList[Random.Range(0, sourceList.Count)];
                    if (!selectedItems.Contains(item))
                    {
                        selectedItems.Add(item);
                    }
                }
            }
            else
            {
                while (selectedItems.Count < 3 && itemList.Count > 0)
                {
                    WeaponData item = itemList[Random.Range(0, itemList.Count)];
                    if (!selectedItems.Contains(item))
                    {
                        selectedItems.Add(item);
                    }
                }
            }
        }
        else if (levelup == false)
        {
            isLevelUp = false;
            sourceList = itemDataList;
            itemList = playerItemList;
            if (itemList.Count != maxItemNumber)
            {
                while (selectedItems.Count < 3 && sourceList.Count > 0)
                {
                    WeaponData item = sourceList[Random.Range(0, sourceList.Count)];
                    if (!selectedItems.Contains(item))
                    {
                        selectedItems.Add(item);
                    }
                }
            }
            else
            {
                while (selectedItems.Count < 3 && itemList.Count > 0)
                {
                    WeaponData item = itemList[Random.Range(0, itemList.Count)];
                    if (!selectedItems.Contains(item))
                    {
                        selectedItems.Add(item);
                    }
                }
            }
        }

        if (sourceList != null)
        {
            for (int i = 0; i < optionButtons.Length; i++)
            {
                if (i < selectedItems.Count)
                {
                    optionNameTexts[i].text = selectedItems[i].weaponName;
                    optionDescTexts[i].text = selectedItems[i].weaponDesc;
                    optionImages[i].sprite = selectedItems[i].weaponImage;
                    optionButtons[i].onClick.RemoveAllListeners();
                    WeaponData selectedItem = selectedItems[i];
                    optionButtons[i].onClick.AddListener(() => OnItemSelected(selectedItem));
                }
                else
                {
                    optionNameTexts[i].text = "";
                    optionDescTexts[i].text = "";
                    optionImages[i].sprite = null;
                    optionButtons[i].onClick.RemoveAllListeners();
                }
            }
        }
    }

    private void OnItemSelected(WeaponData item)
    {
        Debug.Log($"{item.weaponName} 아이템 선택.");

        if (isLevelUp == true)
        {
            AddOrUpgradeItem(playerWeaponList, item, maxItemNumber);
        }
        else if (isLevelUp == false)
        {
            AddOrUpgradeItem(playerItemList, item, maxItemNumber);
        }

        GameManager.Instance.EndUpgrade();
        isLevelUp = false;
    }

    private void AddOrUpgradeItem(List<WeaponData> itemList, WeaponData item, int maxItems = int.MaxValue)
    {
        var existingItem = itemList.Find(i => i.weaponName == item.weaponName);
        if (existingItem != null)
        {
            Debug.Log($"{item.weaponName} upgrade.");
            // upgrade weapon
        }
        else
        {
            itemList.Add(item);
            Debug.Log($"{item.weaponName} added.");
        }
    }
}
