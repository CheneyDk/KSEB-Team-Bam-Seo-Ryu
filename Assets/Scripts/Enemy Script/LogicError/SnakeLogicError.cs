using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeLogicError : Enemy
{
    private float snakeSpeed = 20f;
    private float turnSpeed = 5f;

    // allocate in unity
    [SerializeField] public List<GameObject> bodyParts = new List<GameObject>();
    // actually use in script
    private List<GameObject> snakeBody = new List<GameObject>();
    private float bodyLength = 7;
    private float distanceBetween = 1f;


    private void Start()
    {
        InitSnake();
    }

    private void Update()
    {
        
        EnemyMovement();
    }

    private void InitSnake()
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
        }
    }

    public override void EnemyMovement() 
    {
        // SnakeMovement

        // Head Movement
        // straight
        snakeBody[0].GetComponent<Rigidbody2D>().velocity = snakeBody[0].transform.right * snakeSpeed * Time.deltaTime;
        // zigzag
        // gonna change later - YH

        // The other parts Movement
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


    public override void ResetEnemy() { }
}
