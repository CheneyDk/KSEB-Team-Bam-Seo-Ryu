using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;
using static Enemy;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class HeavyEnemy : Enemy
{
    public StateMachine stateMachine;

    [Foldout("Enemy Information")]
    public float HeavyEnemyMaxHp = 40f;
    public float HeavyEnemyAtk = 10f;
    public float HeavyEnemyMoveSpeed = 5f;
    [SerializeField] private float HeavyEnemyCurHP;
    [SerializeField] private float HeavyEnemyCurAtk;
    [EndFoldout]

    [Header("Dash")]
    public float dashSpeed = 15f;
    public float dashRange = 15f;
    public float dashDelay = 1f;

    public float playerEnemyRange = 8f;
    public bool canDash = false;

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
    [SerializeField]
    private DamageNumber critDamageNumber;

    private Animator heavyAni;
    private Collider2D heavyCollider;

    private SpriteRenderer curSR;
    private Color originColor;

    private bool isDead = false;

    private AudioManager audioManager;

    private EnemySpawner enemySpawner;

    public Player player;

    private void OnEnable()
    {
        StopCoroutine("TakeLastingDamage");
        StopCoroutine("SetActiveToFalse");
    }

    private void Awake()
    {
        curSR = GetComponentInChildren<SpriteRenderer>();
        audioManager = FindObjectOfType<AudioManager>();
        enemySpawner = FindAnyObjectByType<EnemySpawner>();
        originColor = curSR.color;
        HeavyEnemyCurHP = HeavyEnemyMaxHp;
        player = FindAnyObjectByType<Player>();

        heavyAni = GetComponent<Animator>();
        heavyCollider = GetComponent<CircleCollider2D>();

        stateMachine = new StateMachine();
        stateMachine.SetState(new HeavyEnemyMoveState(this));
    }


    private void Update()
    {
        if (player == null) return;
        stateMachine.Update();
    }

    public override void EnemyMovement()
    {
    }

    public override void TakeDamage(float damage, int critOccur)
    {
        Instantiate(hitParticle, transform.position, Quaternion.identity);
        damage *= (1f + elixirAdditionalDamageRate);
        if (critOccur == 1)
        {
            critDamageNumber.Spawn(transform.position, damage);
        }
        else
        {
            damageNumber.Spawn(transform.position, damage);
        }
        HeavyEnemyCurHP -= damage;
        if (elixirAdditionalDamageRate > 0)
        {
            SaveManager.instance.UpdateDamage("Elixir", damage * elixirAdditionalDamageRate);
        }
        if (HeavyEnemyCurHP <= 0)
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
        audioManager.HeavyEnemyDamagedClip();
        DropEXP(dropExpNumber);
        ChanceToDropApple(1);
        ChanceToDropRedBlue(5);

        SaveManager.instance.EnemyDeafeat("HeavyEnemyDefeated");
        SaveManager.instance.AddScore(5);
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
                playerComponent.TakeDamage(HeavyEnemyCurAtk);

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
        var randomChance = Random.Range(1, 101);
        if (randomChance <= chance)
        {
            ItemPooling.Instance.GetRedBlue(transform.position);
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
            HeavyEnemyCurHP -= damage;
            damageTimer += 1f;

            SaveManager.instance.UpdateDamage("React", damage);
        }

        if (HeavyEnemyCurHP <= 0)
        {
            EnemyDead();
        }
        curSR.color = originColor;
    }

    public override void ResetEnemy()
    {
        heavyAni.SetBool("isDead", false);
        ChangeEnemyState(enemySpawner.powerEnemyRate);
        heavyCollider.enabled = true;
        canDash = false;
        isDead = false;
    }

    private void ChangeEnemyState(float num)
    {
        HeavyEnemyCurHP = HeavyEnemyMaxHp * num;
        HeavyEnemyCurAtk = HeavyEnemyAtk * num;
        originColor = Color.white + ((num - 1) / 0.5f) * new Color(0, -0.1f, -0.1f);
        curSR.color = originColor;
    }
}

public class HeavyEnemyMoveState : IEnemyState
{
    private HeavyEnemy enemy;

    public HeavyEnemyMoveState(HeavyEnemy enemy)
    {
        this.enemy = enemy;
    }

    public void EnterState()
    {
    }

    public void ExitState()
    {
    }

    public void UpdateState()
    {
        if (enemy.player == null) return;

        float distanceToPlayer = Vector2.Distance(enemy.transform.position, enemy.player.transform.position);
        if (distanceToPlayer > enemy.playerEnemyRange)
        {
            Vector2 direction = (enemy.player.transform.position - enemy.transform.position).normalized;
            enemy.transform.Translate(direction * enemy.HeavyEnemyMoveSpeed * Time.deltaTime);
        }
        else
        {
            enemy.stateMachine.SetState(new HeavyEnemyDashState(enemy));
        }
    }
}

public class HeavyEnemyDashState : IEnemyState
{
    private HeavyEnemy enemy;

    public HeavyEnemyDashState(HeavyEnemy enemy)
    {
        this.enemy = enemy;
    }

    public void EnterState()
    {
        enemy.StartCoroutine(Dash());
    }

    public void ExitState()
    {
        enemy.StopCoroutine(Dash());
    }

    public void UpdateState()
    {
        if (Vector2.Distance(enemy.transform.position, enemy.player.transform.position) > enemy.playerEnemyRange)
        {
            enemy.stateMachine.SetState(new HeavyEnemyMoveState(enemy));
        }
    }

    private IEnumerator Dash()
    {
        enemy.canDash = true;
        yield return new WaitForSeconds(enemy.dashDelay);
        Vector3 dashTarget = enemy.player.transform.position;

        Vector3 startPosition = enemy.transform.position;
        Vector3 direction = (dashTarget - startPosition).normalized;
        float dashDistance = 0f;

        while (dashDistance < enemy.dashRange)
        {
            float dashStep = enemy.dashSpeed * Time.deltaTime;
            enemy.transform.Translate(direction * dashStep, Space.World);
            dashDistance += dashStep;
            yield return null;
        }

        enemy.canDash = false;
    }
}
