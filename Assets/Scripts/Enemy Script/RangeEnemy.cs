using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : Enemy
{
    [SerializeField]
    private float RangeEnemyMaxHp = 10f;
    [SerializeField]
    private float RangeEnemyCurtHP;
    [SerializeField]
    private float RangeEnemyAtk = 3f;
    [SerializeField]
    private float RangeEnemyMoveSpeed = 7f;


    private float playerEnemyRange = 5f;
    private float attackCooldown = 1f;
    private float bulletSpeed = 5f;

    public GameObject bulletPrefab;

    private bool canAttack = true;
    private Transform player;

    private void Awake()
    {
        RangeEnemyCurtHP = RangeEnemyMaxHp;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }


    private void Update()
    {
        EnemyMovement();
    }

    public override void EnemyMovement()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer > playerEnemyRange)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                transform.Translate(direction * RangeEnemyMoveSpeed * Time.deltaTime);
            }
            else
            {
                if (canAttack)
                {
                    StartCoroutine(RangeAtk());
                }
            }
        }
    }

    private IEnumerator RangeAtk()
    {
        canAttack = false;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Destroy(bullet, 3f);
        Vector2 direction = (player.position - transform.position).normalized;
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }

    public override void TakeDamage(float damage)
    {
        RangeEnemyCurtHP -= damage;
        if (RangeEnemyCurtHP <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<Player>();
            player.TakeDamage(RangeEnemyAtk);
        }
    }
}
