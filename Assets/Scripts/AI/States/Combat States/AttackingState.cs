using System.Collections;
using System.Collections.Generic;
using AI;
using AI.States;
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
    private CombatActionType _actionType;
    private EnemyAction _enemyAction;
    private Transform _playerTransform;

    private const float AttackCDVal = 2f; 
    private bool isReadyNextATK = true;
    private float AttackCD;
    private bool isCDOn = false;
    
    
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
        _enemyAction = _go.GetComponent<EnemyAction>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _rnd = new Random();
        _attackStateCountDown = 7f;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (isReadyNextATK)
        {
            int action = Random.Range(0,2);
            _actionType = (CombatActionType) action;

            switch (_actionType)
            {
                case CombatActionType.HeavyAttack:
                    // DoHeavyAttack();
                    DoLightAttack();
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
            int action = Random.Range(0,2);

            if (action == 0)
                _sm._CurState = new CombatWalk(_go, _sm, false);
            else
                _sm._CurState = new BlockingState(_go, _sm);
        }

        if (Vector3.Distance(_playerTransform.position, _go.transform.position) > 3f)
        {
            _sm._CurState = new FollowState(_go, _sm);
        }

    }

    private void DoLightAttack()
    {
        isReadyNextATK = false;
        isCDOn = true;
        AttackCD = AttackCDVal;
        _anim.SetTrigger(LightAttack);
        _enemyAction.action = EnemyAction.EnemyActionType.LightAttack;
    }

    private void DoHeavyAttack()
    {
        isReadyNextATK = false;
        isCDOn = true;
        AttackCD = AttackCDVal;
        _anim.SetTrigger(HeavyAttack);
        _enemyAction.action = EnemyAction.EnemyActionType.HeavyAttack;
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
