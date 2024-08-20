using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PytorchBullet : PlayerBullet
{
    // bullet rise vars
    private float bulletTimer;
    private Vector2 bulletRiseVector = Vector2.up;
    private float bulletInitSpeed = 100f;
    private float bulletRiseTime;

    // bullet fall vars
    private Vector2 bulletFallVector;
    private float bulletFallTime;
    private float bulletExplodeRange; // physics2d overlap circle needed

    private Vector2 targetPos;

    private bool isPowerWeapon;
    public GameObject subBullet;

    public BulletPool subBulletPool;

    private bool isDestroyed;

    // vfx
    public ParticleSystem particle;
    private SpriteRenderer spriteRenderer;
    private Color originColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originColor = spriteRenderer.color;
    }

    private void OnEnable()
    {
        bulletRiseTime = 0.5f;
        bulletFallTime = 0.5f;
        isDestroyed = false;
        spriteRenderer.color = originColor;
        BulletOrbit().Forget();
    }

    private void Update()
    {
        transform.Translate(bulletVector * bulletSpeed * Time.deltaTime);
    }

    public void SetPytorchBullet(Vector2 fallPos, float explodeRange, bool power, BulletPool pool)
    {
        targetPos = fallPos;
        bulletExplodeRange = explodeRange;
        isPowerWeapon = power;
        subBulletPool = pool;
    }

    private async UniTask BulletOrbit()
    {
        BulletRise();
        await UniTask.WaitForSeconds(bulletRiseTime);
        if(isDestroyed) return;
        BulletFall();
        await UniTask.WaitForSeconds(bulletFallTime);
        if (isDestroyed) return;
        BulletExplode();
    }

    private void BulletRise()
    {
        bulletVector = bulletRiseVector;
        bulletSpeed = bulletInitSpeed;
    }

    private void BulletFall()
    {
        bulletVector = Vector2.zero;

        var tempVector = new Vector2(transform.position.x, transform.position.y);
        bulletFallVector = targetPos - tempVector;
        var distance = Mathf.Sqrt(Mathf.Pow(bulletFallVector.x, 2) + Mathf.Pow(bulletFallVector.y, 2));

        bulletVector = bulletFallVector.normalized;
        bulletSpeed = distance / bulletFallTime;
    }

    private void BulletExplode()
    {
        // time delayed explode
        // physics2d
        bulletVector = Vector2.zero;
        var enemies = Physics2D.OverlapCircleAll(transform.position, bulletExplodeRange, 1 << 8);

        // effects needed
        WaitVFXandDelayedDestroy().Forget();
        if (isPowerWeapon) ClusterSpread();

        if(isDestroyed) return;
        if (enemies != null)
        {
            foreach (var enemy in enemies)
            {
                enemy.GetComponent<Enemy>().TakeDamage(bulletDamage, critOccur);
                SaveManager.instance.UpdateDamage("Pytorch", bulletDamage);
                // enemy.GetComponent<Enemy>().LastingDamage(bulletLastingDamage, 3, pytorchColor);
            }
        }
    }

    private void ClusterSpread()
    {
        float angle = 0f;
        for (int i = 0; i < 4; i++)
        {
            angle = 90 * i;
            var tempBullet = subBulletPool.GetBullet();
            // var tempBullet = Instantiate(subBullet, transform.position, Quaternion.Euler(0f, 0f, angle));
            tempBullet.GetComponent<PlayerBullet>().Init(bulletDamage / 4, critOccur,
                transform.position, Quaternion.Euler(0f, 0f, angle), subBulletPool);
        }
    }

    private async UniTask WaitVFXandDelayedDestroy()
    {
        var tempColor = spriteRenderer.color;
        tempColor.a = 0f;
        spriteRenderer.color = tempColor;
        particle.Play();
        await UniTask.WaitForSeconds(1f);
        if (isDestroyed) return;
        isDestroyed = true;
        bulletPool.SetObj(this);
    }

    private void OnDisable()
    {
        isDestroyed = true;
    }

    private void OnDestroy()
    {
        isDestroyed = true;
    }

    // dummy
    protected override void OnTriggerEnter2D(Collider2D collision)
    {

    }

    public override void ChangeSprite(Sprite powerWeapon)
    {
    }
}
