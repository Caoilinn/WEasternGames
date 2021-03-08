using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction : UnityTemplateProjects.Utilities.EnemyAction
{
    public enum EnemyActionType
    {
        Idle,
        LightAttack,
        HeavyAttack,
        Block,
        PerfectBlockOnly,
    }

    public EnemyActionType action;

    private Animator _anim;
    EnemyBehaviour enemyBehaviour;
    public bool isPerfectBlock = false;
    public bool isKeepBlocking = false;
    public bool isInPerfectBlockOnly = false;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("AnimationController/EnemyAnimator"); //Load controller at runtime https://answers.unity.com/questions/1243273/runtimeanimatorcontroller-not-loading-from-script.html
    }

    private void Awake()
    {
        enemyBehaviour = GetComponent<EnemyBehaviour>();
    }

    void Update()
    {

        switch (action)
        {
            case EnemyActionType.Idle:
                isInPerfectBlockOnly = false;
                isKeepBlocking = false;
                break;

            case EnemyActionType.LightAttack:
                LightAttack();
                isKeepBlocking = false;
                break;

            case EnemyActionType.HeavyAttack:
                HeavyAttack();
                isKeepBlocking = false;
                break;
            case EnemyActionType.Block:
                Block();
                break;
            case EnemyActionType.PerfectBlockOnly:
                PBlockOnly();
                isKeepBlocking = false;
                break;
        }
    }

    void override HeavyAttack()
    {
        _anim.SetTrigger("HeavyAttack");
        isInPerfectBlockOnly = false;
    }

    void LightAttack()
    {
        _anim.SetTrigger("LightAttack");
        isInPerfectBlockOnly = false;
    }

    void Block()
    {
        isKeepBlocking = true;
    }
    void PBlockOnly()
    {
        _anim.SetTrigger("PerfectBlockOnly");
        isInPerfectBlockOnly = true;
    }
}
