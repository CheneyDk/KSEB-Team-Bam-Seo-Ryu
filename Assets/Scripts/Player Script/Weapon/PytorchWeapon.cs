using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PytorchWeapon : PlayerWeapon
{
    private float pytorchFireInterval;
    // private float lastingDamageRate;

    private Vector2 bulletFallPos;
    private float bulletFallRange;

    // init stats
    private void Start()
    {
        weaponDamageRate = 1f;
        weaponFireRate = 20f;
        // lastingDamageRate = 0.2f;
        pytorchFireInterval = 0.5f;

        bulletFallRange = 10f;
    }

    // active func

    // inactive func

    public override void Upgrade()
    {
        if (isMaxLevel) return;

        weaponLevel += 1;
        weaponDamageRate += 0.1f;
        pytorchFireInterval -= 0.05f;
        bulletNum += 10;
    }

    protected override void Fire()
    {
        AutoFire().Forget();
    }

    private async UniTask AutoFire()
    {
        while (true)
        {
            

            // instantiate bullet
            for (int i = 0; i < bulletNum; i++)
            {
                // gen rand pos
                bulletFallPos = new(Random.Range(-bulletFallRange, bulletFallRange),
                                    Random.Range(-bulletFallRange, bulletFallRange));

                var tempBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                tempBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate);
                // set bullet drop pos
                //tempBullet.GetComponent<PytorchBullet>().SetPytorchBullet()

                // how to delete that new vector??? - YH
            }

            await UniTask.WaitForSeconds(pytorchFireInterval);
            await UniTask.WaitUntil(() => GameManager.Instance.isGameContinue);
        }

    }

    //private Vector2 GenRandPos()
    //{

    //}
    // dummy
    public override void Fire(InputAction.CallbackContext context)
    {

    }
}
