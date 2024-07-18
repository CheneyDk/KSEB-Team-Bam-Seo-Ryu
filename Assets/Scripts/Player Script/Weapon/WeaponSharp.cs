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

    private CancellationTokenSource cancelFire;

    private void Start()
    {
        // init stats
        weaponDamageRate = 1f;
        weaponFireRate = 1f;
        bulletNum = 3;
        weaponLevel = 1;

        // player can fire imediately
        fireRateTimer = weaponFireRate;
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
        if (context.started && fireRateTimer > weaponFireRate / player.playerAtkSpeed)
        {
            fireRateTimer = 0f;
            AutoFire().Forget();
        }
    }

    // you can fire while hold gun
    private async UniTask AutoFire()
    {
        // cancel token
        cancelFire = new CancellationTokenSource();

        while (!cancelFire.IsCancellationRequested)
        {
            SharpFire().Forget();
            await UniTask.WaitForSeconds(weaponFireRate / player.playerAtkSpeed, cancellationToken: cancelFire.Token);
        }
    }

    // instantiate bullet for 3 ~ 8 times
    private async UniTask SharpFire()
    {
        for (int i = 0; i < bulletNum; i++)
        {
            var tempBullet = Instantiate(bullet, muzzle.position, muzzle.rotation);
            tempBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate);
            await UniTask.WaitForSeconds(weaponSharpFireInterval);
        }
    }

    public override void Upgrade()
    {
        // max level?
        if (isMaxLevel) return;

        // level up 1
        weaponLevel += 1;

        // stat change - damage rate, fireRate, bulletNum, etc.
        weaponDamageRate += 0.1f;
        weaponSharpFireInterval -= 0.004f;
        bulletNum += 1;

        if (weaponLevel > 4) isMaxLevel = true;
    }

    // dummy override
    protected override void Fire()
    {
        // do not put in any line
    }
}
