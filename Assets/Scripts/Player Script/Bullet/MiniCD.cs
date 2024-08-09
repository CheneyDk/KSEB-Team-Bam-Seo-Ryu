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

    private WaitForSeconds waitForPush;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        bulletLifeTime = 2f;
        waitForPush = new(bulletLifeTime);

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

    private void OnEnable()
    {
        StartCoroutine(PushToPool());
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
                enemyComponent.TakeDamage(bulletDamage, critOccur);
            }

        }
    }

    void Update()
    {
        transform.Translate(Vector2.up * 10f * Time.deltaTime);
    }

    public override void ChangeSprite(Sprite powerWeapon)
    {
        spriteRenderer.sprite = powerWeapon;
    }

    private IEnumerator PushToPool()
    {
        yield return waitForPush;
        bulletPool.SetObj(this);
    }

}