﻿using System.Collections;
using System.Collections.Generic;
using AI;
using AI.States;
using UnityEngine;

public class CombatWalk : State
{
    private Animator _anim;
    private Transform _player;
    private float _moveSpeed;
    private bool _forward;
    
    #region Animation Triggers
    
    private float _zVel;
    private int _zVelHash;

    #endregion
    

    public CombatWalk(GameObject go, StateMachine sm, bool forward) : base(go, sm)
    {
        _forward = forward;
    }

    public override void Enter()
    {
        base.Enter();
        _anim = _go.GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _moveSpeed = 2f;
        _zVelHash = Animator.StringToHash("enemyVelZ");
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        float distanceToPlayer = Vector3.Distance(_go.transform.position, _player.position);

        _zVel = 1;
        _go.transform.LookAt(_player.position);
        
        if(_forward)
            _go.transform.position += _go.transform.forward * _moveSpeed * Time.fixedDeltaTime;
        else
            _go.transform.position -= _go.transform.forward * _moveSpeed * Time.fixedDeltaTime;
        
        
        _anim.SetFloat(_zVelHash, _zVel);

        //The AI is walking toward the player so it will then enter combat again this will also trigger if the player
        //runs after the AI and catches up to them
        if (distanceToPlayer < 1.5)
        {
            _zVel = 0;
            _anim.SetFloat(_zVelHash, _zVel);
            _sm._CurState = new AttackingState(_go, _sm);
        }

        //The AI is walking away from the player to enter an evasive state
        if (distanceToPlayer >= 5.0f)
        {
            _zVel = 0;
            _anim.SetFloat(_zVelHash, _zVel);
            _sm._CurState = new EvasiveState(_go, _sm);
        }
        
    }
}
