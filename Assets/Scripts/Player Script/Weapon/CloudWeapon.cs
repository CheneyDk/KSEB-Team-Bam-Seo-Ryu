using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CloudWeapon : PlayerWeapon
{
    public float fireRate = 3.5f;

    void Start()
    {
        Fire();
        weaponLevel = 1;
        weaponDamageRate = 1f;
        isMaxLevel = false;
        isPowerWeapon = false;
        matchPassive = "BlueTooth";
    }

    IEnumerator FireBullet()
    {
        while (true)
        {
            if (!isPowerWeapon)
            {
                bullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate);
                bullet.GetComponent<CloudBullet>().GiveDamagea();
                yield return new WaitForSeconds(fireRate);
            }
            else if (isPowerWeapon)
            {
                bullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate);
                bullet.GetComponent<CloudBullet>().PowerWeapoonDamage();
                yield return new WaitForSeconds(2f);
            }
        }
    }

    protected override void Fire()
    {
        StartCoroutine(FireBullet());
    }


    public override void Upgrade()
    {
        if (weaponLevel < 5)
        {
            weaponLevel++;
            fireRate -= 0.5f;
        }
        if (weaponLevel > 4)
        {
            isMaxLevel = true;
        }
    }


    public override void Fire(InputAction.CallbackContext context) { }
}
