using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GithubBullet : MonoBehaviour
{
    public float bulletSpeed = 10f;
    public float bulletLifeTime = 5f;

    public int bulletDamage = 10;

    private Vector2 target;

    public AudioSource audioSource;
    public AudioClip audioClip;

    private void Start()
    {
        target = FindNearestEnemy();
        if (target == Vector2.zero)
        {
            gameObject.SetActive(false);
        }
        OffActive();
    }

    private void Update()
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        transform.Translate(direction * bulletSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator OffActive()
    {
        yield return new WaitForSeconds(bulletLifeTime);
        gameObject.SetActive(false);
    }

    private Vector2 FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null)
        {
            return nearestEnemy.transform.position;
        }

        return Vector2.zero;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemyComponent = collision.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.TakeDamage(bulletDamage, 0);
                audioSource.PlayOneShot(audioClip);
            }
        }
    }
}
