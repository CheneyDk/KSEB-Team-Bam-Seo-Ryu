using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected float elixirLastingTime;
    protected float elixirAdditionalDamageRate = 0f;
    protected bool isDestroyed = false;
    

    public interface IEnemyState
    {
        void EnterState();
        void UpdateState();
        void ExitState();
    }

    public class StateMachine
    {
        private IEnemyState currentState;

        public void SetState(IEnemyState newState)
        {
            currentState?.ExitState();
            currentState = newState;
            currentState.EnterState();
        }

        public void Update()
        {
            currentState?.UpdateState();
        }
    }



    public abstract void EnemyMovement();
    public abstract void TakeDamage(float damage, int critOccur);

    public void TakeLastingDamage(float damage, int totalDamageTime, Color color)
    {

        StartCoroutine(LastingDamage(damage, totalDamageTime, color));
    }

    public abstract IEnumerator LastingDamage(float damage, int totalDamageTime, Color color);

    public abstract void DropEXP(int iteamNumber);

    public abstract void ResetEnemy();

    public async UniTask ActivateElixirDebuff(float lastingTime, float additionalDamageRate, bool isPower)
    {
        elixirAdditionalDamageRate = additionalDamageRate;
        elixirLastingTime = lastingTime;

        var tempSp = gameObject.GetComponent<SpriteRenderer>();
        if (isPower)
        {
            tempSp.color = new(1f, 0.2f, 0.2f);
        }
        else
        {
            tempSp.color = new(0.35f, 0f, 1f);
        }
        gameObject.GetComponent<SpriteRenderer>().color = tempSp.color;

        await UniTask.WaitForSeconds(elixirLastingTime);
        if (isDestroyed) return;
        elixirAdditionalDamageRate = 0f;
        tempSp.color = new(1f, 1f, 1f);
        gameObject.GetComponent<SpriteRenderer>().color = tempSp.color;
    }

    private void OnDestroy()
    {
        isDestroyed = true;
    }
}
