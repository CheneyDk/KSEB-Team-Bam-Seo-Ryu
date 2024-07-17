using UnityEngine;
using UnityEngine.UI;

public class SoundIcon : MonoBehaviour
{
    public Slider SoundSlider; // 슬라이더를 참조할 변수
    public GameObject I_100;
    public GameObject I_66;
    public GameObject I_33;
    public GameObject I_0;

    void Start()
    {
        if (SoundSlider != null)
        {
            SoundSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    // 슬라이더 값이 변경될 때 호출되는 메서드
    void OnSliderValueChanged(float value)
    {
        if (value >= 66)
        {
            I_100.SetActive(true);
            I_66.SetActive(false);
            I_33.SetActive(false);
            I_0.SetActive(false);
        }
        else if (value >= 33)
        {
            I_100.SetActive(false);
            I_66.SetActive(true);
            I_33.SetActive(false);
            I_0.SetActive(false);
        }
        else if (value > 0)
        {
            I_100.SetActive(false);
            I_66.SetActive(false);
            I_33.SetActive(true);
            I_0.SetActive(false);
        }
        else if (value == 0)
        {
            I_100.SetActive(false);
            I_66.SetActive(false);
            I_33.SetActive(false);
            I_0.SetActive(true);
        }
    }
}
