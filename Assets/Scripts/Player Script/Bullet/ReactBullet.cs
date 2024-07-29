using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactBullet : PlayerBullet
{
    private Collider2D myCollider;

    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        myCollider = GetComponent<CircleCollider2D>();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        myCollider.enabled = true;
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

    public override void ChangeSprite(Sprite powerWeapon)
    {
        spriteRenderer.sprite = powerWeapon;
    }
}
