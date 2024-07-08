using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpWindow : MonoBehaviour
{
    [SerializeField]
    private Player player;

    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    public void NextButton()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
