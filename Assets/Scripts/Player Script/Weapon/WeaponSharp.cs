using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSharp : PlayerWeapon
{
    private float sharpFireRateTimer;
    private PlayerBullet bulletComponent;

    private void Start()
    {
        // player can fire imediately
        sharpFireRateTimer = weaponFireRate;

        // init stats
        weaponDamageRate = 1f;
        weaponFireRate = 0.25f;
        bulletNum = 1;
        weaponLevel = 1;

        // bullet class call
        bulletComponent = bullet.GetComponent<PlayerBullet>();
    }
    private void Update()
    {
        sharpFireRateTimer += Time.deltaTime;
    }


    protected override void Fire(InputAction.CallbackContext context)
    {
        // when click
        if (context.started && sharpFireRateTimer > weaponFireRate)
        {
            sharpFireRateTimer = 0f;
            bulletComponent.Init(player.playerAtk * weaponDamageRate);
            var tempBullet = Instantiate(bullet, muzzle.position, muzzle.rotation);
            
        }
    }

    // dummy override
    protected override void Fire()
    {

    }
}
