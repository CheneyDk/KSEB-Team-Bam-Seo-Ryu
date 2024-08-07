using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ElixirWeapon : PlayerWeapon
{
    private float autoTargetRange;
    private bool isDestroyed;
    private Vector3 bulletVector;
    private float explodeRange;
    private float debuffAddDamageRate;
    private float debuffLastingTime;

    void Start()
    {
        weaponDamageRate = 0f;
        weaponFireRate = 3f;
        fireRateTimer = 8f;
        autoTargetRange = 15f;
        explodeRange = 5f;
        debuffAddDamageRate = 0.2f;
        debuffLastingTime = 5f;

        weaponLevel = 1;

        isDestroyed = false;
        isMaxLevel = false;
        isPowerWeapon = false;
        matchPassive = "GPU";

        AutoFire().Forget();
    }



    protected override void Fire()
    {
        var tempBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        tempBullet.GetComponent<PlayerBullet>().Init(weaponDamageRate, 0);
        tempBullet.GetComponent<ElixirBullet>().SetElixir(bulletVector, debuffAddDamageRate, explodeRange, debuffLastingTime, isPowerWeapon);
    }

    private async UniTask AutoFire()
    {
        while (true)
        {
            await UniTask.Yield();
            if (isDestroyed) return;

            var targetPos = FindNearestEnemy();
            if (targetPos == Vector2.zero) continue;

            bulletVector = (targetPos - (Vector2)transform.position);

            Fire();
            await UniTask.WaitForSeconds(weaponFireRate);
            if (isDestroyed) return;
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


    public override void Upgrade()
    {
        if (isMaxLevel) return;

        debuffAddDamageRate += 0.06f;
        explodeRange += 1f;
        weaponLevel += 1;
        debuffLastingTime += 1f;

        if (weaponLevel > 4) isMaxLevel = true;
    }


    private void OnDestroy()
    {
        isDestroyed = true;
    }

    public override void Fire(InputAction.CallbackContext context){}
}
