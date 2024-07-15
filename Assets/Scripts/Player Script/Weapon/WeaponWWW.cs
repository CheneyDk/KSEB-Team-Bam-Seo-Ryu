using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponWWW : PlayerWeapon
{
    private float autoTargetRange = 5f;
    private Vector2 bulletVector;

    void Start()
    {
        // Init stats
        weaponDamageRate = 0.5f;
        weaponFireRate = 5f;
        weaponLevel = 1;

        // can fire imediately
        fireRateTimer = weaponFireRate / player.playerAtkSpeed;

    }

    // Update is called once per frame
    void Update()
    {
        fireRateTimer += Time.deltaTime;
    }

    

    public override void Upgrade()
    {

    }

    protected override void Fire()
    {
        if(fireRateTimer > weaponFireRate / player.playerAtkSpeed)
        {
            // calc bullet Vector
            Vector2 targetPosition = FindNearestEnemy();
            bulletVector = (targetPosition - (Vector2)transform.position).normalized;

            // if there are no target, do not shoot
            if (bulletVector == Vector2.zero) return;

            var tempBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            tempBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate);
            tempBullet.GetComponent<BulletWWW>().SetBulletWWW(bulletVector);

        }
    }

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
