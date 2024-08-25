using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeStateManager : Enemy
{
    private int numberToPool = 20;

    EnemyState currentState;

    public RangeRunState rangeRunState = new RangeRunState();
    public RangeFireState rangeFireState = new RangeFireState();

    void Start()
    {
        currentState = rangeRunState;

        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateSate(this);
    }

    public void SwitchState(EnemyState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    private void PutBulletsToPool(List<GameObject> poolList, GameObject enemyPrefab)
    {
        for (int i = 0; i < numberToPool; i++)
        {
            var tmp = Instantiate(enemyPrefab);
            tmp.transform.parent = GameObject.FindGameObjectWithTag("Enemy Bullet Pool").transform;
            tmp.SetActive(false);
            poolList.Add(tmp);
        }
    }

    // Get bullet from pooling list
    public GameObject GetPooledEnemies(List<GameObject> poolList)
    {
        for (int i = 0; i < poolList.Count; i++)
        {
            if (!poolList[i].activeInHierarchy)
            {
                return poolList[i];
            }
        }
        return null;
    }

    public override void EnemyMovement()
    {
    }

    public override void TakeDamage(float damage, int critOccur)
    {
    }

    public override IEnumerator LastingDamage(float damage, int totalDamageTime, Color color)
    {
        throw new System.NotImplementedException();
    }

    public override void DropEXP(int iteamNumber)
    {
    }

    public override void ResetEnemy()
    {
    }
}
