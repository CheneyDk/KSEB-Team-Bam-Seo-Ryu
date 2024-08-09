using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseWindow : MonoBehaviour
{
    public GameObject volumWindow;
    private bool volumOpen = false;

    // texts
    public TextMeshProUGUI character;
    public TextMeshProUGUI hpVal;
    public TextMeshProUGUI atkVal;
    public TextMeshProUGUI atkSpeedVal;
    public TextMeshProUGUI critPerVal;
    public TextMeshProUGUI critDamageVal;
    public TextMeshProUGUI moveSpeedVal;

    private void Start()
    {
        character.text = ScoreManager.instance.GetCharacter() + " Stats";
    }

    private void OnEnable()
    {
        volumOpen = false;
        volumWindow.SetActive(false);
    }

    public void OpenVolumSetting()
    {
        volumOpen = !volumOpen;
        if (volumOpen)
        {
            volumWindow.SetActive(true);
        }
        else
        {
            volumWindow.SetActive(false);
        }
    }

}
