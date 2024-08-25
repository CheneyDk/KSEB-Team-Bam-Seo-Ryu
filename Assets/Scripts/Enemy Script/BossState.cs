using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState
{
    public Action OnEnter;
    public Action Onupdate;
    public Action OnExit;

    public BossState(Action onEnter, Action onupdate, Action onExit)
    {
        OnEnter = onEnter;
        Onupdate = onupdate;
        OnExit = onExit;
    }
}
