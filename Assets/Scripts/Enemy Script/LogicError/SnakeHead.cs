using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHead : SnakePart
{
    private const float Pi = Mathf.PI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // bounce on wall collision
    private void OnTriggerEnter2D(Collider2D other)
    {
        // layer 9: Vertical Wall
        // if collision occur, X axis vector is inversed
        if (other.gameObject.layer == 9)
        {
            // convert degree to Vector
            Vector2 vec = DegreeToVector(transform.eulerAngles.z);
            // product -1 to vector.x
            vec.x *= -1f;

            var rot = transform.eulerAngles;
            // Now convert that vector to degree
            rot.z = VectorToDegree(vec);
            transform.eulerAngles = rot;

            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        // layer 10: Horizontal Wall
        // if collision occur, Y axis vector is inversed.
        else if (other.gameObject.layer == 10)
        {
            var rot = transform.rotation;
            rot.z *= -1f;
            gameObject.transform.rotation = rot;
        }
    }

    // 애초에 이걸 메인 스크립트에 넣을까?
    // charge1 func (activate on main script)

    // charge2 func ( " )


    // Utility Funcs
    private float VectorToDegree(Vector2 vector)
    {
        float radian = Mathf.Atan2(vector.y, vector.x);
        return (radian * 180 / Pi);
    }

    private Vector2 DegreeToVector(float degree)
    {
        float radian = Pi / 180 * degree;
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }
}
