using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // bullet stats / initialize needed
    public float bulletDamage = 2f;
    public float bulletSpeed = 5f;
    
    // direction
    public Vector2 bulletVector = Vector2.up;

    // delete
    private float timeCounter = 0f;
    private const float bulletMaxTime = 5f;

    // Init , Start

    private void Update()
    {
        // bullet move
        transform.Translate(bulletVector * (bulletSpeed * Time.deltaTime));

        timeCounter += Time.deltaTime;
        if (timeCounter > bulletMaxTime)
        {
            Destroy(gameObject);
        }
    }
    
    // Damage - fixflag - YH still on Task
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<MeleeEnemy>().TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
    }
}
