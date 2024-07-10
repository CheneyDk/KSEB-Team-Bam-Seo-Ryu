using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSharp : PlayerWeapon
{
    private float sharpFireRateTimer;

    private void Start()
    {
        // init stats
        weaponDamageRate = 1.5f;
        weaponFireRate = 0.25f;
        bulletNum = 1;
        weaponLevel = 1;

        // player can fire imediately
        sharpFireRateTimer = weaponFireRate;
    }

    private void Update()
    {
        sharpFireRateTimer += Time.deltaTime;
    }

    
    protected override void Fire(InputAction.CallbackContext context)
    {
        // when click
        if (context.started && sharpFireRateTimer > weaponFireRate / player.playerAtkSpeed)
        {
            sharpFireRateTimer = 0f;
            
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
