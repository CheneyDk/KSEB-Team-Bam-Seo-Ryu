using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : Item
{
    public ParticleSystem healParticle;

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
            Instantiate(healParticle, transform.position, Quaternion.Euler(-90f,0,0));
            ItemPooling.Instance.ReturnApple(gameObject);
        }
    }
}
