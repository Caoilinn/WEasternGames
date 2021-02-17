using System.Collections.Generic;
using UnityEngine;

namespace AI.States
{
    public class IdleState : State
    {
        private FieldOfView _fieldOfView;
        private FieldOfView _fieldOfViewR;
        private FieldOfView _fieldOfViewL;
        private Animator _animator;
        private static readonly int Idle = Animator.StringToHash("idle");

        public IdleState(GameObject go, StateMachine sm) : base(go, sm)
        {
            _fieldOfView = _go.GetComponent<FieldOfView>();
            _fieldOfViewR = _go.transform.Find("RightFOV").GetComponent<FieldOfView>();
            _fieldOfViewL = _go.transform.Find("LeftFOV").GetComponent<FieldOfView>();
            
            _animator = _go.GetComponent<Animator>();

            _fieldOfView.PlayerSpotted = false;
            _fieldOfViewR.PlayerSpotted = false;
            _fieldOfViewL.PlayerSpotted = false;
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
            if(_fieldOfView.PlayerSpotted || _fieldOfViewR.PlayerSpotted || _fieldOfViewL.PlayerSpotted)
            {
                _animator.SetBool(Idle, false);
                _sm._CurState = new FollowState(_go, _sm);
            }
        }
    }
}