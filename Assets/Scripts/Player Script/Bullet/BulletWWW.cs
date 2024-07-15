using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletWWW : PlayerBullet
{
    private float dotDamageInterval = 1f;
    private float speedDecreaseRate = 1.5f;
    private Vector3 bulletBiggerSize = new(10f, 10f, 0f);

    // Start is called before the first frame update
    private void Awake()
    {
        bulletLifeTime = 8f;
        bulletSpeed = 5f;
    }

    void Start()
    {
        // time delayed destroy
        Destroy(gameObject, bulletLifeTime);
    }

    public void SetBulletWWW(Vector2 bulletV)
    {
        bulletVector = bulletV;
    }
    
    void Update()
    {
        //speedDecreaseRate += 0.1f * Time.deltaTime;
        //bulletSpeed -= speedDecreaseRate * speedDecreaseRate;
        transform.Translate(bulletVector * (bulletSpeed * Time.deltaTime));
    }

    

    // if hit enemy, bullet get bigger
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            bulletVector = Vector2.zero;
            transform.localScale = bulletBiggerSize;
        }
    }

    // dot damage
    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }
}
