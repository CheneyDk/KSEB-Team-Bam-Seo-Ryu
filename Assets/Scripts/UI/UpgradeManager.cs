using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class UpgradeManager : MonoBehaviour
{
    [Header("Upgrade UI")]
    public GameObject UpgradeUI;
    public Button[] optionButtons;
    public TextMeshProUGUI[] optionNameTexts;
    public TextMeshProUGUI[] optionDescTexts;
    public Image[] optionImages;

    [Header("Weapon and Item")]
    public List<WeaponData> weaponDataList;
    public List<WeaponData> itemDataList;


    [Header("Player Weapon & Item Slot")]
    public List<WeaponData> playerWeaponList = new List<WeaponData>();
    public List<WeaponData> playerItemList = new List<WeaponData>();
    public int maxItemNumber = 4;

    private bool isLevelUp = false;

    [Header("Player Weapon")]
    public Transform playerWeaponParent;

    public GameUI gameUI;

    [Header("Item for MaxLevel")]
    public List<WeaponData> maxLevelItemList;
    private List<WeaponData> randomItemList = new List<WeaponData>();

    // Upgrade UI
    public void OnUpgrade(bool levelup)
    {
        UpgradeUI.SetActive(true);

        List<WeaponData> selectedItems = new List<WeaponData>();
        List<WeaponData> sourceList = null;
        List<WeaponData> itemList = null;

        if (levelup == true)
        {
            isLevelUp = levelup;
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
                while (randomItemList.Count > 0)
                {
                    WeaponData item = randomItemList[Random.Range(0, randomItemList.Count)];
                    if (!selectedItems.Contains(item))
                    {
                        selectedItems.Add(item);
                    }
                }
                if (selectedItems.Count < 3)
                {
                    WeaponData item = maxLevelItemList[Random.Range(0, maxLevelItemList.Count)];
                    selectedItems.Add(item);
                }
            }
        }
        else if (levelup == false)
        {
            isLevelUp = levelup;
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
                while (randomItemList.Count > 0)
                {
                    WeaponData item = randomItemList[Random.Range(0, randomItemList.Count)];
                    if (!selectedItems.Contains(item))
                    {
                        selectedItems.Add(item);
                    }
                }
                if (selectedItems.Count < 3)
                {
                    WeaponData item = maxLevelItemList[Random.Range(0, maxLevelItemList.Count)];
                    selectedItems.Add(item);
                }
            }
        }

        if (sourceList != null && itemList != null)
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

    // Add Selected Weapon or Item
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

    // Add Item or Upgrade Item
    private void AddOrUpgradeItem(List<WeaponData> itemList, WeaponData item, int maxItems)
    {
        var existingItem = itemList.Find(i => i.weaponName == item.weaponName);
        if (existingItem != null)
        {
            Debug.Log($"{item.weaponName} upgrade.");

            // Upgrade
            existingItem.weapon.GetComponent<PlayerWeapon>().Upgrade();
            if(existingItem.weapon.GetComponent<PlayerWeapon>().isMaxLevel == true)
            {
                randomItemList.Remove(existingItem);
            }
        }
        else
        {
            // Add Weapon or Item to itemList
            itemList.Add(item);
            randomItemList.Add(item);
            if (itemList == playerWeaponList)
            {
                gameUI.WeaponIconList(itemList);
            }
            else if (itemList == playerItemList)
            {
                gameUI.ItemIconList(itemList);
            }
            AddItemToPlayer(item.weapon);
            Debug.Log($"{item.weaponName} added.");
        }
    }

    // Add Weapon or Item to Player
    private void AddItemToPlayer(GameObject item)
    {
        Instantiate(item, playerWeaponParent);
    }
}
