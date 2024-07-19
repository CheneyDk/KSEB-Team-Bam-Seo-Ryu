using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXP : Item
{
    private void Awake()
    {
        value = 1f;
    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            var playerObj = collision.GetComponent<Player>();
            playerObj.GetExp(value);
            isDestroyed = true;
            Destroy(gameObject);
        }
    }
    
}
