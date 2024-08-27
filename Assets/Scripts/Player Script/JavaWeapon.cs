using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JavaWeapon : MonoBehaviour
{
    public Player player;

    public float damage = 10f;     
    public float damageRate = 1f;  
    public float damageRange = 5f; 

    private Collider2D[] enemiesInRange;

    void Start()
    {
        StartCoroutine(LastingDamage());
    }

    private IEnumerator LastingDamage()
    {
        while (true)
        {
            enemiesInRange = Physics2D.OverlapCircleAll(transform.position, damageRange, LayerMask.GetMask("Enemy"));

            foreach (Collider2D enemyCollider in enemiesInRange)
            {
                Enemy enemy = enemyCollider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage, 0);  
                }
            }

            yield return new WaitForSeconds(damageRate);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRange);
    }
}
