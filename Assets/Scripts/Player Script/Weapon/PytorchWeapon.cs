using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PytorchWeapon : PlayerWeapon
{
    public float pytorchFireInterval;
    // private float lastingDamageRate;

    private float bulletFallRange;
    private float bulletExplodeRange;
    private float fireRateRevisingValue; // 공속 보정값

    private Transform playerPos;

    // init stats
    private void Start()
    {
        weaponDamageRate = 1f;
        fireRateRevisingValue = 15f;
        // when playerAtkSpeed = 1, fireRate = 20
        // when playerAtkSpeed = 2, fireRate = 17.5
        weaponFireRate = 5f * MathF.Pow(2f, player.playerAtkSpeed - 1f) + fireRateRevisingValue;
        // lastingDamageRate = 0.2f;
        pytorchFireInterval = 0.25f;
        bulletNum = 10;

        bulletExplodeRange = 3f;
        bulletFallRange = 6f;

        playerPos = player.transform;

        // async routine
        Fire();
    }

    // active func

    // inactive func

    public override void Upgrade()
    {
        if (isMaxLevel) return;

        weaponLevel += 1;
        weaponDamageRate += 0.1f;
        pytorchFireInterval -= 0.03f;
        bulletNum += 10;

        if (weaponLevel > 4) isMaxLevel = true;
    }

    protected override void Fire()
    {
        AutoFire().Forget();
    }

    private async UniTask AutoFire()
    {
        while (true)
        {
            // instantiate bullet
            for (int i = 0; i < bulletNum; i++)
            {
                // generate random pos
                var bulletFallPos = new Vector2 (UnityEngine.Random.Range(playerPos.position.x - bulletFallRange, playerPos.position.x + bulletFallRange),
                                    UnityEngine.Random.Range(playerPos.position.y - bulletFallRange, playerPos.position.y + bulletFallRange));

                var tempBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                tempBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate);
                // set bullet drop pos
                tempBullet.GetComponent<PytorchBullet>().SetPytorchBullet(bulletFallPos, bulletExplodeRange);

                await UniTask.WaitForSeconds(pytorchFireInterval);
            }

            await UniTask.WaitForSeconds(weaponFireRate);
            await UniTask.WaitUntil(() => GameManager.Instance.isGameContinue);
        }

    }

    private async UniTask Active()
    {

    }

    private async UniTask Inactive()
    {

    }

    // dummy
    public override void Fire(InputAction.CallbackContext context)
    {

    }
}
