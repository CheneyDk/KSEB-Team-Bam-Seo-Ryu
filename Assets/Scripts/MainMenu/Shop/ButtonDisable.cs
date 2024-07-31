using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDisable : MonoBehaviour
{
    public Button[] buttons;

    private void Awake()
    {
        foreach (var data in ScoreManager.instance.recordData.items)
        {
            if (data.isBought)
            {
                buttons[ScoreManager.instance.match[data.itemName]].interactable = false;
            }
        }
    }
}
