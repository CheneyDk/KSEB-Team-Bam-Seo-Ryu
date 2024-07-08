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

    [SerializeField]
    private Player player;

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
}
