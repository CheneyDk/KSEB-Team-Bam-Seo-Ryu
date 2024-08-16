using System.Collections;
using System.Collections.Generic;
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

    private List<GameObject> bulletPool = new List<GameObject>();

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        if (SaveManager.instance.shopData.isPetUpgrade)
        {
            isUpgraded = true;
        }
        PutBulletsToPool(bulletPool);
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
                var addBullet = GetPooledBullet(bulletPool);
                if (addBullet != null)
                {
                    addBullet.transform.position = transform.position;
                    addBullet.transform.rotation = Quaternion.identity;
                    addBullet.SetActive(true);
                }
                audioSource.PlayOneShot(githubFire);
                yield return new WaitForSeconds(1f);
            }
            else if(isUpgraded)
            {
                animator.Play("Power Github Attack");
                bullet.GetComponent<GithubBullet>().bulletDamage = 15;
                yield return new WaitForSeconds(0.5f);
                var addBullet = GetPooledBullet(bulletPool);
                if (addBullet != null)
                {
                    addBullet.transform.position = transform.position;
                    addBullet.transform.rotation = Quaternion.identity;
                    addBullet.SetActive(true);
                }
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

    private void PutBulletsToPool(List<GameObject> poolList)
    {
        for (int i = 0; i < 10; i++)
        {
            var tmp = Instantiate(bullet);
            tmp.transform.parent = GameObject.FindGameObjectWithTag("Other Pool").transform;
            tmp.SetActive(false);
            poolList.Add(tmp);
        }
    }

    // Get bullet from pooling list
    public GameObject GetPooledBullet(List<GameObject> poolList)
    {
        for (int i = 0; i < poolList.Count; i++)
        {
            if (!poolList[i].activeInHierarchy)
            {
                return poolList[i];
            }
        }
        return null;
    }
}
