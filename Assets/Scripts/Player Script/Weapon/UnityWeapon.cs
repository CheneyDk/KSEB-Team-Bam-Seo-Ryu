using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnityWeapon : PlayerWeapon
{
    public float fireRate = 3f;

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
                var addBullet = bulletPool.GetBullet();
                addBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage), critOccur,
                    transform.position, Quaternion.identity, bulletPool);

            }
            else if (isPowerWeapon)
            {
                bullet.GetComponent<PlayerBullet>().ChangeSprite(powerWeaponSprite);
                yield return new WaitForSeconds(fireRate);
                for (int i = 0; i < 3; i++)
                {
                    float angle = -30f + (i * 30f);
                    var addBullet = bulletPool.GetBullet();
                    addBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage), critOccur,
                        transform.position, Quaternion.Euler(0, 0, angle), bulletPool);
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
            weaponDamageRate += 0.2f;
        }
        if (weaponLevel > 4) 
        { 
            isMaxLevel = true;
        }
    }


    public override void Fire(InputAction.CallbackContext context) { }
}
