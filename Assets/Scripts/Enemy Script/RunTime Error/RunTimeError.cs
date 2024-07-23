using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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

    public GameObject bulletPrefab;

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
    private Transform rotationalAxis;
    [SerializeField] private float rotateBulletSpeed;
    [SerializeField] private float rotateBulletNum; // if 7 -> init 3 and 4

    // random move
    private Vector2 randomMoveVector;
    private float moveDistance;
    private float mapSize = 23f;

    // direct fire
    private float randomChangeInterval;
    private float directFireRate = 5f;
    private int directFireBulletNum = 3;
    private CancellationTokenSource dircetionFireCancelSource;

    // rotation bullet
    public GameObject rotationBulletSpawner;


    private void Awake()
    {
        curSR = this.GetComponent<SpriteRenderer>();
        originColor = curSR.color;
        RunTimeErrorCurtHP = RunTimeErrorMaxHp;

        rotationalAxis = rotationBulletSpawner.GetComponent<Transform>();

        runtimeAni = GetComponent<Animator>();
        runtimeCollider = GetComponent<CircleCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        moveDistance = 5f;
        randomMoveVector = transform.position;
        randomChangeInterval = 5f; // initial wait
    }

    private void Start()
    {
        RandomMoveVector().Forget();
        DirectFire().Forget();
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

    // Enemy move
    public override void EnemyMovement()
    {
        // random move with random speed
        // if (player is null) return;

        transform.position = Vector2.MoveTowards(transform.position, randomMoveVector, RunTimeErrorMoveSpeed * Time.deltaTime);

    }

    private async UniTask RandomMoveVector()
    {
        while (!isDead)
        {
            await UniTask.WaitForSeconds(randomChangeInterval + 5f);

            // 23 == map size, if map changed, this literal needed to change too.
            randomMoveVector = new Vector2(Random.Range(-mapSize, mapSize), Random.Range(-mapSize, mapSize));
            RunTimeErrorMoveSpeed = Random.Range(6f, 8f);
            randomChangeInterval = moveDistance / RunTimeErrorMoveSpeed;

            await UniTask.WaitForSeconds(randomChangeInterval);

            randomMoveVector = Vector2.zero;
        }
        
    }


    // Enemy shot
    private async UniTask DirectFire()
    {
        dircetionFireCancelSource = new CancellationTokenSource();

        await UniTask.WaitForSeconds(randomChangeInterval);

        while (!dircetionFireCancelSource.IsCancellationRequested)
        {
            
            await UniTask.WaitForSeconds(directFireRate, cancellationToken: dircetionFireCancelSource.Token);

            var direction = player.transform.position - transform.position;
            direction.Normalize();

            var pos = transform.position;

            var bulletSpeed = Random.Range(10f, 30f);

            for (int i = 0; i < directFireBulletNum; i++)
            {
                var tempBullet = GameObject.Instantiate(bulletPrefab, pos, Quaternion.identity);
                tempBullet.GetComponent<RunTimeErrorBullet>().Init(bulletSpeed, direction);
                await UniTask.WaitForSeconds(0.3f, cancellationToken: dircetionFireCancelSource.Token);
            }
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
            RunTimeErrorCurtHP -= damage;
            damageTimer += 1f;
        }

        if (RunTimeErrorCurtHP <= 0)
        {
            RunTimeErrorDestroy();
        }
        curSR.color = originColor;
    }



    public override void TakeDamage(float damage)
    {
        RunTimeErrorCurtHP -= damage;
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
        // runtimeAni.SetTrigger("isDead");
        Destroy(gameObject); //, runtimeAni.GetCurrentAnimatorStateInfo(0).length + 1f);
        DropEXP(dropExpNumber);

            ScoreManager.instance.UpdateEnemiesDeafeated();
    }

    public override void ResetEnemy(){}
}
