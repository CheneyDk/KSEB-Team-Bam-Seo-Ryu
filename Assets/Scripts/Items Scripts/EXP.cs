using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXP : Item
{
    private void Awake()
    {
        value = 1f;
    }

    // YH - on test
    public override void Init(float expAmount)
    {
        //value = expAmount;
        return;
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
