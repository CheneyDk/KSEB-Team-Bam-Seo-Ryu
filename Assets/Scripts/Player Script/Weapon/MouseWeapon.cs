using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseWeapon : PlayerWeapon
{
    public float damage;

    public Material material;

    public AudioSource audioSource;
    public AudioClip clip;

    private int critOccur;
    private float critDamage;

    void Start()
    {
        weaponLevel = 1;
        weaponDamageRate = 0.2f;
        damage = player.playerAtk * weaponDamageRate;
        isMaxLevel = false;
        isPowerWeapon = false;
        matchPassive = "BlueTooth";
    }

    private void Update()
    {
        if (!isPowerWeapon)
        {
            material.SetColor("_Color",Color.white);
        }
        else if (isPowerWeapon)
        {
            material.SetColor("_Color", Color.yellow);
        }
        var mousePos = MouseAim();
        transform.position = mousePos;
    }

    private Vector2 MouseAim()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        return new Vector2(worldMousePosition.x, worldMousePosition.y);
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
                if (!isPowerWeapon)
                {
                    EnemyComponent.TakeDamage(damage * (1f + critDamage), critOccur);
                    audioSource.PlayOneShot(clip);
                    SaveManager.instance.UpdateDamage("Mouse", damage);
                }
                else if (isPowerWeapon)
                {
                    EnemyComponent.TakeDamage(player.playerAtk * (1f + critDamage), critOccur);
                    audioSource.PlayOneShot(clip);
                    SaveManager.instance.UpdateDamage("Mouse", player.playerAtk);
                }
                
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
            damage += 1f;
        }
        if (weaponLevel > 4)
        {
            isMaxLevel = true;
        }
    }


    public override void Fire(InputAction.CallbackContext context) { }
}
