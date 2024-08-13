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

    [SerializeField] protected BulletPool bulletPool;

    protected bool isInited = false;

    // YH - call Init func in Start func

    public void Init(float damage, int crit, Vector3 pos, Quaternion rot, BulletPool pool)
    {
        bulletDamage = damage;
        critOccur = crit;
        transform.SetPositionAndRotation(pos, rot);
        bulletPool = pool;
        isInited = true;
    }

    // if stay triggered, give damage
    protected abstract void OnTriggerEnter2D(Collider2D collision);

    public abstract void ChangeSprite(Sprite powerWeapon);
}
