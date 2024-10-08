using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponInternet : PlayerWeapon
{
    private float autoTargetRange = 15f;
    private Vector2 bulletVector;
    private float bulletRadius;
    public GameObject powerBullet;

    private int critOccur;
    private float critDamage;

    private Transform parent;
    [SerializeField] private GameObject powerBulletPoolObj;
    private BulletPool powerBulletPool;

    void Start()
    {
        // Init stats
        weaponDamageRate = 0.25f;
        weaponFireRate = 5f;
        weaponLevel = 1;
        bulletRadius = 15f; // max 25
        isPowerWeapon = false;
        matchPassive = "Wifi";

        // can fire imediately
        fireRateTimer = weaponFireRate;
        parent = GameObject.FindWithTag("PlayerBulletPool").transform;
        InitPool();
    }
    private void InitPool()
    {
        var tempPool = Instantiate(bulletPoolObj, Vector3.zero, Quaternion.identity);
        tempPool.transform.parent = parent;
        bulletPool = tempPool.GetComponent<BulletPool>();
        tempPool = Instantiate(powerBulletPoolObj, Vector3.zero, Quaternion.identity);
        tempPool.transform.parent = parent;
        powerBulletPool = tempPool.GetComponent<BulletPool>();
    }

    void Update()
    {
        fireRateTimer += Time.deltaTime;

        Fire();
    }


    public override void Upgrade()
    {
        if (isMaxLevel) return;

        weaponLevel++;
        weaponDamageRate += 0.05f;

        if (weaponLevel > 4) isMaxLevel = true;

    }

    protected override void Fire()
    {
        if(fireRateTimer > weaponFireRate / player.playerAtkSpeed)
        {
            // calc bullet Vector
            Vector2 targetPosition = FindNearestEnemy();

            // if there are no target, do not shoot
            if (targetPosition == Vector2.zero) return;

            bulletVector = (targetPosition - (Vector2)transform.position).normalized;

            // reset Timer
            fireRateTimer = 0f;

            critOccur = IsCritOccur(player.playerCritPer);
            critDamage = player.playerCritDmg * critOccur;

            if (isPowerWeapon)
            {
                var tempBullet = powerBulletPool.GetBullet();
                tempBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage), critOccur,
                    player.transform.position, Quaternion.identity, powerBulletPool);
                tempBullet.GetComponent<PowerInternetBulllet>().SetBulletInternet(bulletVector, bulletRadius);
            }
            else
            {
                var tempBullet = bulletPool.GetBullet();
                tempBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage), critOccur,
                    player.transform.position, Quaternion.identity, bulletPool);
                tempBullet.GetComponent<BulletInternet>().SetBulletInternet(bulletVector, bulletRadius);
            }

        }
    }

    // Find nearest enemy and pass vector value to bullet
    private Vector2 FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float minDistance = float.MaxValue;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance && distance <= autoTargetRange)
            {
                minDistance = distance;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null)
        {
            return nearestEnemy.transform.position;
        }


        return Vector2.zero;
    }

    // dummy
    public override void Fire(InputAction.CallbackContext context)
    {

    }
}
