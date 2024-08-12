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

    // weapon's bullet
    public GameObject bullet;

    // Player class
    protected Player player;

    // Weapon Sprite
    public Sprite normalWeaponSprite;
    public Sprite powerWeaponSprite;

    // bullet Pool
    [SerializeField] protected GameObject bulletPoolObj;
    protected BulletPool bulletPool;

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

    protected int IsCritOccur(int prob)
    {
        int chance = Random.Range(0, 100);
        if (chance > prob)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }
}
