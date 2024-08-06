using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShieldWeapon : PlayerWeapon
{
    public float fireRate = 2f;
    public float rotationSpeed = 100f;
    public float damage;

    public float shieldTime = 5f;

    private Animator shieldAnimator;

    private BoxCollider2D colider;

    public AudioSource audioSource;
    public AudioClip audioClip;

    void Start()
    {
        shieldAnimator = GetComponent<Animator>();
        colider = GetComponent<BoxCollider2D>();
        weaponLevel = 1;
        weaponDamageRate = 1f;
        damage = player.playerAtk * weaponDamageRate;
        isMaxLevel = false;
        isPowerWeapon = false;
        matchPassive = "Cooler";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Enemy"))
        {
            var EnemyComponent = collision.GetComponent<Enemy>();
            if (EnemyComponent != null)
            {
                EnemyComponent.TakeDamage(damage);

                if (!isPowerWeapon)
                {
                    shieldAnimator.Play("Shield Hit");
                    audioSource.PlayOneShot(audioClip);
                    StartCoroutine(WaitForShield(shieldTime));
                }
                else if (isPowerWeapon)
                {
                    shieldAnimator.Play("Power Shield Hit");
                    audioSource.PlayOneShot(audioClip);
                    StartCoroutine(WaitForShield(shieldTime/2f));
                }
            }
        }
        else if (collision != null && collision.CompareTag("Enemy Bullet"))
        {
            if (!isPowerWeapon)
            {
                shieldAnimator.Play("Shield Hit");
                StartCoroutine(WaitForShield(shieldTime));
            }
            else if (isPowerWeapon)
            {
                shieldAnimator.Play("Power Shield Hit");
                StartCoroutine(WaitForShield(shieldTime / 2f));
            }
        }
    }

    public void ShieldHit()
    {
        colider.enabled = false;
    }

    private IEnumerator WaitForShield(float shieldTime)
    {
        yield return new WaitForSeconds(shieldTime);
        ShieldOpen();
    }

    public void ShieldOpen()
    {
        colider.enabled = true;
        if (!isPowerWeapon)
        {
            shieldAnimator.Play("Shield");
        }
        else if (isPowerWeapon)
        {
            shieldAnimator.Play("Power Shield");
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
        }
        if (weaponLevel > 4)
        {
            isMaxLevel = true;
        }
    }


    public override void Fire(InputAction.CallbackContext context) { }
}
