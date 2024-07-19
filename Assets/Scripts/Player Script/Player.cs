using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
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
    public float playerLevel;
    public float playerMaxExp;
    public float playerCurExp = 0f;
    public float playerAtk { get; set; }
    public float playerAtkSpeed { get; set; }
    public float playerMoveSpeed;
    public float playerCritPer { get; set; }
    public float playerCritDmg { get; set; }

    // magnetic range
    public float playerMagneticRange; // default: 5f
    
    // sprint
    public float sprintTime = 1f; // need private after balancing
    public float sprintCoolDown = 2f;
    private float sprintCoolDownTimer = 2f;
    public float playerAccelerate = 2.5f;

    // invincible time
    private WaitForSeconds invincibleWait = new WaitForSeconds(0.1f);


    // Player Directions
    private Vector2 playerMoveVector;

    // flags
    private bool isInvincible = false;

    // GameObjects
    [Header("- GameObjects")]
    public Transform playerArm;
    public GameObject weapon; // list needed
    public GameObject constText;

    // Input System
    private PlayerInput playerInput;
    private PlayerInput weaponInput;

    // upgrades

    // Energy Drinks
    [Header("-EnergyDrink")]
    public float energyDrinkTimer = 0f;
    public float energyDrinkDuration = 10f;
    private float energyDrinkSprintCDDecrease = 1.25f;
    private bool isEnergyDrinkActive;


    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();

        // player stat init
        playerCurHp = playerMaxHp; // make current hp max
        playerLevel = 1f; 
        playerAtk = 10f;
        playerAtkSpeed = 1f;
        playerMoveSpeed = 10f;
        playerCritPer = 0f;
        playerCritDmg = 2f;

        // player Magnetic Range
        playerMagneticRange = 5f;
        
        // input system init
        playerInput = GetComponent<PlayerInput>();
        weaponInput = weapon.GetComponent<PlayerInput>();

        isEnergyDrinkActive = false;
    }

    private void Update()
    {
        // playerMove
        var player2DDir = playerMoveVector;
        player2DDir.Normalize();
        transform.Translate(player2DDir * playerMoveSpeed * Time.deltaTime, Space.World); // YH

        // playerArmMove
        playerArmRotate();

        // Item Magnetic func
        CheckItemInRange();

        sprintCoolDownTimer += Time.deltaTime;
    }

    // magnetic item check
    private void CheckItemInRange()
    {
        var items = Physics2D.OverlapCircleAll(transform.position, playerMagneticRange, 1 << 6);

        if (items.Length < 1) return;

        foreach (var item in items)
        {
            item.GetComponent<Item>().ItemMagnetic().Forget();
        }
    }

    // Input system Funcs.
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

        // if still CoolDown
        if (sprintCoolDown > sprintCoolDownTimer) return;
        sprintCoolDownTimer = 0f;

        if (!context.started) return;

        Debug.Log("sprint");

        constText.SetActive(true);

        // temp sprint visual effects
        var tempColor = sprite.color;
        sprite.color = Color.yellow;

        // invincible & move speed
        isInvincible = true;
        playerMoveSpeed *= playerAccelerate;
        StartCoroutine(SprintTimer(tempColor));
    }

    // open pause window
    public void OnEscEnter(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ContinueToPause();
        }
    }

    // close pause window
    public void OnEscExit(InputAction.CallbackContext context)
    {
        if (context.canceled && this.enabled == true)
        {
            PauseToContinue();
        }
    }


    private IEnumerator SprintTimer(Color color)
    {
        yield return new WaitForSeconds(sprintTime);
        isInvincible = false;
        playerMoveSpeed /= playerAccelerate;
        

        // flag -- YH: for test
        sprite.color = color;
        constText.SetActive(false);
    }

    private void playerArmRotate()
    {
        // 마우스 위치
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        

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



    // Get Items & Effects
    // player get EXP
    public void GetExp(float expAmount)
    {
        playerCurExp += expAmount;
        if (playerCurExp >= playerMaxExp)
        {
            PlayerLevelUp();
        }
    }

    public void PlayerLevelUp()
    {
        playerLevel += 1;
        // LevelUp func needed // from GM
        playerCurExp -= playerMaxExp;
        playerMaxExp += 5;
        GameManager.Instance.LevelUp();
        SwitchToPlayerControlStop();
    }

    // apple
    public void GetHpPotion(float heal)
    {
        if (playerCurHp > playerMaxHp - heal)
        {
            playerCurHp = playerMaxHp;
        }
        else
        {
            playerCurHp += heal;
        }
    }

    // RedBull
    public void GetEnergyDrink(float value)
    {
        GetHigh(value).Forget();
    }

    private async UniTask GetHigh(float value)
    {
        energyDrinkTimer = energyDrinkDuration;

        if (isEnergyDrinkActive)
        {
            return;
        }

        playerMoveSpeed *= value;
        sprintCoolDown -= energyDrinkSprintCDDecrease;
        isEnergyDrinkActive = true;
        Debug.Log("act");

        EnergyDrinkTimer().Forget();

        await UniTask.WaitUntil(() => energyDrinkTimer <= 0f);
        playerMoveSpeed /= value;
        sprintCoolDown += energyDrinkSprintCDDecrease;
        isEnergyDrinkActive = false;
        Debug.Log("inact");
    }

    private async UniTask EnergyDrinkTimer()
    {
        Debug.Log("Timer Start");
        while (0f < energyDrinkTimer)
        {
            await UniTask.Yield();
            energyDrinkTimer -= Time.deltaTime;
        }

        energyDrinkTimer = 0f;
        Debug.Log("Timer End");
    }

    // System Control

    public void ContinueToPause()
    {
        GameManager.Instance.PauseGame();
        SwitchToUIControl();
    }

    public void PauseToContinue()
    {
        GameManager.Instance.ContinueGame();
        SwitchToPlayerControl();
    }

    public void WaveEndToContinue()
    {
        GameManager.Instance.ContinueGame();
        SwitchToPlayerControl();
    }



    // player keyboard input system toggles
    public void SwitchToPlayerControl()
    {
        playerInput.SwitchCurrentActionMap("playerMove");
        weaponInput.SwitchCurrentActionMap("playerMove");
    }

    private void SwitchToUIControl()
    {
        playerInput.SwitchCurrentActionMap("UIControl");
        weaponInput.SwitchCurrentActionMap("UIControl");
    }

    private void SwitchToPlayerControlStop()
    {
        playerInput.SwitchCurrentActionMap("playerStop");
        weaponInput.SwitchCurrentActionMap("playerStop");
    }
}
