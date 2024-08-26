using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GPTUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private TextMeshProUGUI HPText;
    [SerializeField]
    private TextMeshProUGUI ATKText;

    [SerializeField]
    private List<GameObject> upgradeBox;

    public int upgradeLevel = 0;

    private AudioSource audioSource;
    public AudioClip upgradeClip;
    public AudioClip noMoneyClip;

    public int money;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        UpgradeBoxAdd();
        UpdateText();
    }

    public void AddUpgradeLevel()
    {
        if (money >= 2000 && upgradeLevel < 6)
        {
            audioSource.PlayOneShot(upgradeClip);
            money -= 2000;
            upgradeLevel += 1;
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
        for (int i = 0; i < upgradeLevel; i++)
        {
            upgradeBox[i].SetActive(true);
        }
    }

    void UpdateText()
    {
        levelText.text = ($"Level {upgradeLevel.ToString()}");
        HPText.text = ($"Add {upgradeLevel + 1}0% debugging HP");
        ATKText.text = ($"Add {upgradeLevel + 1}0% debugging ATK");
    }
}
