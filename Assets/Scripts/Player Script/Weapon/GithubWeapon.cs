using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GithubWeapon : MonoBehaviour
{
    public GameObject bullet;
    public int bulletNumber;
    public float playerRange = 8f;
    public float moveSpeed = 5f;
    public float stopDistance = 1f;
    public Transform player;

    private Animator animator;
    private bool isMoving = false;
    private bool isInRange = false;
    private Coroutine attackCoroutine;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= playerRange && isMoving == false)
        {
            if (!isInRange)
            {
                isInRange = true;
                if (attackCoroutine == null)
                {
                    attackCoroutine = StartCoroutine(FireBullet());
                }
            }
        }
        else
        {
            if (isInRange)
            {
                isInRange = false;
                if (attackCoroutine != null)
                {
                    StopCoroutine(attackCoroutine);
                    attackCoroutine = null;
                }
            }
            MoveToPlayer();
        }
    }

    IEnumerator FireBullet()
    {
        while (true)
        {
            animator.Play("Github Attack");
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < bulletNumber; i++)
            {
                Instantiate(bullet, transform.position, Quaternion.identity);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    private void MoveToPlayer()
    {
        animator.Play("Github Move");
        isMoving = true;
        Vector2 direction = (player.position - transform.position).normalized;

        if (Vector2.Distance(transform.position, player.position) > stopDistance)
        {
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
        else
        {
            isMoving = false;
        }
    }
}
