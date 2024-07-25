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

    public void setAll(WeaponDamagesData data, float total)
    {
        if (data.weaponName != "Basic")
        {
            icon.sprite = Resources.Load<Sprite>(data.weaponName);

            if (!data.weaponName.Equals("CD"))
            {
                Color c = icon.color;
                c.r = 0f;
                c.g = 0f;
                c.b = 0f;
                icon.color = c;
            }
        }

        weaponName.text = data.weaponName;
        damage.text = data.damage.ToString();

        graph.GetComponent<RectTransform>().sizeDelta = new Vector2(data.damage / total * 300, 50);
    }

    public void ResetAll()
    {
        icon.sprite = Resources.Load<Sprite>("Invisible");
        Color c = icon.color;
        c.r = 255f;
        c.g = 255f;
        c.b = 255f;
        icon.color = c;
        weaponName.text = "";
        damage.text = "";
        graph.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 0f);
    }
}
