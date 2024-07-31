using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("Wave")]
    [SerializeField]
    private TextMeshProUGUI waveNumber;
    [SerializeField]
    private TextMeshProUGUI waveTimer;

    [Header("Bar")]
    [SerializeField]
    private Slider healthBar;
    [SerializeField]
    private TextMeshProUGUI healthText;
    [SerializeField]
    private Slider expBar;
    [SerializeField]
    private TextMeshProUGUI expText;

    [SerializeField]
    private WaveManager waveManager;

    [Header("Icon")]
    public GameObject[] weaponIconList;
    public Image[] weaponImage;
    public GameObject[] passiveIconList;
    public Image[] passiveImage;

    [SerializeField]
    private Player player;

    public TextMeshProUGUI[] weaponLevel;
    public TextMeshProUGUI[] passiveLevel;


    private void Awake()
    {
        waveNumber.text = "WAVE : " + waveManager.curWave.ToString();
        waveTimer.text = waveManager.time.ToString("N0");
        healthBar.value = healthBar.maxValue;
    }

    private void Update()
    {
        waveNumber.text = "WAVE : " + waveManager.curWave.ToString();
        waveTimer.text = waveManager.time.ToString("N0");
        healthText.text = player.playerCurHp.ToString("N0") + " / " + player.playerMaxHp.ToString("N0");
        expText.text = player.playerCurExp.ToString("N0") + " / " + player.playerMaxExp.ToString("N0");
        healthBar.value = healthBar.maxValue * (player.playerCurHp / player.playerMaxHp);
        expBar.value = expBar.maxValue * (player.playerCurExp / player.playerMaxExp);
    }

    public void WeaponIconList(List<WeaponData> dataList)
    {
        for (int i = 0; i < dataList.Count; i++)
        {
            weaponImage[i].sprite = dataList[i].itemImage;
            weaponIconList[i].gameObject.SetActive(true);
        }
    }

    public void ChangePowerWeaponIcon(Transform playerWeaponBag)
    {
        for (int i = 0; i < playerWeaponBag.childCount; i++)
        {
            var weapon = playerWeaponBag.GetChild(i);
            var compon = weapon.GetComponent<PlayerWeapon>();
            if (compon.isPowerWeapon == true)
            {
                weaponImage[i].sprite = compon.powerWeaponSprite;
            }
        }
    }

    public void PassiveIconList(List<WeaponData> dataList)
    {
        for (int i = 0; i < dataList.Count; i++)
        {
            passiveImage[i].sprite = dataList[i].itemImage;
            passiveIconList[i].gameObject.SetActive(true);
        }
    }

    public void AddWeaponLevel(Transform playerWeaponBag)
    {
        for ( int i = 0; i < playerWeaponBag.childCount; i++)
        {
            var weapons = playerWeaponBag.GetComponentsInChildren<PlayerWeapon>()[i];
            var weaponLevels = weapons.weaponLevel;
            if (weaponLevels > 4f)
            {
                weaponLevel[i].text = "Max";
            }
            else if(weapons.isPowerWeapon == true && weaponLevels == 5)
            {
                weaponLevel[i].text = "Pow";
            }
            else
            {
                weaponLevel[i].text = weaponLevels.ToString();
            }
        }
    }
    public void AddPassiveLevel(Transform playerPassiveBag)
    {
        for (int i = 0; i < playerPassiveBag.childCount; i++)
        {
            var passives = playerPassiveBag.GetComponentsInChildren<PlayerPassive>()[i];
            var passiveLevels = passives.passiveLevel;
            if (passiveLevels > 4f)
            {
                passiveLevel[i].text = "Max";
            }
            else
            {
                passiveLevel[i].text = passiveLevels.ToString();
            }
        }
    }
}
