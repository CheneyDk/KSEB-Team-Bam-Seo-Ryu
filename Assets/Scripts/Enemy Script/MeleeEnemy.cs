using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics.Internal;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    [Header("Enemy Information")]
    [SerializeField]
    private float MeleeEnemyMaxHp = 20f;
    [SerializeField]
    private float MeleeEnemyCurtHP;
    [SerializeField]
    private float MeleeEnemyAtk = 5f;
    [SerializeField]
    private float MeleeEnemyMoveSpeed =10f;

    [Header("Exp")]
    [SerializeField]
    private GameObject Exp;
    public int dropExpNumber = 3;
    private float spawnGroupRadius = 1f;

    private Animator meleeAni;
    private Collider2D meleeCollider;

    private Transform player;
    private float rotationSpeed = 10f;

    private SpriteRenderer curSR;
    private Color originColor;

    private bool isDead = false;

    private void Awake()
    {
        curSR = this.GetComponent<SpriteRenderer>();
        originColor = curSR.color;
        MeleeEnemyCurtHP = MeleeEnemyMaxHp;
        meleeAni = GetComponent<Animator>();
        meleeCollider = GetComponent<CircleCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null) { return; }
        if (!isDead)
        {
            EnemyMovement();
            Rotation();
        }
    }

    public override void EnemyMovement()
    {
        if (player != null)
        {
            transform.Translate(Vector2.up * MeleeEnemyMoveSpeed * Time.deltaTime);
        }
    }

    private void Rotation()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        float angle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, Time.deltaTime * rotationSpeed);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public override void TakeDamage(float damage)
    {
        MeleeEnemyCurtHP -= damage; 
        if (MeleeEnemyCurtHP <= 0)
        {
            meleeCollider.enabled = false;
            isDead = true;
            meleeAni.SetTrigger("isDead");
            Destroy(gameObject, meleeAni.GetCurrentAnimatorStateInfo(0).length + 1f);
            Drop(dropExpNumber);

            ScoreManager.instance.UpdateEnemyKills();
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

    public override IEnumerator LastingDamage(float damage, int totalDamageTime, Color color)
    {
        curSR.color = color;
        var damageTimer = 0f;

        while (damageTimer < totalDamageTime)
        {
            yield return new WaitForSeconds(1f);
            MeleeEnemyCurtHP -= damage;
            damageTimer += 1f;
        }

        if (MeleeEnemyCurtHP <= 0)
        {
            Destroy(gameObject);
            Drop(dropExpNumber);
        }
        curSR.color = originColor;
    }

}
