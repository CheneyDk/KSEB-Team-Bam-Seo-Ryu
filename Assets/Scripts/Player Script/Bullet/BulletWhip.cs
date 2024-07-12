using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletWhip : PlayerBullet
{
    public float fadeDuration = 0.3f; // ������� �� �ɸ��� �ð� (��)
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Start()
    {
        bulletLifeTime = 0.3f;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        else
        {
            Debug.LogError("SpriteRenderer component not found on the GameObject.");
        }

        StartFadeOut();
    }

    private void Update()
    {
        // bullet move
        transform.Translate(bulletVector * (bulletSpeed * Time.deltaTime));

        timeCounter += Time.deltaTime;
        if (timeCounter > bulletLifeTime)
        {
            Destroy(gameObject);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var EnemyComponent = collision.GetComponent<Enemy>();
            if (EnemyComponent != null)
            {
                EnemyComponent.TakeDamage(bulletDamage);
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
        gameObject.SetActive(false); // ������Ʈ�� ��Ȱ��ȭ
    }
}


