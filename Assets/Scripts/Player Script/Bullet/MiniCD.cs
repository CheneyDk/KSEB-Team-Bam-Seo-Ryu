using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniCD : PlayerBullet
{
    private Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemyComponent = collision.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.TakeDamage(player.playerAtk * 0.5f);
                ScoreManager.instance.UpdateDamage("CD", player.playerAtk * 0.5f);
            }

        }
    }

    void Update()
    {
        transform.Translate(Vector2.up * 10f *Time.deltaTime);
        Destroy(gameObject, 2f);
    }
}
