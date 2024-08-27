using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VInspector;

public class GPTUI : MonoBehaviour
{
    [Header("Text UI")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI HPText;
    public TextMeshProUGUI ATKText;
    public TextMeshProUGUI[] moneyText;
    public TextMeshProUGUI upgradePriceText;
    [EndFoldout]

    [SerializeField]
    private List<GameObject> upgradeBox;

    private AudioSource audioSource;
    public AudioClip upgradeClip;
    public AudioClip noMoneyClip;

    private int upgradePrice;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        UpgradeBoxAdd();
        UpdateText();
    }

    private void Update()
    {
        foreach (var text in moneyText)
        {
            text.text = SaveManager.instance.shopData.money.ToString();
        }
        upgradePrice = 2000 + SaveManager.instance.shopData.GPTLv * 500;
        upgradePriceText.text = upgradePrice.ToString();
    }

    public void AddUpgradeLevel()
    {
        if (SaveManager.instance.shopData.money >= upgradePrice && SaveManager.instance.shopData.GPTLv < 6)
        {
            audioSource.PlayOneShot(upgradeClip);
            SaveManager.instance.shopData.money -= upgradePrice;
            SaveManager.instance.shopData.GPTLv += 1;
            UpgradeBoxAdd();
            UpdateText();
        }
        else
        {
            audioSource.PlayOneShot(noMoneyClip);
            return;
        }
    }

    void UpgradeBoxAdd()
    {
        for (int i = 0; i < SaveManager.instance.shopData.GPTLv; i++)
        {
            upgradeBox[i].SetActive(true);
        }
    }

    void UpdateText()
    {
        levelText.text = $"Level {SaveManager.instance.shopData.GPTLv}";
        HPText.text = ($"Add {SaveManager.instance.shopData.GPTLv + 1}0% debugging HP");
        ATKText.text = ($"Add {SaveManager.instance.shopData.GPTLv + 1}0% debugging ATK");
    }
}
