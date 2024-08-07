using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class USBWeapon : PlayerWeapon
{
    public float fireRate = 3.5f;

    void Start()
    {
        Fire();
        weaponLevel = 1;
        weaponDamageRate = 0.5f;
        isMaxLevel = false;
        isPowerWeapon = false;
        matchPassive = "BlueTooth";
    }

    IEnumerator FireBullet()
    {
        int critOccur;
        float critDamage;
        while (true)
        {
            critOccur = IsCritOccur(player.playerCritPer);
            critDamage = player.playerCritDmg * critOccur;
            if (!isPowerWeapon)
            {
                bullet.GetComponent<PlayerBullet>().ChangeSprite(normalWeaponSprite);
                yield return new WaitForSeconds(fireRate);
                for (int i = 0; i < 10; i++)
                {
                    float angle = i * (360f / 10f);
                    var addBullet = Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, angle));
                    addBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage), critOccur);
                }
            }
            else if (isPowerWeapon)
            {
                bullet.GetComponent<PlayerBullet>().ChangeSprite(powerWeaponSprite);
                yield return new WaitForSeconds(fireRate);
                for (int i = 0; i < 10; i++)
                {
                    float angle = i * (360f / 10f);
                    var addBullet = Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, angle));
                    addBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * (1f + critDamage), critOccur);
                }
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
