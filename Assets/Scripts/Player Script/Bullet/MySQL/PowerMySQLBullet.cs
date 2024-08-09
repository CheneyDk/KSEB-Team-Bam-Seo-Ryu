using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        rotateSpeed = Random.Range(0.8f, 1.2f);
        SQLVector = new Vector2(Random.Range(-3f, -10f), Random.Range(22f, 25f));
        rigid = GetComponent<Rigidbody2D>();
        bulletVector = transform.TransformDirection(SQLVector);
        bulletSpeed = 1f;

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
        isDestroyed = false;
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
        while (!isDestroyed)
        {
            await UniTask.Yield();
            await UniTask.WaitUntil(() => GameManager.Instance.isGameContinue);
            if (isDestroyed) return;
            transform.Rotate(Vector3.forward, rotateSpeed);
        }
    }

    private async UniTask ObjDestroyTimer()
    {
        await UniTask.WaitForSeconds(bulletLifeTime);
        isDestroyed = true;
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        isDestroyed = true;
    }

    public override void ChangeSprite(Sprite powerWeapon)
    {
    }
}
