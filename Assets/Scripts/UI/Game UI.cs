using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI waveNumber;
    [SerializeField]
    private TextMeshProUGUI waveTimer;
    [SerializeField]
    private Slider healBar;

    [SerializeField]
    private WaveManager waveManager;

    [SerializeField]
    private Player player;

    private void Awake()
    {
        waveNumber.text = "WAVE : " + waveManager.curWave.ToString();
        waveTimer.text = waveManager.time.ToString("N0");
        healBar.value = healBar.maxValue;
    }

    private void Update()
    {
        waveNumber.text = "WAVE : " + waveManager.curWave.ToString();
        waveTimer.text = waveManager.time.ToString("N0");
        healBar.value = healBar.maxValue * (player.playerCurHp / player.playerMaxHp);
    }
}
