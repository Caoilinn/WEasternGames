using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AI;
using AI.States;
using UnityEngine;
using UnityTemplateProjects.Animation;
using Utilities;
using Random = UnityEngine.Random;


enum CombatActionType
{
    HeavyAttack,
    LightAttack
}

public class AttackingState : State
{
    private float _dashSpeed = 2f;
    private float _dashTime = 0.2f;
    private float _startDashTime = 0.1f;
    
    private Rigidbody _rigidBody;
    //private Animator _anim;
    private List<int> _attackPatterns;
    private CombatActionType _actionType;
    private EnemyAction _enemyAction;
    private Enemy _enemy;
    private Transform _playerTransform;
    private CapsuleCollider _collider;
    private float _colliderRadius;
    private float _colliderHeight;
    
    private List<AnimationAction> _actions;
    private List<AnimationAction> _actionSequence;
    private int _sequenceCount = 0;
    private int _origionalSequence;


    private float _origY;
    
    private bool _actionFlag;
    private bool _isNextSequenceReady = true;
    private bool _rolling = false;
    
    private bool _isReadyNextAtk = true;
    private float _attackCd;
    private bool _isCdOn = false;

    private bool _seqEnd = false;


    #region Attack Sequences
    
    //Normal Sequences - AI Health >= 50% 
    private readonly int[] _sequence1 = {0, 1, 2, 3};
    private readonly int[] _sequence2 = {0, 0, 2};
    private readonly int[] _sequence3 = {1, 0, 1, 3};
    private readonly int[] _sequence4 = {0, 1, 0, 2, 3};
    private readonly int[] _sequence5 = {0, 1, 0, 2, 3};

    //Aggressive Sequences - AI Health < 50%
    private readonly int[] _sequence6 = {0, 1, 0, 2, 3};
    private readonly int[] _sequence7 = {0, 1, 0, 2, 3};
    private readonly int[] _sequence8 = {0, 1, 0, 2, 3};
    private readonly int[] _sequence9 = {0, 1, 0, 2, 3};
    
    #endregion'

    
    
    //This is how long the AI will remain in this state during combat
    private float _attackStateCountDown;
    
    #region Animation Triggers
    private static readonly int Attack1 = Animator.StringToHash("Attack1");
    private static readonly int Attack2 = Animator.StringToHash("Attack2");
    private static readonly int Attack3 = Animator.StringToHash("Attack3");
    private static readonly int MassiveAttack = Animator.StringToHash("MassiveAttack");
    private static readonly int CombatRoll = Animator.StringToHash("CombatRoll");
    
    private static readonly int TakeDamage = Animator.StringToHash("TakeDamage");
    
    #endregion
    
    public AttackingState(GameObject go, StateMachine sm, List<IAIAttribute> attributes, Animator animator,  int sequence = 0, float timeRemaining = 0) : base(go, sm, attributes, animator)
    {
        //These checks are in place to make sure the attacking state picks back up where it left off in the case of a roll
        if (sequence != 0)
            _sequenceCount = sequence;


        _attackStateCountDown = timeRemaining != 0 ? timeRemaining : Random.Range(5,10);
    }

    public override void Enter()
    {
        base.Enter();
        _collider = _go.GetComponent<CapsuleCollider>();
        _enemyAction = (EnemyAction) _attributes.Find(x => x.GetType() == typeof(EnemyAction));
        _enemy = (Enemy) _attributes.Find(x => x.GetType() == typeof(Enemy));
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _actions = new List<AnimationAction>();
        _actionSequence = new List<AnimationAction>();
        _colliderRadius = _collider.radius;
        _colliderHeight = _collider.height;
        //_rigidBody = _go.GetComponent<Rigidbody>();

        
        AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
       
        foreach (AnimationClip clip in clips)
        {
            if (clip.name.Contains("Attack") || clip.name.Contains("roll"))
            {
                _actions.Add(new AnimationAction(clip));
            }
        }
    }
    
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (_isReadyNextAtk)
        {
            _collider.radius = _colliderRadius;
            _collider.height = _colliderHeight;
            _rolling = false;
            //_rigidBody.velocity = Vector3.zero;
            PerformActions(); 
        }

        if(_rolling)
            DoCombatRoll();
           
        ResetAttackCD();
        
