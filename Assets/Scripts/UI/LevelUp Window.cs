using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpWindow : MonoBehaviour
{
    [SerializeField]
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        LevelUp();
    }

    private void LevelUp()
    {
        if( player.playerCurExp >= player.playerMaxExp)
        {
            gameObject.SetActive(true);
            Time.timeScale = 0;
            // level up and choose Weapon Upgrade
        }
    }

    private void ChooseWeapon()
    {
        // iteam animation
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
