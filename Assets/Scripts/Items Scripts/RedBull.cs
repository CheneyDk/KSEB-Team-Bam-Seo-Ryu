using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBull : Item
{
    private void Awake()
    {
        value = 1.2f;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var playerObj = collision.GetComponent<Player>();
            playerObj.GetEnergyDrink(value);

            isDestroyed = true;
            ItemPooling.Instance.ReturnRedBlue(gameObject);
        }
    }
}
