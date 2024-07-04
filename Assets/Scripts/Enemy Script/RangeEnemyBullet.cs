using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyBullet : MonoBehaviour
{
    public float bulletDamage = 5f;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var enemy = collision.GetComponent<Player>();
            enemy.TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
    }
}
