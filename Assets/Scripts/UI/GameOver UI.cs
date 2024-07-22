using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{

    public void MainMenuButtonDown()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void RetryButtonDown()
    {
        GameManager.Instance.ResetGame(); // YH - need to change now to scene reload logic
    }

}
