using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;
using VInspector;

public class RE_um : MonoBehaviour
{
    // �������鿡 UI
    [Foldout("Upgrade UI")]
    public GameObject UpgradeUI;
    public Button[] optionButtons;
    public TextMeshProUGUI[] optionNameTexts;
    public TextMeshProUGUI[] optionDescTexts;
    public Image[] optionImages;

    // ��ü ����, �۵�
    [Foldout("Weapon and Passive")]
    public List<WeaponData> allWeaponDataList;
    public List<WeaponData> myWeaponDataList;
    public List<WeaponData> passiveDataList;
    // �������� ������ �۵� (Max���� ������ ���� �Ҷ�� �뵵)
    private List<WeaponData> candidateWeaponList;
    private List<WeaponData> candidatePassiveList;

    // �۵��� �� �����ߴ��� �˷��ִ� ����Ʈ(���õ� ����� X)
    // �� �ִ����� �˼� �־ GameUI�� ����ϴ� �뵵
    [Foldout("Player Weapon & Item Slot")]
    public List<WeaponData> playerSelectedWeaponList = new List<WeaponData>();
    public List<WeaponData> playerSelectedPassiveList = new List<WeaponData>();
    private int maxItemNumber = 4;

    private bool LvUpFlag = false;

    // ���� ������� �ִ� ��
    // ���⼭ ���� ����鿡 ������ �˼� �ְ� ���� ��ȭ�� ���⼭
    [Foldout("Player Weapon and Item")]
    public Transform playerWeaponBag;
    public Transform playerPassiveBag;

    // ������ UI
    public GameUI gameUI;

    // ��ȭ���߿� MaxLevel�� ���Ƽ� �������� ������ �� ���� 3�� ���϶� ������ �۵�
    [Header("Extra Item")]
    public List<WeaponData> extraItemList = new List<WeaponData>();
    [EndFoldout]

    // �� ���ü��� �ִ��϶� �������� ��Ÿ���� �۵�
    private List<WeaponData> selectedPassiveList = new List<WeaponData>();
    private List<WeaponData> selectedWeaponList = new List<WeaponData>();

    private Animator animator;

    private Dictionary<string, WeaponData> weaponDataDict = new Dictionary<string, WeaponData>();

