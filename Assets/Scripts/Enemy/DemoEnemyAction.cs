using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class DemoEnemyAction : MonoBehaviour
{
    public enum EnemyActionType
    {
        Idle,
        LightAttack,
        HeavyAttack,
        Block,
        PerfectBlockOnly,
        EnterInjured,
        Injured
    }

    public bool demo;

    public EnemyActionType action;

    private Animator _anim;
    EnemyBehaviour enemyBehaviour;
    
    
    public bool isPerfectBlock = false;
    public bool isKeepBlocking = false;
    public bool isInPerfectBlockOnly = false;
    
    #region Debug
    
    private void Start()
    {
        if (demo)
        {
            _anim = GetComponent<Animator>();
            _anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("AnimationController/EnemyAnimator"); //Load controller at runtime https://answers.unity.com/questions/1243273/runtimeanimatorcontroller-not-loading-from-script.html
        }
    }

    private void Awake()
    {
        enemyBehaviour = GetComponent<EnemyBehaviour>();
    }

    void Update()
    {
        if (demo)
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
    }
    
    private void HeavyAttack()
    {
        _anim.SetTrigger("HeavyAttack");
        isInPerfectBlockOnly = false;
    }

    private void LightAttack()
    {
        _anim.SetTrigger("LightAttack");
        isInPerfectBlockOnly = false;
    }

    private void Block()
    {
        isKeepBlocking = true;
    }

    private void PBlockOnly()
    {
        _anim.SetTrigger("PerfectBlockOnly");
        isInPerfectBlockOnly = true;
    }
    
    #endregion
}
