using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyEnemy : Enemy
{
    [SerializeField]
    private float HeavyEnemyMaxHp = 40f;
    [SerializeField]
    private float HeavyEnemyCurtHP;
    [SerializeField]
    private float HeavyEnemyAtk = 10f;
    [SerializeField]
    private float HeavyEnemyMoveSpeed = 5f;
    [SerializeField]
    private float dashSpeed = 15f;
    [SerializeField]
    private float dashRange = 15f;


    private float playerEnemyRange = 8f;

    private bool canDash = true;
    private Transform player;

    private void Awake()
    {
        HeavyEnemyCurtHP = HeavyEnemyMaxHp;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }


    private void Update()
    {
        EnemyMovement();
    }

    public override void EnemyMovement()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer > playerEnemyRange)
            {
                canDash = true;
                DashAtk(dashSpeed, dashRange);
            }
        }
    }

    private void DashAtk(float dashSpeed, float dashRange)
    {

    }

    public override void TakeDamage(float damage)
    {
        HeavyEnemyCurtHP -= damage;
        if (HeavyEnemyCurtHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
