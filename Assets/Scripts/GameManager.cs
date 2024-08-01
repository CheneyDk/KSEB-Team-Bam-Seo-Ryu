﻿using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public ScoreManager scmanager;

    [Header("Hide Mouse Cursor")]
    public bool hideMouse = false;

    public static GameManager Instance;
    // UI
    [Header("- UI")]
    public GameObject GameoverUI;
    public GameObject PauseUI;
    public GameObject UpgradeUI;

    [Header("- System")]
    // public GameObject WaveManager;
    private float timeScaleProduct = 1f;
    public bool isGameContinue = true;

    // class import
    [SerializeField]
    public Player player;
    private PauseWindow pauseWindow;

    private void Awake()
    {
        //////////////////////

        if (hideMouse)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        //////////////////////
        Instance = this; // singleton

        pauseWindow = PauseUI.GetComponent<PauseWindow>();

        GameoverUI.SetActive(false);
        PauseUI.SetActive(false);
        UpgradeUI.SetActive(false);
        Time.timeScale = 1f; // init
    }

    private void Start()
    {
        ScoreManager.instance.ResetData();
    }

    private void Update()
    {
        Time.timeScale = timeScaleProduct;
    }

    public void SetPlayerDead()
    {
        GameoverUI.SetActive(true);
        timeScaleProduct = 0f;
        if (isGameContinue) 
        {
            ScoreManager.instance.SaveData(false);
        }
        isGameContinue = false;
        // WaveManager.SetActive(false); // YH - activate after merge
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        isGameContinue = true;
        ScoreManager.instance.ResetData();
    }

    public void GoMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void PauseGame()
    {
        // 1. pause menu on
        PauseUI.gameObject.SetActive(true);

        // 2. TimeScale 0
        timeScaleProduct = 0f;
        isGameContinue = false;

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
        isGameContinue = true;
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
        isGameContinue = false;
    }

    public void WaveEnd()
    {
        UpgradeUI.SetActive(true);
        UpgradeUI.GetComponent<UpgradeManager>().OnUpgrade(false);
        timeScaleProduct = 0f;
        isGameContinue = false;

        ScoreManager.instance.UpdateWave();
    }

    public void EndUpgrade()
    {
        UpgradeUI.gameObject.SetActive(false);
        player.SwitchToPlayerControl();
        timeScaleProduct = 1f;
        isGameContinue = true;
    }
}