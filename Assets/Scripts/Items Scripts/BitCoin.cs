using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitCoin : Item
{

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SaveManager.instance.AddScore(5000);
            isDestroyed = true;
            ItemPooling.Instance.ReturnBitCoin(gameObject);
        }
    }
}
