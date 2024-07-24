using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    public class Movement
    {
        public Vector3 position;
        public Quaternion rotation;

        public Movement(Vector3 pos, Quaternion rot)
        {
            position = pos;
            rotation = rot;
        }
    }

    public List<Movement> movementList = new List<Movement>();

    private void FixedUpdate()
    {
        UpdateMovementList();
    }

    private void UpdateMovementList()
    {
        movementList.Add(new Movement(transform.position, transform.rotation));
    }

    public void ClearMovementList()
    {
        movementList.Clear();
        movementList.Add(new Movement(transform.position, transform.rotation));
    }
}
