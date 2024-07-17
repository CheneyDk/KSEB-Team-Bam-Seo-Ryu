using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class MainMenuInput : MonoBehaviour
{
    public GameObject shell;

    public void toggle(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (shell != null)
            {
                shell.SetActive(!shell.activeSelf);
            }
        }
    }
}
