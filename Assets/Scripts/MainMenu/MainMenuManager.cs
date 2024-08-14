using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VInspector;
using static UnityEngine.UI.Image;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    [Foldout("Icon")]
    public GameObject window;
    public GameObject log;
    public GameObject detail;
    public GameObject record;
    public GameObject shop;
    public Slider SoundSlider;
    public GameObject I_100;
    public GameObject I_66;
    public GameObject I_33;
    public GameObject I_0;
    public GameObject I_Except;
    [EndFoldout]

    public Texture2D normalCursor;
    public Texture2D spaghettiCursor;
    private bool isSpaghettiCursor = false;

    public LogManager logManager;

    private float Origin = 100;

    private AudioSource audioSource;
    public AudioClip startMenuClip;
    public AudioClip mouseClickClip;
    public AudioClip spaghettiMouseClickClip;
    public AudioClip cursorChangeClip;

    public GameObject loadingWindow;
    public GameObject[] loadBarBlock;

    public Button donateButton;

    public GameObject teamCreditWindow;
    private bool openCredit = false;

    void Awake()
    {
        loadingWindow.SetActive(false);
        foreach(var item in loadBarBlock)
        {
            item.gameObject.SetActive(false);
        }
        Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(startMenuClip);
        instance = this;
    }

    public void ChangeCursore()
    {
        isSpaghettiCursor = !isSpaghettiCursor;
        if (isSpaghettiCursor)
        {
            audioSource.PlayOneShot(cursorChangeClip);
            Cursor.SetCursor(spaghettiCursor, new Vector2(-50, 70), CursorMode.Auto);
        }
        else
        {
            audioSource.PlayOneShot(cursorChangeClip);
            Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    private void Start()
    {
        donateButton.onClick.AddListener(ScoreManager.instance.GetMoney);
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
        StartCoroutine(LoadYourAsyncScene(sceneName));
    }

    IEnumerator LoadYourAsyncScene(string sceneName)
    {
        loadingWindow.SetActive(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        float loadPerBar = 1f / loadBarBlock.Length;

        while (!asyncLoad.isDone)
        {
            float progress = asyncLoad.progress / 0.9f;

            int activeBars = Mathf.CeilToInt(progress / loadPerBar);

            for (int i = 0; i < loadBarBlock.Length; i++)
            {
                loadBarBlock[i].SetActive(i < activeBars);
            }

            if (progress >= 1f)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
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
        SaveLoadHelper.Save(ScoreManager.instance.recordData);

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
        else if (panel == "Shop")
        {
            shop.SetActive(true);
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
        else if (panel == "Shop")
        {
            shop.SetActive(false);
        }
    }

    public void MouseClickSound(InputAction.CallbackContext context)
    {
        if (context.started && !isSpaghettiCursor)
        {
            audioSource.PlayOneShot(mouseClickClip);
        }
        else if (context.started && isSpaghettiCursor)
        {
            audioSource.PlayOneShot(spaghettiMouseClickClip);
        }
    }

    public void OpenCredit()
    {
        openCredit = !openCredit;
        if (openCredit)
        {
            teamCreditWindow.SetActive(true);
        }
        else
        {
            teamCreditWindow.SetActive(false);
        }
    }
}
