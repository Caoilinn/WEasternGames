using UnityEngine;

namespace AI.States
{
    public class IdleState : State
    {
        private FieldOfView _fieldOfView;
        private Animator _animator;
        private static readonly int Idle = Animator.StringToHash("idle");

        public IdleState(GameObject go, StateMachine sm) : base(go, sm)
        {
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
            else
            {
                _animator.SetBool(Idle, false);
                _sm._CurState = new FollowState(_go, _sm);
            }
        }
    }
}