using System.Collections;
using System.Collections.Generic;
using AI;
using AI.States;
using UnityEngine;
using UnityTemplateProjects.Animation;


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
    private EnemyAction2 _enemyAction;
    private Transform _playerTransform;
    private List<AnimationAction> _attackActions;
    private List<AnimationAction> _actionSequence;
    private int _sequenceCount = 0;

    private bool _actionFlag;
    
    private const float AttackCDVal = 2f; 
    private bool isReadyNextATK = true;
    private float AttackCD;
    private bool isCDOn = false;
    
    
    //This is how long the AI will remain in this state during combat
    private float _attackStateCountDown;
    
    #region Animation Triggers

    private static readonly int LightAttack = Animator.StringToHash("LightAttack");
    private static readonly int HeavyAttack = Animator.StringToHash("HeavyAttack");
    
    
    private static readonly int Attack1 = Animator.StringToHash("Attack1");
    private static readonly int Attack2 = Animator.StringToHash("Attack2");
    private static readonly int Attack3 = Animator.StringToHash("Attack3");
    private static readonly int MassiveAttack = Animator.StringToHash("MassiveAttack");
    private static readonly int TakeDamage = Animator.StringToHash("TakeDamage");
    
    #endregion
    
    public AttackingState(GameObject go, StateMachine sm) : base(go, sm)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _anim = _go.GetComponent<Animator>();
        _enemyAction = _go.GetComponent<EnemyAction2>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _rnd = new Random();
        _attackStateCountDown = 7f;
        _attackActions = new List<AnimationAction>();
        _actionSequence = new List<AnimationAction>();
        
        AnimationClip[] clips = _anim.runtimeAnimatorController.animationClips;
       
        foreach (var clip in clips)
        {
            if (clip.name.Contains("Attack"))
            {
               _attackActions.Add(new AnimationAction(clip));
            }
        }
        
        int[] sequence = {2, 1, 3};

        foreach (var t in sequence)
        {
            _actionSequence.Add(_attackActions[t]);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

       if (isReadyNextATK)
        {
            /*int action = Random.Range(0,2);
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

            }*/
            
            PerformActions();
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


    private void PerformActions()
    {
        AnimationAction currentAction = GetNextAction(_actionSequence);

        isReadyNextATK = false;
        isCDOn = true;
        _enemyAction.action = EnemyAction2.EnemyActionType.LightAttack;
        
        Debug.Log(currentAction.AnimationClipName);
        
        switch (currentAction.AnimationClipName)
        {
            case "Attack 1":
                _anim.SetTrigger(Attack1);
                break;
            case "Attack 2":
                _anim.SetTrigger(Attack2);
                break;
            case "Attack 3":
                _anim.SetTrigger(Attack3);
                break;
            case "Attack 4":
                _anim.SetTrigger(MassiveAttack);
                break;
        }
        
        float animClipLength = currentAction.AnimationClipLength;
        AttackCD = animClipLength;
        _sequenceCount++;
    }

    private AnimationAction GetNextAction(List<AnimationAction> actions)
    {
        if (_sequenceCount == 3)
            _sequenceCount = 0;

        return actions[_sequenceCount];
    }

    private void DoLightAttack()
    {
        isReadyNextATK = false;
        isCDOn = true;
        _anim.SetTrigger(MassiveAttack);
        _enemyAction.action = EnemyAction2.EnemyActionType.LightAttack;

        AnimationClip animClipLength = _anim.GetCurrentAnimatorClipInfo(0)[0].clip;
        
        //Debug.Log("Clip Name: " + animClipLength.name);
        //Debug.Log("Clip Length: " + animClipLength.length);
        
        AttackCD = animClipLength.length;
        
        
        /*
        AnimationClip[] clips = _anim.runtimeAnimatorController.animationClips;
         
        foreach (AnimationClip clip in clips)
        {
            Debug.Log("Clip Name: " + clip.name + " Clip Length: " + clip.length);
        }*/
        
    }

    private void DoHeavyAttack()
    {
        isReadyNextATK = false;
        isCDOn = true;
        AttackCD = AttackCDVal;
        _anim.SetTrigger(HeavyAttack);
        _enemyAction.action = EnemyAction2.EnemyActionType.HeavyAttack;
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
