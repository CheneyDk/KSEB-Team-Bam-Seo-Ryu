using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponUnity : PlayerWeapon
{
    public float fireRate = 1f;

    void Start()
    {
        Fire();
        bulletNum = 1;
        weaponLevel = 1;
        weaponDamageRate = 1f;
        isMaxLevel = false;
    }

    IEnumerator FireBullet()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);
            for (int i = 0; i < bulletNum; i++)
            {
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
            bulletNum++;
        }
        else if (weaponLevel == 5) 
        { 
            isMaxLevel = true;
        }
    }


    public override void Fire(InputAction.CallbackContext context) { }
}