        //Currently Changes state to blocking 
        //TODO: Change how the Attacking State transitions to blocking, make it transition based on health

        if (_seqEnd)
        {
            int action = Random.Range(0,2);

            if (action == 0)
                _sm._CurState = new EvasiveState(_go, _sm, _attributes, _animator);
            else
                _sm._CurState = new BlockingState(_go, _sm, _attributes, _animator);
        }

        if (Vector3.Distance(_playerTransform.position, _go.transform.position) > 3f && !_rolling)
        {
            //Debug.Log("Enter Follow from attack");
            _sm._CurState = new FollowState(_go, _sm, _attributes, _animator, _sequenceCount);
        }
    }

    /// <summary>
    /// Sets up the attack sequence that will be use and retrieves the
    /// action from the sequence array and performs said action
    /// </summary>
    private void PerformActions()
    {
        if (_isNextSequenceReady)
        {
            GetSequence();
            return;
        }

        AnimationAction currentAction = GetNextAction(_actionSequence);

        _isReadyNextAtk = false;
        _isCdOn = true;
        _enemyAction.action = EnemyAction.EnemyActionType.LightAttack;

        float animClipLength = 0f;

        if (_seqEnd)
            return;
        
        switch (currentAction.AnimationClipName)
        {
            case "Attack 1":
                _animator.SetTrigger(Attack1);
                animClipLength = currentAction.AnimationClipLength;
                break;
            case "Attack 2":
                _animator.SetTrigger(Attack2);
                animClipLength = currentAction.AnimationClipLength;
                break;
            case "Attack 3":
                _animator.SetTrigger(Attack3);
                animClipLength = currentAction.AnimationClipLength;
                break;
            case "Attack 4":
                _animator.SetTrigger(MassiveAttack);
                animClipLength = currentAction.AnimationClipLength;
                break;
            case "roll":
                _go.transform.Rotate(new Vector3(0, 90,0));
                _animator.SetTrigger(CombatRoll);
                _collider.radius = 0.51f;
                _collider.height = 0;
                animClipLength = currentAction.AnimationClipLength;
                DoCombatRoll();
                _rolling = true;
                break;
        }
        
        _attackCd = animClipLength;
        _sequenceCount++;
    }

    private AnimationAction GetNextAction(List<AnimationAction> actions)
    {
        if (_sequenceCount < actions.Count)
            return actions[_sequenceCount];
        
        _sequenceCount = 0;
        _seqEnd = true;
        return null;
    }

    /// <summary>
    /// Sets the sequence that the AI will use
    /// </summary>
    private void GetSequence()
    {
        int rnd = Random.Range(0, 3);
        int[] seq = new int[] { };

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

        //Debug.Log("Sequence Start");
        foreach (int atk in seq)
        {
            //Debug.Log(atk);
        }
        
        foreach (int action in seq)
        {
            _actionSequence.Add(_actions[action]);
        }

        _isNextSequenceReady = false;
    }

    private void DoCombatRoll()
    {
        _go.transform.position += _go.transform.forward * (2f * Time.fixedDeltaTime);
    }

    private void DoDash()
    {
        float dashDistance = 5f;
        
        if(_dashTime <= 0)
            _dashTime = _startDashTime;
        else
        {
            _dashTime -= Time.fixedDeltaTime;
            _go.transform.position += _go.transform.forward * _dashSpeed;
        }
    }

    /// <summary>
    /// Resets the animation timer so the next attack can begin
    /// </summary>
    private void ResetAttackCD()
    {
        if (_attackCd > 0 && _isCdOn)
        {
            _attackCd -= Time.fixedDeltaTime;
        }

        if (_attackCd <= 0 && _isCdOn)
        {
            _isCdOn = false;
            _isReadyNextAtk = true;
        }
    }

    /// <summary>
    /// Gets a sequence number based on random numbers and probability  
    /// </summary>
    /// <returns>Sequence Number</returns>
    private int GetSequenceValue()
    {
        float rnd = Random.value;

        if (_enemy.HP >= 50)
        {
            if (rnd <= 0.5f)
                return 1;
            if (rnd <= 0.8f)
                return Random.Range(2, 3);
        
            return Random.Range(4, 5);
        }
        
        if (rnd <= 0.5f)
            return Random.Range(6, 7);
        if (rnd <= 0.8f)
            return 8;

        return 9;
    }
}
