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
    private float bulletCurFloatingTime;
    private float parabolaHeight;

    private float bulletRoateSpeed;
    private Vector3 bulletRotateVec;

    private float explodeRange;
    private float damageAddRate;
    private float lastingTime;

    private bool isPowerWeapon;
    private bool isNotBounced;

    private bool isDestroyed;
    private bool isSetted;
    private bool stopRotate;

    public Sprite PowerBullet;
    private SpriteRenderer spriteRenderer;
    public ParticleSystem normalParticle;
    public ParticleSystem powerParticle;
    private Vector3 particleScale;
    private Color originColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originColor = new(1f, 1f, 1f, 1f);

        bulletRotateVec = new(0f, 0f, 1f);

        bulletSpeed = 1f;
        bulletRoateSpeed = 2f;
        parabolaHeight = 3f;
        bulletFloatingTime = 1f; // 1sec
    }

    private void OnEnable()
    {
        isDestroyed = false;
        isSetted = false;
        stopRotate = false;
        isNotBounced = true;
        bulletCurFloatingTime = bulletFloatingTime;

        WaitForInit().Forget();

        ParabolaYFactor().Forget();
        BulletRotate().Forget();
        BottleExplode().Forget();
    }

    private async UniTask WaitForInit()
    {
        await UniTask.WaitUntil(() => isSetted);

        PowerSprite();
        spriteRenderer.color = originColor;
    }

    void Start()
    {
        particleScale = new(explodeRange * 2f / 5f, explodeRange * 2f / 5f, explodeRange * 2f / 5f);
        normalParticle.transform.localScale = particleScale;
        powerParticle.transform.localScale = particleScale; 
    }

    void Update()
    {
        gameObject.transform.Translate((bulletVector + parabolaVector) * bulletSpeed * Time.deltaTime, Space.World);
    }

    private async UniTask ParabolaYFactor()
    {
        parabolaYTimer = 0f;
        while (parabolaYTimer < bulletCurFloatingTime)
        {
            await UniTask.Yield();
            if (isDestroyed) return;
            parabolaYTimer += Time.deltaTime;
            parabolaY = Mathf.Cos(parabolaYTimer * pi / bulletCurFloatingTime);
            parabolaVector = parabolaHeight * new Vector2(0f, parabolaY);
        }
        parabolaYTimer = bulletCurFloatingTime;
        parabolaVector = Vector2.zero;
    }

    private async UniTask BulletRotate()
    {
        while (true)
        {
            await UniTask.Yield();
            await UniTask.WaitUntil(() => GameManager.Instance.isGameContinue);
            if (isDestroyed) return;
            if (stopRotate) return;
            transform.Rotate(Vector3.forward, bulletRoateSpeed);
        }
    }

    private async UniTask BottleExplode()
    {
        await UniTask.WaitUntil(() => parabolaYTimer == bulletCurFloatingTime);
        if(isDestroyed) return;

        var enemies = Physics2D.OverlapCircleAll(transform.position, explodeRange, 1 << 8);
        
        // particle play
        ParticleInstantiate().Forget();

        if (enemies.Length > 0)
        {
            foreach (var enemy in enemies)
            {
                enemy.GetComponent<Enemy>().ActivateElixirDebuff(lastingTime, damageAddRate, isPowerWeapon).Forget();
            }
        }

        // Bounce
        if (isPowerWeapon && isNotBounced)
        {
            //bulletVector *= 0.7f;
            //parabolaHeight *= 0.7f;
            bulletCurFloatingTime *= 0.5f;

            isNotBounced = false;
            ParabolaYFactor().Forget();
            await BottleExplode();
        }

        DelayedDestroy().Forget();
    }

    private async UniTask ParticleInstantiate()
    {
        if (!(isPowerWeapon && isNotBounced))
        {
            bulletVector = Vector3.zero;
            stopRotate = true; // to stop rotation

            var tempColor = spriteRenderer.color;
            tempColor.a = 0;
            spriteRenderer.color = tempColor;
        }

        

        ParticleSystem tempParticle;
        if (isPowerWeapon)
        {
            tempParticle = Instantiate(powerParticle, transform.position, Quaternion.Euler(90f, 0f, 0f));
        }
        else
        {
            tempParticle = Instantiate(normalParticle, transform.position, Quaternion.Euler(90f, 0f, 0f));
        }
        await UniTask.WaitForSeconds(1f);
        if (isDestroyed) return;
        // Destroy(tempParticle.gameObject);
    }

    private async UniTask DelayedDestroy()
    {
        await UniTask.WaitForSeconds(1f);
        if (isDestroyed) return;
        bulletPool.SetObj(this);
    }

    public void SetElixir(Vector3 vec, float damageRate, float explodeRng, float lasting, bool power)
    {
        bulletVector = vec;
        explodeRange = explodeRng;
        damageAddRate = damageRate;
        lastingTime = lasting;
        isPowerWeapon = power;

        // speed distance calc - only 1 sec takes to reach out target position.
        // but it can change in 2 or 3 sec
        bulletSpeed = bulletVector.magnitude;
        bulletVector.Normalize();

        isSetted = true;
    }

    private void PowerSprite()
    {
        if (isPowerWeapon)
        {
            spriteRenderer.sprite = PowerBullet;
        }
    }

    private void OnDisable()
    {
        isDestroyed = true;
    }

    private void OnDestroy()
    {
        isDestroyed = true;
    }


    protected override void OnTriggerEnter2D(Collider2D collision) { }
    public override void ChangeSprite(Sprite powerWeapon){}
}
