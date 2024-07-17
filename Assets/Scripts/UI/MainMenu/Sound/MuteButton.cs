using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    public Slider SoundSlider;

    float Origin = 100;

    public void Mute()
    {
        if (SoundSlider.value == 0)
        {
            SoundSlider.value = Origin;
        }
        else
        {
            Origin = SoundSlider.value;
            SoundSlider.value = 0;
        }
    }
}
