using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponUroboros_R : PlayerWeapon
{
    private void Start()
    {
        // init stats
        weaponDamageRate = 1.2f;
        weaponFireRate = 0.55f;
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

            Vector3 tmp = muzzle.position + muzzle.right * 3.3f;

            // Debug.Log(bullet.GetComponent<PlayerBullet>().bulletDamage);
            var tempBullet = Instantiate(bullet, tmp, muzzle.rotation);
            tempBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate);
        }
    }

    // dummy override
    protected override void Fire()
    {

    }

    public override void Upgrade()
    {
        
    }
}