    private void Awake()
    {
        ResetDataBase();

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

    private void ResetDataBase()
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

        List<WeaponData> selectedItems = new List<WeaponData>();
        List<WeaponData> sourceDataList = null;
        List<WeaponData> itemList = null;

        if (isLevelup == true)
        {
            LvUpFlag = isLevelup;
            sourceDataList = candidateWeaponList;
            itemList = playerSelectedWeaponList;
            CheckMaxLevelItem(playerWeaponBag);
            ItemToSelectedItems(selectedItems, sourceDataList, itemList, selectedWeaponList);
        }
        else if (isLevelup == false)
        {
            LvUpFlag = isLevelup;
            sourceDataList = candidatePassiveList;
            itemList = playerSelectedPassiveList;
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

    // ���Ⱑ Max�̰� ��ȭ�� ���� ���� Ȯ�� �ϰ� ���� �ϸ� �������� �߰�
    private void CheckPowerWeapon()
    {
        for (int i = 0; i < playerWeaponBag.childCount; i++)
        {
            var playerWeapon = playerWeaponBag.GetComponentsInChildren<PlayerWeapon>()[i];

            // �����߿��� maxLevel�� �ִ��� Ȯ��
            if (playerWeapon.isMaxLevel && !playerWeapon.isPowerWeapon)
            {

                // maxLevel ���� �߿� ���� �´� passive���� �ִ��� Ȯ��
                bool hasMatchingPassive = false;
                foreach (var passive in playerSelectedPassiveList)
                {
                    if (playerWeapon.matchPassive == passive.itemName)
                    {
                        hasMatchingPassive = true;
                        break;
                    }
                }

                // ���� �´� passive���� ������ �ٽ� �������� powerWeapon���� �߰�

                if (hasMatchingPassive)
                {
                    var findItem = myWeaponDataList.Find(i => i.item.name + ("(Clone)") == playerWeapon.name);
                    findItem.curImage = findItem.powerImage;
                    findItem.curName = findItem.powerName;
                    findItem.curDesc = findItem.powerDesc;
                    selectedWeaponList.Add(findItem);
                }
            }
        }

        // ���࿡ �� �����̶� �� ������ �ٸ� �����۵� �߰�
        AddRandomMaxLevelItem(selectedWeaponList);
    }

    // �� ���ü��� �ִ밡 �Ƹ��� ���� Max�������� Ȯ��, Max�̸� �������� �� ������ �ϱ�
    private void CheckMaxLevelItem(Transform itemBag)
    {
        if (itemBag == playerWeaponBag)
        {
            for (int i = 0; i < playerWeaponBag.childCount; i++)
            {
                var playerWeapon = playerWeaponBag.GetComponentsInChildren<PlayerWeapon>()[i];
                if (playerWeapon.isMaxLevel && !playerWeapon.isPowerWeapon)
                {
                    // maxLevel ������ ��� ���������� ����
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
        // �� �������� max�ƴϸ� �� data�� �ִ°ſ��� �������� �߰�
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

        // �� ���� max����, �̹� ������ �۸� ������
        else if (itemList.Count == maxItemNumber)
        {
            if (itemListForSelected == candidateWeaponList && selectedItemsList.Count > 0)
            {

                CheckPowerWeapon();
                // ������ ����list���� �������� �������� ������ �ϱ�
                while (showOnSelectedItems.Count < 3 && selectedItemsList.Count > 0)
                {
                    WeaponData item = selectedItemsList[Random.Range(0, selectedItemsList.Count)];
                    if (!showOnSelectedItems.Contains(item))
                    {
                        showOnSelectedItems.Add(item);
                    }
                }
            }

            // passive���� �� ���� ��� �׳� �̹� �����Ѱ� �߿��� �������� ��Ÿ����
            else if (itemListForSelected == candidatePassiveList && selectedItemsList.Count > 0)
            {
                while (selectedPassiveList.Count < 3)
                {
                    foreach (var item in extraItemList)
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

    // �������� max������ �־���ϴ� �۵� �ֱ�
    private void AddRandomMaxLevelItem(List<WeaponData> list)
    {
        var random = Random.Range(0, 3);
        if (list.Count < 3 && !list.Contains(extraItemList[random]))
        {
            list.Add(extraItemList[random]);
        }
    }

    // ������ ���� ��������, passive������, maxLevel������ �����ϱ�
    private void OnItemSelected(WeaponData item)
    {
        // maxLevel��
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

        // �����̶� ����� ������
        else if (LvUpFlag)
        {
            AddOrUpgradeItem(playerSelectedWeaponList, selectedWeaponList, playerWeaponBag, item);
            RemovePowerWeaponInList();
        }

        // wave���̶� passive�� ������
        else if (!LvUpFlag)
        {
            AddOrUpgradeItem(playerSelectedPassiveList, selectedPassiveList, playerPassiveBag, item);
        }

        StartCoroutine(OpenUI(false));
        GameManager.Instance.EndUpgrade();
        LvUpFlag = false;
    }

    private void RemovePowerWeaponInList()
    {
        for (int i = 0; i < playerWeaponBag.childCount; i++)
        {
            var playerWeapon = playerWeaponBag.GetComponentsInChildren<PlayerWeapon>()[i];
            if (playerWeapon.isPowerWeapon)
            {
                // maxLevel ������ ��� ���������� ����
                var findWeapon = selectedWeaponList.Find(i => i.item.name + ("(Clone)") == playerWeapon.name);

                if (findWeapon != null)
                {
                    selectedWeaponList.Remove(findWeapon);
                }

            }
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
            if (LvUpFlag)
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
            else if (!LvUpFlag)
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
            if (itemList == playerSelectedWeaponList)
            {
                gameUI.WeaponIconList(itemList);
                AddWeaponToPlayer(item.item);
                gameUI.AddWeaponLevel(playerWeaponBag);
                SaveManager.instance.AddWeapon(item.itemName);
            }
            else if (itemList == playerSelectedPassiveList)
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
