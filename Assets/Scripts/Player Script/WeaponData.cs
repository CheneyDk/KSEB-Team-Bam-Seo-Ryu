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
    public Sprite powerImage;
    public string powerName;
    [TextArea]
    public string powerDesc;

    [HideInInspector]
    public Sprite curImage;
    [HideInInspector]
    public string curName;
    [HideInInspector]
    public string curDesc;


    private void OnEnable()
    {
        curImage = itemImage;
        curName = itemName;
        curDesc = itemDesc;
    }
}

