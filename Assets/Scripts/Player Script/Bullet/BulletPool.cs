using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public GameObject bullet;

    // stack
    [SerializeField] private int poolSize;
    [SerializeField] public Stack<PlayerBullet> bulletStack = new Stack<PlayerBullet>();

    // this class should activate first. before others move.
    void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GenerateBullet();
        }
    }

    private void GenerateBullet()
    {
        var tempObj = Instantiate(bullet);
        tempObj.SetActive(false);
        tempObj.transform.parent = transform;

        var tempBullet = tempObj.GetComponent<PlayerBullet>();
        bulletStack.Push(tempBullet);
    }
    
    public PlayerBullet GetBullet()
    {
        if (bulletStack.Count < 1) GenerateBullet();

        var tempBullet = bulletStack.Pop();
        tempBullet.gameObject.SetActive(true);
        return tempBullet;
    }

    public void SetObj(PlayerBullet bullet)
    {
        bullet.gameObject.SetActive(false);
        bulletStack.Push(bullet);
    }
}
