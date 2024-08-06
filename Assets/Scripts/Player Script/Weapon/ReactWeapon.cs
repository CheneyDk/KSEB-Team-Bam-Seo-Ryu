using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ReactWeapon : PlayerWeapon
{
    public float fireRate = 5f;

    void Start()
    {
        Fire();
        weaponLevel = 1;
        weaponDamageRate = 0.5f;
        isMaxLevel = false;
        isPowerWeapon = false;
        matchPassive = "CPU";
    }

    IEnumerator FireBullet()
    {
        while (true)
        {
            if (!isPowerWeapon)
            {
                bullet.GetComponent<PlayerBullet>().ChangeSprite(normalWeaponSprite);
                yield return new WaitForSeconds(fireRate);
                var addBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                addBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate);

            }
            else if (isPowerWeapon)
            {
                bullet.GetComponent<PlayerBullet>().ChangeSprite(powerWeaponSprite);
                yield return new WaitForSeconds(fireRate/2f);
                var addBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                addBullet.GetComponent<PlayerBullet>().Init(player.playerAtk * weaponDamageRate);
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
            weaponDamageRate += 0.1f;
        }
        if (weaponLevel > 4)
        {
            isMaxLevel = true;
        }
    }


    public override void Fire(InputAction.CallbackContext context) { }
}
