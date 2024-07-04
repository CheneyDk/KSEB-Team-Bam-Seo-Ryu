using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseWindow : MonoBehaviour
{
    

    private void Awake()
    {
        
    }

    // open pause window
    public void OnEscEnter(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            

        }
    }

    // close pause window
    public void OnEscExit(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {

        }
    }
}
