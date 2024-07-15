using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponMySQL : PlayerWeapon
{
    private float bulletFireInterval = 0.5f;
    private Quaternion rotateRight = new(0f, 180f, 0f, 1f);
    private Quaternion rotateLeft = new(0f, 0f, 0f, 1f);

    private void Start()
    {
        // init stats
        weaponDamageRate = 3.5f;
        weaponFireRate = 3f;
        bulletNum = 1;
        weaponLevel = 1;

        // player can fire imediately
        fireRateTimer = weaponFireRate;

        muzzle = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        fireRateTimer += Time.deltaTime;

        // Auto Fire
        Fire();
    }

    // Auto Fire
    protected override void Fire()
    {


        if (fireRateTimer > weaponFireRate / player.playerAtkSpeed)
        {
            fireRateTimer = 0f;
            ThrowSQL().Forget();
        }
    }

    private async UniTask ThrowSQL()
    {
        // right side
        var tempBulletRight = Instantiate(bullet, muzzle.position, rotateRight);
        tempBulletRight.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate);

        await UniTask.WaitForSeconds(bulletFireInterval); // 0.5f

        // left side
        var tempBulletLeft = Instantiate(bullet, muzzle.position, rotateLeft);
        tempBulletLeft.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate);
    }

    public override void Upgrade()
    {

    }

    // not player control weapon. So, not gonna use this Func.
    public override void Fire(InputAction.CallbackContext context)
    {
        
    }
}
