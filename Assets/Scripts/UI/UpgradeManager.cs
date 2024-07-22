using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public List<WeaponData> playerPassiveList = new List<WeaponData>();
    public int maxItemNumber = 4;

    private bool isLevelUp = false;

    [Header("Player Weapon and Item")]
    public Transform playerWeaponParent;
    public Transform playerPassiveParent;

    public GameUI gameUI;

    [Header("Item for MaxLevel")]
    public List<WeaponData> maxLevelItemList = new List<WeaponData>();

    // Cheak Weapon or Item is Max Level or not
    private List<WeaponData> randomPassiveList = new List<WeaponData>();
    private List<WeaponData> randomWeaponList = new List<WeaponData>();

    private Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }


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
            ItemToSelectedItems(selectedItems, sourceList, itemList, randomWeaponList);
        }
        else if (levelup == false)
        {
            isLevelUp = levelup;
            sourceList = itemDataList;
            itemList = playerPassiveList;
            ItemToSelectedItems(selectedItems, sourceList, itemList, randomPassiveList);
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

    // Put Items in to select windows
    private void ItemToSelectedItems(List<WeaponData> selectedItems, List<WeaponData> sourceList, List<WeaponData> itemList, List<WeaponData> randomList)
    {
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
            while (selectedItems.Count < 3 && randomList.Count > 0)
            {
                WeaponData item = randomList[Random.Range(0, randomList.Count)];
                if (!selectedItems.Contains(item))
                {
                    selectedItems.Add(item);
                }
            }
        }
    }

    // Add Random MaxLevel Item to randomPassiveList or randomWeaponList
    private void AddRandomMaxLevelItem(List<WeaponData> list)
    {
        var random = Random.Range(0, 3);
        if (list.Count < 3 && !list.Contains(maxLevelItemList[random]))
        {
            list.Add(maxLevelItemList[random]);
        }
    }

    // Add Selected Weapon or Item
    private void OnItemSelected(WeaponData item)
    {
        if (item.weapon.tag == "Item")
        {
            Instantiate(item.weapon, player.position, Quaternion.identity);
        }
        else if (isLevelUp == true)
        {
            AddOrUpgradeItem(playerWeaponList, randomWeaponList, playerWeaponParent, item, maxItemNumber);
        }
        else if (isLevelUp == false)
        {
            AddOrUpgradeItem(playerPassiveList, randomPassiveList, playerPassiveParent, item, maxItemNumber);
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
            var upgradeItemName = upgradeItem.Find(existingItem.weapon.name + ("(Clone)"));
            var upgradeWeapon = upgradeItemName.GetComponent<PlayerWeapon>();
            var upgradePassive = upgradeItemName.GetComponent<PlayerPassive>();
            if (isLevelUp)
            {
                if (upgradeWeapon.isMaxLevel == false)
                {
                    upgradeWeapon.Upgrade();
                }
                else if (upgradeWeapon.isMaxLevel == true)
                {
                    otherList.Remove(item);
                    AddRandomMaxLevelItem(otherList);
                }
            }
            else if (!isLevelUp)
            {
                if (upgradePassive.isMaxLevel == false)
                {
                    upgradePassive.Upgrade();
                }
                else if (upgradePassive.isMaxLevel == true)
                {
                    otherList.Remove(item);
                    AddRandomMaxLevelItem(otherList);
                }
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
                ScoreManager.instance.AddWeapon(item.weaponName);
            }
            else if (itemList == playerPassiveList)
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
        Instantiate(item, playerPassiveParent);
    }
}
