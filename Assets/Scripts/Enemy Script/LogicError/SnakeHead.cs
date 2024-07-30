using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHead : SnakePart
{
    private const float Pi = Mathf.PI;

    private void Start()
    {
        snakePartMaxHp = snakeMain.snakeMaxHp; // 5000f
        snakePartCurHp = snakePartMaxHp;
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


    // abstract override
    public override void TakeDamage(float damage)
    {
        hitParticle.Play();
        damageNumber.Spawn(transform.position, damage);
        snakePartCurHp -= damage;
        snakeMain.TakeDamage(damage); // head dont need to got a damage
    }

    public override IEnumerator LastingDamage(float damage, int totalDamageTime, Color color)
    {
        curSR.color = color;
        var damageTimer = 0f;

        while (damageTimer < totalDamageTime)
        {
            yield return new WaitForSeconds(1f);
            hitParticle.Play();
            lastingDamageNumber.Spawn(transform.position, damage);
            snakeMain.TakeDamage(damage);
            damageTimer += 1f;

            ScoreManager.instance.UpdateDamage("React", damage);
        }
        curSR.color = originColor;
    }

    // gof would mad at me
    public override void DropEXP(int iteamNumber){}
    public override void EnemyMovement(){}
    public override void ResetEnemy(){}
}
