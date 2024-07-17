using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    public Slider SoundSlider;
    public GameObject I_100;
    public GameObject I_66;
    public GameObject I_33;
    public GameObject I_0;
    public GameObject I_Except;

    float Origin = 100;

    public void Mute()
    {
        if (SoundSlider.value == 0)
        {
            if (I_0.activeSelf)
            {
                I_0.SetActive(false);
                I_Except.SetActive(true);
            }
            else
            {
                I_0.SetActive(true);
                I_Except.SetActive(false);
            }
        }
        else
        {
            if (I_0.activeSelf)
            {
                SoundSlider.value = Origin - 1;
                SoundSlider.value++;
            }
            else
            {
                Origin = SoundSlider.value;
                I_0.SetActive(true);
                I_100.SetActive(false);
                I_66.SetActive(false);
                I_33.SetActive(false);
            }
        }
    }
}
