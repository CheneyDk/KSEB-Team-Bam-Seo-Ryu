using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public GameObject levelUpUI;
    public Button[] optionButtons;
    public TextMeshProUGUI[] optionTexts;

    private List<string> items = new List<string> { "a", "b", "c", "d", "e", "f" };

    private void Start()
    {
        //levelUpUI.SetActive(false);
        OnLevelUp();
    }

    public void OnLevelUp()
    {
        levelUpUI.SetActive(true);

        List<string> selectedItems = new List<string>();
        while (selectedItems.Count < 3)
        {
            string item = items[Random.Range(0, items.Count)];
            if (!selectedItems.Contains(item))
            {
                selectedItems.Add(item);
            }
        }

        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionTexts[i].text = selectedItems[i];
            optionButtons[i].onClick.RemoveAllListeners();
            string selectedItem = selectedItems[i];
            optionButtons[i].onClick.AddListener(() => OnItemSelected(selectedItem));
        }
    }

    private void OnItemSelected(string item)
    {
        Debug.Log($"{item} 아이템 선택.");

        levelUpUI.SetActive(false);
    }
}
