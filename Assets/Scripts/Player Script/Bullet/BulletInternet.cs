using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletInternet : PlayerBullet
{
    private float dotDamageTimeInterval = 1f;
    private float damageTimer;
    private float radius;
    private Vector3 bulletBiggerSize;

    // Start is called before the first frame update
    private void Awake()
    {
        bulletLifeTime = 8f;
        bulletSpeed = 5f;
        radius = 20f;
        bulletBiggerSize = new(radius, radius, 0f);

        damageTimer = dotDamageTimeInterval;
    }

    void Start()
    {
        // time delayed destroy
        Destroy(gameObject, bulletLifeTime);
    }

    public void SetBulletWWW(Vector2 bulletV)
    {
        bulletVector = bulletV;
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
        if (transform.localScale.x < radius) return;

        var targetEnemies = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x, 1 << 8);

        // if there are no enemy, fast return
        if (targetEnemies.Length < 1) return;

        damageTimer = 0f;
        foreach(var enemy in targetEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(bulletDamage);
        }
    }


    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (damageTimer < dotDamageTimeInterval) return;
    //    damageTimer = 0f;

    //    if (collision.CompareTag("Enemy"))
    //    {
    //        collision.GetComponent<Enemy>().TakeDamage(bulletDamage);
    //    }
    //}    
}
