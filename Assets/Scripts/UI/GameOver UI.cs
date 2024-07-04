using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{

    public void MainMenuButtonDown()
    {
        // move scene to main menu
    }

    public void RetryButtonDown()
    {
        GameManager.Instance.ResetGame(); // YH - need to change now to scene reload logic
    }

}
