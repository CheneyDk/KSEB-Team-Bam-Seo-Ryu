using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnityWeapon : PlayerWeapon
{
    public float fireRate = 3f;

    private Transform parent;

    void Start()
    {
        Fire();
        weaponLevel = 1;
        weaponDamageRate = 1f;
        isMaxLevel = false;
        isPowerWeapon = false;
        matchPassive = "GPU";

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
                
                var addBullet = bulletPool.GetBullet();
                addBullet.GetComponent<PlayerBullet>().ChangeSprite(normalWeaponSprite);
                addBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage), critOccur,
                    transform.position, Quaternion.identity, bulletPool);

            }
            else if (isPowerWeapon)
            {
                for (int i = 0; i < 3; i++)
                {
                    float angle = -30f + (i * 30f);
                    var addBullet = bulletPool.GetBullet();
                    addBullet.GetComponent<PlayerBullet>().ChangeSprite(powerWeaponSprite);
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
