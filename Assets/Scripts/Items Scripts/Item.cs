using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class Item : MonoBehaviour
{
    // item values
    protected float value;
    
    // magnetic system
    private float itemMagneticMoveSpeed = 30f;

    private Transform playerTrans;

    private void Start()
    {
        // Init
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }


    // collision with player
    protected abstract void OnTriggerEnter2D(Collider2D collision);
    // Initialize Exp only.
    public abstract void Init(float expAmount);
    // Hp potion and Red Bool have their' own const value.
    // Init on Awake and make this func null return or something.

    // actual magnetic effects
    // 비동기 + 한 번 걸리면 따라가도록 변경
    // 획득 범위는 플레이어 내 스탯으로 하고
    // 시뮬레이션에서 했던 physics overlay 함수로 탐지
    // 객체가 걸리면 자석 함수를 실행하도록 변경
    public async UniTask ItemMagnetic()
    {
        gameObject.layer = 0;

        // distance calculate
        // var distance = Vector3.Distance(transform.position, playerTrans.position);
        while (gameObject != null)
        {
            await UniTask.NextFrame();
            transform.position = Vector3.MoveTowards(transform.position, playerTrans.position, itemMagneticMoveSpeed * Time.deltaTime);
        }
    }
}
