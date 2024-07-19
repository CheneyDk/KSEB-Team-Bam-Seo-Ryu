using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritDmgPassive : PlayerPassive
{
    // total amount that this passive raise
    public float totalValue;
    private float increaseRate;
    private void OnEnable()
    {
        passiveLevel = 1;
        increaseRate = 0.6f;
        totalValue = 0f;
    }

    private void Start()
    {
        player.playerCritDmg += increaseRate;
        totalValue += increaseRate;
    }

    // just call this func including first add step
    public override void Upgrade()
    {
        if (isMaxLevel) return;

        passiveLevel++;
        player.playerCritDmg += increaseRate;
        totalValue += increaseRate;

        if (passiveLevel > 4) isMaxLevel = true;
    }
}
