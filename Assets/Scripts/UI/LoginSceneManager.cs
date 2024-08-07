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

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
        instance = this;
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
}
