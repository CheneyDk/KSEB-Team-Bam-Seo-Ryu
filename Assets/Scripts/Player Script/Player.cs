using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private SpriteRenderer sprite;

    // Player Variances
    // readonly.
    
    // Player Stats
    [Header("- Player")]
    public float playerMaxHp;
    public float playerCurHp;
    public float playerAtk { get; }
    public float playerAtkSpeed { get; }
    public float playerMoveSpeed;
    public float playerCritPer { get; }
    public float playerCritDmg { get; }

    public float sprintTime = 1f; // need private after balancing
    public float playerAccelerate = 3f;

    private WaitForSeconds invincibleWait = new WaitForSeconds(0.1f);


    // Player Directions
    private Vector2 playerMoveVector;

    // flags
    private bool isInvincible = false;

    // GameObjects
    [Header("- GameObjects")]
    public Transform playerArm;
    public GameObject weapon; // list needed

    // Input System - YH 7/5 이어서 작업할 것
    // public InputAction playerMove

    // upgrades


    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        playerCurHp = playerMaxHp; // make current hp max
        // weapon init need;

    }

    private void Update()
    {
        // playerMove
        var player2DDir = playerMoveVector;
        player2DDir.Normalize();
        transform.Translate(player2DDir * playerMoveSpeed * Time.deltaTime, Space.World);

        // playerArmMove
        playerArmRotate();


    }

    public void playerMove(InputAction.CallbackContext context)
    {
        playerMoveVector = context.ReadValue<Vector2>();
        
        if (context.canceled)
        {
            playerMoveVector = Vector2.zero;
        }
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        // while stop state, player can't rollin'
        if (playerMoveVector == Vector2.zero) return;

        if (!context.started) return;

        Debug.Log("sprint");

        // temp sprint visual effects
        var tempColor = sprite.color;
        sprite.color = Color.red;
        isInvincible = true;
        playerMoveSpeed *= playerAccelerate;
        StartCoroutine(SprintTimer(tempColor));
    }


    private IEnumerator SprintTimer(Color color)
    {
        yield return new WaitForSeconds(sprintTime);
        isInvincible = false;
        playerMoveSpeed /= playerAccelerate;

        // flag -- YH: for test
        sprite.color = color;
    }

    private void playerArmRotate()
    {
        // 마우스 위치
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane)); ;
        

        Vector3 direction = worldMousePosition - playerArm.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        playerArm.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }

    // playerTakeDamage
    public void TakeDamage(float damage)
    {
        if (isInvincible) return; // if invincible -> do not take any damage

        // game over: destroy
        playerCurHp -= damage;
        if (playerCurHp <= 0)
        {
            Destroy(gameObject); // player destroy
            gameObject.SetActive(false); // player obj disable
            GameManager.Instance.SetPlayerDead();
            return; // safe return ; not to activate Coroutine
        }

        // invincible Time
        isInvincible = true;
        StartCoroutine(invincibleTimer());
    }
    
    private IEnumerator invincibleTimer()
    {
        yield return invincibleWait;
        isInvincible = false;
    }
}
