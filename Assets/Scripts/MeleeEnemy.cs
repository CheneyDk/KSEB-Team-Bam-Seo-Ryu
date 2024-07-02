using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [SerializeField]
    private float MeleeEnemyMaxHp = 20f;
    [SerializeField]
    private float MeleeEnemyCurtHP;
    [SerializeField]
    private float MeleeEnemyAtk = 5f;
    [SerializeField]
    private float MeleeEnemyMoveSpeed =10f;

    private Transform player;

    private void Awake()
    {
        MeleeEnemyCurtHP = MeleeEnemyMaxHp;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.Translate(direction * MeleeEnemyMoveSpeed * Time.deltaTime);
        }
    }

    public void TakeDamage(float damage)
    {
        MeleeEnemyCurtHP -= damage;
        if (MeleeEnemyCurtHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
