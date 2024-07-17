using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GoButtonClick : MonoBehaviour
{
    public void Click()
    {
        MainMenuManager.instance.OnlyDisable("FirstPage");
    }
}
