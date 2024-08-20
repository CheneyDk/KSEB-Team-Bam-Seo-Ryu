using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

public class LoadingWeapon : PlayerWeapon
{
    public float fireRate = 2f;
    public float rotationSpeed = 100f;
    public float damage;

    private SpriteRenderer sprite;

    public AudioSource audioSource;
    public AudioClip audioClip;

    private int critOccur;
    private float critDamage;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        weaponLevel = 1;
        weaponDamageRate = 1f;
        damage = player.playerAtk * weaponDamageRate;
        isMaxLevel = false;
        isPowerWeapon = false;
        matchPassive = "Cooler";
    }

    private void Update()
    {
        Rotation();
    }

    void Rotation()
    {
        if(!isPowerWeapon)
        {
            transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime);
            sprite.color = Color.white;
        }
        else if (isPowerWeapon)
        {
            transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime * 2);
            sprite.color = Color.red;
        }

        if (transform.rotation.eulerAngles.z <= -360f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Enemy"))
        {
            var EnemyComponent = collision.GetComponent<Enemy>();
            if (EnemyComponent != null)
            {
                critOccur = IsCritOccur(player.playerCritPer);
                critDamage = player.playerCritDmg * critOccur;

                EnemyComponent.TakeDamage(damage * (1f + critDamage), critOccur);
                audioSource.PlayOneShot(audioClip);

                SaveManager.instance.UpdateDamage("Loading", damage);
            }
        }
    }

    protected override void Fire()
    {
    }

    public override void Upgrade()
    {
        if (weaponLevel < 5)
        {
            weaponLevel++;
            weaponDamageRate += 0.5f;
            damage = player.playerAtk * weaponDamageRate;
            transform.localScale += new Vector3(0.5f, 0.5f, 0f);
        }
        if (weaponLevel > 4)
        {
            isMaxLevel = true;
        }
    }


    public override void Fire(InputAction.CallbackContext context) { }
}