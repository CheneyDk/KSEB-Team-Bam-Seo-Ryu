using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    public List<WeaponData> passiveDataList;


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
    private List<WeaponData> selectedPassiveList = new List<WeaponData>();
    private List<WeaponData> selectedWeaponList = new List<WeaponData>();

    public Animator animator;

    private Transform player;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private IEnumerator OpenUI(bool TorF)
    {
        animator.SetBool("isOpen", TorF);
        yield return new WaitForSeconds(1f);
    }


    // Upgrade UI
    public void OnUpgrade(bool levelup)
    {
        UpgradeUI.SetActive(true);
        StartCoroutine(OpenUI(true));

        List<WeaponData> selectedItems = new List<WeaponData>();
        List<WeaponData> sourceDataList = null;
        List<WeaponData> itemList = null;

        if (levelup == true)
        {
            isLevelUp = levelup;
            sourceDataList = weaponDataList;
            itemList = playerWeaponList;
            ItemToSelectedItems(selectedItems, sourceDataList, itemList, selectedWeaponList);
        }
        else if (levelup == false)
        {
            isLevelUp = levelup;
            sourceDataList = passiveDataList;
            itemList = playerPassiveList;
            ItemToSelectedItems(selectedItems, sourceDataList, itemList, selectedPassiveList);
        }

        if (sourceDataList != null && itemList != null)
        {
            for (int i = 0; i < optionButtons.Length; i++)
            {
                if (i < selectedItems.Count)
                {
                    optionNameTexts[i].text = selectedItems[i].curName;
                    optionDescTexts[i].text = selectedItems[i].curDesc;
                    optionImages[i].sprite = selectedItems[i].curImage;
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

    private void CheckPowerWeapon()
    {
        for (int i = 0; i < selectedWeaponList.Count; i++)
        {
            var weaponData = selectedWeaponList[i];
            var playerWeapon = playerWeaponParent.GetComponentsInChildren<PlayerWeapon>()[i];

            // 무기중에서 maxLevel이 있는지 확인
            if (playerWeapon.isMaxLevel)
            {
                // maxLevel 무기일 경우 선택지에서 빼기
                if (playerWeapon.name == weaponData.item.name + ("(Clone)"))
                {
                    selectedWeaponList.Remove(weaponData);
                }

                // maxLevel 무기 중에 그의 맞는 passive템이 있는지 확인
                bool hasMatchingPassive = false;
                foreach (var passive in playerPassiveList)
                {
                    if (playerWeapon.matchPassive == passive.itemName)
                    {
                        hasMatchingPassive = true;
                        break;
                    }
                }

                // 그의 맞는 passive템이 있으면 다시 선택지에 powerWeapon으로 추가

                if (hasMatchingPassive)
                {
                    var findItem = weaponDataList.Find(i => i.item.name + ("(Clone)") == playerWeapon.name);
                    findItem.curImage = findItem.powerImage;
                    findItem.curName = findItem.powerName;
                    findItem.curDesc = findItem.powerDesc;
                    selectedWeaponList.Add(findItem);
                }
            }
        }

        // 만약에 다 만랩이라 다 빠지고 다른 아이템들 추가
        AddRandomMaxLevelItem(selectedWeaponList);
    }


    // Put Items in to select windows
    private void ItemToSelectedItems(List<WeaponData> showOnSelectedItems, List<WeaponData> sourceList, List<WeaponData> itemList, List<WeaponData> selectedItemsList)
    {
        // 템 소지수가 max아니면 그 data에 있는거에서 랜덤으로 추가
        if (itemList.Count != maxItemNumber)
        {
            while (showOnSelectedItems.Count < 3 && sourceList.Count > 0)
            {
                WeaponData item = sourceList[Random.Range(0, sourceList.Count)];
                if (!showOnSelectedItems.Contains(item))
                {
                    showOnSelectedItems.Add(item);
                }
            }
        }

        // 템 수가 max으면, 이미 선택한 템만 나오게
        else if (itemList.Count == maxItemNumber)
        {
            if (sourceList == weaponDataList)
            {
                CheckPowerWeapon();
                // 선택한 무기list에서 랜덤으로 선택지에 나오게 하기
                while (showOnSelectedItems.Count < 3 && selectedItemsList.Count > 0)
                {
                    WeaponData item = selectedItemsList[Random.Range(0, selectedItemsList.Count)];
                    if (!showOnSelectedItems.Contains(item))
                    {
                        showOnSelectedItems.Add(item);
                    }
                }
            }

            // passive템은 뺄일이 없어서 그냥 이미 선택한거 중에서 랜덤으로 나타나게
            else if (sourceList == passiveDataList)
            {
                while (showOnSelectedItems.Count < 3 && selectedItemsList.Count > 0)
                {
                    WeaponData item = selectedItemsList[Random.Range(0, selectedItemsList.Count)];
                    if (!showOnSelectedItems.Contains(item))
                    {
                        showOnSelectedItems.Add(item);
                    }
                }
            }
        }
    }

    // 랜덤으로 max레벨때 넣어야하는 템들 넣기
    private void AddRandomMaxLevelItem(List<WeaponData> list)
    {
        var random = Random.Range(0, 3);
        if (list.Count < 3 && !list.Contains(maxLevelItemList[random]))
        {
            list.Add(maxLevelItemList[random]);
        }
    }

    // 선택한 템이 무기인지, passive템인지, maxLevel템인지 구분하기
    private void OnItemSelected(WeaponData item)
    {
        // maxLevel템
        if (item.item.tag == "Item")
        {
            Instantiate(item.item, player.position, Quaternion.identity);
        }

        // 레업이라 무기들 나오기
        else if (isLevelUp)
        {
            AddOrUpgradeItem(playerWeaponList, selectedWeaponList, playerWeaponParent, item);
        }

        // wave끝이라 passive템 나오기
        else if (!isLevelUp)
        {
            AddOrUpgradeItem(playerPassiveList, selectedPassiveList, playerPassiveParent, item);
        }

        StartCoroutine(OpenUI(false));
        GameManager.Instance.EndUpgrade();
        isLevelUp = false;
    }

    // Add Item or Upgrade Item
    private void AddOrUpgradeItem(List<WeaponData> itemList, List<WeaponData> selectedList, Transform upgradeItem, WeaponData item)
    {


        var existingItem = itemList.Find(i => i.itemName == item.itemName);
        if (existingItem != null)
        {
            Debug.Log($"{item.itemName} upgrade.");

            // Upgrade
            var upgradeItemName = upgradeItem.Find(existingItem.item.name + ("(Clone)"));
            var upgradeWeapon = upgradeItemName.GetComponent<PlayerWeapon>();
            var upgradePassive = upgradeItemName.GetComponent<PlayerPassive>();
            if (isLevelUp)
            {
                if (upgradeWeapon.isMaxLevel == true)
                {
                    upgradeWeapon.isPowerWeapon = true;
                    selectedList.Remove(item);
                }
                else
                {
                    upgradeWeapon.Upgrade();
                }
            }
            else if (!isLevelUp)
            {
                upgradePassive.Upgrade();
            }
        }
        else
        {
            // Add Weapon or Item to itemList
            itemList.Add(item);
            selectedList.Add(item);
            if (itemList == playerWeaponList)
            {
                gameUI.WeaponIconList(itemList);
                AddWeaponToPlayer(item.item);
                ScoreManager.instance.AddWeapon(item.itemName);
            }
            else if (itemList == playerPassiveList)
            {
                gameUI.ItemIconList(itemList);
                AddItemToPlayer(item.item);
            }
            Debug.Log($"{item.itemName} added.");
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
