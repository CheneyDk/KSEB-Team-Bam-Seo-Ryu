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
    private float bulletRiseTime = 0.5f;

    // bullet fall vars
    private Vector2 bulletFallVector;
    private float bulletFallTime = 0.5f;
    private float bulletExplodeRange; // physics2d overlap circle needed

    private Vector2 targetPos;

    // lasting dmg vars
    private float bulletLastingDamage;
    private Color pytorchColor = new Color(238, 68, 34);

    // vfx
    public ParticleSystem particle;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        BulletOrbit().Forget();
    }

    private void Update()
    {
        transform.Translate(bulletVector * bulletSpeed * Time.deltaTime);
    }

    public void SetPytorchBullet(Vector2 fallPos, float explodeRange, float lastingDmg)
    {
        targetPos = fallPos;
        bulletExplodeRange = explodeRange;
        bulletLastingDamage = lastingDmg;
    }

    private async UniTask BulletOrbit()
    {
        BulletRise();
        await UniTask.WaitForSeconds(bulletRiseTime);
        BulletFall();
        await UniTask.WaitForSeconds(bulletFallTime);
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
        WaitVFX().Forget();

        if (enemies == null) return;
        
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(bulletDamage);
            // enemy.GetComponent<Enemy>().LastingDamage(bulletLastingDamage, 3, pytorchColor);
        }
    }

    private async UniTask WaitVFX()
    {
        var tempColor = spriteRenderer.color;
        tempColor.a = 0f;
        spriteRenderer.color = tempColor;
        particle.Play();
        await UniTask.WaitForSeconds(1f);
        // GameObject.Destroy(gameObject);
    }

    // dummy
    protected override void OnTriggerEnter2D(Collider2D collision)
    {

    }
}
