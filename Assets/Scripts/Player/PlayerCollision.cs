using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    PlayerAnimation playerAnimation;
    PlayerStats playerStats;
    PlayerMovementV2 playerMovement;
    PlayerAction playerAction;
    BlockRadius playerFieldOfView;
    PlayerControl playerControl;
    public delegate void HitPlayer();
    public event HitPlayer OnHitPlayer;

    void Awake()
    {
        playerAnimation = this.GetComponent<PlayerAnimation>();
        playerStats = this.GetComponent<PlayerStats>();
        playerMovement = this.GetComponent<PlayerMovementV2>();
        playerAction = this.GetComponent<PlayerAction>();
        playerFieldOfView = this.GetComponent<BlockRadius>();
        playerControl = this.GetComponent<PlayerControl>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        #region Player Get Enemy Hit
        if (collision.gameObject.tag == "EnemyWeapon" && !playerStats.isDeath)
        {
            Enemy enemy = collision.gameObject.GetComponent<EnemyWeaponCollision>().enemy.GetComponent<Enemy>();
            EnemyWeaponCollision enemyWeaponCollision = collision.gameObject.GetComponent<EnemyWeaponCollision>();
            bool isInPlayerFov = this.GetComponent<BlockRadius>().EnemyInFOV(enemy);
            #region Player Blocking Collision Logic
            // player is blocking and get hit by enemy
            if (collision.gameObject.GetComponent<Collider>().isTrigger == false &&
                playerAction.isKeepBlocking == true &&
                playerAction.isPerfectBlock == false &&
                playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("B"))
            {

                #region get enemy heavy attack
                if (playerStats.hitStunValue > 0 &&
                    enemyWeaponCollision.enemyActionType == EnemyAction.EnemyActionType.HeavyAttack)
                {
                    playerStats.hitStunValue -= 100;
                    if (playerStats.hitStunValue <= 0)
                    {
                        playerStats.DecreaseHPStamina(20, 20);
                        playerStats.readyToRestoreStaminaTime = 5.0f;
                        playerAnimation._anim.ResetTrigger("isInjured");
                        playerAnimation._anim.SetTrigger("isInjured");
                        playerStats.isHitStun = true;
                    }
                }
                #endregion

                #region get enemy light attack
                //get enemy light attack
                if (playerStats.hitStunValue > 0 &&
                    enemyWeaponCollision.enemyActionType == EnemyAction.EnemyActionType.LightAttack &&
                    isInPlayerFov)
                {
                    playerStats.DecreaseHPStamina(1.25f, 1.25f);
                    playerStats.hitStunValue -= 20;
                    playerStats.hitStunRestoreSecond = 5.0f;
                    playerStats.readyToRestoreStaminaTime = 5.0f;

                    if (playerStats.hitStunValue > 0)
                    {
                        playerAnimation._anim.ResetTrigger("isGetBlockingImpact");
                        playerAnimation._anim.SetTrigger("isGetBlockingImpact");

                        // spawn sword clash effect
                        this.GetComponent<SwordEffectSpawner>().SpawnSwordClash();
                    }

                    else if (playerStats.hitStunValue <= 0)
                    {
                        playerStats.DecreaseHPStamina(5, 5);
                        playerStats.readyToRestoreStaminaTime = 5.0f;
                        playerAnimation._anim.ResetTrigger("isInjured");
                        playerAnimation._anim.SetTrigger("isInjured");
                    }
                }
                else if (playerStats.hitStunValue > 0 &&
                        enemyWeaponCollision.enemyActionType == EnemyAction.EnemyActionType.LightAttack &&
                        !isInPlayerFov)
                {
                    collision.gameObject.GetComponent<Collider>().isTrigger = true;
                    playerStats.DecreaseHPStamina(5, 5);  //  actual is 10
                    playerStats.readyToRestoreStaminaTime = 5.0f;
                    playerAnimation._anim.ResetTrigger("isInjured");
                    playerAnimation._anim.SetTrigger("isInjured");
                }
                #endregion
                playerAction.isPlayerAttacking = false;
                playerControl.comboHit = 0;
                playerControl.comboValidTime = 0;
            }

            // player is in blocking impact status and get hit
            if (collision.gameObject.GetComponent<Collider>().isTrigger = false &&
                playerAction.isKeepBlocking == true &&
                playerAction.isPerfectBlock == false &&
                playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("BI"))
            {
                if(isInPlayerFov)
                {
                    playerStats.hitStunValue -= 20;
                    playerAnimation._anim.ResetTrigger("isGetBlockingImpact");
                    playerAnimation._anim.SetTrigger("isGetBlockingImpact");
                    playerAction.isPlayerAttacking = false;
                    collision.gameObject.GetComponent<Collider>().isTrigger = true;
                    // spawn sword clash effect
                    this.GetComponent<SwordEffectSpawner>().SpawnSwordClash();
                }
                else
                {
                    if(enemyWeaponCollision.enemyActionType == EnemyAction.EnemyActionType.LightAttack)
                    {
                        collision.gameObject.GetComponent<Collider>().isTrigger = true;
                        playerStats.DecreaseHPStamina(5, 5);
                        playerStats.readyToRestoreStaminaTime = 5.0f;
                        playerAnimation._anim.ResetTrigger("isInjured");
                        playerAnimation._anim.SetTrigger("isInjured");

                        playerAction.isPlayerAttacking = false;
                    }
                    else if(enemyWeaponCollision.enemyActionType == EnemyAction.EnemyActionType.HeavyAttack)
                    {
                        playerStats.DecreaseHPStamina(10, 10);
                        playerStats.readyToRestoreStaminaTime = 5.0f;
                        playerAnimation._anim.ResetTrigger("isInjured");
                        playerAnimation._anim.SetTrigger("isInjured");
                        playerStats.isHitStun = true;
                        playerMovement.isRunning = false;
                        playerAction.isPlayerAttacking = false;
                        collision.gameObject.GetComponent<Collider>().isTrigger = true;
                    }
                }
                playerControl.comboHit = 0;
                playerControl.comboValidTime = 0;
            }
            #endregion

            OnHitPlayer?.Invoke();

            // player is not in block action and get hit by enemy (Heavy attack)
            if (enemyWeaponCollision.enemyActionType == EnemyAction.EnemyActionType.HeavyAttack &&
               collision.gameObject.GetComponent<Collider>().isTrigger == false &&
               playerAction.isKeepBlocking == false &&
               !playerMovement.isDodging)
            {
                collision.gameObject.GetComponent<Collider>().isTrigger = true;
                playerStats.DecreaseHPStamina(10, 10); 
                playerStats.readyToRestoreStaminaTime = 5.0f;
                playerMovement.isRunning = false;
                playerAnimation._anim.ResetTrigger("isInjured");
                playerAnimation._anim.SetTrigger("isInjured");
                playerStats.isHitStun = true;
                playerAction.isPlayerAttacking = false;
                playerControl.comboHit = 0;
                playerControl.comboValidTime = 0;
            }

            // player is not in block action and get hit by enemy  (light attack)
            else if (enemyWeaponCollision.enemyActionType == EnemyAction.EnemyActionType.LightAttack &&
                     collision.gameObject.GetComponent<Collider>().isTrigger == false &&
                     playerAction.isKeepBlocking == false &&
                     !playerMovement.isDodging)
            {
                collision.gameObject.GetComponent<Collider>().isTrigger = true;
                playerStats.DecreaseHPStamina(5, 5); 
                playerStats.readyToRestoreStaminaTime = 5.0f;
                playerAnimation._anim.ResetTrigger("isInjured");
                playerAnimation._anim.SetTrigger("isInjured");
                playerAction.isPlayerAttacking = false;
                playerControl.comboHit = 0;
                playerControl.comboValidTime = 0;
            }

            // player is in perfect block Transistion but not in perfect block timing (Heavy attack)
            // (GetCurrentAnimatorStateInfo(0).IsTag("PB")) get current animator state by tag https://forum.unity.com/threads/current-animator-state-name.331803/
            else if ((playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("PB") ||
                playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("A")) &&
                playerAction.isPerfectBlock == false &&
                !playerMovement.isDodging &&
                enemyWeaponCollision.enemyActionType == EnemyAction.EnemyActionType.HeavyAttack)
            {
                playerStats.DecreaseHPStamina(10, 10); 
                playerStats.readyToRestoreStaminaTime = 5.0f;
                playerAnimation._anim.ResetTrigger("isInjured");
                playerAnimation._anim.SetTrigger("isInjured");
                playerStats.isHitStun = true;
                playerMovement.isRunning = false;
                playerAction.isPlayerAttacking = false;
                playerControl.comboHit = 0;
                playerControl.comboValidTime = 0;
                collision.gameObject.GetComponent<Collider>().isTrigger = true;
            }

            // player is in perfect block Transistion but not in perfect block timing (light attack)
            else if ((playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("PB") ||
                 playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("A")) &&
                playerAction.isPerfectBlock == false &&
                playerAction.isKeepBlocking == true &&
                !playerMovement.isDodging &&
                enemyWeaponCollision.enemyActionType == EnemyAction.EnemyActionType.LightAttack)
            {
                if(isInPlayerFov)
                {
                    playerStats.DecreaseHPStamina(5, 5); 
                    playerStats.hitStunValue -= 10;
                    playerAnimation._anim.ResetTrigger("isGetBlockingImpact");
                    playerAnimation._anim.SetTrigger("isGetBlockingImpact");
                    playerStats.readyToRestoreStaminaTime = 5.0f;
                    playerMovement.isRunning = false;
                    playerAction.isPlayerAttacking = false;
                    collision.gameObject.GetComponent<Collider>().isTrigger = true;
                    this.GetComponent<SwordEffectSpawner>().SpawnSwordClash();
                }
                else
                {
                    collision.gameObject.GetComponent<Collider>().isTrigger = true;
                    playerStats.DecreaseHPStamina(5, 5); 
                    playerStats.readyToRestoreStaminaTime = 5.0f;
                    playerAnimation._anim.ResetTrigger("isInjured");
                    playerAnimation._anim.SetTrigger("isInjured");

                    playerAction.isPlayerAttacking = false;
                }
                playerControl.comboHit = 0;
                playerControl.comboValidTime = 0;
            }

            else if ((playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("HT") ||
                 playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("LT")))
            {
                playerAction.isPlayerAttacking = false;
                playerControl.comboHit = 0;
                playerControl.comboValidTime = 0;
            }

            else if (playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("GH"))
            {
                if (enemyWeaponCollision.enemyActionType == EnemyAction.EnemyActionType.LightAttack)
                {
                    playerStats.DecreaseHPStamina(5, 5); 
                }
                else if (enemyWeaponCollision.enemyActionType == EnemyAction.EnemyActionType.HeavyAttack)
                {
                    playerStats.DecreaseHPStamina(10, 10); 
                    playerStats.isHitStun = true;
                }
                playerAnimation._anim.ResetTrigger("isInjured");
                playerAnimation._anim.SetTrigger("isInjured");
                playerStats.readyToRestoreStaminaTime = 5.0f;
                playerAction.isPlayerAttacking = false;
                playerControl.comboHit = 0;
                playerControl.comboValidTime = 0;
            }
            #endregion
        }
    }


}
