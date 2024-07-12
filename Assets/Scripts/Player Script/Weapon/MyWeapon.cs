using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyWeapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float fireRate = 1f;

    void Start()
    {
        StartCoroutine(Fire());
    }

    IEnumerator Fire()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);
            Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        }
    }
}
