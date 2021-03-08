using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTemplateProjects.Utilities;

public class PlayerCollisionCopy : MonoBehaviour
{
    private GameObject player;
    PlayerAnimation playerAnimation;
    PlayerStats playerStats;
    PlayerMovement playerMovement;

    private void Start()
    {
        player = gameObject;
        playerAnimation = player.GetComponent<PlayerAnimation>();
        playerStats = player.GetComponent<PlayerStats>();
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        #region Player Get Enemy Hit
        if (collision.gameObject.tag == "EnemyWeapon")
        {
            AIEnemyWeaponCollision enemyWeaponCollision = collision.gameObject.GetComponent<AIEnemyWeaponCollision>();
            #region Player Blocking Collision Logic
            // player is blocking and get hit by enemy
            if (collision.gameObject.GetComponent<Collider>().isTrigger == false && 
                player.GetComponent<PlayerAction>().isKeepBlocking == true &&
                player.GetComponent<PlayerAction>().isPerfectBlock == false && 
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
                    enemyWeaponCollision.enemyActionType == EnemyAction.EnemyActionType.LightAttack)
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
                        player.GetComponent<SwordEffectSpawner>().SpawnSwordClash();
                    }

                    else if (playerStats.hitStunValue <= 0)
                    {
                        playerStats.DecreaseHPStamina(5, 5);
                        playerStats.readyToRestoreStaminaTime = 5.0f;
                        playerAnimation._anim.ResetTrigger("isInjured");
                        playerAnimation._anim.SetTrigger("isInjured");
                    }
                }
                #endregion
                collision.gameObject.GetComponent<Collider>().isTrigger = true;
                player.GetComponent<PlayerAction>().isPlayerAttacking = false;
            }

            // player is in blocking impact status and get hit
            if (collision.gameObject.GetComponent<Collider>().isTrigger = false &&
                player.GetComponent<PlayerAction>().isKeepBlocking == true &&
                player.GetComponent<PlayerAction>().isPerfectBlock == false &&
                playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("BI"))
            {
                playerStats.hitStunValue -= 20;
                playerAnimation._anim.ResetTrigger("isGetBlockingImpact");
                playerAnimation._anim.SetTrigger("isGetBlockingImpact");
                player.GetComponent<PlayerAction>().isPlayerAttacking = false;
                collision.gameObject.GetComponent<Collider>().isTrigger = true;

                // spawn sword clash effect
                player.GetComponent<SwordEffectSpawner>().SpawnSwordClash();
            }
            #endregion

            // player is not in block action and get hit by enemy (Heavy attack)
            if (enemyWeaponCollision.enemyActionType == EnemyAction.EnemyActionType.HeavyAttack &&
               collision.gameObject.GetComponent<Collider>().isTrigger == false &&
               player.GetComponent<PlayerAction>().isKeepBlocking == false &&
               !playerMovement.isDodging)
            {
                collision.gameObject.GetComponent<Collider>().isTrigger = true;
                playerStats.DecreaseHPStamina(10, 10);   //  actual is 20
                playerStats.readyToRestoreStaminaTime = 5.0f;
                playerMovement.isSprinting = false;
                playerAnimation._anim.ResetTrigger("isInjured");
                playerAnimation._anim.SetTrigger("isInjured");
                playerStats.isHitStun = true;
                player.GetComponent<PlayerAction>().isPlayerAttacking = false;
            }

            // player is not in block action and get hit by enemy  (light attack)
            else if (enemyWeaponCollision.enemyActionType == EnemyAction.EnemyActionType.LightAttack &&
                     collision.gameObject.GetComponent<Collider>().isTrigger == false &&
                     player.GetComponent<PlayerAction>().isKeepBlocking == false &&
                     !playerMovement.isDodging)
            {
                collision.gameObject.GetComponent<Collider>().isTrigger = true;
                playerStats.DecreaseHPStamina(5, 5);  //  actual is 10
                playerStats.readyToRestoreStaminaTime = 5.0f;
                playerAnimation._anim.ResetTrigger("isInjured");
                playerAnimation._anim.SetTrigger("isInjured");
                
                player.GetComponent<PlayerAction>().isPlayerAttacking = false;
            }

            // player is in perfect block Transistion but not in perfect block timing (Heavy attack)
            // (GetCurrentAnimatorStateInfo(0).IsTag("PB")) get current animator state by tag https://forum.unity.com/threads/current-animator-state-name.331803/
            else if ((playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("PB") ||
                playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("A")) && 
                player.GetComponent<PlayerAction>().isPerfectBlock == false &&
                !playerMovement.isDodging &&
                enemyWeaponCollision.enemyActionType == EnemyAction.EnemyActionType.HeavyAttack)
            {
                playerStats.DecreaseHPStamina(10, 10);  //  actual is 20
                playerStats.readyToRestoreStaminaTime = 5.0f;
                playerAnimation._anim.ResetTrigger("isInjured");
                playerAnimation._anim.SetTrigger("isInjured");
                playerStats.isHitStun = true;
                playerMovement.isSprinting = false;
                player.GetComponent<PlayerAction>().isPlayerAttacking = false;
                collision.gameObject.GetComponent<Collider>().isTrigger = true;
            }

            // player is in perfect block Transistion but not in perfect block timing (light attack)
            else if ((playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("PB") ||
                 playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("A")) &&
                player.GetComponent<PlayerAction>().isPerfectBlock == false &&
                player.GetComponent<PlayerAction>().isKeepBlocking == true &&
                !playerMovement.isDodging &&
                enemyWeaponCollision.enemyActionType == EnemyAction.EnemyActionType.LightAttack)
            {
                playerStats.DecreaseHPStamina(5, 5);   //  actual is 10
                playerStats.hitStunValue -= 10;
                playerAnimation._anim.ResetTrigger("isGetBlockingImpact");
                playerAnimation._anim.SetTrigger("isGetBlockingImpact");
                playerStats.readyToRestoreStaminaTime = 5.0f;
                playerMovement.isSprinting = false;
                player.GetComponent<PlayerAction>().isPlayerAttacking = false;
                collision.gameObject.GetComponent<Collider>().isTrigger = true;
                player.GetComponent<SwordEffectSpawner>().SpawnSwordClash();
            }

            else if ((playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("HT") || 
                 playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("LT")))
            {
                player.GetComponent<PlayerAction>().isPlayerAttacking = false;
            }

            else if(playerAnimation._anim.GetCurrentAnimatorStateInfo(0).IsTag("GH"))
            {
                if(enemyWeaponCollision.enemyActionType == EnemyAction.EnemyActionType.LightAttack)
                {
                    playerStats.DecreaseHPStamina(5, 5);   //  actual is 10
                }
                else if (enemyWeaponCollision.enemyActionType == EnemyAction.EnemyActionType.HeavyAttack)
                {
                    playerStats.DecreaseHPStamina(10, 10);   //  actual is 20
                    playerStats.isHitStun = true;
                }
                playerAnimation._anim.ResetTrigger("isInjured");
                playerAnimation._anim.SetTrigger("isInjured");
                playerStats.readyToRestoreStaminaTime = 5.0f;
                player.GetComponent<PlayerAction>().isPlayerAttacking = false;
            }

            playerAnimation._anim.ResetTrigger("isPlayerLightAttack");
            playerAnimation._anim.ResetTrigger("isPlayerHeavyAttack");
            playerAnimation._anim.ResetTrigger("Dodge");
#endregion
        }
    }


}
