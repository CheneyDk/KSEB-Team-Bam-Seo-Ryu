using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerInternetBulllet : PlayerBullet
{
    private float dotDamageTimeInterval = 0.8f;
    private float damageTimer;
    private float bulletRadius;

    private float additionalRadius;
    private float radiusBiggerRate;

    private Vector3 bulletBiggerSize;
    private bool isDestroyed;

    private WaitForSeconds waitForPush;

    

    private void OnEnable()
    {
        transform.localScale = new(1f, 1f, 1f); // reset
        isDestroyed = false;
        StartCoroutine(PushToPool());
    }

    void Start()
    {
        bulletLifeTime = 8f;
        bulletSpeed = 5f;


        radiusBiggerRate = 0.5f;
        additionalRadius = 10f;

        isDestroyed = false;

        damageTimer = dotDamageTimeInterval;

        waitForPush = new WaitForSeconds(bulletLifeTime);

        bulletBiggerSize = new(bulletRadius, bulletRadius, 0f);
    }

    public void SetBulletInternet(Vector2 bulletV, float radius)
    {
        bulletVector = bulletV;
        bulletRadius = radius / 2f;
    }

    void Update()
    {
        transform.Translate(bulletVector * (bulletSpeed * Time.deltaTime));
        damageTimer += Time.deltaTime;
        LastingDamage();
    }

    // if hit enemy, bullet get bigger
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            bulletVector = Vector2.zero;
            transform.localScale = bulletBiggerSize;
            GraduallyBigger().Forget();
        }
    }

    // this bullet give long term dot-lasting damage in large area
    private void LastingDamage()
    {
        // if time interval is not enough, fast return
        if (damageTimer < dotDamageTimeInterval) return;
        // if bullet does not become bigger yet, fast return
        if (transform.localScale.x < bulletRadius) return;

        var targetEnemies = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x, 1 << 8);

        // if there are no enemy, fast return
        if (targetEnemies.Length < 1) return;

        damageTimer = 0f;
        foreach (var enemy in targetEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(bulletDamage, critOccur);
            ScoreManager.instance.UpdateDamage("Internet", bulletDamage);
        }
    }

    private async UniTask GraduallyBigger()
    {

        while(additionalRadius > bulletRadius)
        {
            bulletRadius += Time.deltaTime * radiusBiggerRate;
            bulletBiggerSize = new(bulletRadius, bulletRadius, 0f);
            transform.localScale = bulletBiggerSize;
            await UniTask.Yield();
            if (isDestroyed) return;
        }
        bulletRadius = additionalRadius;
        transform.localScale = new(additionalRadius, additionalRadius, 0f);
    }

    private void OnDestroy()
    {
        isDestroyed = true;
    }

    private IEnumerator PushToPool()
    {
        yield return waitForPush;
        isDestroyed = true;
        bulletPool.SetObj(this);
    }

    public override void ChangeSprite(Sprite powerWeapon)
    {
    }
}
