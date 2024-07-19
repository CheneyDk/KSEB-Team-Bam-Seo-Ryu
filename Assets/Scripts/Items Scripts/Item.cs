using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class Item : MonoBehaviour
{
    // item values
    protected float value;

    // flags
    protected bool isDestroyed = false;

    // magnetic system
    private float itemMagneticMoveSpeed = 30f;

    private Transform playerTrans;

    private void Start()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // collision with player
    protected abstract void OnTriggerEnter2D(Collider2D collision);
    // Initialize Exp only.

    // actual magnetic effects
    // �񵿱� + �� �� �ɸ��� ���󰡵��� ����
    // ȹ�� ������ �÷��̾� �� �������� �ϰ�
    // �ùķ��̼ǿ��� �ߴ� physics overlay �Լ��� Ž��
    // ��ü�� �ɸ��� �ڼ� �Լ��� �����ϵ��� ����
    public async UniTask ItemMagnetic()
    {
        gameObject.layer = 0;

        // distance calculate
        // var distance = Vector3.Distance(transform.position, playerTrans.position);
        while (true)
        {
            await UniTask.NextFrame();
            if (isDestroyed) return;
            transform.position = Vector3.MoveTowards(transform.position, playerTrans.position, itemMagneticMoveSpeed * Time.deltaTime);
        }
    }
}
