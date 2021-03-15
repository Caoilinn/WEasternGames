using System.Collections.Generic;
using UnityEngine;

namespace AI.States
{
    public class IdleState : State
    {
        private FieldOfView _fieldOfView;
        private Animator _animator;
        private static readonly int Idle = Animator.StringToHash("idle");
        
        public readonly AIController aiController;

        public IdleState(GameObject go, StateMachine sm, List<IAIAttribute> attributes) : base(go, sm, attributes)
        { 
            //Debug.Log("Enemy with name " + _go.name +  " is printing " + AIManager.current);
            //AIManager.current.OnAttackStateChangeReq += OnAttackStateChange;

            aiController = _go.GetComponent<AIController>();
            _fieldOfView = _go.GetComponent<FieldOfView>();
            
            _animator = _go.GetComponent<Animator>();
            
            
            _fieldOfView.PlayerSpotted = false;
            
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!_fieldOfView.PlayerSpotted)
            {
                //_animator.SetBool(Idle, true);
            }
            if(_fieldOfView.PlayerSpotted)
            {
                _animator.SetBool(Idle, false);
                //Debug.Log("Enter Follow from Idle");
                _sm._CurState = new FollowState(_go, _sm, _attributes);
            }
        }
    }
}