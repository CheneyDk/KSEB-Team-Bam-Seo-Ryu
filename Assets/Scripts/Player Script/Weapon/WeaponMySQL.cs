using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponMySQL : PlayerWeapon
{
    private float bulletFireInterval = 0.5f;
    private Quaternion rotateRight;
    private Quaternion rotateLeft; 


    private void Start()
    {
        // init stats
        weaponDamageRate = 3.5f;
        weaponFireRate = 3f;
        bulletNum = 1;
        weaponLevel = 1;

        // player can fire imediately
        fireRateTimer = weaponFireRate;

        // rotate
        rotateRight = transform.rotation;
        rotateLeft = new Quaternion (rotateRight.x, 0f, rotateRight.z, rotateRight.w);
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

    // not player control weapon. So, not gonna use this Func.
    public override void Fire(InputAction.CallbackContext context)
    {
        
    }
}
