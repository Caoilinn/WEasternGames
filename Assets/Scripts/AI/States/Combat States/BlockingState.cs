using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using AI.States;
using UnityEngine;

public class BlockingState : State
{
    private Animator _anim;
    private EnemyAction2 _enemyAction;
    private Transform _player;
    private float _blockingCountDown;
    private bool _alreadyBlocking;
    private float _moveSpeed = 1f;
    
    public BlockingState(GameObject go, StateMachine sm) : base(go, sm)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _anim = _go.GetComponent<Animator>();
        _enemyAction = _go.GetComponent<EnemyAction2>();
        _alreadyBlocking = false;
        _blockingCountDown = 5f;
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        //bool isLeft = IsLeft();
        //Move(isLeft);
        
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
            Debug.Log("Exit Block");
            _enemyAction.isKeepBlocking = false;
            _sm._CurState = new AttackingState(_go, _sm);
        }

        if (_alreadyBlocking && Vector3.Distance(_go.transform.position , _player.position) < 2f)
        {
            //Change into stagnant block for a time
        }
    }

    private void DoBlock()
    {
       _enemyAction.isKeepBlocking = true;
       _enemyAction.action = EnemyAction2.EnemyActionType.Block;
    }

    private void Blocking()
    {
        _anim.SetBool("Blocking", true);
        _anim.SetTrigger("Blocking1");
        _anim.SetFloat("EnemyZ", -1);
    }

    //Potentially revisit for blocking based off of the player position
    private void Move(bool left)
    {
        _anim.SetFloat("EnemyX", 0);
        _anim.SetFloat("EnemyZ", 0);
        
   
        Vector3 cross = Vector3.Cross(_go.transform.transform.forward, _go.transform.position - _player.position);
        double crossY = Math.Round(cross.y, 1);
        
        if (crossY ==  0.1)
        {
            Debug.Log("it's 0.1");
            //_anim.SetBool("Blocking", true);
            //_anim.SetTrigger("Blocking1");
            _anim.SetFloat("EnemyX", 1);
        }
        else if(crossY == -0.1)
        {
            Debug.Log("it's -0.1");
            //_anim.SetBool("Blocking", true);
            //_anim.SetTrigger("Blocking1");
            _anim.SetFloat("EnemyX", -1);
        }
        else if(crossY == 0)
        {
            Debug.Log("it's 0");
            _anim.SetFloat("EnemyZ", -1);
        }
    }

    private bool IsLeft()
    {
        Vector3 cross = Vector3.Cross(_go.transform.transform.forward, _go.transform.position - _player.position);
        return cross.y > 0f;
    }
    
}
