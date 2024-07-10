using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    // UI
    [Header("- UI")]
    public GameObject GameoverUI;
    public GameObject PauseUI;
    public GameObject UpgradeUI;

    [Header("- System")]
    // public GameObject WaveManager;
    private float timeScaleProduct = 1f;

    // class import
    [SerializeField]
    private Player player;
    private PauseWindow pauseWindow;

    private void Awake()
    {
        Instance = this; // singleton

        pauseWindow = PauseUI.GetComponent<PauseWindow>();

        GameoverUI.SetActive(false);
        PauseUI.SetActive(false);
        UpgradeUI.SetActive(false);
        Time.timeScale = 1f; // init
    }

    private void Update()
    {
        Time.timeScale = timeScaleProduct;
    }

    public void SetPlayerDead()
    {
        GameoverUI.SetActive(true);
        // WaveManager.SetActive(false); // YH - activate after merge
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PauseGame()
    {
        // 1. pause menu on
        PauseUI.gameObject.SetActive(true);

        // 2. TimeScale 0
        timeScaleProduct = 0f;

        // 3. acition map toggle -> before call this func
        // 4. Setting Stats texts
        InitStatsText();
    }

    public void ContinueGame()
    {
        // 1. pause menu off
        PauseUI.gameObject.SetActive(false);

        // 2. TimeScale 1
        timeScaleProduct = 1f;
    }

    public void InitStatsText()
    {
        pauseWindow.hpVal.text = player.playerCurHp + " / " + player.playerMaxHp;
        pauseWindow.atkVal.text = player.playerAtk.ToString();
        pauseWindow.atkSpeedVal.text = player.playerAtkSpeed.ToString();
        pauseWindow.critPerVal.text = player.playerCritPer.ToString();
        pauseWindow.critPerVal.text = player.playerCritPer.ToString();
        pauseWindow.critDamageVal.text = player.playerCritDmg.ToString();
        pauseWindow.moveSpeedVal.text = player.playerMoveSpeed.ToString();
    }

    public void LevelUp()
    {
        UpgradeUI.SetActive(true);
        UpgradeUI.GetComponent<UpgradeManager>().OnUpgrade(true);
        timeScaleProduct = 0f;
    }

    public void WaveEnd()
    {
        UpgradeUI.SetActive(true);
        UpgradeUI.GetComponent<UpgradeManager>().OnUpgrade(false);
        timeScaleProduct = 0f;
    }

    public void EndUpgrade()
    {
        UpgradeUI.gameObject.SetActive(false);
        player.SwitchToPlayerControl();
        timeScaleProduct = 1f;
    }
}