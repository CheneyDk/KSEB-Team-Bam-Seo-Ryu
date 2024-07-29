using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBody : SnakePart
{
    public GameObject bullet;
    public SnakeLogicError snakeMain;
    private float waittimeForFirstFire = 10f;
    private float snakeFireRate = 8f;
    private float fireRateRamdom;


    void Start()
    {
        FireCycle().Forget();
    }

    private async UniTask FireCycle()
    {
        await UniTask.WaitForSeconds(waittimeForFirstFire);

        while (true)
        {
            fireRateRamdom = Random.Range(-0.5f, 0.5f);
            SnakeBodyGunFire();
            await UniTask.WaitForSeconds(snakeFireRate + fireRateRamdom);
            if (snakeMain.isDead) return;
        }
    }

    // gun fire
    private void SnakeBodyGunFire()
    {
        var tempBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        tempBullet.GetComponent<BulletLogicError>().Init(player.transform.position);
    }
}
