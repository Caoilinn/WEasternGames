using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;
using Utilities;

public class BlockingState : State
{
    private EnemyAction _enemyAction;
    private Transform _player;
    private float _blockingCountDown;
    private bool _alreadyBlocking;
    private float _moveSpeed = 1f;
    
    public BlockingState(GameObject go, StateMachine sm, List<IAIAttribute> attributes, Animator animator) : base(go, sm, attributes, animator)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _enemyAction = (EnemyAction) _attributes.Find(x => x.GetType() == typeof(EnemyAction));
        _alreadyBlocking = false;
        _blockingCountDown = 5f;
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (!_alreadyBlocking)
        {
            _alreadyBlocking = true; 
            Blocking();
        }

        if (_alreadyBlocking)
        {
            _go.transform.LookAt(_player.position);
            _go.transform.position -= _go.transform.forward * (_moveSpeed * Time.fixedDeltaTime);
        }
        
        _blockingCountDown -= Time.fixedDeltaTime;

        if (_blockingCountDown <= 0)
        {
            _enemyAction.isKeepBlocking = false;
            _sm._CurState = new AttackingState(_go, _sm, _attributes, _animator);
        }

        if (_alreadyBlocking && Vector3.Distance(_go.transform.position , _player.position) < 2f)
        {
            //Change into stagnant block for a time
        }
    }

    private void DoBlock()
    {
       _enemyAction.isKeepBlocking = true;
       _enemyAction.action = EnemyAction.EnemyActionType.Block;
    }

    private void Blocking()
    {
        _animator.SetBool("Blocking", true);
        _animator.SetTrigger("Blocking1");
        _animator.SetFloat("EnemyZ", -1);
    }

    //Potentially revisit for blocking based off of the player position
    private void Move(bool left)
    {
        _animator.SetFloat("EnemyX", 0);
        _animator.SetFloat("EnemyZ", 0);
        
   
        Vector3 cross = Vector3.Cross(_go.transform.transform.forward, _go.transform.position - _player.position);
        double crossY = Math.Round(cross.y, 1);
        
        if (crossY ==  0.1)
        {
            //_anim.SetBool("Blocking", true);
            //_anim.SetTrigger("Blocking1");
            _animator.SetFloat("EnemyX", 1);
        }
        else if(crossY == -0.1)
        {
           //_anim.SetBool("Blocking", true);
            //_anim.SetTrigger("Blocking1");
            _animator.SetFloat("EnemyX", -1);
        }
        else if(crossY == 0)
        {
            _animator.SetFloat("EnemyZ", -1);
        }
    }

    private bool IsLeft()
    {
        Vector3 cross = Vector3.Cross(_go.transform.transform.forward, _go.transform.position - _player.position);
        return cross.y > 0f;
    }
    
}
