using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // 플레이어 변수
    // 모두 { get; }을 달아서 읽기 전용으로 변경
    
    // 플레이어 스탯
    [Header("- Player")]
    public float playerMaxHp;
    public float playerCurHp { get; }
    public float playerAtk { get; }
    public float playerAtkSpeed { get; }
    public float playerMoveSpeed;
    public float playerCritPer { get; }
    public float playerCritDmg { get; }
    private float playerInvincibleTime;

    // 플레이어 상태 (방향 등)
    private Vector2 playerMoveVector;
    private Vector2 playerDir;

    private void Update()
    {
        var player2DDir = playerMoveVector;
        player2DDir.Normalize();
        transform.Translate(player2DDir * playerMoveSpeed * Time.deltaTime, Space.World);
    }

    public void playerMove(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            playerMoveVector = context.ReadValue<Vector2>();
        }

        if (context.canceled)
        {
            playerMoveVector = Vector2.zero;
        }
    }

}
