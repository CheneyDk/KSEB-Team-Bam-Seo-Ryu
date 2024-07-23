using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CDWeapon : PlayerWeapon
{
    public float fireRate = 3.5f;

    void Start()
    {
        Fire();
        weaponLevel = 1;
        weaponDamageRate = 1f;
        isMaxLevel = false;
        isPowerWeapon = false;
    }

    IEnumerator FireBullet()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);
            var addBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            addBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate);
        }
    }

    IEnumerator PowerFire()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);
            for (int i = 0; i < 5; i++)
            {
                float angle = i * (360f / 5f);
                Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, angle));
            }
        }
    }

    protected override void Fire()
    {
        if (!isPowerWeapon) 
        {
            StartCoroutine(FireBullet());
        }
        else if(isPowerWeapon)
        {
            StartCoroutine(PowerFire());
        }
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
