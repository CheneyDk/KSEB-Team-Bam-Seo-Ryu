using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RunTimeError : Enemy
{

    [Header("Enemy Information")]
    [SerializeField]
    private float RunTimeErrorMaxHp = 500f;
    [SerializeField]
    private float RunTimeErrorCurtHP;
    [SerializeField]
    private float RunTimeErrorAtk = 50f;
    [SerializeField]
    private float RunTimeErrorMoveSpeed = 10f;

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

    private Animator runtimeAni;
    private Collider2D runtimeCollider;

    private SpriteRenderer curSR;
    private Color originColor;

    private bool isDead = false;


    [Header("BossInspector")]
    public GameObject rotateBullet;
    public Transform rotationalAxis;
    [SerializeField] private float rotateBulletSpeed;
    [SerializeField] private float rotateBulletNum; // if 7 -> init 3 and 4
    private Vector2 randomMoveVector;
    private float moveDistance; 
    private float randomChangeInterval;

    private void Awake()
    {
        curSR = this.GetComponent<SpriteRenderer>();
        originColor = curSR.color;
        RunTimeErrorCurtHP = RunTimeErrorMaxHp;
        runtimeAni = GetComponent<Animator>();
        runtimeCollider = GetComponent<CircleCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        moveDistance = 5f;
        randomMoveVector = Vector2.zero;
        randomChangeInterval = 5f; // initial wait
    }

    private void Start()
    {
        RandomMoveVector().Forget();
    }

    private void Update()
    {
        if (player == null) { return; }
        if (!isDead)
        {
            EnemyMovement();
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

    public override void EnemyMovement()
    {
        // random move with random speed
        if (player is null) return;

        transform.Translate(randomMoveVector * RunTimeErrorMoveSpeed * Time.deltaTime);

    }

    private async UniTask RandomMoveVector()
    {
        while (!isDead)
        {
            await UniTask.WaitForSeconds(randomChangeInterval);

            randomMoveVector = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            RunTimeErrorMoveSpeed = Random.Range(6f, 10f);
            randomChangeInterval = moveDistance / RunTimeErrorMoveSpeed;
        }
        
    }

    public override IEnumerator LastingDamage(float damage, int totalDamageTime, Color color)
    {
        curSR.color = color;
        var damageTimer = 0f;

        while (damageTimer < totalDamageTime)
        {
            yield return new WaitForSeconds(1f);
            RunTimeErrorCurtHP -= damage;
            damageTimer += 1f;
        }

        if (RunTimeErrorCurtHP <= 0)
        {
            Destroy(gameObject);
            DropEXP(dropExpNumber);
        }
        curSR.color = originColor;
    }



    public override void TakeDamage(float damage)
    {
        RunTimeErrorCurtHP -= damage;
        if (RunTimeErrorCurtHP <= 0)
        {
            runtimeCollider.enabled = false;
            isDead = true;
            runtimeAni.SetTrigger("isDead");
            Destroy(gameObject, runtimeAni.GetCurrentAnimatorStateInfo(0).length + 1f);
            DropEXP(dropExpNumber);
        }
    }

    public override void ResetEnemy() {}
}
