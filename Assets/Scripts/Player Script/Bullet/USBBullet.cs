using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class USBBullet : PlayerBullet
{
    private Player player;

    public SpriteRenderer spriteRenderer;

    public ParticleSystem USBParticle;

    public AudioSource audioSource;
    public AudioClip audioClip;

    private void Start()
    {
        bulletSpeed = 20f;
        Destroy(gameObject, 2f);
    }

    private void Update()
    {
        transform.Translate(Vector2.up * bulletSpeed * Time.deltaTime);
    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemyComponent = collision.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                Instantiate(USBParticle, transform.position, Quaternion.identity);
                audioSource.PlayOneShot(audioClip);

                enemyComponent.TakeDamage(bulletDamage, critOccur);
                ScoreManager.instance.UpdateDamage("USB", bulletDamage);
            }

        }
    }


    public override void ChangeSprite(Sprite powerWeapon)
    {
        spriteRenderer.sprite = powerWeapon;
    }
}
