using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSharp : PlayerWeapon
{
    // this does not mean fireRate.
    // if 3 rounds of bullet fire for 1 tap,
    // this stat means time interval between round and round.
    private float weaponSharpFireInterval = 0.1f;

    private bool isDestroyed;

    // Auto Fire Control
    private CancellationTokenSource cancelFire;

    private SpriteRenderer spriteRenderer;

    private int random;
    private int critOccur;
    private float critDamage;

    // int random;
    // int critOccur;
    // float critDamage;

    private void Start()
    {
        // init stats
        weaponDamageRate = 1f;
        weaponFireRate = 1f;
        bulletNum = 3;
        weaponLevel = 1;
        isDestroyed = false;

        // player can fire imediately
        fireRateTimer = weaponFireRate;

        spriteRenderer = GetComponent<SpriteRenderer>();
        // Upgrade(); // YH ; activate after merge
        PowerWeaponSpriteChange();
    }
    
    private void Update()
    {
        fireRateTimer += Time.deltaTime;
    }

    // Fire - AutoFire - SharpFire Coroutine
    public override void Fire(InputAction.CallbackContext context)
    {
        // when hold canceled
        if (context.canceled)
        {
            cancelFire?.Cancel();
            cancelFire?.Dispose(); // memory disallocate
            cancelFire = null;
        }

        // when hold click
        if (context.started)
        {
            AutoFire().Forget();
        }
    }

    // you can fire while hold gun
    private async UniTask AutoFire()
    {
        // cancel token
        cancelFire = new CancellationTokenSource();

        // wait for next fire cycle
        await UniTask.WaitUntil(() => fireRateTimer > weaponFireRate / player.playerAtkSpeed, cancellationToken: cancelFire.Token);
        fireRateTimer = 0f;

        while (!cancelFire.IsCancellationRequested)
        {
            SharpFire().Forget();
            await UniTask.WaitForSeconds(weaponFireRate / player.playerAtkSpeed, cancellationToken: cancelFire.Token);
            fireRateTimer = 0f;
        }
    }

    // instantiate bullet for 3 ~ 8 times
    private async UniTask SharpFire()
    {
        
        
        for (int i = 0; i < bulletNum; i++)
        {
            critOccur = IsCritOccur(player.playerCritPer);
            critDamage = player.playerCritDmg * critOccur;
            var tempBullet = Instantiate(bullet, muzzle.position, muzzle.rotation);
            tempBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage), critOccur);
            tempBullet.GetComponent<BulletSharp>().IsPower(isPowerWeapon);
            random = Random.Range(0, 101);
            if (isPowerWeapon && random > 50)
            {
                var randPos = muzzle.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
                tempBullet = Instantiate(bullet, randPos, muzzle.rotation);
                tempBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage), critOccur);
                tempBullet.GetComponent<BulletSharp>().IsPower(isPowerWeapon);
            }
            await UniTask.WaitForSeconds(weaponSharpFireInterval);
            if (isDestroyed) return;
        }
    }

    private void PowerWeaponSpriteChange()
    {
        if (isPowerWeapon)
        {
            spriteRenderer.sprite = powerWeaponSprite;
        }
        else
        {
            spriteRenderer.sprite = normalWeaponSprite;
        }
    }

    public override void Upgrade()
    {
        // isPowerWeapon = ScoreManager.instance.recordData.isCUpgrade;
    }

    public void isPowerWeaponTrue()
    {
        isPowerWeapon = true;
    }

    private void OnDestroy()
    {
        isDestroyed = true;
    }

    // dummy override
    protected override void Fire()
    {
        // do not put in any line
    }
}
