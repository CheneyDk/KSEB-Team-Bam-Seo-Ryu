using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class PlayerBullet : MonoBehaviour
{
    // bullet stats / initialize needed
    
    protected float bulletDamage;
    protected int critOccur;
    public float bulletSpeed;

    protected Vector2 bulletVector;

    // delete
    protected float timeCounter = 0f;
    protected float bulletLifeTime = 5f;

    // YH - call Init func in Start func

    public void Init(/*Vector3 pos, Quaternion rot,*/float damage, int crit)
    {
        //transform.position = pos;
        //transform.rotation = rot;
        bulletDamage = damage;
        critOccur = crit;
    }

    // if stay triggered, give damage
    protected abstract void OnTriggerEnter2D(Collider2D collision);

    public abstract void ChangeSprite(Sprite powerWeapon);
}
