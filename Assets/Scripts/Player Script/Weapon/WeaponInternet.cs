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

    void Start()
    {
        // Init stats
        weaponDamageRate = 0.5f;
        weaponFireRate = 5f;
        weaponLevel = 1;
        bulletRadius = 15f; // max 25
        isPowerWeapon = false;
        matchPassive = "Wifi";

        // can fire imediately
        fireRateTimer = weaponFireRate;
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
        bulletRadius += 1f;

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

            if (isPowerWeapon)
            {
                var tempBullet = Instantiate(powerBullet, transform.position, Quaternion.identity);
                tempBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate);
                tempBullet.GetComponent<PowerInternetBulllet>().SetBulletInternet(bulletVector, bulletRadius);
            }
            else
            {
                var tempBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                tempBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate);
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
