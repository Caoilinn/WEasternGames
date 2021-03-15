using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;

namespace Utilities
{
    public class EnemyAction : MonoBehaviour, IAIAttribute
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

        public bool Demo;
    
        public EnemyActionType action;
        public EnemyActionType debugAction;
    
        private Animator _anim;
        EnemyBehaviour enemyBehaviour;
        public bool isPerfectBlock = false;
        public bool isKeepBlocking = false;
        public bool isInPerfectBlockOnly = false;
    
        #region Demo
        
        private void Start()
        {
            _anim = GetComponent<Animator>();
        }
    
        private void Awake()
        {
            enemyBehaviour = GetComponent<EnemyBehaviour>();
        }
    
        void Update()
        {
            switch (debugAction)
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
    
        void HeavyAttack()
        {
            _anim.SetTrigger("HeavyAttack");
            isInPerfectBlockOnly = false;
        }
    
        void LightAttack()
        {
            //Debug.Log("Light Attack Playing");
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
        
        #endregion
    }
}

