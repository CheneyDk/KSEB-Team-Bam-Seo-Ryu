using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnityBullet : PlayerBullet
{
    public float range = 10f;

    private Vector2 direction;

    public SpriteRenderer spriteRenderer;

    public ParticleSystem unityParticle;

    public AudioSource audioSource;
    public AudioClip audioClip;

    private void Start()
    {
        bulletSpeed = 5f;
        bulletLifeTime = 5f;

        Vector2 targetPosition = FindNearestEnemy();
        
        direction = (targetPosition - (Vector2)transform.position).normalized;
        

        Destroy(gameObject, bulletLifeTime);
    }

    private void Update()
    {
        transform.Translate(direction * bulletSpeed * Time.deltaTime);
    }

    public override void ChangeSprite(Sprite powerWeapon)
    {
        spriteRenderer.sprite = powerWeapon;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemyComponent = collision.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.TakeDamage(bulletDamage);
                Instantiate(unityParticle, transform.position, Quaternion.identity);
                audioSource.PlayOneShot(audioClip);

                ScoreManager.instance.UpdateDamage("Unity", bulletDamage);
            }
        }
    }

    private Vector2 FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance && distance <= range)
            {
                minDistance = distance;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null)
        {
            return nearestEnemy.transform.position;
        }


        return MouseAim();
    }

    private Vector2 MouseAim()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        return new Vector2(worldMousePosition.x, worldMousePosition.y);
    }
}
