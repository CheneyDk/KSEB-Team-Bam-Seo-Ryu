using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponUroboros_R : PlayerWeapon
{
    private CancellationTokenSource cancelFire;

    private void Start()
    {
        // init stats
        weaponDamageRate = 1.2f;
        weaponFireRate = 0.55f;
        bulletNum = 1;
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
            Vector3 tmp = muzzle.position + muzzle.right * 3.3f;

            // Debug.Log(bullet.GetComponent<PlayerBullet>().bulletDamage);
            var tempBullet = Instantiate(bullet, tmp, muzzle.rotation);
            tempBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate);

            await UniTask.WaitForSeconds(weaponFireRate / player.playerAtkSpeed, cancellationToken: cancelFire.Token);
        }
    }

    public override void Upgrade()
    {

    }

    // dummy override
    protected override void Fire()
    {
        // do not put in any line
    }
}
