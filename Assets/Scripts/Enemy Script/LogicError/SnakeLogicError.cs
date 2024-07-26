using Cysharp.Threading.Tasks;
using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// 
/// Guides about Second Boss, LogicError (Snake) 
/// 
/// basic rulse
/// 1. snake always go straight
/// 2. we only can change rotation (left or right)
/// 3. if there is wall in front of snake, snake have to rotate (left or right half and half)
/// -> gonna use snake tongue (kind of wall detection)
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
    [SerializeField] private float snakeMaxHp = 20f;
    [SerializeField] private float snakeCurtHP;
    [SerializeField] private float snakeAtk = 5f;
    [SerializeField] private float snakeMoveSpeed = 10f;

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
    private float rotationSpeed = 10f;

    private SpriteRenderer curSR;
    private Color originColor;

    private bool isDead = false;

    // Snake Inspector
    [Header("Snake Inspector")]
    public float snakeSpeed = 20f;
    private float curTurnSpeed;
    private float slowTurnSpeed = 15f;
    private float fastTurnSpeed = 25f;
    private float turnTime;
    private Vector3 turningVector = new Vector3(0f, 0f, 1f);

    // bodyParts need to pre-allocate in unity
    // 0: Head, 1: body, 2: tail
    [SerializeField] public List<GameObject> bodyParts = new List<GameObject>();
    // actually use in script
    private List<GameObject> snakeBody = new List<GameObject>();
    private float bodyLength = 15;
    private float distanceBetween = 0.1f;

    public GameObject tongue; // ?

    private void Awake()
    {
        // YH - move some components to each parts
        // curSR = gameObject.GetComponent<SpriteRenderer>();
        // originColor = curSR.color;
        snakeCurtHP = snakeMaxHp;
        // snakeAni = GetComponent<Animator>();
        // snakeCollider = GetComponent<CircleCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        InitSnakeObj().Forget();
        curTurnSpeed = slowTurnSpeed;
        turningVector *= curTurnSpeed;
    }

    private void Start()
    {
        // SnakeRoundZigZag().Forget();
    }

    private void FixedUpdate()
    {
        SnakeMovement();
    }


    private async UniTask InitSnakeObj()
    {
        // need to fix
        int bodytype = 0;

        for (int i = 0; i < bodyLength; i++)
        {
            // i == 1: right after head, bodyL - 1: right before tail
            if (i == 1 || i == bodyLength - 1) bodytype++;
            var tempBodypart = Instantiate(bodyParts[bodytype], transform.position, transform.rotation, transform);
            tempBodypart.GetComponent<SnakeMovement>().ClearMovementList();
            snakeBody.Add(tempBodypart);

            await UniTask.WaitForSeconds(distanceBetween);
        }
    }

    public void SnakeMovement() 
    {
        // SnakeMovement

        // Head Movement
        snakeBody[0].GetComponent<Rigidbody2D>().velocity = snakeSpeed * snakeBody[0].transform.right;
        // direction
        // gonna change later - YH

        // The other parts Movement
        if (snakeBody.Count < 1) return;
        for (int i = 1; i < snakeBody.Count; i++)
        {
            var movement = snakeBody[i - 1].GetComponent<SnakeMovement>();
            snakeBody[i].transform.position = movement.movementList[0].position;
            snakeBody[i].transform.rotation = movement.movementList[0].rotation;
            movement.movementList.RemoveAt(0);
        }
    }

    private async UniTask SnakeRoundZigZag()
    {
        // start
        await TurningSnake(80, 1);

        // zigzag - YH - need to fix. fast turn and go straight
        await TurningSnake(160, -1);
        await UniTask.WaitForSeconds(5f);
        await TurningSnake(160, 1);

        // end - back to straight
        await TurningSnake(80, -1);
    }

    private void SnakeSharpZigZag()
    {

    }

    // key value: 45, 90, 135, 180 -> rotate degree
    // dir: left: 1, right: -1
    private async UniTask TurningSnake(float degree, float dir)
    {
        curTurnSpeed = fastTurnSpeed;
        var curRot = snakeBody[0].transform.eulerAngles;
        curRot.z += dir * degree;

        float timer = 0f;
        float turningTime = degree / curTurnSpeed;
        while (timer < turningTime)
        {
            Debug.Log("rotating");
            snakeBody[0].transform.Rotate(turningVector * Time.deltaTime * dir);
            timer += Time.deltaTime;
            await UniTask.Yield();
        }

        // still not a strict angle, need to correct angle in int value
        snakeBody[0].transform.eulerAngles = curRot;
        curTurnSpeed = slowTurnSpeed;
    }


    public override IEnumerator LastingDamage(float damage, int totalDamageTime, Color color)
    {
        curSR.color = color;
        var damageTimer = 0f;

        while (damageTimer < totalDamageTime)
        {
            yield return new WaitForSeconds(1f);
            // hitParticle.Play();
            // lastingDamageNumber.Spawn(transform.position, damage);
            snakeCurtHP -= damage;
            damageTimer += 1f;

            ScoreManager.instance.UpdateDamage("React", damage);
        }

        if (snakeCurtHP <= 0)
        {
            snakeDead();
        }
        curSR.color = originColor;
    }

    public override void TakeDamage(float damage)
    {
        // hitParticle.Play();
        // damageNumber.Spawn(transform.position, damage);
        snakeCurtHP -= damage;
        if (snakeCurtHP <= 0)
        {
            snakeDead();
        }
    }

    private void snakeDead()
    {
        snakeCollider.enabled = false;
        isDead = true;
        // snakeAni.SetBool("isDead", true); // YH - animations
        // StartCoroutine("SetActiveToFalse"); // YH - dead motion wait?
        DropEXP(dropExpNumber);
        ChanceToDropItem(healingItem, 1);

        ScoreManager.instance.UpdateEnemiesDeafeated();
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
        // var randomChance = Random.Range(1, 11);
        Instantiate(item, transform.position, Quaternion.identity);
    }


    public override void EnemyMovement() {}
    public override void ResetEnemy() { }
}
