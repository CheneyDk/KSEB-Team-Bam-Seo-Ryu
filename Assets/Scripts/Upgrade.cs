using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;
using VInspector;

public class Upgrade : MonoBehaviour
{
    // 선택지들에 UI
    [Foldout("Upgrade UI")]
    public GameObject UpgradeUI;
    public Button[] optionButtons;
    public TextMeshProUGUI[] optionNameTexts;
    public TextMeshProUGUI[] optionDescTexts;
    public Image[] optionImages;

    // 전체 무기, 템들
    [Foldout("Weapon and Passive")]
    public List<WeaponData> allWeaponDataList;
    public List<WeaponData> weaponDataList;
    public List<WeaponData> passiveDataList;
    // 선택지로 나오는 템들 (Max템이 있으면 제거 할라는 용도)
    private List<WeaponData> listForWeapons;
    private List<WeaponData> listForPassives;

    // 템들을 뭘 선택했는지 알려주는 리스트(선택된 무기들 X)
    // 뭐 있는지만 알수 있어서 GameUI에 사용하는 용도
    [Foldout("Player Weapon & Item Slot")]
    public List<WeaponData> playerWeaponList = new List<WeaponData>();
    public List<WeaponData> playerPassiveList = new List<WeaponData>();
    public int maxItemNumber = 4;

    private bool isLevelUp = false;

    // 실제 무기들이 있는 곳
    // 여기서 실제 무기들에 레벨을 알수 있고 실제 강화도 여기서
    [Foldout("Player Weapon and Item")]
    public Transform playerWeaponBag;
    public Transform playerPassiveBag;

    // 게임의 UI
    public GameUI gameUI;

    // 강화템중에 MaxLevel이 많아서 선택지의 나오는 템 수가 3개 아하때 나오는 템들
    [Header("Item for MaxLevel")]
    public List<WeaponData> maxLevelItemList = new List<WeaponData>();
    [EndFoldout]

    // 플레이어가 선택했던 템들
    private List<WeaponData> selectedWeaponList = new List<WeaponData>();
    private List<WeaponData> selectedPassiveList = new List<WeaponData>();

    private Animator animator;

    public Dictionary<string, WeaponData> weaponDataDict = new Dictionary<string, WeaponData>();

    private void Awake()
    {
        ResetDataBase();

        foreach (var data in allWeaponDataList)
        {
            weaponDataDict.Add(data.name, data);
        }

        foreach (string name in SaveManager.instance.shopData.installedItemList)
        {
            weaponDataList.Add(weaponDataDict[name]);
        }

        animator = GetComponent<Animator>();
    }


    private void ResetDataBase()
    {
        foreach (var weapon in allWeaponDataList)
        {
            weapon.ResetToNew();
        }
        foreach (var passive in passiveDataList)
        {
            passive.ResetToNew();
        }
    }

    private IEnumerator OpenUI(bool TorF)
    {
        animator.SetBool("isOpen", TorF);
        yield return new WaitForSeconds(1f);
    }

    public void LevelUpUpgradeUI()
    {
        // 레업인지 웨이브 끝인지 확인
        // 선택창에 아이템 넣기
        // SeletItem(들어갈 템 list);

    }

