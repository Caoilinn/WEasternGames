using System.Collections.Generic;
using UnityEngine;

namespace AI.States
{
    public class IdleState : State
    {
        private FieldOfView _fieldOfView;
        private static readonly int Idle = Animator.StringToHash("idle");
        
        public readonly AIController aiController;

        public IdleState(GameObject go, StateMachine sm, List<IAIAttribute> attributes, Animator animator) : base(go, sm, attributes, animator)
        { 
            //Debug.Log("Enemy with name " + _go.name +  " is printing " + AIManager.current);
            //AIManager.current.OnAttackStateChangeReq += OnAttackStateChange;

            aiController = (AIController) _attributes.Find(x => x.GetType() == typeof(AIController));
            _fieldOfView = (FieldOfView) _attributes.Find(x => x.GetType() == typeof(FieldOfView));
            _fieldOfView.PlayerSpotted = false;
            
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            if (!_fieldOfView.PlayerSpotted) return;
            
            _animator.SetBool(Idle, false);
            _sm._CurState = new FollowState(_go, _sm, _attributes, _animator);
        }
    }
}