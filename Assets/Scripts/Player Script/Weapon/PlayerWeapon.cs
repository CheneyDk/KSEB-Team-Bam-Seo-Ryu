using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerWeapon : MonoBehaviour // abstract class�� ��ȯ! + ���׷��̵�DBȭ
{
    // weapon stats
    [SerializeField]
    protected float weaponDamageRate;
    protected float weaponFireRate; // weapon it's own stat
    protected float fireRateTimer;
    protected int bulletNum; // Ex ) shotgun
    public int weaponLevel = 1;
    public bool isMaxLevel = false;
    public bool isPowerWeapon = false;
    public string matchPassive;

    // muzzle position
    public Transform muzzle;
    public Transform muzzle_sub;

    // weapon's bullet
    public GameObject bullet;
    public GameObject bullet_sub;

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
