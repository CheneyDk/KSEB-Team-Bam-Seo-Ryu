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

    private bool isPowerWeapon;
    public GameObject subBullet;

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

    public void SetPytorchBullet(Vector2 fallPos, float explodeRange, bool power)
    {
        targetPos = fallPos;
        bulletExplodeRange = explodeRange;
        isPowerWeapon = power;
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

        if (enemies != null)
        {
            foreach (var enemy in enemies)
            {
                enemy.GetComponent<Enemy>().TakeDamage(bulletDamage);
                ScoreManager.instance.UpdateDamage("Pytorch", bulletDamage);
                // enemy.GetComponent<Enemy>().LastingDamage(bulletLastingDamage, 3, pytorchColor);
            }
        }
        if (isPowerWeapon) ClusterSpread();
    }

    private void ClusterSpread()
    {
        float angle = 0f;
        for (int i = 0; i < 4; i++)
        {
            angle = 90 * i;
            var tempBullet = Instantiate(subBullet, transform.position, Quaternion.Euler(0f, 0f, angle));
            tempBullet.GetComponent<PlayerBullet>().Init(bulletDamage / 4);
        }
    }

    private async UniTask WaitVFX()
    {
        var tempColor = spriteRenderer.color;
        tempColor.a = 0f;
        spriteRenderer.color = tempColor;
        particle.Play();
        await UniTask.WaitForSeconds(1f);
        Destroy(gameObject);
    }

    // dummy
    protected override void OnTriggerEnter2D(Collider2D collision)
    {

    }

    public override void ChangeSprite(Sprite powerWeapon)
    {
    }
}
