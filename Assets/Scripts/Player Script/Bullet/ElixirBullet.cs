using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ElixirBullet : PlayerBullet
{
    private Vector2 parabolaVector;
    private float parabolaY;
    private float parabolaSpeed;
    private float parabolaYTimer;
    private const float pi = Mathf.PI;

    private float bulletFloatingTime;
    private float parabolaHeight;

    private float bulletRoateSpeed;
    private Vector3 bulletRotateVec;

    private float explodeRange;
    private float damageAddRate;
    private float lastingTime;

    private bool isDestroyed;
    
    void Start()
    {
        isDestroyed = false;
        bulletRoateSpeed = 1f;
        parabolaYTimer = 0f;
        parabolaHeight = 4f;
        bulletSpeed = 0f;
        bulletFloatingTime = 2f; // 2sec

        bulletRotateVec = new(0f, 0f, 1f);

        ParabolaYFactor().Forget();
        BulletRotate().Forget();
    }



    void Update()
    {
        gameObject.transform.Translate((bulletVector + parabolaVector) * Time.deltaTime);
    }

    private async UniTask ParabolaYFactor()
    {
        while (parabolaYTimer < bulletFloatingTime)
        {
            await UniTask.Yield();
            if (isDestroyed) return;
            parabolaYTimer += Time.deltaTime;
            parabolaY = Mathf.Cos(parabolaYTimer * pi / bulletFloatingTime);
            parabolaVector = parabolaHeight * new Vector2(0f, parabolaY);
        }
        parabolaYTimer = bulletFloatingTime;
    }

    private async UniTask BulletRotate()
    {
        while (true)
        {
            await UniTask.Yield();
            if (isDestroyed) return;
            gameObject.transform.Rotate(bulletRotateVec * bulletRoateSpeed * Time.deltaTime);
        }
    }

    private async UniTask BottleExplode()
    {
        await UniTask.WaitUntil(() => parabolaYTimer == bulletFloatingTime);

        var enemies = Physics2D.OverlapCircleAll(transform.position, explodeRange, 1 << 8);
        if (enemies.Length < 1) return;

        foreach(var enemy in enemies)
        {
            // enemy.gameObject.GetComponent<>();
        }
    }

    public void SetVec(Vector3 vec, float damageRate, float explodeRng, float lasting)
    {
        bulletVector = vec;
        explodeRange = explodeRng;
        damageAddRate = damageRate;
        lastingTime = lasting;
    }
    
    private void OnDestroy()
    {
        isDestroyed = true;
    }


    protected override void OnTriggerEnter2D(Collider2D collision) { }
    public override void ChangeSprite(Sprite powerWeapon){}
}
