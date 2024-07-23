using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveDown : MonoBehaviour
{
    public Canvas bottom;
    public GameObject result;
    public Button button;

    private bool flag = true;

    public void ShowDetails()
    {
        result.SetActive(flag);

        if (flag) 
        {
            bottom.transform.Translate(new Vector3(0, -250, 0));
            flag = false;
            Image buttonImage = button.GetComponent<Image>();
            Color color = buttonImage.color;
            color.a = 1f;
            buttonImage.color = color;
        }
        else
        {
            bottom.transform.Translate(new Vector3(0, 250, 0));
            flag = true;
            Image buttonImage = button.GetComponent<Image>();
            Color color = buttonImage.color;
            color.a = 0f;
            buttonImage.color = color;
        }
    }
}
