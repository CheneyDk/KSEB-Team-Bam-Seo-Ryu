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
    private bool isExist = true;

    private void Awake()
    {
        SQLVector = new Vector2(Random.Range(-0.5f, -3f), Random.Range(1.5f, 4f));
        rigid = GetComponent<Rigidbody2D>();
        bulletVector = transform.TransformDirection(SQLVector);

    }

    private void Start()
    {
        // throw in parabola (Æ÷¹°¼±)
        
        bulletLifeTime = 7f;

        // bullet move
        rigid.AddForce(bulletVector * bulletSpeed, ForceMode2D.Impulse);

        // rotation
        DolpineRotate().Forget();

        // Destroy
        ObjDestroyTimer().Forget();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var EnemyComponent = collision.GetComponent<Enemy>();
            if (EnemyComponent != null)
            {
                EnemyComponent.TakeDamage(bulletDamage);
            }
        }
    }

    private async UniTask DolpineRotate()
    {
        while (isExist)
        {
            await UniTask.NextFrame();
            if (!isExist) return;
            await UniTask.WaitUntil(() => GameManager.Instance.isGameContinue);
            transform.Rotate(Vector3.forward, rotateSpeed);
        }
    }

    private async UniTask ObjDestroyTimer()
    {
        await UniTask.WaitForSeconds(bulletLifeTime);
        isExist = false;
        Destroy(gameObject);
    }
}
