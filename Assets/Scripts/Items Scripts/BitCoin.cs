using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitCoin : Item
{

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //RE_SaveManager.instance.enabled = true;
            isDestroyed = true;
            ItemPooling.Instance.ReturnBitCoin(gameObject);
        }
    }
}
