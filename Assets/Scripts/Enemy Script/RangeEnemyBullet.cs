using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyBullet : MonoBehaviour
{
    public int bulletDamage;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<Player>();
            player.TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
    }

    public void Init(int damage)
    {
        bulletDamage = damage;
    }
}
