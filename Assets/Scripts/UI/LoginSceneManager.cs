using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginSceneManager : MonoBehaviour
{
    public static LoginSceneManager instance;

    void Awake()
    {
        instance = this;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainMenuScene");
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
