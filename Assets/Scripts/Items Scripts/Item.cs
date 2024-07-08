using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    protected float value;

    // collision with player
    protected abstract void OnTriggerEnter2D(Collider2D collision);
    // Initialize Exp only.
    public abstract void Init(float expAmount);
    // Hp potion and Red Bool have their' own const value.
    // Init on Awake and make this func null return or something.
}
