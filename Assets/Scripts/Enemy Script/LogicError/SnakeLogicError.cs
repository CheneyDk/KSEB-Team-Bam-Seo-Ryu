using Cysharp.Threading.Tasks;
using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

/// <summary>
/// 
/// Guides about Second Boss, LogicError (Snake) 
/// 
/// basic rulse
/// 1. snake always go straight
/// 2. we only can change rotation (left or right)
/// 
/// important functions in this class
/// 1. snake zigzag move
/// 2. snake straight move
/// 3. straight - zigzag state transit (of course, this is not state pattern)
/// 
/// 4. 90 degree rotate (you can rotate 180 degree to do this one more time)
/// 5. charge to player - need to ready motion
/// 6. Gun Fire to player - each 
/// 
/// </summary>

public class SnakeLogicError : Enemy
{
    // Enemy Inspector
    [Header("Enemy Information")]
    [SerializeField] public float snakeMaxHp = 5000f;
    [SerializeField] private float snakeCurHP;
    [SerializeField] private float snakeAtk = 30f;

    [Header("Exp")]
    [SerializeField] private GameObject Exp;
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

    private Animator snakeAni;
    private Collider2D snakeCollider;

    private Transform player;

    private SpriteRenderer curSR;
    private Color originColor;

    public bool isDead { get; set; }

    // Snake Inspector
    [Header("Snake Inspector")]
    public float snakeSpeed;
    private float slowSpeed = 10f;
    private float normalSpeed = 20f;
    private float chargeSpeed = 50f;
    private float curTurnSpeed;
    private float slowTurnSpeed = 60f;
    private float fastTurnSpeed = 360f;
    private float turnTime;
    private Vector3 turningVector = new Vector3(0f, 0f, 1f);

    // bodyParts need to pre-allocate in unity
    // 0: Head, 1: body, 2: tail
    [SerializeField] public List<GameObject> bodyParts = new List<GameObject>();

    // actually use in script
    private List<GameObject> snakeBody = new List<GameObject>();
    private float bodyLength = 10;
    private float distanceBetween = 0.21f; // speed 20, distance 0.2f

    private float initTurningAngle;
    private int zigzagRepeat;
    private float chargeWaitTime = 2f;

    private CancellationTokenSource snakeStop;

    private EnemySpawner enemySpawner;

    [SerializeField]
    private GameObject[] healthBar;
    [SerializeField]
    private TextMeshProUGUI healthText;

    // FSM
    private BossState curState;
    private BossState nextState;

    private BossState idleState;
    private BossState slowZigZagState;
    private BossState singleChargeState;
    private BossState multiChargeState;

    private bool isTransit;
    private int idleCase; // idle or slow zigzag
    private float chargeTimer; // minus
    private float chargeTime = 0f;
    private float idleTimer; // plus
    private float idleTime = 5f;
    private int chargeCase;

    private void Awake()
    {
        // YH - move some components to each parts
        // curSR = gameObject.GetComponent<SpriteRenderer>();
        // originColor = curSR.color;
        snakeCurHP = snakeMaxHp;
        // snakeAni = GetComponent<Animator>();
        // snakeCollider = GetComponent<CircleCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        InitSnakeObj().Forget();
        curTurnSpeed = slowTurnSpeed;
        snakeSpeed = normalSpeed;

        enemySpawner = GameObject.FindAnyObjectByType<EnemySpawner>().GetComponent<EnemySpawner>();
        enemySpawner.bossIsAlive = true;

        healthText.text = ($"{snakeCurHP.ToString("N0")}MB of {snakeMaxHp}MB");

        isTransit = true;
        chargeTimer = 0f;
        StateInit();
    }

    private void StateInit()
    {
        idleState = new BossState(IdleEnter, null, null);
        slowZigZagState = new BossState(SlowZigZagEnter, null, null);
        singleChargeState = new BossState(SingleChargeEnter, null, null);
        multiChargeState = new BossState(MultiChargeEnter, null,null);

        curState = slowZigZagState;
        nextState = slowZigZagState;
    }

