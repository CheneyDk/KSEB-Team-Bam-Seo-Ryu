using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBullet : MonoBehaviour
{
    // bullet stats / initialize needed
    
    protected float bulletDamage;
    public float bulletSpeed;

    protected Vector2 bulletVector;

    // delete
    protected float timeCounter = 0f;
    protected float bulletLifeTime = 5f;

    // YH - call Init func in Start func

    public void Init(float damage)
    {
        bulletDamage = damage;
    }

    // if stay triggered, give damage
    protected abstract void OnTriggerEnter2D(Collider2D collision);
}
