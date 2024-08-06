using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using VInspector;

public class MeleeEnemy : Enemy
{
    [Foldout("Enemy Information")]
    [SerializeField]
    private float MeleeEnemyMaxHp = 20f;
    [SerializeField]
    private float MeleeEnemyCurtHP;
    [SerializeField]
    private float MeleeEnemyAtk = 5f;
    [SerializeField]
    private float MeleeEnemyMoveSpeed =10f;
    [EndFoldout]

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

    private Animator meleeAni;
    private Collider2D meleeCollider;

    private Transform player;
    private float rotationSpeed = 10f;

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
        MeleeEnemyCurtHP = MeleeEnemyMaxHp;
        meleeAni = GetComponent<Animator>();
        meleeCollider = GetComponent<CircleCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null) { return; }
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
            transform.Translate(Vector2.up * MeleeEnemyMoveSpeed * Time.deltaTime);
        }
    }

    private void Rotation()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        float angle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, Time.deltaTime * rotationSpeed);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public override void TakeDamage(float damage)
    {
        Instantiate(hitParticle, transform.position, Quaternion.identity);
        damageNumber.Spawn(transform.position, damage);
        MeleeEnemyCurtHP -= damage * (1f + elixirAdditionalDamageRate);
        ScoreManager.instance.UpdateDamage("Elixir", damage * elixirAdditionalDamageRate);
        if (MeleeEnemyCurtHP <= 0)
        {
            EnemyDead();
        }
    }

    private void EnemyDead()
    {
        meleeCollider.enabled = false;
        isDead = true;
        meleeAni.SetBool("isDead", true);
        StartCoroutine("SetActiveToFalse");
        DropEXP(dropExpNumber);
        ChanceToDropApple(1);
        ChanceToDropRedBlue(0);

        ScoreManager.instance.UpdateEnemiesDeafeated();
    }

    private IEnumerator SetActiveToFalse()
    {
        yield return new WaitForSeconds(.8f);
        gameObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var playerComponent = collision.GetComponent<Player>();
            if (playerComponent != null)
            {
                playerComponent.TakeDamage(MeleeEnemyAtk);

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
            MeleeEnemyCurtHP -= damage;
            damageTimer += 1f;

            ScoreManager.instance.UpdateDamage("React", damage);
        }

        if (MeleeEnemyCurtHP <= 0)
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
        meleeAni.SetBool("isDead", false);
        curSR.color = originColor;
        MeleeEnemyCurtHP = MeleeEnemyMaxHp;
        meleeCollider.enabled = true;
        isDead = false;
    }
}
