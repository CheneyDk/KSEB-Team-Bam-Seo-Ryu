using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected float elixirLastingTime;
    protected float elixirAdditionalDamageRate = 0f;

    public abstract void EnemyMovement();
    public abstract void TakeDamage(float damage);

    public void TakeLastingDamage(float damage, int totalDamageTime, Color color)
    {

        StartCoroutine(LastingDamage(damage, totalDamageTime, color));
    }

    public abstract IEnumerator LastingDamage(float damage, int totalDamageTime, Color color);

    public abstract void DropEXP(int iteamNumber);

    public abstract void ResetEnemy();

    public async UniTask ActivateElixirDebuff(float lastingTime, float additionalDamageRate)
    {
        elixirAdditionalDamageRate = additionalDamageRate;
        elixirLastingTime = lastingTime;

        await UniTask.WaitForSeconds(elixirLastingTime);
        elixirAdditionalDamageRate = 0f;
    }
}
