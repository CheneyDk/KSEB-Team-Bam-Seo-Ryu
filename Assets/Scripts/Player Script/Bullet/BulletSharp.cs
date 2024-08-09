using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSharp : PlayerBullet
{
    bool isPowerBullet;

    private SpriteRenderer spriteRenderer;
    public Sprite normalBullet;
    public Sprite powerBullet;

    private AudioManager audioManager;

    private WaitForSeconds waitForPush;


    private void Start()
    {
        // go straight
        bulletVector = Vector2.right;
        bulletSpeed = 70f;
        bulletLifeTime = 1.5f;
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioManager = FindObjectOfType<AudioManager>();
        waitForPush = new WaitForSeconds(bulletLifeTime);


        // Destroy(gameObject, bulletLifeTime);
        StartCoroutine(PushToPool()); // instead destroy
        PowerSprite();
    }

    private void Update()
    {
        // bullet move
        transform.Translate(bulletVector * (bulletSpeed * Time.deltaTime));
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var EnemyComponent = collision.GetComponent<Enemy>();
            if (EnemyComponent != null)
            {
                EnemyComponent.TakeDamage(bulletDamage, critOccur);
                audioManager.SharpClip();
                bulletPool.SetObj(this);

                ScoreManager.instance.UpdateDamage("Basic", bulletDamage);
            }
        }
    }

    public void IsPower(bool power)
    {
        isPowerBullet = power;
    }

    private void PowerSprite()
    {
        if (!isPowerBullet) return;
        spriteRenderer.sprite = powerBullet;
    }

    private IEnumerator PushToPool()
    {
        yield return waitForPush;
        bulletPool.SetObj(this);
    }

    public override void ChangeSprite(Sprite powerWeapon){}
}


