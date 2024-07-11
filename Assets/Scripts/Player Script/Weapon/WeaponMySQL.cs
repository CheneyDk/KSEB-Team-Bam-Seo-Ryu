using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponMySQL : PlayerWeapon
{
    private void Start()
    {
        // init stats
        weaponDamageRate = 3.5f;
        weaponFireRate = 0.1f;
        bulletNum = 1;
        weaponLevel = 1;

        // player can fire imediately
        fireRateTimer = weaponFireRate;
    }

    private void Update()
    {
        fireRateTimer += Time.deltaTime;

        // Auto Fire
        Fire();
    }

    // Auto Fire
    protected override void Fire()
    {
        if (fireRateTimer > weaponFireRate / player.playerAtkSpeed)
        {
            fireRateTimer = 0f;

            // left side
            var tempBulletLeft = Instantiate(bullet, muzzle.position, Quaternion.identity);
            
            // right side
            var tempBulletRight = Instantiate(bullet, muzzle.position, Quaternion.identity);
            tempBulletLeft.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate);
            tempBulletLeft.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate);
        }
    }

    // not player control weapon. So, not gonna use this Func.
    protected override void Fire(InputAction.CallbackContext context)
    {
        
    }
}
