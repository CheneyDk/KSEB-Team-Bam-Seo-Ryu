using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerPassive : MonoBehaviour
{
    protected int passiveLevel;
    public bool isMaxLevel = false;
    protected Player player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public abstract void Upgrade();
}
