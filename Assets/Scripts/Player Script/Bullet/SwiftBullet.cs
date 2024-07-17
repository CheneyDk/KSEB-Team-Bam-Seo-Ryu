using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwiftBullet : PlayerBullet
{
    public float range = 10f;
    private float elapsedTime;
    private float bulletAngle = 10f;

    private Rigidbody2D rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        bulletSpeed = 3f;
        bulletLifeTime = 10f;

        Destroy(gameObject, bulletLifeTime);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        float angle = bulletSpeed * elapsedTime; 
        float radius = bulletAngle * elapsedTime;

        Vector2 newPosition = new Vector2(
            radius * Mathf.Cos(angle),
            radius * Mathf.Sin(angle)
        );

        rigid.MovePosition((Vector2)transform.position + newPosition * Time.deltaTime);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemyComponent = collision.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.TakeDamage(bulletDamage);
            }
        }
    }
}
