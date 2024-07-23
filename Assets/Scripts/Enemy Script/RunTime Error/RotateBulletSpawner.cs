using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RotateBulletSpawner : MonoBehaviour
{
    public GameObject bullet;
    private int rowBulletNum = 4;
    private int colBulletNum = 3;
    private int bulletInterDiv;

    // divide 26 ~ 5 in seven part. bullets are located in cross
    private float bulletCircleMinRadius = 5f;
    private float bulletCircleMaxRadius = 29f;
    private float bulletCircleIntervalUnit;

    // rotation
    private Transform bulletParent;
    private float rotationSpeed = 0.3f;
    private CancellationTokenSource stopRotate;


    private void Start()
    {
        bulletParent = transform;

        bulletInterDiv = rowBulletNum + colBulletNum - 1;
        bulletCircleIntervalUnit = (bulletCircleMaxRadius - bulletCircleMinRadius) / bulletInterDiv;

        SpawnBullet();
        rotateBullet().Forget();
    }

    private async UniTask rotateBullet()
    {
        stopRotate = new CancellationTokenSource();

        await UniTask.WaitForSeconds(5f);

        while(!stopRotate.IsCancellationRequested)
        {
            transform.Rotate(0f, 0f, rotationSpeed);
            await UniTask.Yield(cancellationToken: stopRotate.Token);
        }

    }

    private void SpawnBullet()
    {
        // row positive
        for (int i = 0; i < rowBulletNum; i++)
        {
            var tempVector = Vector2.right;
            var time = bulletCircleMinRadius + bulletCircleIntervalUnit * 2 * i;
            var tempBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            tempBullet.transform.SetParent(bulletParent, true);
            tempBullet.GetComponent<RotationBullet>().Init(tempVector, time);
        }

        // row negative
        for (int i = 0; i < rowBulletNum; i++)
        {
            var tempVector = Vector2.left;
            var time = (bulletCircleMinRadius + bulletCircleIntervalUnit * 2 * i);
            var tempBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            tempBullet.transform.SetParent(bulletParent, true);
            tempBullet.GetComponent<RotationBullet>().Init(tempVector, time);
        }

        // col positive
        for (int i = 0; i < colBulletNum; i++)
        {
            var tempVector = Vector2.up;
            var time = (bulletCircleMinRadius + bulletCircleIntervalUnit * (2 * i + 1));
            var tempBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            tempBullet.transform.SetParent(bulletParent, true);
            tempBullet.GetComponent<RotationBullet>().Init(tempVector, time);
        }

        // col negative
        for (int i = 0; i < colBulletNum; i++)
        {
            var tempVector = Vector2.down;
            var time = (bulletCircleMinRadius + bulletCircleIntervalUnit * (2 * i + 1));
            var tempBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            tempBullet.transform.SetParent(bulletParent, true);
            tempBullet.GetComponent<RotationBullet>().Init(tempVector, time);
        }
    }

    public void StopBullets()
    {
        stopRotate.Cancel();
    }
}
