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

    private AudioSource audioSource;
    public AudioClip buyClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Set(string name, int price)
    {
        itemName.text = name;
        itemImage.sprite = Resources.Load<Sprite>(name);
        itemPrice[0].text = "$" + price.ToString() + "\nInstall";
        itemPrice[1].text = "$" + price.ToString() + "\nInstall";
    }

    public void Installed() {
        button.interactable = false;
        audioSource.PlayOneShot(buyClip);
        itemPrice[0].text = "$-\nInstalled";
        itemPrice[1].text = "$-\nInstalled";
    }

    public void ChangeUpdatePage() {
        audioSource.PlayOneShot(buyClip);
        itemName.text = "PowerPet";
        itemImage.sprite = Resources.Load<Sprite>("PowerPet");
        itemPrice[0].text = "$1000\nInstall";
        itemPrice[1].text = "$1000\nInstall";
    }
}
