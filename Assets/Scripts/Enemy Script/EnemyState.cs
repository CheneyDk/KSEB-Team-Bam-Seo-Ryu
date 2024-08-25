using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class EnemyState
{
    public abstract void EnterState(Enemy enemy);

    public abstract void UpdateSate(Enemy enemy);

    public abstract void ExitState(Enemy enemy);
}
