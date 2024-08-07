using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniCD : PlayerBullet
{
    private Player player;

    private SpriteRenderer spriteRenderer;

    public ParticleSystem CDparticle;

    public AudioSource audioSource;
    public AudioClip audioClip;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        int chance = Random.Range(0, 100);
        if (chance > player.playerCritPer)
        {
            critOccur = 0;
        }
        else
        {
            critOccur = 1;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemyComponent = collision.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                Instantiate(CDparticle, transform.position, Quaternion.identity);
                audioSource.PlayOneShot(audioClip);
                enemyComponent.TakeDamage(player.playerAtk * (1f + player.playerCritDmg * critOccur) * 0.5f);
            }

        }
    }

    void Update()
    {
        transform.Translate(Vector2.up * 10f * Time.deltaTime);
        Destroy(gameObject, 2f);
    }

    public override void ChangeSprite(Sprite powerWeapon)
    {
        spriteRenderer.sprite = powerWeapon;
    }
}