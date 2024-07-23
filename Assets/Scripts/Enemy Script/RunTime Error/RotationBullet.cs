using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationBullet : MonoBehaviour
{
    // like brotato normal boss.
    // bullets are spread by time.
    // rotate in the same line and same orbit.

    public float bulletDamage = 30f;
    public float bulletMoveSpeed = 3f;
    public Vector2 bulletInitPos;

    public Transform parent;
    
    public void Update()
    {
        transform.Translate(bulletInitPos * bulletMoveSpeed * Time.deltaTime, Space.Self);
    }

    // - 30 ~ -10 , 10 ~ 30: bullets range. num: 3 ~ 4 bullet
    // if not enough add more bullets
    public void Init(Vector2 pos)
    {
        bulletInitPos = pos;
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