    private bool TransitCheck()
    {
        if (curState == idleState)
        {
            idleTimer += Time.deltaTime;

            if (idleTimer > idleTime && chargeTimer < chargeTime)
            {
                chargeCase = Random.Range(0, 10);

                if (chargeCase > 4)
                {
                    nextState = singleChargeState;
                    return true;
                }
                else
                {
                    nextState = multiChargeState;
                    return true;
                }
            }

            return false;
        }

        if (curState == slowZigZagState)
        {
            nextState = idleState;
            return true;
        }

        if (curState == singleChargeState)
        {
            idleCase = Random.Range(0, 10);

            if (idleCase > 3)
            {
                nextState = idleState;
                return true;
            }
            else
            {
                nextState = slowZigZagState;
                return true;
            }
        }

        if (curState == multiChargeState)
        {
            idleCase = Random.Range(0, 10);

            if (idleCase > 7)
            {
                nextState = idleState;
                return true;
            }
            else
            {
                nextState = slowZigZagState;
                return true;
            }
        }

        return false;
    }

    private void Start()
    {
        isDead = false;
    }

    private void Update()
    {
        if (player == null) { return; }

        chargeTimer -= Time.deltaTime;

        if (isTransit)
        {
            curState = nextState;
            curState.OnEnter?.Invoke();
            isTransit = false;
        }

        isTransit = TransitCheck();

        if (isTransit) curState.OnExit?.Invoke();

        ChangeHPBar();
    }

    private void FixedUpdate()
    {
        SnakeMovement();
    }

    public void ChangeHPBar()
    {
        if (snakeCurHP > 0)
        {
            healthText.text = ($"{snakeCurHP.ToString("N0")}MB of {snakeMaxHp}MB");
        }
        else if(snakeCurHP <= 0)
        {
            healthText.text = ($"0MB of {snakeMaxHp}MB");
        }
        float maxHp = snakeMaxHp;
        float curHp = snakeCurHP;

        float hpPerBar = maxHp / healthBar.Length;

        int activeBars = Mathf.CeilToInt(curHp / hpPerBar);

        for (int i = 0; i < healthBar.Length; i++)
        {
            healthBar[i].SetActive(i < activeBars);
        }
    }

    // Initialize Snake Body Parts
    // 
    private async UniTask InitSnakeObj()
    {
        snakeStop = new CancellationTokenSource();

        // need to fix
        int bodytype = 0;

        for (int i = 0; i < bodyLength; i++)
        {
            // i == 1: right after head, bodyLength - 1: right before tail
            if (i == 1 || i == bodyLength - 1) bodytype++;
            var tempBodypart = Instantiate(bodyParts[bodytype], transform.position, transform.rotation, transform);
            tempBodypart.GetComponent<SnakeMovement>().ClearMovementList();
            tempBodypart.GetComponent<SnakePart>().InitSnakeBodypart(snakeAtk);
            snakeBody.Add(tempBodypart);

            await UniTask.WaitForSeconds(distanceBetween, cancellationToken: snakeStop.Token);
        }
    }

    public void SnakeMovement() 
    {
        if (isDead) return;
        // SnakeMovement

        // Head Movement
        snakeBody[0].GetComponent<Rigidbody2D>().velocity = snakeSpeed * snakeBody[0].transform.right;
        // direction
        // gonna change later - YH

        // The other parts Movement
        if (snakeBody.Count < 1) return;
        for (int i = 1; i < snakeBody.Count; i++)
        {
            if (isDead) return;

            var movement = snakeBody[i - 1].GetComponent<SnakeMovement>();

            // YH - how can I make distance shorter.
            snakeBody[i].transform.position = SnakePosLerp(movement.movementList[8].position, movement.movementList[0].position);
            snakeBody[i].transform.rotation = SnakeRotLerp(movement.movementList[8].rotation, movement.movementList[0].rotation);
            movement.movementList.RemoveAt(0);
        }
    }

