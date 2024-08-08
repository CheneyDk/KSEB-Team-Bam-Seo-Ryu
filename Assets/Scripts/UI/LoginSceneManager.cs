using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LoginSceneManager : MonoBehaviour
{
    public static LoginSceneManager instance;

    public TextMeshProUGUI passwordText;

    public Texture2D normalCursor;

    private AudioSource audioSource;
    public AudioClip mouseClickClip;
    public AudioClip[] keyboardClip;

    private bool introOpen = false;
    public GameObject teamIntroductionWindow;

    public GameObject logoWindow;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        introOpen = false;
        teamIntroductionWindow.SetActive(false);
        logoWindow.SetActive(false);
        Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(ShowLogo());
    }

    private IEnumerator StartLogin()
    {
        yield return new WaitForSeconds(0.5f);
        passwordText.text = "*";
        PlayRandomClip();
        yield return new WaitForSeconds(0.1f);
        passwordText.text = "**";
        PlayRandomClip();
        yield return new WaitForSeconds(0.1f);
        passwordText.text = "***";
        PlayRandomClip();
        yield return new WaitForSeconds(0.2f);
        passwordText.text = "****";
        PlayRandomClip();
        yield return new WaitForSeconds(0.1f);
        passwordText.text = "*****";
        PlayRandomClip();
        yield return new WaitForSeconds(0.1f);
        passwordText.text = "******";
        PlayRandomClip();
        yield return new WaitForSeconds(0.1f);
        passwordText.text = "*******";
        PlayRandomClip();
        SceneManager.LoadScene("MainMenuScene");
    }
    private IEnumerator ShowLogo()
    {
        logoWindow.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        logoWindow.SetActive(false);
    }


    private void PlayRandomClip()
    {
        var randomClip = Random.Range(0, keyboardClip.Length);
        audioSource.PlayOneShot(keyboardClip[randomClip]);
    }

    public void MouseClickSound(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            audioSource.PlayOneShot(mouseClickClip);
        }
    }

    public void StartGame()
    {
        StartCoroutine(StartLogin());
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OpenTeamIntroduction()
    {
        introOpen = !introOpen;
        if (introOpen)
        {
            teamIntroductionWindow.SetActive(true);
        }
        else if (!introOpen)
        {
            teamIntroductionWindow.SetActive(false);
        }
    }
}
