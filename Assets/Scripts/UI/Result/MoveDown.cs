using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Xml;
using System;

public class MoveDown : MonoBehaviour
{
    public Canvas bottom;
    public GameObject result;
    public Button button;

    public TextMeshProUGUI survived;
    public TextMeshProUGUI waveReached;
    public TextMeshProUGUI levelReached;
    public TextMeshProUGUI enemiesDefeated;

    public GameObject Right;

    private Dictionary<string, string> spriteDictionary = new Dictionary<string, string>
    {
        { "Unity", "Image/weapon/Communal/Unity/new unity (1)" },
        { "WWW Internet", "Image/weapon/Communal/WWW Effect/WWW Effect_1" },
        { "MySQL", "Image/weapon/Communal/MYSQL(ax)" },
        { "React", "Image/weapon/Communal/React" },
        { "Loading", "Image/weapon/Communal/Loading Chain" },
        { "Swift", "Image/weapon/Communal/Swift" }
    };

    private bool flag = true;

    public void ShowDetails()
    {
        result.SetActive(flag);

        if (flag) 
        {
            bottom.transform.Translate(new Vector3(0, -250, 0));
            flag = false;
            Image buttonImage = button.GetComponent<Image>();

            waveReached.text = ScoreManager.instance.GetWave().ToString();
            levelReached.text = ScoreManager.instance.GetLevel().ToString();
            enemiesDefeated.text = ScoreManager.instance.GetEnemiesDeafeated().ToString();

            Dictionary<string, float> weapons = ScoreManager.instance.GetweaponDamages();

            int cnt = 0;
            foreach (KeyValuePair<string, float> kvp in weapons)
            {
                Transform w = Right.transform.GetChild(cnt);

                if (cnt != 0)
                {
                    w.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(spriteDictionary[kvp.Key]);
                }
                w.GetChild(1).GetComponent<TextMeshProUGUI>().text = kvp.Key;
                w.GetChild(2).GetComponent<TextMeshProUGUI>().text = kvp.Value.ToString();
                w.GetChild(3).GetComponent<Image>().GetComponent<RectTransform>().sizeDelta = new Vector2(kvp.Value / ScoreManager.instance.GetTotalDamages() * 25, 6);

                cnt++;
            }

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
