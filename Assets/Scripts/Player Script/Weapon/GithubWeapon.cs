using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GithubWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject bullet;

    public float playerRange = 8f;
    public float moveSpeed = 5f;
    public float stopDistance = 1f;

    private Transform playerPos;
    private Animator animator;
    private bool isMoving = false;
    private bool isInRange = false;
    private Coroutine attackCoroutine;

    public bool isUpgraded = false;

    private AudioSource audioSource;
    public AudioClip githubFire;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        if (ScoreManager.instance.recordData.isPetUpgrade)
        {
            isUpgraded = true;
        }
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
            if (!isUpgraded)
            {
                animator.Play("Github Attack");
                bullet.GetComponent<GithubBullet>().bulletDamage = 10;
                yield return new WaitForSeconds(1f);
                var addBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                audioSource.PlayOneShot(githubFire);
                yield return new WaitForSeconds(1f);
            }
            else if(isUpgraded)
            {
                animator.Play("Power Github Attack");
                bullet.GetComponent<GithubBullet>().bulletDamage = 15;
                yield return new WaitForSeconds(0.5f);
                var addBullet = Instantiate(bullet, transform.position, Quaternion.identity);
                audioSource.PlayOneShot(githubFire);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    private void MoveToPlayer()
    {
        if (!isUpgraded)
        {
            animator.Play("Github Move");
        }
        else if (isUpgraded)
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
}
