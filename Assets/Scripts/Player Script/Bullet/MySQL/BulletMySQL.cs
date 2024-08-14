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

    // components
    private Rigidbody2D rigid;

    // flags
    private bool isDestroy;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        bulletLifeTime = 5f;
        bulletSpeed = 1f;
    }

    private void OnEnable()
    {
        rigid.simulated = true;
        rigid.velocity = Vector2.zero;
        rotateSpeed = Random.Range(0.8f, 1.2f);
        
        waitInitAndAddForce().Forget();

        isDestroy = false;
        // Pooling
        ObjPoolingTimer().Forget();
    }

    private async UniTask waitInitAndAddForce()
    {
        await UniTask.WaitUntil(() => isInited);
        SQLVector = new Vector2(Random.Range(-2f, -4f), Random.Range(10f, 12f));
        bulletVector = transform.TransformDirection(SQLVector);

        DolphineRotate().Forget();

        // bullet move
        rigid.AddForce(bulletVector * bulletSpeed * 50f);
    }

    // private void Start(){}

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var EnemyComponent = collision.GetComponent<Enemy>();
            if (EnemyComponent != null)
            {
                EnemyComponent.TakeDamage(bulletDamage, critOccur);

                RE_SaveManager.instance.UpdateDamage("MySQL", bulletDamage);
            }
        }
    }

    private async UniTask DolphineRotate()
    {
        while (!isDestroy)
        {
            await UniTask.Yield();
            await UniTask.WaitUntil(() => GameManager.Instance.isGameContinue);
            if (isDestroy) return;
            transform.Rotate(Vector3.forward, rotateSpeed);
        }
    }

    private async UniTask ObjPoolingTimer()
    {
        await UniTask.WaitForSeconds(bulletLifeTime);
        if(isDestroy) return;
        isDestroy = true;
        bulletPool.SetObj(this);
    }

    private void OnDisable()
    {
        rigid.simulated = false;
        isDestroy = true;
    }

    private void OnDestroy()
    {
        isDestroy = true;
    }

    public override void ChangeSprite(Sprite powerWeapon)
    {
    }
}
