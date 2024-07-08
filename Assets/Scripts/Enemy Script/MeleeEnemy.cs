using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    [SerializeField]
    private float MeleeEnemyMaxHp = 20f;
    [SerializeField]
    private float MeleeEnemyCurtHP;
    [SerializeField]
    private float MeleeEnemyAtk = 5f;
    [SerializeField]
    private float MeleeEnemyMoveSpeed =10f;

    [SerializeField]
    private GameObject Exp;
    public int dropNumber = 3;
    private float spawnGroupRadius = 1f;

    private Transform player;

    private void Awake()
    {
        MeleeEnemyCurtHP = MeleeEnemyMaxHp;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null) { return; }
        EnemyMovement();
    }

    public override void EnemyMovement()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.Translate(direction * MeleeEnemyMoveSpeed * Time.deltaTime);
        }
    }

    public override void TakeDamage(float damage)
    {
        MeleeEnemyCurtHP -= damage; 
        if (MeleeEnemyCurtHP <= 0)
        {
            Destroy(gameObject);
            Drop(dropNumber);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var playerComponent = collision.GetComponent<Player>();
            if (playerComponent != null)
            {
                playerComponent.TakeDamage(MeleeEnemyAtk);

                if (playerComponent.playerCurHp <= 0)
                {
                    player = null;
                }
            }
        }
    }

    public override void Drop(int itemNumber)
    {
        for (int i = 0; i < itemNumber; i++)
        {
            Vector2 spawnPlace = (Vector2)transform.position + (Vector2)Random.insideUnitCircle * spawnGroupRadius;
            Instantiate(Exp, spawnPlace, Quaternion.identity);
        }
    }
}
