using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ReactWeapon : PlayerWeapon
{
    public float fireRate = 2f;

    void Start()
    {
        Fire();
        weaponLevel = 1;
        weaponDamageRate = 0.5f;
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
            weaponDamageRate += 0.1f;
        }
        else if (weaponLevel == 5)
        {
            isMaxLevel = true;
            return;
        }
    }


    public override void Fire(InputAction.CallbackContext context) { }
}
