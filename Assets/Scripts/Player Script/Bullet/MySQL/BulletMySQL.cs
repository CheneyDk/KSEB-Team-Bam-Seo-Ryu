using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletMySQL : PlayerBullet
{
    // transform
    private Vector2 SQLVector; // new(-1f, 3f) - standard
    private float rotateSpeed = 1f;

    // parabola vector
    private Vector2 parabolaVector;
    private float parabolaY;
    private float parabolaSpeed;
    private float parabolaYTimer;
    private const float pi = Mathf.PI;

    private float bulletFloatingTime;
    private float parabolaHeight;

    // components
    private Rigidbody2D rigid;

    // flags
    private bool isDestroyed;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        bulletLifeTime = 5f;
        bulletSpeed = 0.5f;
        bulletVector = Vector2.zero;
    }

    private void OnEnable()
    {
        parabolaVector = Vector2.zero;
        bulletVector.x = Random.Range(-3f, -5f);
        parabolaHeight = Random.Range(2f, 3f);
        rotateSpeed = Random.Range(0.8f, 1.2f);

        isDestroyed = false;

        ParabolaYFactor().Forget();
        DolphineRotate().Forget();

        // Pooling
        ObjPoolingTimer().Forget();
    }

    private async UniTask ParabolaYFactor()
    {
        parabolaYTimer = 0f;
        while (parabolaYTimer < bulletFloatingTime)
        {
            await UniTask.Yield();
            if (isDestroyed) return;
            parabolaYTimer += Time.deltaTime;
            parabolaY = Mathf.Cos(parabolaYTimer * pi / bulletFloatingTime);
            parabolaVector = parabolaHeight * new Vector2(0f, parabolaY);
        }
        parabolaYTimer = bulletFloatingTime;
    }

    private async UniTask DolphineRotate()
    {
        while (!isDestroyed)
        {
            await UniTask.Yield();
            await UniTask.WaitUntil(() => GameManager.Instance.isGameContinue);
            if (isDestroyed) return;
            transform.Rotate(Vector3.forward, rotateSpeed);
        }
    }

    // private void Start(){}

    private void Update()
    {
        transform.Translate((parabolaVector + bulletVector) * bulletSpeed * Time.deltaTime);
    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var EnemyComponent = collision.GetComponent<Enemy>();
            if (EnemyComponent != null)
            {
                EnemyComponent.TakeDamage(bulletDamage, critOccur);

                ScoreManager.instance.UpdateDamage("MySQL", bulletDamage);
            }
        }
    }

    

    private async UniTask ObjPoolingTimer()
    {
        await UniTask.WaitForSeconds(bulletLifeTime);
        if(isDestroyed) return;
        isDestroyed = true;
        bulletPool.SetObj(this);
    }

    private void OnDisable()
    {
        rigid.simulated = false;
        isDestroyed = true;
    }

    private void OnDestroy()
    {
        isDestroyed = true;
    }

    public override void ChangeSprite(Sprite powerWeapon)
    {
    }
}
