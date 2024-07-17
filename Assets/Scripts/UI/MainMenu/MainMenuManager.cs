using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;
    
    public GameObject windowPanel;
    public GameObject firstPagePanel;

    void Awake()
    {
        instance = this;
    }

    public void OnlyDisable(string panel)
    {
        if (panel == "Window")
        {
            if (windowPanel.activeSelf)
            {
                windowPanel.SetActive(false);
            }
        }

        if (panel == "FirstPage")
        {
            if (firstPagePanel.activeSelf)
            {
                firstPagePanel.SetActive(false);
            }
        }
    }

    public void Toggle(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            windowPanel.SetActive(!windowPanel.activeSelf);
        }
    }
    public void Toggle()
    {
        windowPanel.SetActive(!windowPanel.activeSelf);
    }

    public void MoveScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
