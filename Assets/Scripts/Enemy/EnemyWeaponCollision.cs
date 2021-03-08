﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class EnemyWeaponCollision : MonoBehaviour
{
    public Enemy enemy;
    public EnemyAction.EnemyActionType enemyActionType;
    private Collider collider;
    EnemyAction enemyAction;
    public delegate void HitPlayer();
    public event HitPlayer OnHitPlayer;

    void Start()
    {
        enemyAction = enemy.GetComponent<EnemyAction>();

        //enemy = this.transform.root.Find("EnemyHolder/Enemy").gameObject;
    }

    void FixedUpdate()
    {
        enemyActionType = enemyAction.action;
        collider = this.GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // publish event
            OnHitPlayer?.Invoke();

            PlayerAnimation  playerAnimation = collision.gameObject.GetComponent<PlayerAnimation>();
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            PlayerAction playerAction = collision.gameObject.GetComponent<PlayerAction>();

            Enemy enemy = this.enemy.GetComponent<Enemy>();
            bool isInPlayerFov = collision.gameObject.GetComponent<BlockRadius>().EnemyInFOV(enemy);
            //get player perfect block
            if (collision.gameObject.GetComponent<PlayerAction>().isPerfectBlock == true && this.GetComponent<Collider>().isTrigger == false)
            {
                if(isInPlayerFov)
                {
                    TriggerPerfectBlock(collision.gameObject);
                }
                else
                {
                    if(enemyActionType == EnemyAction.EnemyActionType.LightAttack)
                    {
                        playerStats.DecreaseHPStamina(5, 5);
                    }
                    else if(enemyActionType == EnemyAction.EnemyActionType.HeavyAttack)
                    {
                        playerStats.DecreaseHPStamina(10, 10);
                        playerStats.isHitStun = true;
                    }
                    playerAnimation._anim.ResetTrigger("isInjured");
                    playerAnimation._anim.SetTrigger("isInjured");
                    playerStats.readyToRestoreStaminaTime = 5.0f;
                    playerAction.isPlayerAttacking = false;
                }
            }


        }
    }

    public void TriggerPerfectBlock(GameObject swordObject) {
        enemy.GetComponent<EnemyAnimation>()._anim.SetTrigger("getPlayerPerfectBlockImpact");
        // spawn sword clash effect
        swordObject.GetComponent<SwordEffectSpawner>().SpawnBigSwordClash();
    }
}
