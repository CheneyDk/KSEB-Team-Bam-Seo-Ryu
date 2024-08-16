using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemPooling : MonoBehaviour
{
    public static ItemPooling Instance;
    public int poolSize;
    [SerializeField]
    private GameObject EXP;
    [SerializeField] 
    private GameObject apple;
    [SerializeField] 
    private GameObject redblue;
    [SerializeField]
    private GameObject bitcoin;
    [SerializeField]
    private Transform itemPoolingZone;

    public Queue<GameObject> EXPPool = new();
    public Queue<GameObject> applePool = new();
    public Queue<GameObject> redbluePool = new();

    void Awake()
    {
        Instance = this;
        for (int i = 0; i < poolSize; i++)
        {
            GenerateEXP();
            GenerateApple();
            GenerateRedBlue();
        }
    }

    private void GenerateEXP()
    {
        var tempItem = Instantiate(EXP);
        tempItem.SetActive(false);
        tempItem.transform.parent = itemPoolingZone;

        EXPPool.Enqueue(tempItem);
    }

    private void GenerateApple()
    {
        var tempItem = Instantiate(apple);
        tempItem.SetActive(false);
        tempItem.transform.parent = itemPoolingZone;

        applePool.Enqueue(tempItem);
    }

    private void GenerateRedBlue()
    {
        var tempItem = Instantiate(redblue);
        tempItem.SetActive(false);
        tempItem.transform.parent = itemPoolingZone;

        redbluePool.Enqueue(tempItem);
    }

    private void GenerateBitCoin()
    {
        var tempItem = Instantiate(bitcoin);
        tempItem.SetActive(false);
        tempItem.transform.parent = itemPoolingZone;

        redbluePool.Enqueue(tempItem);
    }

    public GameObject GetEXP(Vector3 spawnPos)
    { 
        if (EXPPool.Count < 1)
        {
            GenerateEXP();
        }
        var item = EXPPool.Dequeue();

        item.GetComponent<Item>().isDestroyed = false;
        item.gameObject.layer = 6;
        item.gameObject.SetActive(true);
        item.transform.position = spawnPos;
        return item;
    }

    public GameObject GetApple(Vector3 spawnPos)
    {
        if (applePool.Count < 1)
        {
            GenerateApple();
        }
        var item = applePool.Dequeue();

        item.GetComponent<Item>().isDestroyed = false;
        item.gameObject.layer = 6;
        item.gameObject.SetActive(true);
        item.transform.position = spawnPos;
        return item;
    }

    public GameObject GetRedBlue(Vector3 spawnPos)
    {
        if (redbluePool.Count < 1)
        {
            GenerateRedBlue();
        }
        var item = redbluePool.Dequeue();

        item.GetComponent<Item>().isDestroyed = false;
        item.gameObject.layer = 6;
        item.gameObject.SetActive(true);
        item.transform.position = spawnPos;
        return item;
    }

    public GameObject GetBitCoin(Vector3 spawnPos)
    {
        if (redbluePool.Count < 1)
        {
            GenerateBitCoin();
        }
        var item = redbluePool.Dequeue();

        item.GetComponent<Item>().isDestroyed = false;
        item.gameObject.layer = 6;
        item.gameObject.SetActive(true);
        item.transform.position = spawnPos;
        return item;
    }

    //오브젝트 넣어서 재활용
    public void ReturnEXP(GameObject item)
    {
        item.gameObject.SetActive(false);
        item.GetComponent<Item>().isDestroyed = true;
        item.transform.parent = itemPoolingZone;
        EXPPool.Enqueue(item);
    }
    public void ReturnApple(GameObject item)
    {
        item.gameObject.SetActive(false);
        item.transform.parent = itemPoolingZone;
        item.GetComponent<Item>().isDestroyed = true;
        applePool.Enqueue(item);
    }
    public void ReturnRedBlue(GameObject item)
    {
        item.gameObject.SetActive(false);
        item.transform.parent = itemPoolingZone;
        item.GetComponent<Item>().isDestroyed = true;
        redbluePool.Enqueue(item);
    }

    public void ReturnBitCoin(GameObject item)
    {
        item.gameObject.SetActive(false);
        item.transform.parent = itemPoolingZone;
        item.GetComponent<Item>().isDestroyed = true;
        redbluePool.Enqueue(item);
    }
}
