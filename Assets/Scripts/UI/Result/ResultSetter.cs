using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultSetter : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI weaponDamage;
    public Image graph;

    public void SetAll(string name, float damage)
    {
        if (name != "Basic")
        {
            icon.sprite = Resources.Load<Sprite>(name);
        }
        else
        {
            icon.sprite = Resources.Load<Sprite>(RE_SaveManager.instance.GetGameRecord().character);
        }

        weaponName.text = name;
        weaponDamage.text = ((int)damage).ToString();

        graph.GetComponent<RectTransform>().sizeDelta = new Vector2(damage / RE_SaveManager.instance.GetGameRecord().totalDamage * 160, 24);
    }
}
