using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletWhip : PlayerBullet
{
    public float fadeDuration = 0.3f; // ������� �� �ɸ��� �ð� (��)
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        else
        {
            Debug.LogError("SpriteRenderer component not found on the GameObject.");
        }

        bulletLifeTime = fadeDuration;
    }
    private void OnEnable()
    {
        StartFadeOut();
    }

    // private void Start(){}

    private void Update()
    {
        // bullet move
        transform.Translate(bulletVector * (bulletSpeed * Time.deltaTime));

        timeCounter += Time.deltaTime;
        if (timeCounter > bulletLifeTime)
        {
            spriteRenderer.color = originalColor;
            timeCounter = 0f;
            bulletPool.SetObj(this);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var EnemyComponent = collision.GetComponent<Enemy>();
            if (EnemyComponent != null)
            {
                EnemyComponent.TakeDamage(bulletDamage, critOccur);

                ScoreManager.instance.UpdateDamage("Basic", bulletDamage);
            }
        }
    }

    public void StartFadeOut()
    {
        if (spriteRenderer != null)
        {
            StartCoroutine(FadeOutRoutine());
        }
    }

    private IEnumerator FadeOutRoutine()
    {
        float elapsedTime = 0.0f;
        Color color = originalColor;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1.0f, 0.0f, elapsedTime / fadeDuration);
            color.a = alpha;
            spriteRenderer.color = color;
            yield return null;
        }

        // ������ �����ϰ� �Ǿ��� ��
        color.a = 0.0f;
        spriteRenderer.color = color;
    }

    public override void ChangeSprite(Sprite powerWeapon)
    {
    }
}


