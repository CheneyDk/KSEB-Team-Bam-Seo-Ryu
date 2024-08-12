using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyBullet : MonoBehaviour
{
    public int bulletDamage;
    public float bulletSpeed = 5f;

    private Player player;

    private Vector2 direction;

    private void OnEnable()
    {
        StartCoroutine(BulletTime());
    }

    private void Start()
    {
        player = FindAnyObjectByType<Player>();

        direction = (player.transform.position - transform.position).normalized;
    }

    private void Update()
    {
        transform.Translate(direction * bulletSpeed * Time.deltaTime, Space.Self);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<Player>();
            player.TakeDamage(bulletDamage);
            gameObject.SetActive(false);
        }
    }

    IEnumerator BulletTime()
    {
        yield return new WaitForSeconds(10);
        gameObject.SetActive(false);
    }

    public void Init(int damage)
    {
        bulletDamage = damage;
    }
}
