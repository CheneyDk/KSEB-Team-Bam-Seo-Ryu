using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginSceneManager : MonoBehaviour
{
    public static LoginSceneManager instance;

    public TextMeshProUGUI passwordText;

    void Awake()
    {
        instance = this;
    }

    private IEnumerator StartLogin()
    {
        yield return new WaitForSeconds(0.5f);
        passwordText.text = "*";
        yield return new WaitForSeconds(0.1f);
        passwordText.text = "**";
        yield return new WaitForSeconds(0.1f);
        passwordText.text = "***";
        yield return new WaitForSeconds(0.2f);
        passwordText.text = "****";
        yield return new WaitForSeconds(0.1f);
        passwordText.text = "*****";
        yield return new WaitForSeconds(0.1f);
        passwordText.text = "******";
        yield return new WaitForSeconds(0.1f);
        passwordText.text = "*******";
        SceneManager.LoadScene("MainMenuScene");
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
