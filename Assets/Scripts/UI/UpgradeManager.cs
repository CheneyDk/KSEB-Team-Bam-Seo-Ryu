using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [Header("Player Weapon and Item")]
    public Transform playerWeaponParent;
    public Transform playerItemParent;

    public GameUI gameUI;

    [Header("Item for MaxLevel")]
    public List<WeaponData> maxLevelItemList = new List<WeaponData>();

    // Cheak Weapon or Item is Max Level or not
    private List<WeaponData> randomItemList = new List<WeaponData>();
    [SerializeField]
    private List<WeaponData> randomWeaponList = new List<WeaponData>();

    [SerializeField]
    private Transform playerWeaponsBag;
    [SerializeField]
    private Transform playerItemsBag;

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
            else if (itemList.Count == maxItemNumber)
            {
                while (selectedItems.Count < 3 && randomWeaponList.Count > 0)
                {
                    WeaponData item = randomWeaponList[Random.Range(0, randomWeaponList.Count)];
                    WeaponData maxItem = maxLevelItemList[Random.Range(0, maxLevelItemList.Count)];
                    if (!selectedItems.Contains(item) && item != null)
                    {
                        selectedItems.Add(item);
                    }
                    else
                    {
                        selectedItems.Add(maxItem);
                    }
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
                while (selectedItems.Count< 3 && sourceList.Count > 0)
                {
                    WeaponData item = sourceList[Random.Range(0, sourceList.Count)];
                    if (!selectedItems.Contains(item))
                    {
                        selectedItems.Add(item);
                    }
                }
            }
            else if (itemList.Count == maxItemNumber)
            {
                while (selectedItems.Count < 3 && randomItemList.Count > 0)
                {
                    WeaponData item = randomItemList[Random.Range(0, randomItemList.Count)];
                    WeaponData maxItem = maxLevelItemList[Random.Range(0, maxLevelItemList.Count)];
                    if (!selectedItems.Contains(item) && item != null)
                    {
                        selectedItems.Add(item);
                    }
                    else
                    {
                        selectedItems.Add(maxItem);
                    }
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
        if (isLevelUp == true)
        {
            AddOrUpgradeItem(playerWeaponList, randomWeaponList, playerWeaponsBag, item, maxItemNumber);
        }
        else if (isLevelUp == false)
        {
            AddOrUpgradeItem(playerItemList, randomItemList, playerItemsBag, item, maxItemNumber);
        }

        GameManager.Instance.EndUpgrade();
        isLevelUp = false;
    }

    // Add Item or Upgrade Item
    private void AddOrUpgradeItem(List<WeaponData> itemList, List<WeaponData> otherList, Transform upgradeItem, WeaponData item, int maxItems)
    {
        var existingItem = itemList.Find(i => i.weaponName == item.weaponName);
        if (existingItem != null)
        {
            Debug.Log($"{item.weaponName} upgrade.");

            // Upgrade
            var upgradeWeaponName = upgradeItem.Find(existingItem.weapon.name + ("(Clone)"));
            var upgradeWeapon = upgradeWeaponName.GetComponent<PlayerWeapon>();
            if (upgradeWeapon.isMaxLevel == false)
            {
                upgradeWeapon.Upgrade();
            }
            else if (upgradeWeapon.isMaxLevel == true)
            {
                otherList.Remove(item);
            }
        }
        else
        {
            // Add Weapon or Item to itemList
            itemList.Add(item);
            otherList.Add(item);
            if (itemList == playerWeaponList)
            {
                gameUI.WeaponIconList(itemList);
                AddWeaponToPlayer(item.weapon);
            }
            else if (itemList == playerItemList)
            {
                gameUI.ItemIconList(itemList);
                AddItemToPlayer(item.weapon);
            }
            Debug.Log($"{item.weaponName} added.");
        }
    }

    // Add Weapon to Player
    private void AddWeaponToPlayer(GameObject item)
    {
        Instantiate(item, playerWeaponParent);
    }

    // Add Item to Player
    private void AddItemToPlayer(GameObject item)
    {
        Instantiate(item, playerItemParent);
    }
}
