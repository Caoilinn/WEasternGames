﻿using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;


enum CombatActionType
{
    HeavyAttack,
    LightAttack
}

public class AttackingState : State
{
    private Animator _anim;
    private List<int> _attackPatterns;
    private Random _rnd;

    private const float AttackCDVal = 2f; 
    private bool isReadyNextATK = true;
    private float AttackCD;
    private bool isCDOn = false;
    private CombatActionType _actionType;
    
    //This is how long the AI will remain in this state during combat
    private float _attackStateCountDown;
    
    #region Animation Triggers

    private static readonly int LightAttack = Animator.StringToHash("LightAttack");
    private static readonly int HeavyAttack = Animator.StringToHash("HeavyAttack");

    #endregion
    
    public AttackingState(GameObject go, StateMachine sm) : base(go, sm)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _anim = _go.GetComponent<Animator>();
        _rnd = new Random();
        _attackStateCountDown = 10f;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (isReadyNextATK)
        {
            int action = Random.Range(0,2);
            Debug.Log(action);
            _actionType = (CombatActionType) action;

            switch (_actionType)
            {
                case CombatActionType.HeavyAttack:
                    DoHeavyAttack();
                    break;
                case CombatActionType.LightAttack:
                    DoLightAttack();
                    break;

            }
        }
        ResetAttackCD();

        _attackStateCountDown -= Time.fixedDeltaTime;
        
        if (_attackStateCountDown <= 0)
        {
            _sm._CurState = new CombatFollow(_go, _sm, false);
        }
    }

    private void DoLightAttack()
    {
        isReadyNextATK = false;
        isCDOn = true;
        AttackCD = AttackCDVal;
        _anim.SetTrigger(LightAttack);
    }

    private void DoHeavyAttack()
    {
        isReadyNextATK = false;
        isCDOn = true;
        AttackCD = AttackCDVal;
        _anim.SetTrigger(HeavyAttack);
    }
    
    private void ResetAttackCD()
    {
        if (AttackCD > 0 && isCDOn)
        {
            AttackCD -= Time.fixedDeltaTime;
        }

        if (AttackCD <= 0 && isCDOn)
        {
            isCDOn = false;
            isReadyNextATK = true;
        }
    }
    
}
