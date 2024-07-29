using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GithubBullet : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 10f;
    private Vector2 target;

    private void Start()
    {
        target = FindNearestEnemy();
        if (target == Vector2.zero)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        Destroy(gameObject, 5f); // Destroy bullet after 5 seconds if it doesn't hit anything
    }

    private void Update()
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject);
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
}
