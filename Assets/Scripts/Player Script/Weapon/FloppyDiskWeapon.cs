using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FloppyDiskWeapon : PlayerWeapon
{
    public float fireRate = 2f;
    private Transform parent;

    void Start()
    {
        weaponLevel = 1;
        weaponDamageRate = 1f;
        bulletNum = 1;
        isMaxLevel = false;
        isPowerWeapon = false;
        matchPassive = "SSD";

        parent = GameObject.FindWithTag("PlayerBulletPool").transform;
        InitPool();

        Fire();
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
                var addBullet = bulletPool.GetBullet();
                addBullet.GetComponent<PlayerBullet>().ChangeSprite(normalWeaponSprite);
                addBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * weaponDamageRate * (1f + critDamage), critOccur,
                    transform.position, Quaternion.identity, bulletPool);
            }
            else if (isPowerWeapon)
            {
                bullet.GetComponent<PlayerBullet>().ChangeSprite(powerWeaponSprite);
                
                for (int i = 0; i < bulletNum; i++)
                {
                    yield return new WaitForSeconds(0.2f);
                    var addBullet = bulletPool.GetBullet();
                    addBullet.GetComponent<PlayerBullet>().ChangeSprite(powerWeaponSprite);
                    addBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * weaponDamageRate * (1f + critDamage), critOccur,
                        transform.position, Quaternion.identity, bulletPool);
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
            bulletNum += 1;
        }
        if (weaponLevel > 4)
        {
            isMaxLevel = true;
        }
    }


    public override void Fire(InputAction.CallbackContext context) { }

}
