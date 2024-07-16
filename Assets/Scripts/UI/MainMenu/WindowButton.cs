using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowButton : MonoBehaviour
{
    public GameObject shell;

    public void Toggle()
    {
        if (shell != null)
        {
            shell.SetActive(!shell.activeSelf);
        }
    }
}
