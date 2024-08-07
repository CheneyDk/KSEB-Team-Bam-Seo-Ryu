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

    private float bulletFallRange;
    private float bulletExplodeRange;
    private float fireRateRevisingValue; // 공속 보정값

    private Transform playerPos;
    private SpriteRenderer weaponSprite;
    private SpriteRenderer bulletSprite;
    private Color pytorchColor;

    private bool isDestroyed = false;

    // init stats
    private void Start()
    {
        weaponDamageRate = 1f;

        fireRateRevisingValue = 15f;
        // when playerAtkSpeed = 1, fireRate = 20
        // when playerAtkSpeed = 2, fireRate = 17.5
        weaponFireRate = 5f * MathF.Pow(2f, player.playerAtkSpeed - 1f) + fireRateRevisingValue;
        pytorchFireInterval = 0.25f; // 1: 0.25
        bulletNum = 20; // 1: 10

        bulletExplodeRange = 5f;
        bulletFallRange = 7f;

        playerPos = player.transform;

        weaponSprite = gameObject.GetComponent<SpriteRenderer>();
        bulletSprite = bullet.GetComponent<SpriteRenderer>();

        pytorchColor = new Color(1f, 0.2f, 0f);
        bulletSprite.color = pytorchColor;

        isPowerWeapon = false;
        matchPassive = "Overclock";

        SpriteChange().Forget();

        // async routine
        Fire();
    }

    // active func

    // inactive func

    public override void Upgrade()
    {
        if (isMaxLevel) return;

        weaponLevel += 1;
        weaponDamageRate += 0.075f;
        pytorchFireInterval -= 0.03f;
        bulletNum += 8;

        if (weaponLevel > 4) isMaxLevel = true;
    }

    protected override void Fire()
    {
        AutoFire().Forget();
    }

    private async UniTask AutoFire()
    {
        int critOccur;
        float critDamage;
        while (true)
        {
            await Active();

            // instantiate bullet
            for (int i = 0; i < bulletNum; i++)
            {
                // generate random pos
                var bulletFallPos = new Vector2 (UnityEngine.Random.Range(playerPos.position.x - bulletFallRange, playerPos.position.x + bulletFallRange),
                                    UnityEngine.Random.Range(playerPos.position.y - bulletFallRange, playerPos.position.y + bulletFallRange));

                critOccur = IsCritOccur(player.playerCritPer);
                critDamage = player.playerCritDmg * critOccur;

                var tempBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                tempBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage), critOccur);
                // set bullet drop pos
                tempBullet.GetComponent<PytorchBullet>().SetPytorchBullet(bulletFallPos, bulletExplodeRange, isPowerWeapon);

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
        float timer = 0f;
        const float duration = 1f;

        var tempColor = weaponSprite.color;

        while (timer < duration)
        {
            await UniTask.Yield();
            if (isDestroyed) return;

            tempColor.a = timer / duration;
            weaponSprite.color = tempColor;
            timer += Time.deltaTime;
        }

        tempColor.a = 1f;
        weaponSprite.color = tempColor;
    }

    private async UniTask Inactive()
    {
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

    private async UniTask SpriteChange()
    {
        await UniTask.WaitUntil(() => isPowerWeapon);
        if (isDestroyed) return;
        weaponSprite.sprite = powerWeaponSprite;
        var tempColor = weaponSprite.color;
        tempColor = new Color(1f, 1f, 1f, tempColor.a);
        weaponSprite.color = tempColor;

        bulletSprite.sprite = powerWeaponSprite;
        tempColor.a = 1f;
        bulletSprite.color = tempColor;
    }

    private void OnDestroy()
    {
        isDestroyed = true;
        bulletSprite.sprite = normalWeaponSprite;
        bulletSprite.color = pytorchColor;
    }

    // dummy
    public override void Fire(InputAction.CallbackContext context)
    {

    }
}
