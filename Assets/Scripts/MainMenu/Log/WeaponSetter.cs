using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSetter : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI damage;
    public Image graph;

    public void SetAll(WeaponDamagesData data, int total, string charName)
    {
        if (data.weaponName != "Basic")
        {
            icon.sprite = Resources.Load<Sprite>(data.weaponName);
        }
        else
        {
            icon.sprite = Resources.Load<Sprite>(charName);
        }

        weaponName.text = data.weaponName;
        damage.text = ((int)data.damage).ToString();

        graph.GetComponent<RectTransform>().sizeDelta = new Vector2(data.damage / total * 270, 40);
    }

    public void ResetAll()
    {
        icon.sprite = Resources.Load<Sprite>("Invisible");
        weaponName.text = "";
        damage.text = "";
        graph.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 0f);
    }
}
