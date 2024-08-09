using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CloudWeapon : PlayerWeapon
{
    public float fireRate = 3.5f;

    public AudioSource audioSource;
    public AudioClip audioClip;

    void Start()
    {
        Fire();
        weaponLevel = 1;
        weaponDamageRate = 1f;
        isMaxLevel = false;
        isPowerWeapon = false;
        matchPassive = "Wifi";
    }

    IEnumerator FireBullet()
    {
        int critOccur;
        float critDamage;

        while (true)
        {
            critOccur = IsCritOccur(player.playerCritPer);
            critDamage = player.playerCritDmg * critOccur;
            if (!isPowerWeapon)
            {
                bullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage), critOccur,
                    Vector3.zero, Quaternion.identity, bulletPool);
                bullet.GetComponent<CloudBullet>().GiveDamagea();
                audioSource.PlayOneShot(audioClip);
                yield return new WaitForSeconds(fireRate);
            }
            else if (isPowerWeapon)
            {
                bullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate * (1f + critDamage), critOccur,
                    Vector3.zero, Quaternion.identity, bulletPool);
                bullet.GetComponent<CloudBullet>().PowerWeapoonDamage();
                audioSource.PlayOneShot(audioClip);
                yield return new WaitForSeconds(2f);
            }
        }
    }

    protected override void Fire()
    {
        StartCoroutine(FireBullet());
    }


    public override void Upgrade()
    {
        if (weaponLevel < 5)
        {
            weaponLevel++;
            fireRate -= 0.5f;
        }
        if (weaponLevel > 4)
        {
            isMaxLevel = true;
        }
    }


    public override void Fire(InputAction.CallbackContext context) { }
}
