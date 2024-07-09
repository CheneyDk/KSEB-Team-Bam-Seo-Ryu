using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public GameObject UpgradeUI;
    public Button[] optionButtons;
    public TextMeshProUGUI[] optionNameTexts;
    public TextMeshProUGUI[] optionDescTexts;
    public Image[] optionImages;

    public bool isLevelUp = false;

    public List<WeaponData> weaponDataList;
    public List<WeaponData> itemDataList;


    public void OnUpgrade(WeaponType type, bool levelup)
    {
        UpgradeUI.SetActive(true);

        List<WeaponData> selectedItems = new List<WeaponData>();
        List<WeaponData> sourceList = null;

        if (levelup == true)
        {
            sourceList = weaponDataList;
        }
        else if (levelup == false)
        {
            sourceList = itemDataList;
        }

        if (sourceList != null)
        {
            while (selectedItems.Count < 3 && sourceList.Count > 0)
            {
                WeaponData item = sourceList[Random.Range(0, sourceList.Count)];
                if (!selectedItems.Contains(item))
                {
                    selectedItems.Add(item);
                }
            }

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

        GameManager.Instance.EndUpgrade();

        // 선택된 아이템에 대한 추가 로직
    }
}
