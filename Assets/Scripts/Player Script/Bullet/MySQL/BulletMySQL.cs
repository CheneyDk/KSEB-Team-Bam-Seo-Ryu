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

    private void Start()
    {
        SQLVector = new Vector2(Random.Range(-0.5f, -3f), Random.Range(2f, 4f));
        rigid = GetComponent<Rigidbody2D>();
        bulletVector = transform.TransformDirection(SQLVector);

        // throw in parabola (Æ÷¹°¼±)

        bulletLifeTime = 5f;

        // bullet move
        rigid.AddForce(bulletVector * bulletSpeed, ForceMode2D.Impulse);

        // rotation
        DolphineRotate().Forget();

        // Destroy
        ObjDestroyTimer().Forget();
    }

    private void OnEnable()
    {
        isDestroy = false;
        transform.rotation = Quaternion.identity;
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
        while (!isDestroy)
        {
            await UniTask.Yield();
            await UniTask.WaitUntil(() => GameManager.Instance.isGameContinue);
            if (isDestroy) return;
            transform.Rotate(Vector3.forward, rotateSpeed);
        }
    }

    private async UniTask ObjDestroyTimer()
    {
        await UniTask.WaitForSeconds(bulletLifeTime);
        if(isDestroy) return;
        isDestroy = true;
        bulletPool.SetObj(this);
    }

    private void OnDestroy()
    {
        isDestroy = true;
    }

    public override void ChangeSprite(Sprite powerWeapon)
    {
    }
}
