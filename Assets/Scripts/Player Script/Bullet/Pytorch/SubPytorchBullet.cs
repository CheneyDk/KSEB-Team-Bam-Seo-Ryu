using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPytorchBullet : PlayerBullet
{
    // Start is called before the first frame update
    void Start()
    {
        bulletVector = Vector2.down;
        bulletLifeTime = 3f;
        bulletSpeed = 10f;
        Destroy(gameObject, bulletLifeTime);    
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
                Destroy(gameObject);

                ScoreManager.instance.UpdateDamage("Pytorch", bulletDamage);
            }
        }
    }


    public override void ChangeSprite(Sprite powerWeapon) { }
}
