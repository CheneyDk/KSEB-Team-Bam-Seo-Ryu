using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwiftWeapon : PlayerWeapon
{
    public float fireRate = 3.5f;

    void Start()
    {
        Fire();
        weaponLevel = 1;
        weaponDamageRate = 1f;
        isMaxLevel = false;
    }

    IEnumerator FireBullet()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);
            var addBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            addBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate);
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
        else if (weaponLevel == 5)
        {
            isMaxLevel = true;
            return;
        }
    }


    public override void Fire(InputAction.CallbackContext context) { }
}
