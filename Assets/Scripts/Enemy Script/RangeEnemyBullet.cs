using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyBullet : MonoBehaviour
{
    public float bulletDamage = 5f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            var enemy = collision.collider.GetComponent<Player>();
            enemy.TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
    }
}
