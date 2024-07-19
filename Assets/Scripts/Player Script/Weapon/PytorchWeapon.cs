using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PytorchWeapon : PlayerWeapon
{
    public float pytorchFireInterval;
    // private float lastingDamageRate;

    private float weaponLastingDamageRate;

    private float bulletFallRange;
    private float bulletExplodeRange;
    private float fireRateRevisingValue; // 공속 보정값

    private Transform playerPos;
    private SpriteRenderer weaponSprite;

    // init stats
    private void Start()
    {
        weaponDamageRate = 1f;
        weaponLastingDamageRate = 0.2f;

        fireRateRevisingValue = 15f;
        // when playerAtkSpeed = 1, fireRate = 20
        // when playerAtkSpeed = 2, fireRate = 17.5
        weaponFireRate = 5f * MathF.Pow(2f, player.playerAtkSpeed - 1f) + fireRateRevisingValue;
        // lastingDamageRate = 0.2f;
        pytorchFireInterval = 0.25f; // 1: 0.25
        bulletNum = 20; // 1: 10

        bulletExplodeRange = 5f;
        bulletFallRange = 7f;

        playerPos = player.transform;

        weaponSprite = gameObject.GetComponent<SpriteRenderer>();

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
            await Active();

            // instantiate bullet
            for (int i = 0; i < bulletNum; i++)
            {
                // generate random pos
                var bulletFallPos = new Vector2 (UnityEngine.Random.Range(playerPos.position.x - bulletFallRange, playerPos.position.x + bulletFallRange),
                                    UnityEngine.Random.Range(playerPos.position.y - bulletFallRange, playerPos.position.y + bulletFallRange));

                var tempBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                tempBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate);
                // set bullet drop pos
                tempBullet.GetComponent<PytorchBullet>().SetPytorchBullet(bulletFallPos, bulletExplodeRange, player.playerAtk * weaponLastingDamageRate);

                await UniTask.WaitForSeconds(pytorchFireInterval);
            }


            Inactive().Forget();
            await UniTask.WaitForSeconds(weaponFireRate);
            await UniTask.WaitUntil(() => GameManager.Instance.isGameContinue);
        }

    }

    private async UniTask Active()
    {
        // pytorch active
        transform.position += transform.up;
        float timer = 0f;
        const float duration = 1f;

        var tempColor = weaponSprite.color;

        while (timer < duration)
        {
            await UniTask.Yield();

            tempColor.a = timer / duration;
            weaponSprite.color = tempColor;
            timer += Time.deltaTime;
        }

        tempColor.a = 1f;
        weaponSprite.color = tempColor;
    }

    private async UniTask Inactive()
    {
        transform.position -= transform.up;
        float timer = 0f;
        const float duration = 1f;
        var tempColor = weaponSprite.color;

        while (timer < duration)
        {
            await UniTask.Yield();

            
            tempColor.a = 1 - (timer / duration);
            weaponSprite.color = tempColor;
            timer += Time.deltaTime;
            
        }

        tempColor.a = 0f;
        weaponSprite.color = tempColor;
    }

    // dummy
    public override void Fire(InputAction.CallbackContext context)
    {

    }
}
