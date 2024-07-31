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
            icon.sprite = Resources.Load<Sprite>(ScoreManager.instance.GetCharacter());
        }

        weaponName.text = name;
        weaponDamage.text = damage.ToString();

        graph.GetComponent<RectTransform>().sizeDelta = new Vector2(damage / ScoreManager.instance.GetTotalDamages() * 175, 25);
    }
}
