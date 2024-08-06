using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Xml;
using System;

public class ToggleDetail : MonoBehaviour
{
    public TextMeshProUGUI GameOvetText;
    
    public GameObject window;
    public GameObject result;
    public GameObject bottom;
    public Button button;

    public TextMeshProUGUI survived;
    public TextMeshProUGUI waveReached;
    public TextMeshProUGUI levelReached;
    public TextMeshProUGUI enemiesDefeated;

    public ResultSetter[] setters;

    private bool flag = true;

    private void Awake()
    {
        GameOvetText.text = ScoreManager.instance.GetCharacter() + ".exe has stopped working";
    }

    public void Toggle()
    {
        result.SetActive(flag);

        if (flag) 
        {
            window.transform.Translate(new Vector3(0, 80, 0));
            bottom.transform.Translate(new Vector3(0, -220, 0));
            flag = false;
            Image buttonImage = button.GetComponent<Image>();

            survived.text = ScoreManager.instance.GetSurvived().ToString();
            waveReached.text = ScoreManager.instance.GetWave().ToString();
            levelReached.text = ScoreManager.instance.GetLevel().ToString();
            enemiesDefeated.text = ScoreManager.instance.GetEnemiesDeafeated().ToString();

            var weapons = ScoreManager.instance.WeaponDamages;

            int cnt = 0;
            foreach (KeyValuePair<string, float> kvp in weapons)
            {
                setters[cnt++].SetAll(kvp.Key, kvp.Value);
            }

            Color color = buttonImage.color;
            color.a = 1f;
            buttonImage.color = color;
        }
        else
        {
            window.transform.Translate(new Vector3(0, -80, 0));
            bottom.transform.Translate(new Vector3(0, 220, 0));
            flag = true;
            Image buttonImage = button.GetComponent<Image>();
            Color color = buttonImage.color;
            color.a = 0f;
            buttonImage.color = color;
        }
    }
}
