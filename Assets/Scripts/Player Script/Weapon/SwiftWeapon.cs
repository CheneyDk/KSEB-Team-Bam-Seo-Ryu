using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwiftWeapon : PlayerWeapon
{
    private float fireRate = 3.5f;


    void Start()
    {
        Fire();
        weaponLevel = 1;
        weaponDamageRate = 1f;
        isMaxLevel = false;
        isPowerWeapon = false;
        matchPassive = "GPU";
    }

    IEnumerator FireBullet()
    {
        while (true)
        {
            if (!isPowerWeapon)
            {
                bullet.GetComponent<PlayerBullet>().ChangeSprite(normalWeaponSprite);
                bullet.transform.localScale = new Vector3(1f, 1f, 1f);
                yield return new WaitForSeconds(fireRate);
                var addBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                addBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate);
            }
            else if (isPowerWeapon)
            {
                bullet.GetComponent<PlayerBullet>().ChangeSprite(powerWeaponSprite);
                bullet.transform.localScale = new Vector3(2f, 2f, 1f);
                yield return new WaitForSeconds(fireRate);
                var addBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                addBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate);
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
