using Cysharp.Threading.Tasks;
using DamageNumbersPro;
// using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SnakeBody : SnakePart
{
    public GameObject bullet;

    private float waittimeForFirstFire = 10f;
    private float snakeFireRate = 8f;
    private float fireRateRamdom;



    private CancellationTokenSource cancelFire;

    private void Start()
    {
        snakePartMaxHp = 200f;
        snakePartCurHp = snakePartMaxHp;
        FireCycle().Forget();
    }

    private async UniTask FireCycle()
    {
        cancelFire = new CancellationTokenSource();
        await UniTask.WaitForSeconds(waittimeForFirstFire);

        while (!cancelFire.IsCancellationRequested)
        {
            fireRateRamdom = UnityEngine.Random.Range(-0.5f, 0.5f);
            SnakeBodyGunFire();
            await UniTask.WaitForSeconds(snakeFireRate + fireRateRamdom, cancellationToken: cancelFire.Token);
            if (snakeMain.isDead) return;
        }
    }

    // gun fire
    private void SnakeBodyGunFire()
    {
        var tempBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        tempBullet.GetComponent<BulletLogicError>().Init(player.transform.position);
    }

    // abstract override
    public override void TakeDamage(float damage, int critOccur)
    {
        hitParticle.Play();

        damage *= (1f + elixirAdditionalDamageRate);

        if (isDestroyed) damage /= 2f;

        if (critOccur == 1)
        {
            critDamageNumber.Spawn(transform.position, damage);
        }
        else
        {
            damageNumber.Spawn(transform.position, damage);
        }


        snakePartCurHp -= damage;
        if (elixirAdditionalDamageRate > 0)
        {
            SaveManager.instance.UpdateDamage("Elixir", damage * elixirAdditionalDamageRate);
        }
        snakeMain.TakeDamage(damage, -1); // just passing damage value, not actual take damage part
        if (snakePartCurHp <= 0)
        {
            PartDestroyed();
        }
    }

    public override IEnumerator LastingDamage(float damage, int totalDamageTime, Color color)
    {
        curSR.color = color;
        var damageTimer = 0f;

        while (damageTimer < totalDamageTime)
        {
            yield return new WaitForSeconds(1f);
            hitParticle.Play();
            lastingDamageNumber.Spawn(transform.position, damage);
            snakeMain.TakeDamage(damage, -1);
            damageTimer += 1f;

            SaveManager.instance.UpdateDamage("React", damage);
        }

        if (snakePartCurHp <= 0)
        {
            PartDestroyed();
        }
        curSR.color = originColor;
    }

    private void PartDestroyed()
    {
        // snakePartCollider.enabled = false; - ?
        isDestroyed = true;
        cancelFire.Cancel();

        // play animation
        // sprite change
    }

    private void OnDestroy()
    {
        if (isDestroyed) return; // token already used
        cancelFire.Cancel();
    }

    // gof would mad at me
    public override void DropEXP(int itemNumber) {}
    public override void EnemyMovement() {}
    public override void ResetEnemy() {}
}
