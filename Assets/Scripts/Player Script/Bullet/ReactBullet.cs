using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactBullet : PlayerBullet
{
    private Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Enemy"))
        {
            var EnemyComponent = collision.GetComponent<Enemy>();
            if (EnemyComponent != null)
            {
                EnemyComponent.TakeLastingDamage(bulletDamage, 5, Color.green);
            }
        }
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
