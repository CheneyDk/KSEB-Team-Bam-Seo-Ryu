using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : Item
{
    private void Awake()
    {
        value = 10f;
    }

    public override void Init(float expAmount)
    {
        // not use
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var playerObj = collision.GetComponent<Player>();
            if (playerObj.playerCurHp > 90)
            {
                playerObj.playerCurHp = playerObj.playerMaxHp;
            }
            else
            {
                playerObj.playerCurHp += value;
            }

            isDestroyed = true;
            Destroy(gameObject);
        }
    }
}
