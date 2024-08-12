using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CDBullet : PlayerBullet
{
    public float range = 10f;

    public GameObject miniCD;

    private Vector2 direction;

    public SpriteRenderer spriteRenderer;

    public ParticleSystem CDParticle;

    public AudioSource audioSource;
    public AudioClip audioClip;

    private WaitForSeconds waitForPush;

    private BulletPool subBulletPool;

    private void Awake()
    {
        bulletLifeTime = 7f;
        waitForPush = new(bulletLifeTime);
    }

    private void Start()
    {
        bulletSpeed = 5f;
        Vector2 targetPosition = MouseAim();

        direction = (targetPosition - (Vector2)transform.position).normalized;

        StartCoroutine("FireCD");
    }

    private void OnEnable()
    {
        StartCoroutine(PushToPool());
    }

    private void Update()
    {
        transform.Translate(direction * bulletSpeed * Time.deltaTime);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemyComponent = collision.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                // YH - need pooling this part later
                Instantiate(CDParticle, transform.position, Quaternion.identity);
                audioSource.PlayOneShot(audioClip);
                enemyComponent.TakeDamage(bulletDamage, critOccur);
            }
        }
    }

    private IEnumerator FireCD()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            for (int i = 0; i < 5; i++)
            {
                float angle = i * (360f / 5f);
                var tempBullet = subBulletPool.GetBullet();
                tempBullet.GetComponent<PlayerBullet>().Init(bulletDamage * 0.5f, critOccur,
                    transform.position, Quaternion.Euler(0, 0, angle), subBulletPool);
            }
            yield return new WaitForSeconds(3f);
        }
    }

    public void PassSubPool(BulletPool pool)
    {
        subBulletPool = pool;
    }

    private IEnumerator PushToPool()
    {
        yield return waitForPush;
        bulletPool.SetObj(this);
    }

    private Vector2 MouseAim()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        return new Vector2(worldMousePosition.x, worldMousePosition.y);
    }

    public override void ChangeSprite(Sprite powerWeapon)
    {
        spriteRenderer.sprite = powerWeapon;
    }
}
