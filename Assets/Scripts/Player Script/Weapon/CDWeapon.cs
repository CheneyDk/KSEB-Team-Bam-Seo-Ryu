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
        matchPassive = "SSD";
    }

    IEnumerator FireBullet()
    {
        while (true)
        {
            int critOccur;
            float critDamage;
            if (!isPowerWeapon)
            {   critOccur = IsCritOccur(player.playerCritPer);
                critDamage = player.playerCritDmg * critOccur;
                bullet.GetComponent<PlayerBullet>().ChangeSprite(normalWeaponSprite);
                yield return new WaitForSeconds(fireRate);
                var addBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                addBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage), critOccur);
            }
            else if(isPowerWeapon)
            {
                critOccur = IsCritOccur(player.playerCritPer);
                critDamage = player.playerCritDmg * critOccur;
                bullet.GetComponent<PlayerBullet>().ChangeSprite(powerWeaponSprite);
                yield return new WaitForSeconds(fireRate);
                for (int i = 0; i < 3; i++)
                {
                    float angle = i * ((360f / 4f) + 30);
                    var addBullet = Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, angle));
                    addBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage), critOccur);
                }
            }
        }
    }

    private void Update()
    {
        if (isPowerWeapon)
        {
            bullet.GetComponent<PlayerBullet>().ChangeSprite(powerWeaponSprite);
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
