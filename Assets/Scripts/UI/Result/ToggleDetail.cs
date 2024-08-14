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
    private GameRecord gameRecord;

    private void Awake()
    {
        GameOvetText.text = RE_SaveManager.instance.GetGameRecord().character + ".exe has stopped working";
        gameRecord = RE_SaveManager.instance.GetGameRecord();
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

            survived.text = gameRecord.survived.ToString();
            waveReached.text = gameRecord.waveReached.ToString();
            levelReached.text = gameRecord.levelReached.ToString();
            enemiesDefeated.text = gameRecord.totalEnemiesDeafeated.ToString();

            var weapons = RE_SaveManager.instance.readOnlyWeaponDataDict;

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
