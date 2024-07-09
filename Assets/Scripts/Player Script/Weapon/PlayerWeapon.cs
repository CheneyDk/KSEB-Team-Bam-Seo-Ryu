using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerWeapon : MonoBehaviour // abstract class�� ��ȯ! + ���׷��̵�DBȭ
{
    // weapon stats
    public float weaponDamageRate;
    public float weaponFireRate;
    protected int weaponLevel = 1;

    // muzzle position
    public Transform muzzle;


    // weapon's bullet
    public GameObject bullet;
    private Transform bulletRotation; // temp


    // YH - flag
    // Weapon class Init func
    // called by Player - Awake(or Start)
    // or just connect on Unity.

    //public void Init(Transform muzzlePos)
    //{
    //    muzzle = muzzlePos;
    //}

    public abstract void Fire(InputAction.CallbackContext context);
}
