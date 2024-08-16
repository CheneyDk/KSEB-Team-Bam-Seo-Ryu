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
    private Vector2 additionalYVec;

    private float bulletFloatingTime;
    private float parabolaHeight;

    // components
    private Rigidbody2D rigid;

    // flags
    private bool isDestroyed;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        bulletLifeTime = 2f;
        bulletSpeed = 1.2f;
        bulletVector = new(0f, -3f);

        bulletFloatingTime = 1f;
    }

    private void OnEnable()
    {
        isInited = false;
        parabolaVector = Vector2.zero;
        bulletVector.x = Random.Range(5f, 8f);
        VectorXInverse().Forget();
        parabolaHeight = Random.Range(20f, 24f);
        rotateSpeed = Random.Range(0.8f, 1.2f);

        isDestroyed = false;

        ParabolaYFactor().Forget();
        DolphineRotate().Forget();

        // Pooling
        ObjPoolingTimer().Forget();
    }

    private async UniTask VectorXInverse()
    {
        await UniTask.WaitUntil(() => isInited);
        if (transform.rotation.eulerAngles.y < 1f)
        {
            bulletVector.x *= -1f;
        }
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
        transform.Translate((parabolaVector + bulletVector ) * bulletSpeed * Time.deltaTime, Space.World);
    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var EnemyComponent = collision.GetComponent<Enemy>();
            if (EnemyComponent != null)
            {
                EnemyComponent.TakeDamage(bulletDamage, critOccur);

                SaveManager.instance.UpdateDamage("MySQL", bulletDamage);
            }
        }
    }

    

    private async UniTask ObjPoolingTimer()
    {
        await UniTask.WaitForSeconds(bulletLifeTime);
        if(isDestroyed) return;
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
