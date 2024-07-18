using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PytorchBullet : PlayerBullet
{
    // bullet rise distance = 5f?
    private float bulletTimer;
    private Vector2 bulletRiseVector = Vector2.up;
    private float bulletInitSpeed = 100f;
    private float bulletRiseTime = 0.5f;


    private Vector2 bulletFallVector;
    private float bulletFallTime = 0.5f;
    private float bulletExplodeRange; // physics2d overlap circle needed

    private Vector2 targetPos;


    private void Start()
    {
        BulletOrbit().Forget();
    }

    private void Update()
    {
        transform.Translate(bulletVector * bulletSpeed * Time.deltaTime);
    }

    public void SetPytorchBullet(Vector2 fallPos, float explodeRange)
    {
        targetPos = fallPos;
        bulletExplodeRange = explodeRange;
    }

    private async UniTask BulletOrbit()
    {
        BulletRise();
        await UniTask.WaitForSeconds(bulletRiseTime);
        BulletFall();
        await UniTask.WaitForSeconds(bulletFallTime);
        BulletExplode();
    }

    private void BulletRise()
    {
        bulletVector = bulletRiseVector;
        bulletSpeed = bulletInitSpeed;
    }

    private void BulletFall()
    {
        bulletVector = Vector2.zero;

        var tempVector = new Vector2(transform.position.x, transform.position.y);
        bulletFallVector = targetPos - tempVector;
        var distance = Mathf.Sqrt(Mathf.Pow(bulletFallVector.x, 2) + Mathf.Pow(bulletFallVector.y, 2));

        bulletVector = bulletFallVector.normalized;
        bulletSpeed = distance / bulletFallTime;
    }

    private void BulletExplode()
    {
        // time delayed explode
        // physics2d
        var enemies = Physics2D.OverlapCircleAll(transform.position, bulletExplodeRange, 1 << 8);

        // effects needed
        GameObject.Destroy(gameObject);

        if (enemies == null) return;
        
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(bulletDamage);
            // enemy.GetComponent<Enemy>().TakeLastingDamage(); - YH
        }

        

    }

    // dummy
    protected override void OnTriggerEnter2D(Collider2D collision)
    {

    }
}
