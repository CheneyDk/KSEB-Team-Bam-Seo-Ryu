using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    // UI
    [Header("- UI")]
    public GameObject GameoverUI;
    public GameObject PauseUI;

    [Header("- System")]
    // public GameObject WaveManager;
    private float timeScaleProduct = 1f;

    // flags
    private bool isPlayerAlive;


    private void Awake()
    {
        Instance = this; // singleton?
        isPlayerAlive = true;
        GameoverUI.SetActive(false);
        PauseUI.SetActive(false);
        // time scale = 1 init need // YH
    }

    private void Update()
    {
        Time.timeScale = timeScaleProduct;
    }

    public void SetPlayerDead()
    {
        isPlayerAlive = false;
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

        // 3. acition map toggle

    }

    public void ContinueGame()
    {
        // 1. pause menu off
        PauseUI.gameObject.SetActive(false);

        // 2. TimeScale 1
        timeScaleProduct = 0f;

        // 3. acition map toggle

    }

}
