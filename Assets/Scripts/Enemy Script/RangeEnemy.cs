using Cysharp.Threading.Tasks;
using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class RangeEnemy : Enemy
{
    [Foldout("Enemy Information")]
    [SerializeField]
    private float RangeEnemyMaxHp = 10f;
    [SerializeField]
    private float RangeEnemyCurtHP;
    [SerializeField]
    private float RangeEnemyAtk = 3f;
    [SerializeField]
    private float RangeEnemyMoveSpeed = 7f;
    [SerializeField]
    private float playerEnemyRange = 15f;
    [SerializeField]
    private float attackCooldown = 1f;
    [SerializeField]
    private float bulletSpeed = 5f;
    private float rotationSpeed = 10f;
    [EndFoldout]

    public GameObject bulletPrefab;

    private bool canAttack = true;
    private Transform player;

    [Header("Exp")]
    [SerializeField]
    private GameObject Exp;
    public int dropExpNumber = 3;
    private float spawnGroupRadius = 1f;

    [Header("Drop Item"), SerializeField]
    private GameObject healingItem;
    [SerializeField]
    private GameObject redbuleItem;

    [Header("Hit Particle"), SerializeField]
    private ParticleSystem hitParticle;
    [SerializeField]
    private DamageNumber damageNumber;
    [SerializeField]
    private DamageNumber lastingDamageNumber;

    private Animator rangeAni;
    private Collider2D rangeCollider;

    private SpriteRenderer curSR;
    private Color originColor;

    private bool isDead = false;

    private void OnEnable()
    {
        StopAllCoroutines();
    }

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
        if (!isDead)
        {
            EnemyMovement();
            Rotation();
        }
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
        Destroy(bullet, 10f);
        Vector2 direction = (player.position - transform.position).normalized;
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }

    public override void TakeDamage(float damage)
    {
        Instantiate(hitParticle, transform.position, Quaternion.identity);
        damageNumber.Spawn(transform.position, damage);
        RangeEnemyCurtHP -= damage;
        if (RangeEnemyCurtHP <= 0)
        {
            EnemyDead();
        }
    }

    private void EnemyDead()
    {
        rangeCollider.enabled = false;
        isDead = true;
        rangeAni.SetBool("isDead", true);
        StartCoroutine("SetActiveToFalse");
        DropEXP(dropExpNumber);
        ChanceToDropApple(1);
        ChanceToDropRedBlue(0);
        ScoreManager.instance.UpdateEnemiesDeafeated();
    }

    private IEnumerator SetActiveToFalse()
    {
        yield return new WaitForSeconds(0.8f);
        gameObject.SetActive(false);
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

    public override void DropEXP(int itemNumber)
    {
        for (int i = 0; i < itemNumber; i++)
        {
            Vector2 spawnPlace = (Vector2)transform.position + (Vector2)Random.insideUnitCircle * spawnGroupRadius;
            ItemPooling.Instance.GetEXP(spawnPlace);
        }
    }

    public override IEnumerator LastingDamage(float damage, int totalDamageTime, Color color)
    {
        curSR.color = color;
        var damageTimer = 0f;

        while (damageTimer < totalDamageTime)
        {
            yield return new WaitForSeconds(1f);
            hitParticle.Play();
            lastingDamageNumber.Spawn(transform.position, damage);
            RangeEnemyCurtHP -= damage;
            damageTimer += 1f;

            ScoreManager.instance.UpdateDamage("React", damage);
        }

        if (RangeEnemyCurtHP <= 0)
        {
            EnemyDead();
        }
        curSR.color = originColor;
    }

    private void ChanceToDropApple(int chance)
    {
        var randomChance = Random.Range(1, 11);
        if (randomChance <= chance)
        {
            ItemPooling.Instance.GetApple(transform.position);
        }
    }
    private void ChanceToDropRedBlue(int chance)
    {
        var randomChance = Random.Range(1, 11);
        if (randomChance <= chance)
        {
            ItemPooling.Instance.GetRedBlue(transform.position);
        }
    }

    public override void ResetEnemy()
    {
        rangeAni.SetBool("isDead", false);
        curSR.color = originColor;
        RangeEnemyCurtHP = RangeEnemyMaxHp;
        rangeCollider.enabled = true;
        isDead = false;
        canAttack = true;
    }
}
