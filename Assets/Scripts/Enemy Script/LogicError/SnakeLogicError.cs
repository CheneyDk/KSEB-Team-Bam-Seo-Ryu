using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeLogicError : Enemy
{
    private float snakeSpeed = 500f;
    private float turnSpeed = 300f;

    // allocate in unity
    // 0: head, 1: body, 2: tail
    [SerializeField] public List<GameObject> bodyParts = new List<GameObject>();
    // actually use in script
    private List<GameObject> snakeBody = new List<GameObject>();
    private float bodyLength = 7;
    private float distanceBetween = 0.2f;


    private void Awake()
    {
        InitSnake().Forget();
    }

    private void FixedUpdate()
    {
        
        SnakeMovement();
    }

    private async UniTask InitSnake()
    {
        // need to fix
        int bodytype = 0;

        // need delta timer

        for (int i = 0; i < bodyLength; i++)
        {
            // i == 1: right after head, bodyL - 1: right before tail
            if (i == 1 || i == bodyLength - 1) bodytype++;
            var tempBodypart = Instantiate(bodyParts[bodytype], transform.position, transform.rotation, transform);
            tempBodypart.GetComponent<SnakeMovement>().ClearMovementList();
            snakeBody.Add(tempBodypart);

            await UniTask.WaitForSeconds(distanceBetween);
        }
    }

    public void SnakeMovement() 
    {
        // SnakeMovement

        // Head Movement
        // straight
        snakeBody[0].GetComponent<Rigidbody2D>().velocity = snakeSpeed * Time.deltaTime * snakeBody[0].transform.right;
        // zigzag
        // gonna change later - YH

        // The other parts Movement
        if (snakeBody.Count < 1) return;
        for (int i = 1; i < snakeBody.Count; i++)
        {
            var movement = snakeBody[i - 1].GetComponent<SnakeMovement>();
            snakeBody[i].transform.position = movement.movementList[0].position;
            snakeBody[i].transform.rotation = movement.movementList[0].rotation;
            movement.movementList.RemoveAt(0);
        }
    }



    public override IEnumerator LastingDamage(float damage, int totalDamageTime, Color color)
    {
        yield return null; // temp
    }

    public override void TakeDamage(float damage)
    {
        
    }

    public override void DropEXP(int iteamNumber)
    {

    }



    public override void EnemyMovement() {}
    public override void ResetEnemy() { }
}
