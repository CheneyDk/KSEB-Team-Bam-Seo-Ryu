using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Weapon,
    Item
}

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "CustomData/Create Weapon Data")]
public class WeaponData : ScriptableObject
{
    public Sprite weaponImage;
    public string weaponName;

    [TextArea]
    public string weaponDesc;
    public WeaponType type;
}