    private void SeletIteamList(List<WeaponData> listOnSeletWindow)
    {
        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < listOnSeletWindow.Count)
            {
                optionNameTexts[i].text = listOnSeletWindow[i].curName;
                optionDescTexts[i].text = listOnSeletWindow[i].curDesc;
                optionImages[i].sprite = listOnSeletWindow[i].curImage;
                optionButtons[i].onClick.RemoveAllListeners();
                WeaponData selectedItem = listOnSeletWindow[i];
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



    // 선택한 템
    private void OnItemSelected(WeaponData selectedItem)
    {
        // 선택한게 아이템이면 그냥 플레이러에게 주기
        if (selectedItem.item.tag == "Item")
        {
            if (selectedItem.itemName == "Apple")
            {
                ItemPooling.Instance.GetApple(GameObject.FindGameObjectWithTag("Player").transform.position);
            }
            else if (selectedItem.itemName == "RedBlue")
            {
                ItemPooling.Instance.GetRedBlue(GameObject.FindGameObjectWithTag("Player").transform.position);
            }
            else if (selectedItem.itemName == "BitCoin")
            {
                ItemPooling.Instance.GetBitCoin(GameObject.FindGameObjectWithTag("Player").transform.position);
            }
        }

        // 레업이라 무기들 나오기
        else if (isLevelUp && selectedItem.item.tag == "Item")
        {
            AddOrUpgradeWeapon(selectedItem);
        }

        // wave끝이라 passive템 나오기
        else if (!isLevelUp && selectedItem.item.tag == "Item")
        {
            // 패시프 템 추가 혹은 강화
        }

        StartCoroutine(OpenUI(false));
        GameManager.Instance.EndUpgrade();
        isLevelUp = false;
    }

    private void AddOrUpgradeWeapon(WeaponData selectedItem)
    {
        var existingWeapon = selectedWeaponList.Find(i => i.itemName == selectedItem.itemName);
        if (existingWeapon != null)
        {
            GameInfoManager.Instance.DisplayGameInfo($"{selectedItem.itemName} upgrade.");

            var upgradeItemName = playerWeaponBag.Find(existingWeapon.item.name + ("(Clone)"));
            var upgradeWeapon = upgradeItemName.GetComponent<PlayerWeapon>();
            if (upgradeWeapon.isMaxLevel == true) // 선택힌게 강화 무기면 무기 강화 하고 선택지에서 빼기
            {
                upgradeWeapon.isPowerWeapon = true;
                gameUI.ChangePowerWeaponIcon(playerWeaponBag);
                WeaponData removeWeapon = selectedWeaponList.Find(i => i.name == upgradeWeapon.name);
                selectedWeaponList.Remove(removeWeapon);
            }
            else if (upgradeWeapon.weaponLevel == 4) // 무기upgrade선택후 max level이라 선택지에서 빼기
            {
                upgradeWeapon.Upgrade();
                gameUI.AddWeaponLevel(playerWeaponBag);
                WeaponData removeWeapon = selectedWeaponList.Find(i => i.name == upgradeWeapon.name);
                selectedWeaponList.Remove(removeWeapon);
            }
            else // 그냥 무기 강화
            {
                upgradeWeapon.Upgrade();
                gameUI.AddWeaponLevel(playerWeaponBag);
            }
        }
        else // 처음으로 선택한거면 그냥 무기 추가
        {
            selectedWeaponList.Add(selectedItem);
            gameUI.WeaponIconList(selectedWeaponList);
            AddWeaponToPlayer(selectedItem.item);
            gameUI.AddWeaponLevel(playerWeaponBag);
            SaveManager.instance.AddWeapon(selectedItem.itemName);

            GameInfoManager.Instance.DisplayGameInfo($"{selectedItem.itemName} added.");
        }
    }




    //-----------------------------------------------------------------------------------------------------














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
            sourceDataList = listForWeapons;
            itemList = playerWeaponList;
            CheckMaxLevelItem(playerWeaponBag);
            ItemToSelectedItems(selectedItems, sourceDataList, itemList, selectedWeaponList);
        }
        else if (levelup == false)
        {
            isLevelUp = levelup;
            sourceDataList = listForPassives;
            itemList = playerPassiveList;
            CheckMaxLevelItem(playerPassiveBag);
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


    // 무기가 Max이고 강화도 가능 한지 확인 하고 가능 하면 선택지에 추가
    private void CheckPowerWeapon()
    {
        for (int i = 0; i < playerWeaponBag.childCount; i++)
        {
            var playerWeapon = playerWeaponBag.GetComponentsInChildren<PlayerWeapon>()[i];

            // 무기중에서 maxLevel이 있는지 확인
            if (playerWeapon.isMaxLevel && !playerWeapon.isPowerWeapon)
            {

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

    // 템 선택수가 최대가 아리고 템이 Max레벨인지 확인, Max이면 선택지에 안 나오개 하기
    private void CheckMaxLevelItem(Transform itemBag)
    {
        if (itemBag == playerWeaponBag)
        {
            for (int i = 0; i < playerWeaponBag.childCount; i++)
            {
                var playerWeapon = playerWeaponBag.GetComponentsInChildren<PlayerWeapon>()[i];
                if (playerWeapon.isMaxLevel && !playerWeapon.isPowerWeapon)
                {
                    // maxLevel 무기일 경우 선택지에서 빼기
                    var findWeapon = selectedWeaponList.Find(i => i.item.name + ("(Clone)") == playerWeapon.name);

                    if (findWeapon != null)
                    {
                        selectedWeaponList.Remove(findWeapon);
                    }

                }
            }
        }
        else if (itemBag == playerPassiveBag)
        {
            for (int i = 0; i < playerPassiveBag.childCount; i++)
            {
                var playerPassive = playerPassiveBag.GetComponentsInChildren<PlayerPassive>()[i];
                if (playerPassive.isMaxLevel)
                {
                    var findPassive = selectedPassiveList.Find(i => i.item.name + ("(Clone)") == playerPassive.name);
                    if (findPassive != null)
                    {
                        selectedPassiveList.Remove(findPassive);
                    }

                }

            }
        }
    }

    // Put Items in to select windows
    private void ItemToSelectedItems(List<WeaponData> showOnSelectedItems, List<WeaponData> itemListForSelected, List<WeaponData> itemList, List<WeaponData> selectedItemsList)
    {
        // 템 소지수가 max아니면 그 data에 있는거에서 랜덤으로 추가
        if (itemList.Count != maxItemNumber)
        {
            while (showOnSelectedItems.Count < 3 && itemListForSelected.Count > 0)
            {
                WeaponData item = itemListForSelected[Random.Range(0, itemListForSelected.Count)];
                if (!showOnSelectedItems.Contains(item))
                {
                    showOnSelectedItems.Add(item);
                }
            }
        }

        // 템 수가 max으면, 이미 선택한 템만 나오게
        else if (itemList.Count == maxItemNumber)
        {
            if (itemListForSelected == listForWeapons && selectedItemsList.Count > 0)
            {

                CheckPowerWeapon();
                RemovePowerWeaponInList();
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

            // passive템은 뺄 일이 없어서 그냥 이미 선택한거 중에서 랜덤으로 나타나게
            else if (itemListForSelected == listForPassives && selectedItemsList.Count > 0)
            {
                while (selectedPassiveList.Count < 3)
                {
                    foreach (var item in maxLevelItemList)
                    {
                        if (!selectedPassiveList.Contains(item))
                        {
                            selectedPassiveList.Add(item);
                        }
                    }
                }

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


    private void RemovePowerWeaponInList()
    {
        // 먼저 제거할 항목들을 별도의 리스트에 담습니다.
        List<WeaponData> weaponsToRemove = new List<WeaponData>();

        for (int i = 0; i < playerWeaponBag.childCount; i++)
        {
            var playerWeapon = playerWeaponBag.GetComponentsInChildren<PlayerWeapon>()[i];
            if (playerWeapon.isPowerWeapon)
            {
                var findWeapon = selectedWeaponList.Find(w => w.item.name + "(Clone)" == playerWeapon.name);
                if (findWeapon != null)
                {
                    weaponsToRemove.Add(findWeapon);
                }
            }
        }

        // 이후 한 번에 제거합니다.
        foreach (var weapon in weaponsToRemove)
        {
            selectedWeaponList.Remove(weapon);
        }
    }


    // Add Item or Upgrade Item
    private void AddOrUpgradeItem(List<WeaponData> itemList, List<WeaponData> selectedList, Transform playerItemBag, WeaponData item)
    {

        var existingItem = itemList.Find(i => i.itemName == item.itemName);
        if (existingItem != null)
        {
            GameInfoManager.Instance.DisplayGameInfo($"{item.itemName} upgrade.");

            // Upgrade
            var upgradeItemName = playerItemBag.Find(existingItem.item.name + ("(Clone)"));
            var upgradeWeapon = upgradeItemName.GetComponent<PlayerWeapon>();
            var upgradePassive = upgradeItemName.GetComponent<PlayerPassive>();
            if (isLevelUp)
            {
                if (upgradeWeapon.isMaxLevel == true)
                {
                    upgradeWeapon.isPowerWeapon = true;
                    gameUI.ChangePowerWeaponIcon(playerItemBag);
                    WeaponData removeWeapon = selectedList.Find(i => i.name == upgradeWeapon.name);
                    selectedList.Remove(removeWeapon);
                }
                else
                {
                    upgradeWeapon.Upgrade();
                    gameUI.AddWeaponLevel(playerWeaponBag);
                }
            }
            else if (!isLevelUp)
            {
                upgradePassive.Upgrade();
                gameUI.AddPassiveLevel(playerPassiveBag);
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
                gameUI.AddWeaponLevel(playerWeaponBag);
                SaveManager.instance.AddWeapon(item.itemName);
            }
            else if (itemList == playerPassiveList)
            {
                gameUI.PassiveIconList(itemList);
                AddItemToPlayer(item.item);
                gameUI.AddPassiveLevel(playerPassiveBag);
            }
            GameInfoManager.Instance.DisplayGameInfo($"{item.itemName} added.");
        }
    }

    // Add Weapon to Player
    private void AddWeaponToPlayer(GameObject item)
    {
        Instantiate(item, playerWeaponBag);
    }

    // Add Item to Player
    private void AddItemToPlayer(GameObject item)
    {
        Instantiate(item, playerPassiveBag);
    }

}
