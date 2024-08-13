using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwiftWeapon : PlayerWeapon
{
    private float fireRate = 3.5f;

    private Transform parent;

    void Start()
    {
        Fire();
        weaponLevel = 1;
        weaponDamageRate = 1f;
        isMaxLevel = false;
        isPowerWeapon = false;
        matchPassive = "CPU";

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
            yield return new WaitForSeconds(fireRate);
            critOccur = IsCritOccur(player.playerCritPer);
            critDamage = player.playerCritDmg * critOccur;
            if (!isPowerWeapon)
            {
                bullet.transform.localScale = new Vector3(1f, 1f, 1f);
                var addBullet = bulletPool.GetBullet();
                addBullet.GetComponent<PlayerBullet>().ChangeSprite(normalWeaponSprite);
                addBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage), critOccur,
                    transform.position, Quaternion.identity, bulletPool);
            }
            else if (isPowerWeapon)
            {
                bullet.transform.localScale = new Vector3(2f, 2f, 1f);
                var addBullet = bulletPool.GetBullet();
                addBullet.GetComponent<PlayerBullet>().ChangeSprite(powerWeaponSprite);
                addBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage), critOccur,
                    transform.position, Quaternion.identity, bulletPool);
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
