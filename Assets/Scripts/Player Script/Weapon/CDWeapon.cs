using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CDWeapon : PlayerWeapon
{
    public float fireRate = 3.5f;

    private Transform parent;
    [SerializeField] private GameObject subBulletPoolObj;
    private BulletPool subBulletPool;

    void Start()
    {
        Fire();
        weaponLevel = 1;
        weaponDamageRate = 1f;
        isMaxLevel = false;
        isPowerWeapon = false;
        matchPassive = "SSD";

        parent = GameObject.FindWithTag("PlayerBulletPool").transform;
        InitPool();
    }

    private void InitPool()
    {
        var tempPool = Instantiate(bulletPoolObj, Vector3.zero, Quaternion.identity);
        tempPool.transform.parent = parent;
        bulletPool = tempPool.GetComponent<BulletPool>();
        tempPool = Instantiate(subBulletPoolObj, Vector3.zero, Quaternion.identity);
        tempPool.transform.parent = parent;
        subBulletPool = tempPool.GetComponent<BulletPool>();
    }

    IEnumerator FireBullet()
    {
        int critOccur;
        float critDamage;
        while (true)
        {
            yield return new WaitForSeconds(fireRate);
            if (!isPowerWeapon)
            {   
                critOccur = IsCritOccur(player.playerCritPer);
                critDamage = player.playerCritDmg * critOccur;
                var addBullet = bulletPool.GetBullet();
                addBullet.GetComponent<PlayerBullet>().ChangeSprite(normalWeaponSprite);
                addBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage), critOccur,
                    player.transform.position, Quaternion.identity, bulletPool);
                addBullet.GetComponent<CDBullet>().PassSubPool(subBulletPool);
            }
            else if(isPowerWeapon)
            {
                
                for (int i = 0; i < 3; i++)
                {
                    critOccur = IsCritOccur(player.playerCritPer);
                    critDamage = player.playerCritDmg * critOccur;
                    float angle = i * ((360f / 4f) + 30);
                    var addBullet = bulletPool.GetBullet();
                    addBullet.GetComponent<PlayerBullet>().ChangeSprite(powerWeaponSprite);
                    addBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage), critOccur,
                        player.transform.position, Quaternion.identity, bulletPool);
                    addBullet.GetComponent<CDBullet>().PassSubPool(subBulletPool);
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
