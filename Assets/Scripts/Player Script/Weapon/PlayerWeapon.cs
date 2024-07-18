using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerWeapon : MonoBehaviour // abstract class로 전환! + 업그레이드DB화
{
    // weapon stats
    [SerializeField]
    protected float weaponDamageRate;
    protected float weaponFireRate; // weapon it's own stat
    protected float fireRateTimer;
    protected int bulletNum; // Ex ) shotgun
    protected int weaponLevel = 1;
    public bool isMaxLevel = false;

    // muzzle position
    public Transform muzzle;

    // weapon's bullet
    public GameObject bullet;

    // Player class
    protected Player player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // auto fire
    protected abstract void Fire();
    // need to click
    public abstract void Fire(InputAction.CallbackContext context);

    // Upgrade func
    public abstract void Upgrade();
}
