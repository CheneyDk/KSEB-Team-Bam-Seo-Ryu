using Cysharp.Threading.Tasks;
using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.PlayerSettings;

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

    public GameObject bulletPrefab;

    private Transform player;

    [SerializeField]
    private DamageNumber damageNumber;
    [SerializeField]
    private DamageNumber lastingDamageNumber;
    [SerializeField]
    private DamageNumber critDamageNumber;

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
    private Transform rotationalAxis;
    [SerializeField] private float rotateBulletSpeed;
    [SerializeField] private float rotateBulletNum; // if 7 -> init 3 and 4

    // random move
    private Vector2 randomMoveVector;
    // private float moveDistance;
    // private float mapSize = 23f;

    // direct fire
    private int directFireBulletNum = 3;
    private CancellationTokenSource dircetionFireCancelSource;
    [SerializeField] private float fireRange = 20f;

    // rotation bullet
    public GameObject rotationBulletSpawner;
    public Sprite destroySprite;

    private EnemySpawner enemySpawner;

    [SerializeField]
    private GameObject[] healthBar;
    [SerializeField]
    private TextMeshProUGUI healthText;


    // FSM Vars
    private BossState idleState;
    private BossState movingState;
    private BossState fireState;

    [SerializeField] private BossState curState;
    private BossState nextState;

    private float idleTime = 1f;
    private float idleTimer;
    private float fireTime = 5f;
    private float fireTimer;

    private bool isTransit = false;

    private void Awake()
    {
        enemySpawner = GameObject.FindAnyObjectByType<EnemySpawner>().GetComponent<EnemySpawner>();
        enemySpawner.bossIsAlive = true;
        curSR = gameObject.GetComponent<SpriteRenderer>();
        originColor = curSR.color;
        RunTimeErrorCurtHP = RunTimeErrorMaxHp;

        rotationalAxis = rotationBulletSpawner.GetComponent<Transform>();

        runtimeAni = GetComponent<Animator>();
        runtimeCollider = GetComponent<CircleCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        randomMoveVector = transform.position;

        healthText.text = ($"{RunTimeErrorCurtHP.ToString("N0")}MB of {RunTimeErrorMaxHp}MB");

        StateInit();
    }

    private void Start()
    {
        fireTimer = 0f;
        idleTimer = 0f;
    }

    private void Update()
    {
        if (player == null) { return; }

        fireTimer += Time.deltaTime;

        if (isTransit)
        {
            curState = nextState;
            curState.OnEnter?.Invoke();
            isTransit = false;
        }
        
        isTransit = TransitCheck();

        if(isTransit) curState.OnExit?.Invoke();

        // move
        transform.position = Vector2.MoveTowards(transform.position, randomMoveVector, RunTimeErrorMoveSpeed * Time.deltaTime);
        
        ChangeHPBar();
    }

    private void StateInit()
    {
        idleState = new BossState(idleEnter, null, null);
        movingState = new BossState(EnemyMovement, null, null);
        fireState = new BossState(DirectFire, null, null);
        curState = idleState;
    }

    public void ChangeHPBar()
    {
        healthText.text = ($"{RunTimeErrorCurtHP.ToString("N0")}MB of {RunTimeErrorMaxHp}MB");

        float maxHp = RunTimeErrorMaxHp;
        float curHp = RunTimeErrorCurtHP;

        float hpPerBar = maxHp / healthBar.Length;

        int activeBars = Mathf.CeilToInt(curHp / hpPerBar);

        for (int i = 0; i < healthBar.Length; i++)
        {
            healthBar[i].SetActive(i < activeBars);
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

    // Enemy move
    public override void EnemyMovement()
    {
        // random move with random speed
        // if (player is null) return;

        RandomMoveVector();
    }

    private void RandomMoveVector()
    {
        // 23 == map size, if map changed, this literal needed to change too.
        randomMoveVector = new Vector2(Random.Range(-15f, 15f), Random.Range(-15f, 15f));
        RunTimeErrorMoveSpeed = Random.Range(5f, 7f);
    }


    // Enemy shot
    private void DirectFire()
    {
        if(isDead) return;

        FireBullet().Forget();
    }

    // UniTask is not void type.
    // so I wrap this func with the func above (DirectFire)
    private async UniTask FireBullet()
    {
        dircetionFireCancelSource = new CancellationTokenSource();
        await UniTask.WaitUntil(() => fireTime < fireTimer, cancellationToken: dircetionFireCancelSource.Token);
        fireTimer = 0f;

        var direction = player.transform.position - transform.position;
        direction.Normalize();

        var pos = transform.position;

        var bulletSpeed = Random.Range(20f, 30f);

        for (int i = 0; i < directFireBulletNum; i++)
        {
            var tempBullet = GameObject.Instantiate(bulletPrefab, pos, Quaternion.identity);
            tempBullet.GetComponent<RunTimeErrorBullet>().Init(bulletSpeed, direction);
            await UniTask.WaitForSeconds(0.3f, cancellationToken: dircetionFireCancelSource.Token);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var obj = collision.gameObject.GetComponent<Player>();
            obj.TakeDamage(RunTimeErrorAtk);

            if (obj.playerCurHp <= 0)
            {
                obj = null;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 9)
        {
            randomMoveVector = Vector3.zero - transform.position;
        }
    }
    public override IEnumerator LastingDamage(float damage, int totalDamageTime, Color color)
    {
        curSR.color = color;
        var damageTimer = 0f;

        while (damageTimer < totalDamageTime)
        {
            yield return new WaitForSeconds(1f);
            lastingDamageNumber.Spawn(transform.position, damage);
            RunTimeErrorCurtHP -= damage;
            damageTimer += 1f;
        }

        if (RunTimeErrorCurtHP <= 0)
        {
            RunTimeErrorDestroy();
        }
        curSR.color = originColor;
    }



    public override void TakeDamage(float damage, int critOccur)
    {
        damage *= (1f + elixirAdditionalDamageRate);

        if (critOccur == 1)
        {
            critDamageNumber.Spawn(transform.position, damage);
        }
        else
        {
            damageNumber.Spawn(transform.position, damage);
        }

        RunTimeErrorCurtHP -= damage;
        if (elixirAdditionalDamageRate > 0)
        {
            SaveManager.instance.UpdateDamage("Elixir", damage * elixirAdditionalDamageRate);
        }
        if (RunTimeErrorCurtHP <= 0)
        {
            RunTimeErrorDestroy();
        }
    }

    private void RunTimeErrorDestroy()
    {
        runtimeCollider.enabled = false;
        isDead = true;
        dircetionFireCancelSource.Cancel();
        rotationBulletSpawner.GetComponent<RotateBulletSpawner>().StopBullets();

        // gonna make destroy animation - YH
        runtimeAni.SetTrigger("isRuntimeErrorDead");

        curSR.sprite = destroySprite;


        // need player win func in GameManger
        enemySpawner.bossIsAlive = false;

        Destroy(gameObject, runtimeAni.GetCurrentAnimatorStateInfo(0).length + 4f);
        DropEXP(dropExpNumber);

        SaveManager.instance.EnemyDeafeat("RuntimeErrorDeafeated");
        SaveManager.instance.AddScore(500);
    }

    // FSM func
    private bool TransitCheck()
    {
        // idle
        if (curState == idleState)
        {
            idleTimer += Time.deltaTime;

            if (idleTime < idleTimer)
            {
                nextState = movingState;
                return true;
            }

            return IsPlayerInRange();
        }

        // moving
        if (curState == movingState)
        {
            var distance = (randomMoveVector - (Vector2)transform.position).magnitude;
            if (distance < 0.5f)
            {
                nextState = idleState;
                return true;
            }

            return IsPlayerInRange();
        }

        // fire
        if (curState == fireState)
        {
            nextState = idleState;
            return true;
        }

        return false;
    }

    private bool IsPlayerInRange()
    {
        var distance = player.transform.position - transform.position;

        if (distance.magnitude < fireRange)
        {
            nextState = fireState;
            return true;
        }

        return false;
    }

    private void idleEnter()
    {
        idleTimer = 0f;
        randomMoveVector = Vector2.zero;
    }

    public override void ResetEnemy(){}
}
