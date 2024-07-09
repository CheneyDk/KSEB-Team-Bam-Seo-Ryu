using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSharp : PlayerWeapon
{
    private float sharpFireRateTimer;

    private void Start()
    {
        // player can fire imediately
        sharpFireRateTimer = weaponFireRate;

        // init stats
        weaponDamageRate = 1f;
        weaponFireRate = 0.25f;
        weaponLevel = 1;
    }
    private void Update()
    {
        sharpFireRateTimer += Time.deltaTime;
    }


    public override void Fire(InputAction.CallbackContext context)
    {
        // when click
        if (context.started && sharpFireRateTimer > weaponFireRate)
        {
            sharpFireRateTimer = 0f;
            var tempBullet = Instantiate(bullet, muzzle.position, muzzle.rotation);
            // YH - flag: need bullet Init func
        }
    }
}
