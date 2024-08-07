using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponUroboros_R : PlayerWeapon
{
    private CancellationTokenSource cancelFire;

    private int critOccur;
    private float critDamage;

    private void Start()
    {
        // init stats
        weaponDamageRate = 1.2f;
        weaponFireRate = 1f;
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
            Vector3 tmp_sub = muzzle_sub.position - muzzle_sub.right * 3.3f;

            var tempBullet = Instantiate(bullet, tmp, muzzle.rotation);
            tempBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage), critOccur);

            var tempBullet_sub = Instantiate(bullet_sub, tmp_sub, muzzle_sub.rotation);
            tempBullet_sub.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage), critOccur);

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
