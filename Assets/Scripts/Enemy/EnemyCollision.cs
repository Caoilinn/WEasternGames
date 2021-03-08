using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTemplateProjects.Utilities;

public class EnemyCollision : MonoBehaviour
{
    private bool _isInjured;
    private EnemyAction.EnemyActionType _enemyActionType;
    private EnemyAction _enemyAction;

    private void Start()
    {
        _isInjured = false;
        _enemyAction = this.GetComponent<EnemyAction>();
    }

    private void FixedUpdate()
    {
        _enemyActionType = _enemyAction.action;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("PlayerWeapon")) return;
        
        Debug.Log("Collided");
        _isInjured = true;
            
        //Stops repeated stun locking
        if(_enemyAction.action != EnemyAction.EnemyActionType.Injured || 
           _enemyAction.action != EnemyAction.EnemyActionType.EnterInjured)
            _enemyAction.action = EnemyAction.EnemyActionType.EnterInjured;
    }
}
