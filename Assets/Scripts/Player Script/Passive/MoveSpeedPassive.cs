using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpeedPassive : PlayerPassive
{
    // total amount that this passive raise
    public float totalValue;
    private float increaseRate;

    private void Awake()
    {
        passiveLevel = 1;
        increaseRate = 2f;
        totalValue = 0f;
    }

    private void OnEnable()
    {
        player.playerMoveSpeed += increaseRate;
        totalValue += increaseRate;
    }

    // just call this func including first add step
    public override void Upgrade()
    {
        if (isMaxLevel) return;

        passiveLevel++;
        player.playerMoveSpeed += increaseRate;
        totalValue += increaseRate;

        if (passiveLevel > 4) isMaxLevel = true;
    }
}
