using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerMySQLBullet : PlayerBullet
{
    // transform
    private Vector2 SQLVector; // new(-1f, 3f) - standard
    private float rotateSpeed;

    // components
    private Rigidbody2D rigid;

    // flags
    private bool isDestroyed;

    private void Awake()
    {
        bulletLifeTime = 5f;
        bulletSpeed = 1f;
        rigid = GetComponent<Rigidbody2D>();
    }

    // private void Start(){}

    private void OnEnable()
    {
        rigid.velocity = Vector2.zero;
        rotateSpeed = Random.Range(0.8f, 1.2f);

        waitInitAndAddForce().Forget();

        isDestroyed = false;
        // Pooling
        ObjPoolingTimer().Forget();
    }

    private async UniTask waitInitAndAddForce()
    {
        await UniTask.WaitUntil(() => isInited);
        SQLVector = new Vector2(Random.Range(-3f, -10f), Random.Range(22f, 25f));
        bulletVector = transform.TransformDirection(SQLVector);

        DolphineRotate().Forget();

        // bullet move
        rigid.AddForce(bulletVector * bulletSpeed, ForceMode2D.Impulse);
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

    private async UniTask ObjPoolingTimer()
    {
        await UniTask.WaitForSeconds(bulletLifeTime);
        if (isDestroyed)
        {
            return;
        }
        bulletPool.SetObj(this);
    }

    private void OnDisable()
    {
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
