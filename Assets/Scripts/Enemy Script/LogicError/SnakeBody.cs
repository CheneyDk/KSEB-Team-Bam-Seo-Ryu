using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBody : SnakePart
{
    public GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // gun fire
    private void SnakeBodyGunFire()
    {
        var tempBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        tempBullet.GetComponent<BulletLogicError>().Init(player.transform.position);
    }
}