    // Lerp func - snakespeed
    private Vector3 SnakePosLerp(Vector3 front, Vector3 back)
    {
        // speed: 50 -> ratio 0.2, speed: 10 -> ratio: 1
        float ratio = slowSpeed / snakeSpeed;
        return Vector3.Lerp(front, back, ratio);
    }

    private Quaternion SnakeRotLerp(Quaternion front, Quaternion back)
    {
        float ratio = slowSpeed / snakeSpeed;
        return Quaternion.Lerp(front, back, ratio);
    }

    //private async UniTask SnakePatternCycle()
    //{
    //    snakeStop = new CancellationTokenSource();

    //    while (!snakeStop.IsCancellationRequested)
    //    {
    //        await UniTask.WaitForSeconds(3f, cancellationToken: snakeStop.Token);

    //        await SnakeSlowZigZag();
    //        await UniTask.WaitForSeconds(3f, cancellationToken: snakeStop.Token);

    //        await SnakeChargeZigZag();
    //        await UniTask.WaitForSeconds(3f, cancellationToken: snakeStop.Token);

    //        await SnakeChargeStraight();
    //    }
    //}

    private void IdleEnter()
    {
        idleTimer = 0f;
    }

    private void SlowZigZagEnter()
    {
        SnakeSlowZigZag().Forget();
    }

    private async UniTask SnakeSlowZigZag()
    {
        initTurningAngle = Random.Range(0f, 90f);
        zigzagRepeat = Random.Range(1, 3);
        snakeSpeed = slowSpeed;

        // start
        curTurnSpeed = fastTurnSpeed;
        await TurningSnake(initTurningAngle, 1);

        // zigzag
        for (int i = 0; i < zigzagRepeat; i++)
        {
            await UniTask.WaitForSeconds(0.5f, cancellationToken: snakeStop.Token);
            await TurningSnake(170, -1);
            await UniTask.WaitForSeconds(0.5f, cancellationToken: snakeStop.Token);
            await TurningSnake(170, 1);
        }

        // end - back to straight
        await TurningSnake(initTurningAngle, -1);
        snakeSpeed = normalSpeed;
        curTurnSpeed = slowTurnSpeed;
    }

    private void MultiChargeEnter()
    {
        chargeTimer = 30f;
        SnakeChargeStraight().Forget();
    }

    private async UniTask SnakeChargeStraight()
    {
        for (int i = 0; i < 3; i++) 
        {
            await ChargeCasting();
            await GoBackNForth(1.2f);

            // start
            snakeSpeed = chargeSpeed;
            await UniTask.WaitForSeconds(1.5f, cancellationToken: snakeStop.Token);
            await SpeedGraduallySlowDown(5f, 15f);
            Debug.Log("slow down end");
        }
    }

    private void SingleChargeEnter()
    {
        chargeTimer = 15f;
        SnakeChargeZigZag().Forget();
    }

    private async UniTask SnakeChargeZigZag()
    {
        // casting motion
        await ChargeCasting();
        await GoBackNForth(1.2f);

        snakeSpeed = chargeSpeed;
        curTurnSpeed = fastTurnSpeed;
        await TurningSnake(20, 1);
        await UniTask.WaitForSeconds(0.2f, cancellationToken: snakeStop.Token);

        curTurnSpeed = slowTurnSpeed;
        await TurningSnake(15, -1);
        await UniTask.WaitForSeconds(0.4f, cancellationToken: snakeStop.Token);
        await TurningSnake(15, 1);

        curTurnSpeed = slowTurnSpeed;
        await UniTask.WaitForSeconds(0.2f, cancellationToken: snakeStop.Token);
        await TurningSnake(20, -1);
        await SpeedGraduallySlowDown(5f, 15f);
    }

    private async UniTask ChargeCasting()
    {
        var head = snakeBody[0].GetComponent<SnakeHead>();
        head.warnigPrefab.SetActive(true);

        float timer = 0f;
        snakeSpeed = 8f;

        // looking at player
        while (timer < chargeWaitTime)
        {
            LookAtPlayer();
            timer += Time.deltaTime;
            await UniTask.Yield(cancellationToken: snakeStop.Token);
        }

        head.warnigPrefab.SetActive(false);
    }

