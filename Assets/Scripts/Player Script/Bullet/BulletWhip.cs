using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletWhip : PlayerBullet
{
    private void Start()
    {
        
        bulletLifeTime = 0.3f;
    }

    private void Update()
    {
        // bullet move
        transform.Translate(bulletVector * (bulletSpeed * Time.deltaTime));

        timeCounter += Time.deltaTime;
        if (timeCounter > bulletLifeTime)
        {
            Destroy(gameObject);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var EnemyComponent = collision.GetComponent<Enemy>();
            if (EnemyComponent != null)
            {
                EnemyComponent.TakeDamage(bulletDamage);
            }
        }
    }
}


