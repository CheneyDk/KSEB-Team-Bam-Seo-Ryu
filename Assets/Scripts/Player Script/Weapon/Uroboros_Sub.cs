using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class Uroboros_Sub : PlayerWeapon
{
    private CancellationTokenSource cancelFire;

    private int critOccur;
    private float critDamage;

    public GameObject bulletFront;
    public GameObject bulletBack;
    public GameObject bulletPowerFront;
    public GameObject bulletPowerBack;

    public Transform muzzleTopFront;
    public Transform muzzleTopBack;
    public Transform muzzleMidFront;
    public Transform muzzleMidBack;
    public Transform muzzleBotFront;
    public Transform muzzleBotBack;

    private void Start()
    {
        // init stats
        weaponDamageRate = 1f;
        weaponFireRate = 1f;
        bulletNum = 1;
        weaponLevel = 1;

        // player can fire imediately
        fireRateTimer = weaponFireRate;

        isPowerWeapon = ScoreManager.instance.recordData.isPythonUpgrade;
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
            if (isPowerWeapon)
            {
                Vector3 midFront = muzzleMidFront.position + muzzleMidFront.right * 3f - muzzleMidFront.up;
                Vector3 midBack = muzzleMidBack.position - muzzleMidBack.right * 3f + muzzleMidBack.up; ;

                var bulletMidFront = Instantiate(bulletPowerFront, midFront, muzzleMidFront.rotation);
                bulletMidFront.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage) * 0.5f, critOccur);

                var bulletMidBack = Instantiate(bulletPowerBack, midBack, muzzleMidBack.rotation);
                bulletMidBack.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage) * 0.5f, critOccur);

                Vector3 topFront = muzzleTopFront.position + muzzleTopFront.right * 3f - muzzleTopFront.up * 2;
                Vector3 topBack = muzzleTopBack.position - muzzleTopBack.right * 3f + muzzleTopBack.up * 2; ;

                var topFrontBullet = Instantiate(bulletPowerFront, topFront, muzzleTopFront.rotation);
                topFrontBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage) * 0.5f, critOccur);

                var topBackBullet = Instantiate(bulletPowerBack, topBack, muzzleTopBack.rotation);
                topBackBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage) * 0.5f, critOccur);

                Vector3 botFront = muzzleBotFront.position + muzzleBotFront.right * 2.5f;
                Vector3 botBack = muzzleBotBack.position - muzzleBotBack.right * 2.5f;

                var botFrontBullet = Instantiate(bulletPowerFront, botFront, muzzleBotFront.rotation);
                botFrontBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage) * 0.5f, critOccur);

                var botBackBullet = Instantiate(bulletPowerBack, botBack, muzzleBotBack.rotation);
                botBackBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage) * 0.5f, critOccur);
            }

            else
            {
                Vector3 midFront = muzzleMidFront.position + muzzleMidFront.right * 3.3f;
                Vector3 midBack = muzzleMidBack.position - muzzleMidBack.right * 3.3f;

                var bulletMidFront = Instantiate(bulletFront, midFront, muzzleMidFront.rotation);
                bulletMidFront.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage), critOccur);

                var bulletMidBack = Instantiate(bulletBack, midBack, muzzleMidBack.rotation);
                bulletMidBack.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage), critOccur);
            }

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
