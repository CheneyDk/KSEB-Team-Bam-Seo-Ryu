using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    // UI
    [Header("- UI")]
    public GameObject GameoverCanvas;

    // flags
    private bool isPlayerAlive = true;


    private void Awake()
    {
        Instance = this; // singleton?
        GameoverCanvas.SetActive(false);
    }

    public void SetPlayerDead()
    {
        isPlayerAlive = false;
        GameoverCanvas.SetActive(true);
    }
}
