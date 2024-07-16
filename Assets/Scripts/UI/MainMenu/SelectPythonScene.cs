using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectPythonScene : MonoBehaviour
{
    public void Go()
    {
        SceneManager.LoadScene("UDD_Scene");
    }
}
