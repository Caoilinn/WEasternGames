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
    public float comboValidTime = 0;
    public int comboHit = 0;

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
    }

    void Control()
    {
        if(!playerStats.isBlockStun && !playerStats.isHitStun && !playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("BI") && !playerAction.isPlayerAttacking)
        {
            AttackType();
            Block();
            Sprint();
            //changeAction();
        }
        attackButtonPressing();
        releaseButton();
        comboTimeAlgorithm();
    }

    private void comboTimeAlgorithm()
    {
        if(comboValidTime > 0)
        {
            comboValidTime -= Time.deltaTime;
        }
        if(comboValidTime <= 0)
        {
            comboValidTime = 0;
            comboHit = 0;
        }
    }

    private void attackButtonPressing()
    {
        if (Input.GetMouseButtonDown(0))
        {
            onHoldTime += Time.deltaTime;
        }
        if (Input.GetMouseButton(0))
        {
            onHoldTime += Time.deltaTime;
        }
        if(playerAction.isPlayerAttacking)
        {
            onHoldTime = 0;
        }
    }

    private void releaseButton()
    {
        if (Input.GetMouseButtonUp(1) || !Input.GetMouseButton(1))
        {
            playerAction.isKeepBlocking = false;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
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
                playerMovement.DodgeTime = 0.3f;
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
        if(!playerAction.isPlayerAttacking && Input.GetMouseButton(0))
        {
            if (onHoldTime >= 0.35f)
            {
                playerAction.action = ActionType.HeavyAttack;
                onHoldTime = 0;
                playerAction.isPlayerAttacking = true;
                comboHit = 0;
                comboValidTime = 0;
            }
        }
        if(!playerAction.isPlayerAttacking && Input.GetMouseButtonUp(0))
        {
            if (onHoldTime < 0.35f)
            {
                onHoldTime = 0;
                comboHit++;
                if(comboHit == 1)
                {
                    playerAction.action = ActionType.LightAttack;
                    comboValidTime = 3;
                    playerAction.isPlayerAttacking = true;
                }
                else if(comboHit == 2)
                {
                    playerAction.action = ActionType.LightAttackCombo2;
                    comboValidTime = 5;
                    playerAction.isPlayerAttacking = true;
                }
                else if (comboHit == 3)
                {
                    playerAction.action = ActionType.LightAttackCombo3;
                    comboValidTime = 5;
                    playerAction.isPlayerAttacking = true;
                }
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
