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

    // muzzle position
    public Transform muzzle;


    // weapon's bullet
    public GameObject bullet;

    // Weapon Description


    // Player class
    public Player player;

    // auto fire
    protected abstract void Fire();
    // need to click
    protected abstract void Fire(InputAction.CallbackContext context);
}
