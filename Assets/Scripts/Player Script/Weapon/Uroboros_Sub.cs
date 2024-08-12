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

    private Transform parent;
    [SerializeField] private GameObject subBulletPoolObj;
    private BulletPool subBulletPool;

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

        parent = GameObject.FindWithTag("PlayerBulletPool").transform;
        InitPool();
    }

    private void InitPool()
    {
        var tempPool = Instantiate(bulletPoolObj, Vector3.zero, Quaternion.identity);
        tempPool.transform.parent = parent;
        bulletPool = tempPool.GetComponent<BulletPool>();
        tempPool = Instantiate(subBulletPoolObj, Vector3.zero, Quaternion.identity);
        tempPool.transform.parent = parent;
        subBulletPool = tempPool.GetComponent<BulletPool>();
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
                bulletMidFront.transform.parent = player.transform;

                var bulletMidBack = Instantiate(bulletPowerBack, midBack, muzzleMidBack.rotation);
                bulletMidBack.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage) * 0.5f, critOccur);
                bulletMidBack.transform.parent = player.transform;

                Vector3 topFront = muzzleTopFront.position + muzzleTopFront.right * 3f - muzzleTopFront.up * 2;
                Vector3 topBack = muzzleTopBack.position - muzzleTopBack.right * 3f + muzzleTopBack.up * 2; ;

                var bulletTopFront = Instantiate(bulletPowerFront, topFront, muzzleTopFront.rotation);
                bulletTopFront.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage) * 0.5f, critOccur);
                bulletTopFront.transform.parent = player.transform;

                var bulletTopBack = Instantiate(bulletPowerBack, topBack, muzzleTopBack.rotation);
                bulletTopBack.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage) * 0.5f, critOccur);
                bulletTopBack.transform.parent = player.transform;

                Vector3 botFront = muzzleBotFront.position + muzzleBotFront.right * 2.5f;
                Vector3 botBack = muzzleBotBack.position - muzzleBotBack.right * 2.5f;

                var bulletBotFront = Instantiate(bulletPowerFront, botFront, muzzleBotFront.rotation);
                bulletBotFront.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage) * 0.5f, critOccur);
                bulletBotFront.transform.parent = player.transform;

                var bulletBotBack = Instantiate(bulletPowerBack, botBack, muzzleBotBack.rotation);
                bulletBotBack.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage) * 0.5f, critOccur);
                bulletBotBack.transform.parent = player.transform;
            }

            else
            {
                Vector3 midFront = muzzleMidFront.position + muzzleMidFront.right * 3.3f;
                Vector3 midBack = muzzleMidBack.position - muzzleMidBack.right * 3.3f;

                var bulletMidFront = Instantiate(bulletFront, midFront, muzzleMidFront.rotation);
                bulletMidFront.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage), critOccur);
                bulletMidFront.transform.parent = player.transform;

                var bulletMidBack = Instantiate(bulletBack, midBack, muzzleMidBack.rotation);
                bulletMidBack.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage), critOccur);
                bulletMidBack.transform.parent = player.transform;
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
