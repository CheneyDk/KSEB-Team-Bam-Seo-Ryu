using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSetter : MonoBehaviour
{
    public TextMeshProUGUI itemName;
    public Image itemImage;
    public Button button;
    public TextMeshProUGUI[] itemPrice;

    public void Set(string name, int price)
    {
        itemName.text = name;
        itemImage.sprite = Resources.Load<Sprite>(name);
        itemPrice[0].text = "$" + price.ToString() + "\nInstall";
        itemPrice[1].text = "$" + price.ToString() + "\nInstall";
    }

    public void Installed() {
        button.interactable = false;
        itemPrice[0].text = "$-\nInstalled";
        itemPrice[1].text = "$-\nInstalled";
    }
}
