using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SnakePart : MonoBehaviour
{
    protected float snakeDamage;

    protected Player player;

    // Start is called before the first frame update

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
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
