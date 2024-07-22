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
    
    public GameObject panel;
    public Slider SoundSlider;
    public GameObject I_100;
    public GameObject I_66;
    public GameObject I_33;
    public GameObject I_0;
    public GameObject I_Except;

    private float Origin = 100;

    void Awake()
    {
        instance = this;
    }

    public void OnlyDisable()
    {
        if (panel.activeSelf)
        {
            panel.SetActive(false);
        }
    }

    public void Toggle(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            panel.SetActive(!panel.activeSelf);
        }
    }
    public void Toggle()
    {
        panel.SetActive(!panel.activeSelf);
    }

    public void MoveScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
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
}
