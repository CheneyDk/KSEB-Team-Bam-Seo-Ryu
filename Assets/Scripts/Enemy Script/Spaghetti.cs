using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaghetti : Enemy
{
    [SerializeField]
    private float spaghettiDamage = 50f;
    [SerializeField]
    private float moveSpeed = 20f;

    private Transform player;

    public AudioSource audioSource;
    public AudioClip spaghettiClip;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        audioSource = GetComponent<AudioSource>();
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
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var playerComponent = collision.GetComponent<Player>();
            audioSource.PlayOneShot(spaghettiClip);
            if (playerComponent != null)
            {
                playerComponent.TakeDamage(spaghettiDamage);

                if (playerComponent.playerCurHp <= 0)
                {
                    player = null;
                }
            }
        }
    }

    public override void TakeDamage(float damage, int critOccur)
    {
    }

    public override IEnumerator LastingDamage(float damage, int totalDamageTime, Color color)
    {
        throw new System.NotImplementedException();
    }

    public override void DropEXP(int iteamNumber)
    {
    }

    public override void ResetEnemy()
    {
    }
}
