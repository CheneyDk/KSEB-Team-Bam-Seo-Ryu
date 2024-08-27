using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;
using VInspector;

public class UpgradeManager : MonoBehaviour
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
    public List<WeaponData> myWeaponDataList;
    public List<WeaponData> passiveDataList;
    // 선택지로 나오는 템들 (Max템이 있으면 제거 할라는 용도)
    private List<WeaponData> candidateWeaponList;
    private List<WeaponData> candidatePassiveList;

    // 템들을 뭘 선택했는지 알려주는 리스트(선택된 무기들 X)
    // 뭐 있는지만 알수 있어서 GameUI에 사용하는 용도
    [Foldout("Player Weapon & Item Slot")]
    public List<WeaponData> playerSelectedWeaponList = new List<WeaponData>();
    public List<WeaponData> playerSelectedPassiveList = new List<WeaponData>();
    private int maxItemNumber = 4;

    private bool LvUpFlag = false;

    // 실제 무기들이 있는 곳
    // 여기서 실제 무기들에 레벨을 알수 있고 실제 강화도 여기서
    [Foldout("Player Weapon and Item")]
    public Transform playerWeaponBag;
    public Transform playerPassiveBag;

    // 게임의 UI
    public GameUI gameUI;

    // 강화템중에 MaxLevel이 많아서 선택지의 나오는 템 수가 3개 아하때 나오는 템들
    [Header("Extra Item")]
    public List<WeaponData> extraItemList = new List<WeaponData>();
    [EndFoldout]

    private Animator animator;

    private Dictionary<string, WeaponData> weaponDataDict = new Dictionary<string, WeaponData>();

    private void Awake()
    {
        ResetEvolIcon();

        foreach (var data in allWeaponDataList)
        {
            weaponDataDict.Add(data.name, data);
        }

        foreach (string name in SaveManager.instance.shopData.installedItemList)
        {
            myWeaponDataList.Add(weaponDataDict[name]);
        }

        candidateWeaponList = new(myWeaponDataList);
        candidatePassiveList = new(passiveDataList);
        animator = GetComponent<Animator>();
    }

    private void ResetEvolIcon()
    {
        foreach (var weapon in allWeaponDataList)
        {
            weapon.ResetToNew();
        }
    }

    private IEnumerator OpenUI(bool TorF)
    {
        animator.SetBool("isOpen", TorF);
        yield return new WaitForSeconds(1f);
    }


    // Upgrade UI
    public void OnUpgrade(bool isLevelup)
    {
        UpgradeUI.SetActive(true);
        StartCoroutine(OpenUI(true));

        LvUpFlag = isLevelup;

        List<WeaponData> UIAppearItemList = new List<WeaponData>();
        List<WeaponData> candidateItemList = LvUpFlag ? candidateWeaponList : candidatePassiveList;
        List<WeaponData> playerSelectedItemList = LvUpFlag ? playerSelectedWeaponList : playerSelectedPassiveList;

        if (playerSelectedItemList.Count == maxItemNumber)
        {
            candidateItemList.Clear();
            foreach (var item in playerSelectedItemList)
            {
                candidateItemList.Add(item);
            }
        }

        CheckMaxLevelItem();

        if (LvUpFlag)
        {
            ItemToSelectedItems(UIAppearItemList, candidateItemList, playerSelectedItemList);
        }
        else
        {
            ItemToSelectedItems(UIAppearItemList, candidateItemList, playerSelectedItemList);
        }

        if (candidateItemList != null && playerSelectedItemList != null)
        {
            for (int i = 0; i < optionButtons.Length; i++)
            {
                if (i < UIAppearItemList.Count)
                {
                    optionNameTexts[i].text = UIAppearItemList[i].curName;
                    optionDescTexts[i].text = UIAppearItemList[i].curDesc;
                    optionImages[i].sprite = UIAppearItemList[i].curImage;
                    optionButtons[i].onClick.RemoveAllListeners();
                    WeaponData selectedItem = UIAppearItemList[i];
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
                foreach (var passive in playerSelectedPassiveList)
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
                    var findItem = myWeaponDataList.Find(i => i.item.name + ("(Clone)") == playerWeapon.name);
                    findItem.curImage = findItem.powerImage;
                    findItem.curName = findItem.powerName;
                    findItem.curDesc = findItem.powerDesc;
                    candidateWeaponList.Add(findItem);
                }
            }
        }
    }

    private void CheckMaxLevelItem()
    {
        if (LvUpFlag)
        {
            for (int i = 0; i < playerWeaponBag.childCount; i++)
            {
                var playerWeapon = playerWeaponBag.GetComponentsInChildren<PlayerWeapon>()[i];
                if (playerWeapon.isMaxLevel)
                {
                    // maxLevel 무기일 경우 선택지에서 빼기
                    var maxLvWeapon = candidateWeaponList.Find(i => i.item.name + ("(Clone)") == playerWeapon.name);

                    if (maxLvWeapon != null)
                    {
                        candidateWeaponList.Remove(maxLvWeapon);
                    }

                }
            }
        }
        else
        {
            for (int i = 0; i < playerPassiveBag.childCount; i++)
            {
                var playerPassive = playerPassiveBag.GetComponentsInChildren<PlayerPassive>()[i];
                if (playerPassive.isMaxLevel)
                {
                    var maxLvPassive = candidatePassiveList.Find(i => i.item.name + ("(Clone)") == playerPassive.name);
                    if (maxLvPassive != null)
                    {
                        candidatePassiveList.Remove(maxLvPassive);
                    }
                }
            }
        }
    }

    // Put Items in to select windows
    private void ItemToSelectedItems(List<WeaponData> UIAppearItemList, List<WeaponData> candidateItemList, List<WeaponData> playerSelectedItemList)
    {
        // 템 소지수가 max아니면 그 data에 있는거에서 랜덤으로 추가
        if (playerSelectedItemList.Count < maxItemNumber)
        {
            while (UIAppearItemList.Count < 3 && candidateItemList.Count > 0)
            {
                WeaponData item = candidateItemList[Random.Range(0, candidateItemList.Count)];
                if (!UIAppearItemList.Contains(item))
                {
                    UIAppearItemList.Add(item);
                }
            }
        }

        // playerSelectedItemList.Count == maxItemNumber
        else
        {
            AddExtraItem(candidateItemList);

            if (LvUpFlag)
            {
                CheckPowerWeapon();
                // 선택한 무기list에서 랜덤으로 선택지에 나오게 하기
                while (UIAppearItemList.Count < 3 && candidateItemList.Count > 0)
                {
                    WeaponData item = candidateItemList[Random.Range(0, candidateItemList.Count)];
                    if (!UIAppearItemList.Contains(item))
                    {
                        UIAppearItemList.Add(item);
                    }
                }
            }

            // passive
            else
            {
                while (UIAppearItemList.Count < 3 && candidateItemList.Count > 0)
                {
                    WeaponData item = candidateItemList[Random.Range(0, candidateItemList.Count)];
                    if (!UIAppearItemList.Contains(item))
                    {
                        UIAppearItemList.Add(item);
                    }
                }
            }
        }
    }

    private void AddExtraItem(List<WeaponData> list)
    {
        while (list.Count < 3)
        {
            var random = Random.Range(0, extraItemList.Count);
            if (!list.Contains(extraItemList[random]))
            {
                list.Add(extraItemList[random]);
            }
        }
    }

    // 선택한 템이 무기인지, passive템인지, maxLevel템인지 구분하기
    private void OnItemSelected(WeaponData item)
    {
        // maxLevel템
        if (item.item.tag == "Item")
        {
            if (item.itemName == "Apple")
            {
                ItemPooling.Instance.GetApple(GameObject.FindGameObjectWithTag("Player").transform.position);
            }
            else if (item.itemName == "RedBlue")
            {
                ItemPooling.Instance.GetRedBlue(GameObject.FindGameObjectWithTag("Player").transform.position);
            }
            else if (item.itemName == "BitCoin")
            {
                ItemPooling.Instance.GetBitCoin(GameObject.FindGameObjectWithTag("Player").transform.position);
            }
        }

        else
        {
            if (LvUpFlag)
            {
                AddOrUpgradeItem(playerSelectedWeaponList, candidateWeaponList, playerWeaponBag, item);
            }
            else
            {
                AddOrUpgradeItem(playerSelectedPassiveList, candidatePassiveList, playerPassiveBag, item);
            }
        }

        StartCoroutine(OpenUI(false));
        GameManager.Instance.EndUpgrade();
        LvUpFlag = false;
    }

    // Add Item or Upgrade Item
    private void AddOrUpgradeItem(List<WeaponData> playerSelectedItemList, List<WeaponData> candidateItemList, Transform playerItemBag, WeaponData item)
    {
        var existingItem = playerSelectedItemList.Find(i => i.itemName == item.itemName);

        // Upgrade(Lv Up) Item
        if (existingItem != null)
        {
            GameInfoManager.Instance.DisplayGameInfo($"{item.itemName} upgrade.");

            // Upgrade
            var upgradeItemName = playerItemBag.Find(existingItem.item.name + ("(Clone)"));
            var upgradeWeapon = upgradeItemName.GetComponent<PlayerWeapon>();
            var upgradePassive = upgradeItemName.GetComponent<PlayerPassive>();
            if (LvUpFlag)
            {
                if (upgradeWeapon.isMaxLevel == true)
                {
                    upgradeWeapon.isPowerWeapon = true;
                    gameUI.ChangePowerWeaponIcon(playerItemBag);
                    WeaponData removeWeapon = candidateWeaponList.Find(i => i.name == upgradeWeapon.name);
                    candidateWeaponList.Remove(removeWeapon);
                }
                else
                {
                    upgradeWeapon.Upgrade();
                    gameUI.AddWeaponLevel(playerWeaponBag);
                }
            }
            else
            {
                upgradePassive.Upgrade();
                gameUI.AddPassiveLevel(playerPassiveBag);
            }
        }

        // Add Item
        else
        {
            playerSelectedItemList.Add(item);
            if (LvUpFlag)
            {
                gameUI.WeaponIconList(playerSelectedItemList);
                AddWeaponToPlayer(item.item);
                gameUI.AddWeaponLevel(playerWeaponBag);
                SaveManager.instance.AddWeapon(item.itemName);
            }
            else
            {
                gameUI.PassiveIconList(playerSelectedItemList);
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
