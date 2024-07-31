using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuyItem : MonoBehaviour
{
    public void Buy(string name)
    {
        foreach (var item in ScoreManager.instance.recordData.items)
        {
            if (item.itemName == name)
            {
                item.isBought = true;
                break;
            }
        }

        GetComponent<Button>().interactable = false;
    }
}
