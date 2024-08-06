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

    public SpriteRenderer spriteRenderer;

    public ParticleSystem swiftParticle;

    public AudioSource audioSource;
    public AudioClip audioClip;

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

        var angle = bulletSpeed * elapsedTime; 
        var radius = bulletAngle * elapsedTime;

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
                Instantiate(swiftParticle, transform.position, Quaternion.identity);
                audioSource.PlayOneShot(audioClip);

                ScoreManager.instance.UpdateDamage("Swift", bulletDamage);
            }
        }
    }

    public override void ChangeSprite(Sprite powerWeapon)
    {
        spriteRenderer.sprite = powerWeapon;
    }
}
