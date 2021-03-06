using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public bool attackEnabled = true;
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
    CameraManager cameraManager;

    private void Start()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    void Awake()
    {
        playerAction = GetComponent<PlayerAction>();
        playerJump = GetComponent<PlayerJump>();
        playerMovement = GetComponent<PlayerMovementV2>();
        playerStats = GetComponent<PlayerStats>();
        playerAnimation = GetComponent<PlayerAnimation>();
        sprintTrigger = false;
        cameraManager = GameObject.FindGameObjectWithTag("GameSetting").GetComponent<CameraManager>();
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
        switchToLockOnCamera();
        switchToDifferentEnemy();
    }

    private void switchToDifferentEnemy()
    {
        if (Input.GetKeyDown(KeyCode.Q) && 
            cameraManager.isLockOnMode && 
            cameraManager.EnemyLockOnList.Count >= 1)
        {
            if (cameraManager.enemyCursor <= cameraManager.EnemyLockOnList.Count - 1)
            {
               cameraManager.enemyCursor += 1;
            }
            if(cameraManager.enemyCursor == cameraManager.EnemyLockOnList.Count)
            {
                cameraManager.enemyCursor = 0;
            }
        }
    }

    private void switchToLockOnCamera()
    {
        if (Input.GetKeyDown(KeyCode.F) && cameraManager.canLockOn)
        {
            if (!cameraManager.isLockOnMode)
            {
                cameraManager.playerCamera.enabled =false;
                cameraManager.lockOnCamera.enabled = true;
                cameraManager.isLockOnMode = true;
                cameraManager.playerCamera.gameObject.GetComponent<AudioListener>().enabled = false;
                cameraManager.lockOnCamera.gameObject.GetComponent<AudioListener>().enabled = true;
                playerMovement.playerCameraTransform = cameraManager.lockOnCamera.transform;
            }
            else
            {
                cameraManager.playerCamera.enabled = true;
                cameraManager.lockOnCamera.enabled = false;
                cameraManager.isLockOnMode = false;
                cameraManager.playerCamera.gameObject.GetComponent<AudioListener>().enabled = true;
                cameraManager.lockOnCamera.gameObject.GetComponent<AudioListener>().enabled = false;
                cameraManager.enemyCursor = 0;
                playerMovement.playerCameraTransform = cameraManager.playerCamera.transform;
            }
        }
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
        if (!attackEnabled) { return; }

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
        if(/*playerJump.isJump == false &&*/ !playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("LT") && 
            !playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("HT") && 
            playerAction.isKeepBlocking == false && playerStats.stamina > 0 && !sprintTrigger)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
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
        if (!attackEnabled) { return; }
        
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
                comboValidTime = 2;
                if (comboHit == 1)
                {
                    playerAction.action = ActionType.LightAttack;
                    if (playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("LT"))
                    {
                        comboValidTime = playerAnimation._anim.GetCurrentAnimatorStateInfo(0).length + 1;
                    }
                    playerAction.isPlayerAttacking = true;
                }
                else if(comboHit == 2)
                {
                    playerAction.action = ActionType.LightAttackCombo2;
                    if (playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("LT"))
                    {
                        comboValidTime = playerAnimation._anim.GetCurrentAnimatorStateInfo(0).length + 1;
                    }
                    playerAction.isPlayerAttacking = true;
                }
                else if (comboHit == 3)
                {
                    playerAction.action = ActionType.LightAttackCombo3;
                    if (playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("LT"))
                    {
                        comboValidTime = playerAnimation._anim.GetCurrentAnimatorStateInfo(0).length + 1;
                    }
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
