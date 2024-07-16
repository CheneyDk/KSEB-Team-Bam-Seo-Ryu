using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoButton : MonoBehaviour
{
    public GameObject shell;

    public void Deactivate()
    {
        if (shell != null)
        {
            shell.SetActive(false);
        }
    }
}
