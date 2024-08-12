using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponMySQL : PlayerWeapon
{
    private float bulletFireInterval = 0.5f;
    private Quaternion rotateRight;
    private Quaternion rotateLeft;
    public GameObject powerBullet;
    
    private Transform parent;
    private BulletPool powerBulletPool;
    [SerializeField] private GameObject powerBulletPoolObj;

    private void Start()
    {
        // init stats
        weaponDamageRate = 1.5f;
        weaponFireRate = 3f;
        bulletNum = 1;
        weaponLevel = 1;
        isPowerWeapon = false;
        matchPassive = "Overclock";

        rotateRight = Quaternion.Euler(0f, 180f, 0f);
        rotateLeft = Quaternion.Euler(0f, 0f, 0f);

        // player can fire imediately
        fireRateTimer = weaponFireRate;

        muzzle = GameObject.FindGameObjectWithTag("Player").transform;

        parent = GameObject.FindWithTag("PlayerBulletPool").transform;
        InitPool();
    }

    private void InitPool()
    {
        var tempPool = Instantiate(bulletPoolObj, Vector3.zero, Quaternion.identity);
        tempPool.transform.parent = parent;
        bulletPool = tempPool.GetComponent<BulletPool>();
        tempPool = Instantiate(powerBulletPoolObj, Vector3.zero, Quaternion.identity);
        tempPool.transform.parent = parent;
        powerBulletPool = tempPool.GetComponent<BulletPool>();
    }

    private void Update()
    {
        fireRateTimer += Time.deltaTime;

        // Auto Fire
        Fire();
    }

    // Auto Fire
    protected override void Fire()
    {
        if (fireRateTimer > weaponFireRate / player.playerAtkSpeed)
        {
            fireRateTimer = 0f;
            ThrowSQL().Forget();
        }
    }

    private async UniTask ThrowSQL()
    {
        int critOccur;
        float critDamage;
        if (isPowerWeapon)
        {
            for (int j = 0; j < 2; j++)
            {
                // first shot
                for (int i = 0; i < bulletNum - 1; i++)
                {
                    critOccur = IsCritOccur(player.playerCritPer);
                    critDamage = player.playerCritDmg * critOccur;
                    var tempBulletRight = powerBulletPool.GetBullet();
                    tempBulletRight.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage), critOccur,
                        player.transform.position, rotateRight, powerBulletPool);
                    var tempBulletLeft = powerBulletPool.GetBullet();
                    tempBulletLeft.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage), critOccur,
                        player.transform.position, rotateLeft, powerBulletPool);
                }

                await UniTask.WaitForSeconds(bulletFireInterval); // 0.5f
            }
            
        }
        else
        {
            // right side
            for (int i = 0; i < bulletNum; i++)
            {
                critOccur = IsCritOccur(player.playerCritPer);
                critDamage = player.playerCritDmg * critOccur;
                var tempBulletRight = bulletPool.GetBullet();
                tempBulletRight.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage), critOccur,
                    player.transform.position, rotateRight, bulletPool);
            }

            await UniTask.WaitForSeconds(bulletFireInterval); // 0.5f

            // left side
            for (int i = 0; i < bulletNum; i++)
            {
                critOccur = IsCritOccur(player.playerCritPer);
                critDamage = player.playerCritDmg * critOccur;
                var tempBulletLeft = bulletPool.GetBullet();
                tempBulletLeft.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage), critOccur,
                    player.transform.position, rotateLeft, bulletPool);
            }

        }

    }

    public override void Upgrade()
    {
        if (isMaxLevel) return;

        weaponLevel += 1;
        weaponDamageRate += 0.1f;

        // when weapon Level 3, 5 => bulletNum++
        if (weaponLevel % 2 == 1) bulletNum++;

        if (weaponLevel > 4) isMaxLevel = true;
    }

    // not player control weapon. So, not gonna use this Func.
    public override void Fire(InputAction.CallbackContext context)
    {
        
    }
}
