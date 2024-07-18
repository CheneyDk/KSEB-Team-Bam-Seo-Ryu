using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : Enemy
{
    [Header("Enemy Information")]
    [SerializeField]
    private float RangeEnemyMaxHp = 10f;
    [SerializeField]
    private float RangeEnemyCurtHP;
    [SerializeField]
    private float RangeEnemyAtk = 3f;
    [SerializeField]
    private float RangeEnemyMoveSpeed = 7f;

    private float rotationSpeed = 10f;
    private float playerEnemyRange = 15f;
    private float attackCooldown = 1f;
    private float bulletSpeed = 5f;

    public GameObject bulletPrefab;

    private bool canAttack = true;
    private Transform player;

    [Header("Exp")]
    [SerializeField]
    private GameObject Exp;
    public int dropExpNumber = 3;
    private float spawnGroupRadius = 1f;

    private Animator rangeAni;
    private Collider2D rangeCollider;

    private SpriteRenderer curSR;
    private Color originColor;

    private void Awake()
    {
        curSR = this.GetComponent<SpriteRenderer>();
        originColor = curSR.color;
        RangeEnemyCurtHP = RangeEnemyMaxHp;
        rangeAni = GetComponent<Animator>();
        rangeCollider = GetComponent<CircleCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }


    private void Update()
    {
        if(player == null) { return; }
        EnemyMovement();
        Rotation();
    }

    public override void EnemyMovement()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer > playerEnemyRange)
            {
                transform.Translate(Vector2.up * RangeEnemyMoveSpeed * Time.deltaTime);
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

    private void Rotation()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        float angle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, Time.deltaTime * rotationSpeed);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
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
            rangeCollider.enabled = false;
            rangeAni.SetTrigger("isDead");
            Destroy(gameObject, rangeAni.GetCurrentAnimatorStateInfo(0).length + 1f);
            Drop(dropExpNumber);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var playerComponent = collision.GetComponent<Player>();
            if (playerComponent != null)
            {
                playerComponent.TakeDamage(RangeEnemyAtk);

                if (playerComponent.playerCurHp <= 0)
                {
                    player = null;
                }
            }
        }
    }

    public override void Drop(int itemNumber)
    {
        for (int i = 0; i < itemNumber; i++)
        {
            Vector2 spawnPlace = (Vector2)transform.position + (Vector2)Random.insideUnitCircle * spawnGroupRadius;
            Instantiate(Exp, spawnPlace, Quaternion.identity);
        }
    }

    public override IEnumerator LastingDamage(float damage, int totalDamageTime, Color color)
    {
        curSR.color = color;
        var damageTimer = 0f;

        while (damageTimer < totalDamageTime)
        {
            yield return new WaitForSeconds(1f);
            RangeEnemyCurtHP -= damage;
            damageTimer += 1f;
        }

        if (RangeEnemyCurtHP <= 0)
        {
            Destroy(gameObject);
            Drop(dropExpNumber);
        }
        curSR.color = originColor;
    }
}
