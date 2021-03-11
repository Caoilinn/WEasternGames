using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTemplateProjects.Animation;
using Utilities;

public class InjuredState : State
{
    private Animator _anim;
    private AnimationAction _injuredAction;
    
    
    private bool _complete = false;
    private float _animTime;
    private EnemyAction _enemyAction;
    private static readonly int IsInjured = Animator.StringToHash("isInjured");

    public InjuredState(GameObject go, StateMachine sm) : base(go, sm)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _anim = _go.GetComponent<Animator>();
        _enemyAction = _go.GetComponent<EnemyAction>();
        
        _enemyAction.action = EnemyAction.EnemyActionType.Injured;
        
        foreach (AnimationClip clip in _anim.runtimeAnimatorController.animationClips)
        {
            if (!clip.name.Contains("Injured")) continue;
            
            _injuredAction = new AnimationAction(clip);
            _animTime = _injuredAction.AnimationClipLength;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if(!_complete)
            PlayInjured();
        
        if (_complete)
            _sm._CurState = new AttackingState(_go, _sm);
    }

    private void PlayInjured()
    {
        _anim.SetTrigger(IsInjured);
        _animTime -= Time.fixedDeltaTime;

        if (_animTime <= 0f)
            _complete = true;
    }
}
