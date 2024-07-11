using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSharp : PlayerWeapon
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
    }

    
    public override void Fire(InputAction.CallbackContext context)
    {
        // when click
        if (context.started && fireRateTimer > weaponFireRate / player.playerAtkSpeed)
        {
            fireRateTimer = 0f;
            
            // Debug.Log(bullet.GetComponent<PlayerBullet>().bulletDamage);
            var tempBullet = Instantiate(bullet, muzzle.position, muzzle.rotation);
            tempBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate);
        }
    }

    // dummy override
    protected override void Fire()
    {

    }
}
