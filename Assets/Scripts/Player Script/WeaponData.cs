using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "CustomData/Create Weapon Data")]
public class WeaponData : ScriptableObject
{
    public Sprite weaponImage;
    public string weaponName;
    public GameObject weapon;

    [TextArea]
    public string weaponDesc;
}
