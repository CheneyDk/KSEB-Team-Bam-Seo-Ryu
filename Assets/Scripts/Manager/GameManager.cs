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
    public bool isGameContinue = true;

    // class import
    [SerializeField]
    public Player player;
    public GameObject pet;
    private PauseWindow pauseWindow;

    [Header("- Mouse Cursor")]
    public Texture2D aimCursor;
    public Texture2D normalCursor;

    private BGM BGMAudio;

    private void Awake()
    {
        BGMAudio = FindObjectOfType<BGM>();

        ChangeMouseCursor(aimCursor);

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

        if (ScoreManager.instance.recordData.isPet)
        {
            pet.SetActive(true);
        }
    }

    private void Update()
    {
        Time.timeScale = timeScaleProduct;

    }

    public void SetPlayerDead()
    {
        BGMAudio.GetComponent<AudioSource>().Stop();
        GameoverUI.gameObject.SetActive(true);
        ChangeMouseCursor(normalCursor);
        timeScaleProduct = 0f;
        if (isGameContinue) 
        {
            ScoreManager.instance.SaveData(false);
        }
        isGameContinue = false;
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

        ChangeMouseCursor(normalCursor);

        // 2. TimeScale 0
        timeScaleProduct = 0f;
        isGameContinue = false;

        BGMAudio.GetComponent<AudioSource>().volume = 0.15f;

        // 3. acition map toggle -> before call this func
        // 4. Setting Stats texts
        InitStatsText();
    }

    public void ContinueGame()
    {
        // 1. pause menu off
        PauseUI.gameObject.SetActive(false);

        ChangeMouseCursor(aimCursor);

        BGMAudio.GetComponent<AudioSource>().volume = 0.3f;

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
        ChangeMouseCursor(normalCursor);
        UpgradeUI.GetComponent<UpgradeManager>().OnUpgrade(true);
        BGMAudio.GetComponent<AudioSource>().volume = 0.15f;
        timeScaleProduct = 0f;
        isGameContinue = false;
    }

    public void WaveEnd()
    {
        UpgradeUI.SetActive(true);
        ChangeMouseCursor(normalCursor);
        UpgradeUI.GetComponent<UpgradeManager>().OnUpgrade(false);
        BGMAudio.GetComponent<AudioSource>().volume = 0.15f;
        timeScaleProduct = 0f;
        isGameContinue = false;

        ScoreManager.instance.UpdateWave();
    }

    public void EndUpgrade()
    {
        UpgradeUI.gameObject.SetActive(false);
        ChangeMouseCursor(aimCursor);
        player.SwitchToPlayerControl();
        BGMAudio.GetComponent<AudioSource>().volume = 0.3f;
        timeScaleProduct = 1f;
        isGameContinue = true;
    }

    private void ChangeMouseCursor(Texture2D cursor)
    {
        if(cursor == aimCursor)
        {
            Cursor.SetCursor(cursor, new Vector2(80,80), CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
        }
    }
}