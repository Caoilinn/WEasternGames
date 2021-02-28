using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    PlayerAction playerAction;
    PlayerJump playerJump;
    PlayerMovementV2 playerMovement;
    PlayerStats playerStats;
    PlayerAnimation playerAnimation;
    public float onHoldTime = 0;
    public float sprintCD = 0;
    public bool sprintTrigger;

    void Awake()
    {
        playerAction = GetComponent<PlayerAction>();
        playerJump = GetComponent<PlayerJump>();
        playerMovement = GetComponent<PlayerMovementV2>();
        playerStats = GetComponent<PlayerStats>();
        playerAnimation = GetComponent<PlayerAnimation>();
        sprintTrigger = false;
    }

    void Update()
    {
        Control();
        //Debug.Log(GamePreload.images[1].name);
    }

    void Control()
    {
        if(!playerStats.isBlockStun && !playerStats.isHitStun && !playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("BI"))
        {
            AttackType();
            Block();
            Sprint();
            changeAction();
        }
        releaseButton();
    }

    private void releaseButton()
    {
        if (Input.GetMouseButtonUp(1))
        {
            playerAction.isKeepBlocking = false;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            playerMovement.isRunning = false;
        }
    }

    void changeAction()
    {
        if(playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("GH") && !playerStats.isHitStun)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                playerAnimation._anim.SetTrigger("changeToDodge");
            }

            if(Input.GetKey(KeyCode.LeftShift))
            {
                playerAnimation._anim.SetTrigger("changeToSprint");
            }

            //if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            //{
            //    playerAnimation._anim.SetTrigger("changeToDefault");
            //}

            //if (Input.GetKey(KeyCode.Space))
            //{
            //    playerAnimation._anim.SetTrigger("changeToJump");
            //}
        }
    }

    void Sprint()
    {
        if(playerJump.isJump == false && !playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("LT") && 
            !playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("HT") && 
            playerAction.isKeepBlocking == false && playerStats.stamina > 0 && !sprintTrigger)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                playerMovement.DodgeTime = 0.3f;
                sprintCD = 1.0f;
                sprintTrigger = true;
                playerMovement.isRunning = true;
                playerMovement.isDodging = true;
                playerAction.action = ActionType.Dodge;
            }
        }
        if(sprintCD > 0)
        {
            sprintCD -= Time.deltaTime;
        }
        if(sprintCD <= 0)
        {
            sprintTrigger = false;
        }

    }

    void AttackType()
    {
        if(!playerAction.isPlayerAttacking && !playerStats.isHitStun) //if the player didnt do any attack action
        {
            if (Input.GetMouseButtonDown(0))
            {
                onHoldTime += Time.deltaTime;
            }

            if (Input.GetMouseButton(0))
            {
                onHoldTime += Time.deltaTime;
                if (onHoldTime >= 0.4f)
                {
                    playerAction.action = ActionType.HeavyAttack;
                    onHoldTime = 0;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (onHoldTime < 0.25f)
                {
                    playerAction.action = ActionType.LightAttack;
                }
                onHoldTime = 0;
            }
        }
    }

    void Block()
    {
        if (Input.GetMouseButtonDown(1))
        {
            playerAction.action = ActionType.SwordBlock;
        }
        if (Input.GetMouseButton(1))
        {
            playerAction.isKeepBlocking = true;
        }
    }
}
