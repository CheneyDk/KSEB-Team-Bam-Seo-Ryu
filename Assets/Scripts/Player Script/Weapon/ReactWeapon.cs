using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ReactWeapon : PlayerWeapon
{
    public float fireRate = 5f;

    private Transform parent;

    void Start()
    {
        Fire();
        weaponLevel = 1;
        weaponDamageRate = 0.5f;
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
            
            critOccur = IsCritOccur(player.playerCritPer);
            critDamage = player.playerCritDmg * critOccur;
            if (!isPowerWeapon)
            {
                yield return new WaitForSeconds(fireRate);
                var addBullet = bulletPool.GetBullet();
                addBullet.GetComponent<PlayerBullet>().ChangeSprite(normalWeaponSprite);
                addBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage), critOccur,
                    transform.position, Quaternion.identity, bulletPool);

            }
            else if (isPowerWeapon)
            {
                yield return new WaitForSeconds(fireRate/2f);
                var addBullet = bulletPool.GetBullet();
                addBullet.GetComponent<PlayerBullet>().ChangeSprite(powerWeaponSprite);
                addBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage), critOccur,
                    transform.position, Quaternion.identity.normalized, bulletPool);
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
            weaponDamageRate += 0.1f;
        }
        if (weaponLevel > 4)
        {
            isMaxLevel = true;
        }
    }


    public override void Fire(InputAction.CallbackContext context) { }
}
