using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CDBullet : PlayerBullet
{
    public float range = 10f;

    public GameObject miniCD;

    private Vector2 direction;

    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        bulletSpeed = 5f;
        bulletLifeTime = 7f;


        Vector2 targetPosition = MouseAim();

        direction = (targetPosition - (Vector2)transform.position).normalized;

        StartCoroutine("FireCD");

        Destroy(gameObject, bulletLifeTime);
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
                enemyComponent.TakeDamage(bulletDamage);
                ScoreManager.instance.UpdateDamage("CD", bulletDamage);
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
                Instantiate(miniCD, transform.position, Quaternion.Euler(0, 0, angle));
            }
            yield return new WaitForSeconds(3f);
        }
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
