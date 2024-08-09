using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPytorchBullet : PlayerBullet
{
    private WaitForSeconds waitForPush;

    // Start is called before the first frame update
    void Start()
    {
        bulletVector = Vector2.down;
        bulletLifeTime = 3f;
        bulletSpeed = 10f;
        waitForPush = new WaitForSeconds(bulletLifeTime);

        StartCoroutine(PushToPool());   
    }

    void Update()
    {
        transform.Translate(bulletVector * bulletSpeed * Time.deltaTime);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var EnemyComponent = collision.GetComponent<Enemy>();
            if (EnemyComponent != null)
            {
                EnemyComponent.TakeDamage(bulletDamage, critOccur);
                bulletPool.SetObj(this);

                ScoreManager.instance.UpdateDamage("Pytorch", bulletDamage);
            }
        }
    }

    private IEnumerator PushToPool()
    {
        yield return waitForPush;
        bulletPool.SetObj(this);
    }

    public override void ChangeSprite(Sprite powerWeapon) { }
}
