using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    // item values
    protected float value;
    
    // magnetic system
    private float itemMagneticRange;
    private float itemMagneticMoveSpeed = 30f;

    private Transform playerTrans;

    private void Start()
    {
        // Init
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        itemMagneticRange = 5f; // YH - test
    }

    private void Update()
    {
        ItemMagnetic();
    }


    // collision with player
    protected abstract void OnTriggerEnter2D(Collider2D collision);
    // Initialize Exp only.
    public abstract void Init(float expAmount);
    // Hp potion and Red Bool have their' own const value.
    // Init on Awake and make this func null return or something.


    public void SetMagneticRange(float range)
    {
        itemMagneticRange = range;
    }


    private void ItemMagnetic()
    {
        // distance calculate
        var distance = Vector3.Distance(transform.position, playerTrans.position);

        if (distance < itemMagneticRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTrans.position, itemMagneticMoveSpeed * Time.deltaTime);
        } 
    }
}
