using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBullet : MonoBehaviour
{
    // bullet stats / initialize needed
    public float bulletDamage;
    public float bulletSpeed;
    public float bulletNum; // Ex ) shotgun
    
    // delete
    protected float timeCounter = 0f;
    public const float bulletLifeTime = 5f;

    // YH - call Init func in Start func

    
    // if triggered, give damage
    private void OnTriggerStay2D(Collider2D collision)
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
