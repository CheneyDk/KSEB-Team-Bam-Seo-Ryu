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
        bulletSpeed = 10f; 
    }

    private void Awake()
    {
        bulletLifeTime = 3f;
        waitForPush = new WaitForSeconds(bulletLifeTime);
    }

    private void OnEnable()
    {
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

                RE_SaveManager.instance.UpdateDamage("Pytorch", bulletDamage);
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
