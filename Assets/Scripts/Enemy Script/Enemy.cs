using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public abstract void EnemyMovement();
    public abstract void TakeDamage(float damage);

    public void TakeLastingDamage(float damage, int totalDamageTime, Color color)
    {

        StartCoroutine(LastingDamage(damage, totalDamageTime, color));
    }

    public abstract IEnumerator LastingDamage(float damage, int totalDamageTime, Color color);

    public abstract void DropEXP(int iteamNumber);

    public abstract void ResetEnemy();
}
