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

    [SerializeField] private GameObject bulletPoolBackObj;
    private BulletPool bulletPoolBack;
    [SerializeField] private GameObject powerBulletPoolFrontObj;
    private BulletPool powerBulletPoolFront;
    [SerializeField] private GameObject powerBulletPoolBackObj;
    private BulletPool powerBulletPoolBack;

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
        tempPool = Instantiate(bulletPoolBackObj, Vector3.zero, Quaternion.identity);
        tempPool.transform.parent = parent;
        bulletPoolBack = tempPool.GetComponent<BulletPool>();
        tempPool = Instantiate(powerBulletPoolFrontObj, Vector3.zero, Quaternion.identity);
        tempPool.transform.parent = parent;
        powerBulletPoolFront = tempPool.GetComponent<BulletPool>();
        tempPool = Instantiate(powerBulletPoolBackObj, Vector3.zero, Quaternion.identity);
        tempPool.transform.parent = parent;
        powerBulletPoolBack = tempPool.GetComponent<BulletPool>();
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

                var bulletMidFront = powerBulletPoolFront.GetBullet();
                bulletMidFront.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage) * 0.5f, critOccur,
                    midFront, muzzleMidFront.rotation, powerBulletPoolFront);

                var bulletMidBack = powerBulletPoolBack.GetBullet();
                bulletMidBack.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage) * 0.5f, critOccur,
                    midBack, muzzleMidBack.rotation, powerBulletPoolBack);

                Vector3 topFront = muzzleTopFront.position + muzzleTopFront.right * 3f - muzzleTopFront.up * 2;
                Vector3 topBack = muzzleTopBack.position - muzzleTopBack.right * 3f + muzzleTopBack.up * 2; ;

                var bulletTopFront = powerBulletPoolFront.GetBullet();
                bulletTopFront.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage) * 0.5f, critOccur,
                    topFront, muzzleTopFront.rotation, powerBulletPoolFront);

                var bulletTopBack = powerBulletPoolBack.GetBullet();
                bulletTopBack.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage) * 0.5f, critOccur,
                    topBack, muzzleTopBack.rotation, powerBulletPoolBack);

                Vector3 botFront = muzzleBotFront.position + muzzleBotFront.right * 2.5f;
                Vector3 botBack = muzzleBotBack.position - muzzleBotBack.right * 2.5f;

                var bulletBotFront = powerBulletPoolFront.GetBullet();
                bulletBotFront.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage) * 0.5f, critOccur,
                    botFront, muzzleBotFront.rotation, powerBulletPoolFront);

                var bulletBotBack = powerBulletPoolBack.GetBullet();
                bulletBotBack.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage) * 0.5f, critOccur,
                    botBack, muzzleBotBack.rotation, powerBulletPoolBack);
            }

            else
            {
                Vector3 midFront = muzzleMidFront.position + muzzleMidFront.right * 3.3f;
                Vector3 midBack = muzzleMidBack.position - muzzleMidBack.right * 3.3f;

                var bulletMidFront = bulletPool.GetBullet();
                bulletMidFront.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage), critOccur,
                    midFront, muzzleMidFront.rotation, bulletPool) ;
                bulletMidFront.transform.parent = player.transform;

                var bulletMidBack = bulletPoolBack.GetBullet();
                bulletMidBack.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage), critOccur,
                    midBack, muzzleMidBack.rotation, bulletPoolBack);
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
