using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "CustomData/Create Weapon Data")]
public class WeaponData : ScriptableObject
{
    public GameObject item;
    [Header("Normal Item")]
    public Sprite itemImage;
    public string itemName;
    [TextArea]
    public string itemDesc;

    [Header("Power Weapon")]
    public string passiveForPowerWeapon;
    public Sprite powerImage;
    public string powerName;
    [TextArea]
    public string powerDesc;
}
