using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSharp : PlayerBullet
{
    private void Start()
    {
        // go straight
        bulletVector = Vector2.right;
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
                Destroy(gameObject);
            }
        }
    }
}


