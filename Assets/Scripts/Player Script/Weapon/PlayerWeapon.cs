using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerWeapon : MonoBehaviour // abstract class로 전환! + 업그레이드DB화
{
    // weapon stats
    protected float weaponDamageRate;
    protected float weaponFireRate; // weapon it's own stat
    protected float fireRateTimer;
    protected int bulletNum; // Ex ) shotgun
    protected int weaponLevel = 1;

    // weapon description
    public WeaponData weaponData;

    // muzzle position
    public Transform muzzle;


    // weapon's bullet
    public GameObject bullet;
    private Transform bulletRotation; // temp

    // Weapon Description


    // Player class
    public Player player;

    // YH - flag
    // Weapon class Init func
    // called by Player - Awake(or Start)
    // or just connect on Unity.

    //public void Init(Transform muzzlePos)
    //{
    //    muzzle = muzzlePos;
    //}

    // live player Attack Speed apply func.
    // if player Attack Speed is changed, plz call this func.


    // auto fire
    protected abstract void Fire();
    // need to click
    protected abstract void Fire(InputAction.CallbackContext context);
}
