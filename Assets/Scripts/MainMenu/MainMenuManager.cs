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
    public Slider SoundSlider;
    public GameObject Icon100;
    public GameObject Icon66;
    public GameObject Icon33;
    public GameObject Icon0;
    public GameObject IconExcept;
    [EndFoldout]

    public Texture2D normalCursor;
    public Texture2D spaghettiCursor;
    private bool isSpaghettiCursor = false;

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

    private float originSoundValue = 100;

    void Awake()
    {
        loadingWindow.SetActive(false);
        foreach (var item in loadBarBlock)
        {
            item.gameObject.SetActive(false);
        }
        Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(startMenuClip);
        instance = this;
    }

    public void ChangeMouseCursor()
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
        donateButton.onClick.AddListener(SaveManager.instance.EarnMoney);
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
    public void CloseWindow()
    {
        window.SetActive(false);
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


    public void ChangeSoundIcon(float value)
    {
        if (value >= 66)
        {
            Icon100.SetActive(true);
            Icon66.SetActive(false);
            Icon33.SetActive(false);
            Icon0.SetActive(false);
            IconExcept.SetActive(false);
        }
        else if (value >= 33)
        {
            Icon100.SetActive(false);
            Icon66.SetActive(true);
            Icon33.SetActive(false);
            Icon0.SetActive(false);
            IconExcept.SetActive(false);
        }
        else if (value > 0)
        {
            Icon100.SetActive(false);
            Icon66.SetActive(false);
            Icon33.SetActive(true);
            Icon0.SetActive(false);
            IconExcept.SetActive(false);
        }
        else if (value == 0)
        {
            Icon100.SetActive(false);
            Icon66.SetActive(false);
            Icon33.SetActive(false);
            Icon0.SetActive(true);
        }
    }

    public void Mute()
    {
        if (SoundSlider.value == 0)
        {
            if (Icon0.activeSelf)
            {
                Icon0.SetActive(false);
                IconExcept.SetActive(true);
            }
            else
            {
                Icon0.SetActive(true);
                IconExcept.SetActive(false);
            }
        }
        else
        {
            if (Icon0.activeSelf)
            {
                SoundSlider.value = originSoundValue - 1;
                SoundSlider.value++;
            }
            else
            {
                originSoundValue = SoundSlider.value;
                Icon0.SetActive(true);
                Icon100.SetActive(false);
                Icon66.SetActive(false);
                Icon33.SetActive(false);
            }
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

    public void QuitGame()
    {
        SaveManager.instance.SaveAllData();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
