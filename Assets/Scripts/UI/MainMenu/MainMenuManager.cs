using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;
    
    public GameObject window;
    public GameObject log;
    public GameObject detail;
    public GameObject record;
    public Slider SoundSlider;
    public GameObject I_100;
    public GameObject I_66;
    public GameObject I_33;
    public GameObject I_0;
    public GameObject I_Except;

    public LogManager logManager;

    private float Origin = 100;

    void Awake()
    {
        instance = this;
    }

    public void OnlyDisable()
    {
        if (window.activeSelf)
        {
            window.SetActive(false);
        }
    }

    public void Toggle(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            window.SetActive(!window.activeSelf);
        }
    }
    public void Toggle()
    {
        window.SetActive(!window.activeSelf);
    }

    public void MoveScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        //ScoreManager.instance.SetCharacter(sceneName);
    }

    public void ChangeIcon(float value)
    {
        if (value >= 66)
        {
            I_100.SetActive(true);
            I_66.SetActive(false);
            I_33.SetActive(false);
            I_0.SetActive(false);
            I_Except.SetActive(false);
        }
        else if (value >= 33)
        {
            I_100.SetActive(false);
            I_66.SetActive(true);
            I_33.SetActive(false);
            I_0.SetActive(false);
            I_Except.SetActive(false);
        }
        else if (value > 0)
        {
            I_100.SetActive(false);
            I_66.SetActive(false);
            I_33.SetActive(true);
            I_0.SetActive(false);
            I_Except.SetActive(false);
        }
        else if (value == 0)
        {
            I_100.SetActive(false);
            I_66.SetActive(false);
            I_33.SetActive(false);
            I_0.SetActive(true);
        }
    }

    public void Mute()
    {
        if (SoundSlider.value == 0)
        {
            if (I_0.activeSelf)
            {
                I_0.SetActive(false);
                I_Except.SetActive(true);
            }
            else
            {
                I_0.SetActive(true);
                I_Except.SetActive(false);
            }
        }
        else
        {
            if (I_0.activeSelf)
            {
                SoundSlider.value = Origin - 1;
                SoundSlider.value++;
            }
            else
            {
                Origin = SoundSlider.value;
                I_0.SetActive(true);
                I_100.SetActive(false);
                I_66.SetActive(false);
                I_33.SetActive(false);
            }
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OpenPanel(string panel)
    {
        if (panel == "Log")
        {
            log.SetActive(true);
            logManager.SetStart();
        }
        else if (panel == "Detail")
        {
            detail.SetActive(true);
        }
        else if (panel == "Record")
        {
            record.SetActive(true);
            window.SetActive(false);
            RecordManager.instance.Calc();
        }
    }

    public void ClosePanel(string panel)
    {
        if (panel == "Log")
        {
            log.SetActive(false);
        }
        else if (panel == "Detail")
        {
            detail.SetActive(false);
        }
        else if (panel == "Record")
        {
            record.SetActive(false);
        }
    }
}
