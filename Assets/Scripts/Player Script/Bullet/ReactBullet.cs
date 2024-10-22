using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactBullet : PlayerBullet
{
    private Collider2D myCollider;

    public SpriteRenderer spriteRenderer;

    public ParticleSystem reactParticle;


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

    private void ParticleStart()
    {
        Instantiate(reactParticle, transform.position, Quaternion.identity);
    }

    private void DestroyBullet()
    {
        bulletPool.SetObj(this);
    }


    public override void ChangeSprite(Sprite powerWeapon)
    {
        spriteRenderer.sprite = powerWeapon;
    }
}
