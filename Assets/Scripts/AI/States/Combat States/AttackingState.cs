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
    private bool _isNextSequenceReady = true;
    
    private bool isReadyNextATK = true;
    private float AttackCD;
    private bool isCDOn = false;
    
    private readonly int[] _sequence1 = {0, 0, 0};
    private readonly int[] _sequence2 = {1, 1, 1};
    private readonly int[] _sequence3 = {2, 2, 2};
    private readonly int[] _sequence4 = {3, 3, 3};
    
    
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
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

       if (isReadyNextATK) 
           PerformActions();
        
        ResetAttackCD();

        _attackStateCountDown -= Time.fixedDeltaTime;
        
        //Currently Changes state to blocking 
        //TODO: Change how the Attacking State transitions to blocking, make it transition based on health
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
        if (_isNextSequenceReady)
        {
            GetNextSequence();
            return;
        }

        AnimationAction currentAction = GetNextAction(_actionSequence);

        isReadyNextATK = false;
        isCDOn = true;
        _enemyAction.action = EnemyAction2.EnemyActionType.LightAttack;
    
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
            default:
                //Step forward code here 
                break;
        }
    
        float animClipLength = currentAction.AnimationClipLength;
        AttackCD = animClipLength;
        _sequenceCount++;
    }

    private AnimationAction GetNextAction(List<AnimationAction> actions)
    {
        if (_sequenceCount == actions.Count)
        {
            _sequenceCount = 0;
            _isNextSequenceReady = true;
            
            return null;
        }
        
        return actions[_sequenceCount];
    }

    //Returns the next sequence that the AI needs to use
    private void GetNextSequence()
    {
        int rnd = Random.Range(0, 3);
        int[] seq = new int[] { };

        Debug.Log("Getting next sequence: " + rnd);
        switch (rnd)
        {
            case 0:
                seq = _sequence1;
                break;
            case 1:
                seq = _sequence2;
                break;
            case 2:
                seq = _sequence3;
                break;
            case 3:
                seq = _sequence4;
                break;
        }

        _actionSequence.Clear();
        
        foreach (int action in seq)
        {
            _actionSequence.Add(_attackActions[action]);
        }

        _isNextSequenceReady = false;
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
