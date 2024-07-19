using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkSpeedPassive : PlayerPassive
{
    // total amount that this passive raise
    public float totalValue;
    private float increaseRate;
    private void OnEnable()
    {
        passiveLevel = 1;
        increaseRate = 1f;
        totalValue = 0f;
    }

    private void Start()
    {
        player.playerAtkSpeed += increaseRate;
        totalValue += increaseRate;
    }

    // just call this func including first add step
    public override void Upgrade()
    {
        if (isMaxLevel) return;

        passiveLevel++;
        player.playerAtkSpeed += increaseRate;
        totalValue += increaseRate;

        if (passiveLevel > 4) isMaxLevel = true;
    }
}
