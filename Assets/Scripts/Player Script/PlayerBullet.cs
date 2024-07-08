using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    // bullet stats / initialize needed
    public float bulletDamage = 2f;
    public float bulletSpeed = 5f;
    
    // direction
    private Vector2 bulletVector = Vector2.right;

    // delete
    private float timeCounter = 0f;
    private const float bulletLifeTime = 5f;

    // Init , Start

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
