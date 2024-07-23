using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyEnemy : Enemy
{
    [Header("Enemy Information")]
    [SerializeField]
    private float HeavyEnemyMaxHp = 40f;
    [SerializeField]
    private float HeavyEnemyCurtHP;
    [SerializeField]
    private float HeavyEnemyAtk = 10f;
    [SerializeField]
    private float HeavyEnemyMoveSpeed = 5f;

    [Header("Dash")]
    public float dashSpeed = 15f;
    public float dashRange = 15f;
    public float dashDelay = 1f;

    private float playerEnemyRange = 8f;
    private bool canDash = false;

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

    private Animator heavyAni;
    private Collider2D heavyCollider;

    private SpriteRenderer curSR;
    private Color originColor;

    private bool isDead = false;

    private void OnEnable()
    {
        StopCoroutine("TakeLastingDamage");
        StopCoroutine("SetActiveToFalse");
    }

    private void Awake()
    {
        curSR = this.GetComponent<SpriteRenderer>();
        originColor = curSR.color;
        HeavyEnemyCurtHP = HeavyEnemyMaxHp;

        heavyAni = GetComponent<Animator>();
        heavyCollider = GetComponent<CircleCollider2D>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }


    private void Update()
    {
        if (player == null) { return; }
        if (!canDash && !isDead)
        {
            EnemyMovement();
        }
    }

    public override void EnemyMovement()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer > playerEnemyRange)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                transform.Translate(direction * HeavyEnemyMoveSpeed * Time.deltaTime);
            }
            else
            {
                StartCoroutine(Dash());
            }
        }
    }

    private IEnumerator Dash()
    {
        canDash = true;
        yield return new WaitForSeconds(dashDelay);
        Vector3 dashTarget = player.position;

        Vector3 startPosition = transform.position;
        Vector3 direction = (dashTarget - startPosition).normalized;
        float dashDistance = 0f;

        while (dashDistance < dashRange)
        {
            float dashStep = dashSpeed * Time.deltaTime;
            transform.Translate(direction * dashStep, Space.World);
            dashDistance += dashStep;
            yield return null;
        }

        canDash = false;
        EnemyMovement();
    }

    public override void TakeDamage(float damage)
    {
        hitParticle.Play();
        HeavyEnemyCurtHP -= damage;
        if (HeavyEnemyCurtHP <= 0)
        {
            EnemyDead();
        }
    }

    private void EnemyDead()
    {
        heavyCollider.enabled = false;
        isDead = true;
        heavyAni.SetBool("isDead", true);
        StartCoroutine("SetActiveToFalse");
        DropEXP(dropExpNumber);
        ChanceToDropItem(healingItem, 2);
        ChanceToDropItem(redbuleItem, 1);

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
                playerComponent.TakeDamage(HeavyEnemyAtk);

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
            Instantiate(Exp, spawnPlace, Quaternion.identity);
        }
    }

    private void ChanceToDropItem(GameObject item, int chance)
    {
        var randomChance = Random.Range(1, 11);
        if (randomChance <= chance)
        {
            Instantiate(item, transform.position, Quaternion.identity);
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
            HeavyEnemyCurtHP -= damage;
            damageTimer += 1f;

            ScoreManager.instance.UpdateDamage("React", damage);
        }

        if (HeavyEnemyCurtHP <= 0)
        {
            EnemyDead();
        }
        curSR.color = originColor;
    }

    public override void ResetEnemy()
    {
        heavyAni.SetBool("isDead", false);
        curSR.color = originColor;
        HeavyEnemyCurtHP = HeavyEnemyMaxHp;
        heavyCollider.enabled = true;
        canDash = false;
        isDead = false;
    }
}
