using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletInternet : PlayerBullet
{
    private float dotDamageTimeInterval = 1f;
    private float damageTimer;
    private float bulletRadius;
    private Vector3 bulletBiggerSize;

    private WaitForSeconds waitForPush;

    private void Awake()
    {
        bulletLifeTime = 8f;
        waitForPush = new WaitForSeconds(bulletLifeTime);
    }

    void Start()
    {
        
        bulletSpeed = 5f;
        bulletRadius = 20f;

        damageTimer = dotDamageTimeInterval;

        // time delayed destroy
        
        bulletBiggerSize = new(bulletRadius, bulletRadius, 0f);
    }

    private void OnEnable()
    {
        transform.localScale = new(5f, 5f, 5f);
        StartCoroutine(PushToPool());
    }

    public void SetBulletInternet(Vector2 bulletV, float radius)
    {
        bulletVector = bulletV;
        bulletRadius = radius;
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
        }
    }

    // this bullet give long term dot-lasting damage in large area
    private void LastingDamage()
    {
        // if time interval is not enough, fast return
        if (damageTimer < dotDamageTimeInterval) return;
        // if bullet does not become bigger yet, fast return
        if (transform.localScale.x < bulletRadius) return;

        var targetEnemies = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x / 4, 1 << 8);

        // if there are no enemy, fast return
        if (targetEnemies.Length < 1) return;

        damageTimer = 0f;
        foreach(var enemy in targetEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(bulletDamage, critOccur);
            SaveManager.instance.UpdateDamage("Internet", bulletDamage);
        }
    }

    private IEnumerator PushToPool()
    {
        yield return waitForPush;
        bulletPool.SetObj(this);
    }

    public override void ChangeSprite(Sprite powerWeapon)
    {
    }
}
