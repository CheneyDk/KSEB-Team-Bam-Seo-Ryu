using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    // weapon stats
    public float weaponDamageRate;
    public float weaponBulletSpeed;

    // muzzle position
    public Transform muzzle;


    // weapon's bullet
    public GameObject bullet;
    private Transform bulletRot; // temp

    
    // YH - flag
    // Weapon class Init func
    // called by Player - Awake(or Start)
    //public void Init(Transform muzzlePos)
    //{
    //    muzzle = muzzlePos;
    //}

    public void Fire(InputAction.CallbackContext context)
    {
        // when click
        if (context.started)
        {
            var tempBullet = Instantiate(bullet, muzzle.position, muzzle.rotation);
            // YH - flag: need bullet Init func
        }

    }
}
