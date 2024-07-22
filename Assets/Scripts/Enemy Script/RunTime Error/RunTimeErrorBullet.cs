using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunTimeErrorBullet : MonoBehaviour
{
    public float bulletDamage = 30f;
    public float bulletSpeed;
    public Vector2 bulletDirection;

    private void Update()
    {
        transform.Translate(bulletDirection * bulletSpeed * Time.deltaTime);
    }

    public void Init(float speed, Vector2 dir)
    {
        bulletSpeed = speed;
        bulletDirection = dir;
        GameObject.Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<Player>();

            if (player.playerCurHp <= 0)
            {
                player = null;
                return;
            }
            player.TakeDamage(bulletDamage);
        }
    }
}
