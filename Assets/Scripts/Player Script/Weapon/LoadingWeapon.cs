using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

public class LodingWeapon : PlayerWeapon
{
    public float fireRate = 2f;
    public float rotationSpeed = 100f;
    public float damage;

    void Start()
    {
        weaponLevel = 1;
        weaponDamageRate = 1f;
        damage = player.playerAtk * weaponDamageRate;
        isMaxLevel = false;
    }

    private void Update()
    {
        Rotation();
    }

    void Rotation()
    {
        transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime);

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
                EnemyComponent.TakeDamage(damage);

                ScoreManager.instance.UpdateDamage("Laoding", damage);
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
            transform.localScale += new Vector3(0.5f, 0.5f, 0f);
        }
        if (weaponLevel > 4)
        {
            isMaxLevel = true;
        }
    }


    public override void Fire(InputAction.CallbackContext context) { }
}