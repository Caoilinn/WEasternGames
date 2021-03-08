using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator _anim;
    public RuntimeAnimatorController animatorController;
    private PlayerAction playerAction;
    private PlayerControl playerControl;
    private PlayerJump playerJump;
    private DoubleJump doubleJump;
    private PlayerStats playerStats;
    private AnimatorClipInfo[] clipInfo;
    public Collider collider;
    private float oldSpeed;
    PlayerMovementV2 playerMovementV2;
    Rigidbody rigidbody;

    void Start()
    {
        _anim = GetComponent<Animator>();
        _anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("AnimationController/PlayerAnimator"); //Load controller at runtime https://answers.unity.com/questions/1243273/runtimeanimatorcontroller-not-loading-from-script.html
        _anim.runtimeAnimatorController = animatorController; //Load controller at runtime https://answers.unity.com/questions/1243273/runtimeanimatorcontroller-not-loading-from-script.html
        playerAction = GetComponent<PlayerAction>();
        playerControl = GetComponent<PlayerControl>();
        playerJump = GetComponent<PlayerJump>();
        doubleJump = GetComponent<DoubleJump>();
        playerStats = GetComponent<PlayerStats>();
        playerMovementV2 = GetComponent<PlayerMovementV2>();
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        initialiseAnimatorBool();
        stopDodging();
        playDeathAnimation();
    }

    private void playDeathAnimation()
    {
        if(playerStats.playDeathOnce)
        {
            _anim.SetTrigger("isPlayerDead");
            playerStats.playDeathOnce = false;
            //rigidbody.useGravity = true;
        }
    }

    private void stopDodging()
    {
        if(_anim.GetCurrentAnimatorStateInfo(0).IsTag("GH") || _anim.GetCurrentAnimatorStateInfo(0).IsTag("GEPB") || _anim.GetCurrentAnimatorStateInfo(0).IsTag("BI"))
        {
            playerMovementV2.isDodging = false;
        }
    }

    void initialiseAnimatorBool()
    {
        #region Player Block
        _anim.SetBool("isKeepBlocking", playerAction.isKeepBlocking);
        _anim.SetBool("isPerfectBlock", playerAction.isPerfectBlock);
        _anim.SetBool("isAttackTriggered", collider.isTrigger);
        #endregion

        #region Jump
        // _anim.SetBool("isSecondJump", doubleJump.isDoubleJump);
        // _anim.SetBool("isFalling", playerJump.isFalling);
        // _anim.SetBool("isGrounded", playerJump.isGrounded);
        // _anim.SetBool("FallingToGround", playerJump.fallingToGround);
        // _anim.SetInteger("jumpTimes", playerJump.jumpTimes);
        #endregion

        #region Sprint
        _anim.SetBool("isRunning", playerMovementV2.isRunning);
        _anim.SetBool("isDodging", playerMovementV2.isDodging);
        _anim.SetBool("isHitStun", playerStats.isHitStun);
        _anim.SetBool("isBlockStun", playerStats.isBlockStun);
        _anim.SetBool("moveKeyPressed", playerMovementV2.moveKeyPressed);
        _anim.SetFloat("comboValidTime", playerControl.comboValidTime);
        _anim.SetInteger("comboHit", playerControl.comboHit);
        #endregion
    }

    #region Player Block Logic
    public void OnAnimation_isPerfectBlock()
    {
        playerAction.isPerfectBlock = true;
    }

    public void OnAnimation_isPerfectBlockEnd()
    {
        playerAction.isPerfectBlock = false;
    }

    public void OnAnimation_isBlockStart()
    {
        playerAction.isKeepBlocking = true;
    }

    public void OnAnimation_isBlockEnd()
    {
        playerAction.isKeepBlocking = false;
    }
    #endregion

    #region Player Attack Logic 
    public void OnAnimation_IsHeavyAttackActive()
    {
        collider.isTrigger = false;
    }

    public void OnAnimation_IsHeavyAttackDeactive()
    {
        collider.isTrigger = true;
    }

    public void OnAnimation_isHeavyAttacking()
    {

    }

    public void OnAnimation_isHeavyAttackingEnd()
    {
        playerAction.isPlayerAttacking = false;
    }

    public void OnAnimation_IsLightAttackActive()
    {
        collider.isTrigger = false;
    }

    public void OnAnimation_IsLightAttackDeactive()
    {
        collider.isTrigger = true;
    }

    public void OnAnimation_isLightAttacking()
    {

    }

    public void OnAnimation_isLightAttackingEnd()
    {
        playerAction.isPlayerAttacking = false;
    }

    public void OnAnimation_isLastLightAttackEnd()
    {
        playerAction.isPlayerAttacking = false;
        playerControl.comboHit = 0;
    }
    #endregion

    #region Player Get Hurt Logic
    public void OnAnimation_isGetCriticalHit()
    {
        playerMovementV2.setSpeedDebuffTime(0.5f);
        playerMovementV2.isDodging = false;
    }

    public void OnAnimation_isStunFinished()
    {
        playerStats.isHitStun = false;
    }

    public void OnAnimation_isBlockStun()
    {
        playerMovementV2.GetComponent<PlayerMovementV2>().isDodging = false;
    }
    #endregion

    #region Player Dodge
    public void OnAnimation_isDodging()
    {
        playerMovementV2.isDodging = false;
    }
    #endregion


}
