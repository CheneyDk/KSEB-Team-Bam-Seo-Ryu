using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public abstract void EnemyMovement();
    public abstract void TakeDamage(float damage);

    public abstract void Drop(int iteamNumber);
}
