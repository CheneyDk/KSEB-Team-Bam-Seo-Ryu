using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GithubWeapon : PlayerWeapon
{
    public int bulletNumber;
    public float playerRange = 8f;
    public float moveSpeed = 5f;
    public float stopDistance = 1f;

    private Transform playerPos;
    private Player players;
    private Animator animator;
    private bool isMoving = false;
    private bool isInRange = false;
    private Coroutine attackCoroutine;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        players = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Start()
    {
        weaponLevel = 1;
        weaponDamageRate = 0.5f;
        isMaxLevel = false;
        isPowerWeapon = false;
        matchPassive = "Google";
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerPos.position);
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
            if (!isPowerWeapon)
            {
                animator.Play("Github Attack");
                yield return new WaitForSeconds(1f);
                for (int i = 0; i < bulletNumber; i++)
                {
                    var addBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                    addBullet.GetComponent<PlayerBullet>().Init(players.playerAtk * weaponDamageRate, 0);
                    yield return new WaitForSeconds(1f);
                }
            }
            else if(isPowerWeapon)
            {
                animator.Play("Power Github Attack");
                yield return new WaitForSeconds(0.5f);
                for (int i = 0; i < bulletNumber; i++)
                {
                    var addBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                    addBullet.GetComponent<PlayerBullet>().Init(players.playerAtk * weaponDamageRate, 0);
                    yield return new WaitForSeconds(0.5f);
                }
            }
        }
    }

    private void MoveToPlayer()
    {
        if (!isPowerWeapon)
        {
            animator.Play("Github Move");
        }
        else if (isPowerWeapon)
        {
            animator.Play("Power Github Move");
        }
        isMoving = true;
        Vector2 direction = (playerPos.position - transform.position).normalized;

        if (Vector2.Distance(transform.position, playerPos.position) > stopDistance)
        {
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
        else
        {
            isMoving = false;
        }
    }

    protected override void Fire()
    {
    }

    public override void Fire(InputAction.CallbackContext context)
    {
    }

    public override void Upgrade()
    {
        if (weaponLevel < 5)
        {
            weaponLevel++;
            bulletNumber += 1;
        }
        if (weaponLevel > 4)
        {
            isMaxLevel = true;
        }
    }
}
