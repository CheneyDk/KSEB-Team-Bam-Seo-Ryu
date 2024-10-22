using Cysharp.Threading.Tasks;
using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using VInspector;

public class JavaPlayer : MonoBehaviour
{
    public Dictionary<string, float> weaponList = new Dictionary<string, float>();

    // Player Variances
    // readonly.

    // Player Stats
    [Foldout("- Player")]
    public float playerMaxHp;
    public float playerCurHp;
    public int playerLevel;
    public float playerMaxExp;
    public float playerCurExp = 0f;
    public float playerAtk { get; set; }
    public float playerAtkSpeed { get; set; }
    public float playerMoveSpeed;
    public int playerCritPer { get; set; }
    public float playerCritDmg { get; set; }

    // magnetic range
    public float playerMagneticRange; // default: 5f

    // sprint
    public float sprintTime = 1f; // need private after balancing
    public float sprintCoolDown = 2f;
    private float sprintCoolDownTimer = 2f;
    public float playerAccelerate = 2.5f;
    [EndFoldout]

    // invincible time
    private WaitForSeconds invincibleWait = new WaitForSeconds(1f);


    // Player Directions
    private Vector2 playerMoveVector;

    // flags
    private bool isInvincible = false;

    // GameObjects
    [Foldout("- GameObjects")]
    public GameObject weaponN;
    public GameObject weaponP;
    public GameObject constText;
    public GameObject wing;
    [EndFoldout]

    // Input System
    private PlayerInput playerInput;
    private PlayerInput weaponInput;

    // upgrades

    // Energy Drinks
    [Foldout("-EnergyDrink")]
    public float energyDrinkTimer = 0f;
    public float energyDrinkDuration = 10f;
    private float energyDrinkSprintCDDecrease = 1.25f;
    private bool isEnergyDrinkActive;
    [EndFoldout]

    [SerializeField, Header("-Spawn Damage Number")]
    private DamageNumber damageNumber;

    [SerializeField]
    private ParticleSystem levelUp;

    private Collider2D playerCollider;
    private List<Collider2D> ignoredColliders = new List<Collider2D>();
    private Animator playAnimator;
    public Animator hitUIAnimator;

    private AudioManager audioManager;

    private AudioSource audioSource;
    public AudioClip levelUpClip;

    bool isAdd;

    public bool isUpgrade = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        playAnimator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();
        audioManager = FindObjectOfType<AudioManager>();

        // player stat init
        playerCurHp = playerMaxHp; // make current hp max
        playerLevel = 1;
        playerAtk = 10f;
        playerAtkSpeed = 1f;
        playerMoveSpeed = 10f;
        playerCritPer = 0;
        playerCritDmg = 1f; // CritDmg 1f == actual damage x2

        /********************************************************************************************
        // More Explaination about how critical damage is calculated
        // >> GetComponent<PlayerBullet>().Init(playerAtk * weaponDamageRate * (1 + criticalDamageRate))
        // and look at the last part of bullet Initialize method.
        // if critical does not occur, criticalDamageRate becomes 0 value. (by multiply 0)
        // the function "IsCritOccur" make a decision multiply 0 or 1 (this func is in PlayerWeapon script)
        *********************************************************************************************/
        // player Magnetic Range
        playerMagneticRange = 5f;

        // input system init
        playerInput = GetComponent<PlayerInput>();

        isEnergyDrinkActive = false;

        weaponList.Add("basic", 0);
        isAdd = true;
    }

    private void Update()
    {
        // playerMove
        var player2DDir = playerMoveVector;
        player2DDir.Normalize();
        transform.Translate(player2DDir * playerMoveSpeed * Time.deltaTime, Space.World); // YH

        // Item Magnetic func
        CheckItemInRange();

        sprintCoolDownTimer += Time.deltaTime;
    }

    private  void CheckWeapon()
    {
        if (isUpgrade == false)
        {
            weaponN.SetActive(true);
            weaponP.SetActive(false);
        }
        else if(isUpgrade == true)
        {
            weaponN.SetActive(false);
            weaponP.SetActive(true);
        }
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

        // only key input down active sprint
        if (!context.started) return;

        // if still CoolDown
        if (sprintCoolDown > sprintCoolDownTimer) return;
        sprintCoolDownTimer = 0f;

        Debug.Log("sprint");

        IgnoreEnemyCollisions(true);

        constText.SetActive(true);

        // temp sprint visual effects
        playAnimator.Play("Sprint");


        // invincible & move speed
        isInvincible = true;
        playerMoveSpeed *= playerAccelerate;
        StartCoroutine(SprintTimer());
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

    private IEnumerator SprintTimer()
    {
        yield return new WaitForSeconds(sprintTime);
        isInvincible = false;
        playerMoveSpeed /= playerAccelerate;

        IgnoreEnemyCollisions(false);
        // flag -- YH: for test
        playAnimator.Play("Idle");
        constText.SetActive(false);
    }

    // playerTakeDamage
    public void TakeDamage(float damage)
    {
        if (isInvincible) return; // if invincible -> do not take any damage
        damageNumber.Spawn(transform.position, damage);
        playAnimator.Play("TakeDamage");
        hitUIAnimator.Play("HitUI");
        audioManager.PlayerDamagedClip();

        // game over: destroy
        playerCurHp -= damage;
        if (playerCurHp <= 0)
        {
            if (SaveManager.instance.shopData.isPetInstalled && isAdd)
            {
                SaveManager.instance.AddWeapon("Pet");
                SaveManager.instance.UpdateDamage("Pet", SaveManager.instance.GetPetDamage());

                isAdd = false;
            }
            //Destroy(gameObject); // player destroy
            //gameObject.SetActive(false); // player obj disable
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
            Instantiate(levelUp, transform.position, Quaternion.identity);
            SaveManager.instance.UpdateGameRecord("level");
        }
    }

    public void PlayerLevelUp()
    {
        playerLevel += 1;
        audioSource.PlayOneShot(levelUpClip);
        // LevelUp func needed // from GM
        playerCurExp -= playerMaxExp;
        playerMaxExp += 5;
        GameManager.Instance.LevelUp();
        SwitchToPlayerControlStop();
    }

    // apple
    public void GetHpPotion(float heal)
    {
        audioManager.AppleClip();
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
        audioManager.RedBlueClip();
        wing.SetActive(true);
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

        EnergyDrinkTimer().Forget();

        await UniTask.WaitUntil(() => energyDrinkTimer <= 0f);
        playerMoveSpeed /= value;
        sprintCoolDown += energyDrinkSprintCDDecrease;
        isEnergyDrinkActive = false;
        wing.SetActive(false);
    }

    private async UniTask EnergyDrinkTimer()
    {
        while (0f < energyDrinkTimer)
        {
            await UniTask.Yield();
            energyDrinkTimer -= Time.deltaTime;
        }

        energyDrinkTimer = 0f;
    }

    private void IgnoreEnemyCollisions(bool ignore)
    {
        Collider2D[] enemies = FindObjectsOfType<Collider2D>();

        foreach (var enemyCollider in enemies)
        {
            if (enemyCollider.CompareTag("Enemy"))
            {
                Physics2D.IgnoreCollision(playerCollider, enemyCollider, ignore);

                if (ignore)
                {
                    ignoredColliders.Add(enemyCollider);
                }
                else
                {
                    ignoredColliders.Remove(enemyCollider);
                }
            }
        }
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
