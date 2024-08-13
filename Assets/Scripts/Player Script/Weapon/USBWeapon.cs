using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class USBWeapon : PlayerWeapon
{
    public float fireRate = 3.5f;

    private Transform parent;

    void Start()
    {
        Fire();
        weaponLevel = 1;
        weaponDamageRate = 0.5f;
        isMaxLevel = false;
        isPowerWeapon = false;
        matchPassive = "BlueTooth";

        parent = GameObject.FindWithTag("PlayerBulletPool").transform;
        InitPool();
    }

    private void InitPool()
    {
        var tempPool = Instantiate(bulletPoolObj, Vector3.zero, Quaternion.identity);
        tempPool.transform.parent = parent;
        bulletPool = tempPool.GetComponent<BulletPool>();
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
                yield return new WaitForSeconds(fireRate);
                for (int i = 0; i < 10; i++)
                {
                    float angle = i * (360f / 10f);
                    var addBullet = bulletPool.GetBullet();
                    addBullet.GetComponent<PlayerBullet>().ChangeSprite(normalWeaponSprite);
                    addBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage), critOccur,
                        transform.position, Quaternion.Euler(0, 0, angle), bulletPool);
                }
            }
            else if (isPowerWeapon)
            {
                yield return new WaitForSeconds(fireRate);
                for (int i = 0; i < 10; i++)
                {
                    float angle = i * (360f / 10f);
                    var addBullet = bulletPool.GetBullet();
                    addBullet.GetComponent<PlayerBullet>().ChangeSprite(powerWeaponSprite);
                    addBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * (1f + critDamage), critOccur,
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
            fireRate -= 0.5f;
        }
        if (weaponLevel > 4)
        {
            isMaxLevel = true;
        }
    }


    public override void Fire(InputAction.CallbackContext context) { }
}