    private void LookAtPlayer()
    {
        Vector3 playerPos = player.transform.position;
        Vector3 dir = playerPos - snakeBody[0].transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        snakeBody[0].transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    // key value: 45, 90, 135, 180 -> rotate degree
    // dir: left: 1, right: -1
    private async UniTask TurningSnake(float degree, float dir)
    {
        var curRot = snakeBody[0].transform.eulerAngles;
        curRot.z += dir * degree;

        float timer = 0f;
        float turningTime = degree / curTurnSpeed;
        while (timer < turningTime)
        {
            snakeBody[0].transform.Rotate(turningVector * curTurnSpeed * Time.deltaTime *  dir);
            timer += Time.deltaTime;
            await UniTask.Yield(cancellationToken: snakeStop.Token);
        }

        // need to correct angle in int value
        snakeBody[0].transform.eulerAngles = curRot;
    }

    private async UniTask SpeedGraduallySlowDown(float decRate, float underLine)
    {
        while (snakeSpeed > underLine)
        {
            snakeSpeed -= decRate * Time.deltaTime;
            await UniTask.Yield(cancellationToken: snakeStop.Token);
            if (isDead) return;
        }
        snakeSpeed = normalSpeed;
    }

    private async UniTask GoBackNForth(float time)
    {
        // white shining ani
        snakeSpeed = -normalSpeed;
        float incRate = chargeSpeed / time;
        while (snakeSpeed < 0f)
        {
            snakeSpeed += incRate * Time.deltaTime;
            await UniTask.Yield(cancellationToken: snakeStop.Token);
        }
        while (snakeSpeed < chargeSpeed)
        {
            snakeSpeed += incRate * 2 * Time.deltaTime;
            await UniTask.Yield(cancellationToken: snakeStop.Token);
        }
    }

    public override IEnumerator LastingDamage(float damage, int totalDamageTime, Color color)
    {
        //curSR.color = color;
        //var damageTimer = 0f;

        //while (damageTimer < totalDamageTime)
        //{
        //    yield return new WaitForSeconds(1f);
        //    // hitParticle.Play();
        //    // lastingDamageNumber.Spawn(transform.position, damage);
        //    snakeCurHp -= damage;
        //    damageTimer += 1f;

        //    ScoreManager.instance.UpdateDamage("React", damage);
        //}

        //if (snakeCurHp <= 0)
        //{
        //    SnakeDead();
        //}
        //curSR.color = originColor;

        yield return null;
    }

    public override void TakeDamage(float damage, int critOccur)
    {
        snakeCurHP -= damage;
        // ScoreManager flag
        if (snakeCurHP <= 0)
        {
            SnakeDead();
        }
    }

    private void SnakeDead()
    {
        if (isDead) return;
        isDead = true;
        snakeStop.Cancel();
        

        // call all parts' dead func
        CallDeadFunc().Forget();
    

        enemySpawner.bossIsAlive = false;

        // StartCoroutine("SetActiveToFalse"); // YH - dead motion wait?
        DropEXP(dropExpNumber);

        SaveManager.instance.EnemyDeafeat("LogicErrorDeafeated");
        SaveManager.instance.AddScore(10000);
    }

    private async UniTask CallDeadFunc()
    {
        foreach (var part in snakeBody)
        {
            part.GetComponent<SnakePart>().PartDead();
            await UniTask.WaitForSeconds(0.1f);
        }

        Destroy(gameObject);
    }

    public override void DropEXP(int itemNumber)
    {
        for (int i = 0; i < itemNumber; i++)
        {
            Vector2 spawnPlace = (Vector2)transform.position + (Vector2)Random.insideUnitCircle * spawnGroupRadius;
            Instantiate(Exp, spawnPlace, Quaternion.identity);
        }
    }

    public override void EnemyMovement() {}
    public override void ResetEnemy() { }
}
