using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerInternetBulllet : PlayerBullet
{
    private float dotDamageTimeInterval = 1f;
    private float damageTimer;
    private float bulletRadius;

    private float additionalRadius;
    private float radiusBiggerRate;

    private Vector3 bulletBiggerSize;
    private bool isDestroyed;

    // Start is called before the first frame update
    private void Awake()
    {
        bulletLifeTime = 8f;
        bulletSpeed = 5f;
        bulletRadius = 5f;

        radiusBiggerRate = 0.5f;
        additionalRadius = 10f;
        bulletBiggerSize = new(bulletRadius, bulletRadius, 0f);
        isDestroyed = false;
        
        damageTimer = dotDamageTimeInterval;
    }

    void Start()
    {
        // time delayed destroy
        Destroy(gameObject, bulletLifeTime);
    }

    public void SetBulletInternet(Vector2 bulletV, float radius)
    {
        bulletVector = bulletV;
        bulletRadius = radius / 2;
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

        var targetEnemies = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x * 4, 1 << 8);

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
        transform.localScale = new(additionalRadius, additionalRadius, 0f);
    }

    private void OnDestroy()
    {
        isDestroyed = true;
    }

    public override void ChangeSprite(Sprite powerWeapon)
    {
    }
}
