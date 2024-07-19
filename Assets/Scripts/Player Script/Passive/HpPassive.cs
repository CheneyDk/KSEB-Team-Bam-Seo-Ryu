using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpPassive : PlayerPassive
{
    // total amount that this passive raise
    public float totalValue;
    private float increaseRate;
    private void OnEnable()
    {
        passiveLevel = 1;
        increaseRate = 20f;
        totalValue = 0f;
    }

    private void Start()
    {
        player.playerMaxHp += increaseRate;
        player.playerCurHp += increaseRate;
        totalValue += increaseRate;
    }

    // just call this func including first add step
    public override void Upgrade()
    {
        if (isMaxLevel) return;

        passiveLevel++;
        player.playerMaxHp += increaseRate;
        player.playerCurHp += increaseRate;
        totalValue += increaseRate;

        if (passiveLevel > 4) isMaxLevel = true;
    }
}
