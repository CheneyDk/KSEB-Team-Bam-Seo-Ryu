using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PytorchBullet : PlayerBullet
{
    // bullet rise distance = 5f?
    private float bulletRiseTime;
    private float bulletRiseSpeed; // slower and stop
    private float bulletRiseDistance;

    private float bulletFallTime;
    private float bulletFallSpeed; // stop to faster

    private Vector2 bulletFallPos; // inited by weapon
    private float bulletExplodeRange; // physics2d overlap circle needed

    private void Start()
    {
        bulletRiseTime = 0.5f;
        bulletFallTime = 0.5f;

        bulletRiseDistance = 30f;
        
    }

    public void SetPytorchBullet(Vector2 fallPos, float explodeRange)
    {
        bulletFallPos = fallPos;
        bulletExplodeRange = explodeRange;

    }

    private void BulletExplode()
    {
        // time delayed explode
        // physics2d
    }

    // dummy
    protected override void OnTriggerEnter2D(Collider2D collision)
    {

    }
}
