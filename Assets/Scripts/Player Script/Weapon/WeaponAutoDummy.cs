using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponAutoDummy : PlayerWeapon
{
    private void Start()
    {
        // init stats
        weaponDamageRate = 1.5f;
        weaponFireRate = 0.25f;
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

    protected override void Fire()
    {
        if (fireRateTimer > weaponFireRate / player.playerAtkSpeed)
        {
            fireRateTimer = 0f;
            
            var tempBullet = Instantiate(bullet, muzzle.position, Quaternion.identity);
            tempBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate);
        }
    }

    // not player control weapon
    public override void Fire(InputAction.CallbackContext context)
    {
        
    }
}
