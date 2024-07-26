using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : Item
{
    private void Awake()
    {
        value = 10f;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var playerObj = collision.GetComponent<Player>();
            playerObj.GetHpPotion(value);
            isDestroyed = true;
            ItemPooling.Instance.ReturnApple(gameObject);
        }
    }
}
