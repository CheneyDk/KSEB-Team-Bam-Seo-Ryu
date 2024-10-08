using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogicError : MonoBehaviour
{
    private float bulletDamage = 30f;
    private float bulletSpeed = 20f;
    private Vector3 bulletVector;
    private float bulletLifeTime = 4f;

    private void Update()
    {
        transform.Translate(bulletVector * bulletSpeed * Time.deltaTime);
    }

    public void Init(Vector3 vec)
    {
        bulletVector = (vec - transform.position);
        bulletVector.Normalize();
        GameObject.Destroy(gameObject, bulletLifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<Player>();
            player.TakeDamage(bulletDamage);

            if (player.playerCurHp <= 0)
            {
                player = null;
                return;
            }
        }
    }

}
