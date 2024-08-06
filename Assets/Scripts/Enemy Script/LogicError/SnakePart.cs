using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SnakePart : Enemy
{
    [Header("Snake Part Stat")]
    public float snakePartMaxHp;
    public float snakePartCurHp;
    protected float snakeDamage;

    protected Player player;
    protected SnakeLogicError snakeMain;

    [Header("Hit Particle")]
    [SerializeField] protected ParticleSystem hitParticle;
    [SerializeField] protected DamageNumber damageNumber;
    [SerializeField] protected DamageNumber lastingDamageNumber;

    protected Animator snakePartAni;
    protected Collider2D snakePartCollider;

    protected SpriteRenderer curSR;
    protected Color originColor;

    private void Awake()
    {
        snakeMain = GameObject.FindGameObjectWithTag("SnakeMain").GetComponent<SnakeLogicError>();
        curSR = gameObject.GetComponent<SpriteRenderer>();
        originColor = curSR.color;
        snakePartAni = GetComponent<Animator>();
        snakePartCollider = GetComponent<CircleCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        isDestroyed = false;
    }

    public void InitSnakeBodypart(float dmg)
    {
        snakeDamage = dmg;
    }

    public void PartDead()
    {
        snakePartCollider.enabled = false;
        gameObject.SetActive(false);

    }

    // give player bodyhit damage
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var playerComponent = collision.GetComponent<Player>();
            if (playerComponent != null)
            {
                playerComponent.TakeDamage(snakeDamage);

                if (playerComponent.playerCurHp <= 0)
                {
                    player = null;
                }
            }
        }
    }
}
