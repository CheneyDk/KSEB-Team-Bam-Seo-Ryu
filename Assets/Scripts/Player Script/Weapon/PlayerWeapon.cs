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

    // Weapon Sprite
    public Sprite normalWeaponSprite;
    public Sprite powerWeaponSprite;

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

    protected int IsCritOccur(float prob)
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
