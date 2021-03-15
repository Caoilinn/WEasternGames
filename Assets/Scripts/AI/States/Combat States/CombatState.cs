using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Utilities;

namespace AI.States
{
    enum CombatActionType
    {
        HeavyAttack,
        LightAttack,
        Defend
    }
    public class CombatState : State
    {
        private Random _rnd;
        private EnemyAction _enemyAction;
        private FieldOfView _fieldOfView;
        private State _previous;
        
        private const float AttackCDVal = 2f; 
        private bool isReadyNextATK = true;
        private float AttackCD;
        private bool isCDOn = false;
        private CombatActionType _actionType;

        #region Animation Trggers
        private static readonly int Block = Animator.StringToHash("Block");
        private static readonly int Attack = Animator.StringToHash("LightAttack");
        private static readonly int HeavyAttack1 = Animator.StringToHash("HeavyAttack");
        #endregion
        
        public CombatState(GameObject go, StateMachine sm, List<IAIAttribute> attributes, Animator animator, State previous) : base(go, sm, attributes, animator)
        {
            _previous = previous;
        }

        public override void Enter()
        {
            base.Enter();
            _rnd = new Random();
            _enemyAction = _go.GetComponent<EnemyAction>();
            _fieldOfView = _go.GetComponent<FieldOfView>();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (isReadyNextATK)
            {
                int action = Random.Range(0, 3);
                _actionType = (CombatActionType) action;

                switch (_actionType)
                {
                    case CombatActionType.HeavyAttack:
                        HeavyAttack();
                        break;
                    case CombatActionType.LightAttack:
                        LightAttack();
                        break;
                    case CombatActionType.Defend:
                        Defend();
                        break;

                }
            }
            
            ResetAttackCD();

            if (_fieldOfView.DistanceToPlayer > 5)
            {
                _sm._CurState = new IdleState(_go, _sm, _attributes, _animator);
            }
            
        }


        private void HeavyAttack()
        {
            isReadyNextATK = false;
            isCDOn = true;
            AttackCD = AttackCDVal;
            _animator.SetTrigger(HeavyAttack1);
            
        }

        private void LightAttack()
        {
            isReadyNextATK = false;
            isCDOn = true;
            AttackCD = AttackCDVal;
            _animator.SetTrigger(Attack);
        }

        private void Defend()
        {
            isReadyNextATK = false;
            isCDOn = true;
            AttackCD = AttackCDVal;
            _animator.SetTrigger(Block);
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
}