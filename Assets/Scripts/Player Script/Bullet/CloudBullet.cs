using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CloudBullet : PlayerBullet
{
    public float range = 15f;

    public GameObject player;

    public ParticleSystem CloudParticle;

    public ParticleSystem powerCloudParticle;

    public void GiveDamagea()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        var enemy = FindNearestEnemy();
        if (enemy == null)
        {
            return;
        }
        var enemyComp = enemy.GetComponent<Enemy>();
        Instantiate(CloudParticle, enemy.transform.position, Quaternion.Euler(-90f,0,0));
        enemyComp.TakeDamage(bulletDamage);
    }

    public void PowerWeapoonDamage()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies == null)
        {
            return;
        }
        foreach (GameObject enemy in enemies)
        {
            var enemyComp = enemy.GetComponent<Enemy>();
            Instantiate(powerCloudParticle, enemy.transform.position, Quaternion.Euler(-90f, 0, 0));
            enemyComp.TakeDamage(bulletDamage);
            ScoreManager.instance.UpdateDamage("Cloud", bulletDamage);
        }
    }

    public override void ChangeSprite(Sprite powerWeapon)
    {
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(player.transform.position, enemy.transform.position);
            if (distance < minDistance && distance <= range)
            {
                minDistance = distance;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null)
        {
            return nearestEnemy;
        }


        return null;
    }
}
