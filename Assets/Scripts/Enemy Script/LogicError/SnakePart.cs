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

    [Header("Hit Particle"), SerializeField]
    private ParticleSystem hitParticle;
    [SerializeField]
    private DamageNumber damageNumber;
    [SerializeField]
    private DamageNumber lastingDamageNumber;

    private Animator snakePartAni;
    private Collider2D snakePartCollider;

    private SpriteRenderer curSR;
    private Color originColor;

    private bool isDead;


    private void Awake()
    {
        curSR = gameObject.GetComponent<SpriteRenderer>();
        originColor = curSR.color;
        snakePartAni = GetComponent<Animator>();
        snakePartCollider = GetComponent<CircleCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        isDead = false;
    }

    public void InitSnakeBodypart(float dmg)
    {
        snakeDamage = dmg;
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
