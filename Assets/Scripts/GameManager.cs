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
    // esc menua

    [Header("- System")]
    // public GameObject WaveManager;

    // flags
    private bool isPlayerAlive;


    private void Awake()
    {
        Instance = this; // singleton?
        isPlayerAlive = true;
        GameoverUI.SetActive(false);
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
    
}
