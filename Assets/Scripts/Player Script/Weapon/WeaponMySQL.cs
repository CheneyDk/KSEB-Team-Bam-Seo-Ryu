using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponMySQL : PlayerWeapon
{
    private float bulletFireInterval = 0.5f;
    private Quaternion rotateRight = new(0f, 180f, 0f, 1f);
    private Quaternion rotateLeft = new(0f, 0f, 0f, 1f);
    public GameObject powerBullet;

    private void Start()
    {
        // init stats
        weaponDamageRate = 1.5f;
        weaponFireRate = 3f;
        bulletNum = 1;
        weaponLevel = 1;
        isPowerWeapon = false;
        matchPassive = "Overclock";
            
        // player can fire imediately
        fireRateTimer = weaponFireRate;

        muzzle = GameObject.FindGameObjectWithTag("Player").transform;
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
        if (isPowerWeapon)
        {
            for (int j = 0; j < 2; j++)
            {
                // first shot
                for (int i = 0; i < bulletNum - 1; i++)
                {
                    var tempBulletRight = Instantiate(powerBullet, muzzle.position, rotateRight);
                    tempBulletRight.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate);
                    var tempBulletLeft = Instantiate(powerBullet, muzzle.position, rotateLeft);
                    tempBulletLeft.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate);
                }

                await UniTask.WaitForSeconds(bulletFireInterval); // 0.5f
            }
            
        }
        else
        {
            // right side
            for (int i = 0; i < bulletNum; i++)
            {
                var tempBulletRight = Instantiate(bullet, muzzle.position, rotateRight);
                tempBulletRight.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate);
            }

            await UniTask.WaitForSeconds(bulletFireInterval); // 0.5f

            // left side
            for (int i = 0; i < bulletNum; i++)
            {
                var tempBulletLeft = Instantiate(bullet, muzzle.position, rotateLeft);
                tempBulletLeft.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate);
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
