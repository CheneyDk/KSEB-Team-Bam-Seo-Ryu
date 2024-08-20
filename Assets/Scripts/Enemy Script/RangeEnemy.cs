using Cysharp.Threading.Tasks;
using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using VInspector;

public class RangeEnemy : Enemy
{
    [Foldout("Enemy Information")]
    [SerializeField]
    private float RangeEnemyMaxHp = 10f;
    [SerializeField]
    private float RangeEnemyAtk = 3f;
    [SerializeField]
    private float RangeEnemyMoveSpeed = 7f;
    [SerializeField]
    private float playerEnemyRange = 15f;
    [SerializeField]
    private float attackCooldown = 1f;
    [SerializeField]
    private float rotationSpeed = 10f;
    [SerializeField] private float RangeEnemyCurHP;
    [SerializeField] private float RangeEnemyCurAtk;
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
    [SerializeField]
    private DamageNumber critDamageNumber;

    private Animator rangeAni;
    private Collider2D rangeCollider;

    private SpriteRenderer curSR;
    private Color originColor;

    private bool isDead = false;

    private AudioManager audioManager;

    private EnemySpawner enemySpawner;

    // pooling
    public int numberToPool = 5;
    private List<GameObject> pooledEnemyBullet = new List<GameObject>();

    private void OnEnable()
    {
        StopAllCoroutines();
    }

    private void Awake()
    {
        curSR = this.GetComponent<SpriteRenderer>();
        audioManager = FindObjectOfType<AudioManager>();
        enemySpawner = FindAnyObjectByType<EnemySpawner>();
        originColor = curSR.color;
        RangeEnemyCurHP = RangeEnemyMaxHp;
        rangeAni = GetComponent<Animator>();
        rangeCollider = GetComponent<CircleCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bulletPrefab.GetComponent<RangeEnemyBullet>().Init(5);
    }

    private void Start()
    {
        PutBulletsToPool(pooledEnemyBullet, bulletPrefab);
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

        GameObject bullet = GetPooledEnemies(pooledEnemyBullet);
        if (bullet != null)
        {
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;
            bullet.SetActive(true);
        }

        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
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
        
        RangeEnemyCurHP -= damage;
        if (elixirAdditionalDamageRate > 0)
        {
            SaveManager.instance.UpdateDamage("Elixir", damage * elixirAdditionalDamageRate);
        }
        if (RangeEnemyCurHP <= 0)
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
        audioManager.NormalEnemyDamagedClip();
        DropEXP(dropExpNumber);
        ChanceToDropApple(3);
        ChanceToDropRedBlue(0);
        SaveManager.instance.EnemyDeafeat("RangeEnemyDefeated");
        SaveManager.instance.AddScore(7);
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
                playerComponent.TakeDamage(RangeEnemyCurAtk);

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
            RangeEnemyCurHP -= damage;
            damageTimer += 1f;

            SaveManager.instance.UpdateDamage("React", damage);
        }

        if (RangeEnemyCurHP <= 0)
        {
            EnemyDead();
        }
        curSR.color = originColor;
    }

    private void ChanceToDropApple(int chance)
    {
        var randomChance = Random.Range(1, 101);
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


    // pooling
    private void PutBulletsToPool(List<GameObject> poolList, GameObject enemyPrefab)
    {
        for (int i = 0; i < numberToPool; i++)
        {
            var tmp = Instantiate(enemyPrefab);
            tmp.transform.parent = GameObject.FindGameObjectWithTag("Enemy Bullet Pool").transform;
            tmp.SetActive(false);
            poolList.Add(tmp);
        }
    }

    // Get bullet from pooling list
    public GameObject GetPooledEnemies(List<GameObject> poolList)
    {
        for (int i = 0; i < poolList.Count; i++)
        {
            if (!poolList[i].activeInHierarchy)
            {
                return poolList[i];
            }
        }
        return null;
    }

    public override void ResetEnemy()
    {
        rangeAni.SetBool("isDead", false);
        ChangeEnemyState(enemySpawner.powerEnemyRate);
        rangeCollider.enabled = true;
        isDead = false;
        canAttack = true;
    }

    private void ChangeEnemyState(float num)
    {
        RangeEnemyCurHP = RangeEnemyMaxHp * num;
        RangeEnemyCurAtk = RangeEnemyAtk * num;
        bulletPrefab.GetComponent<RangeEnemyBullet>().bulletDamage += ((int)num - 1) * 5;
        originColor = Color.white + ((num - 1) / 0.5f) * new Color(0, -0.1f, -0.1f);
        curSR.color = originColor;
    }
}
