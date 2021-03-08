using System;
using UnityEngine;

namespace AI.States
{
    public class FollowState : State
    {
        private float _moveSpeed;
        
        private FieldOfView _fieldOfView;
        private FieldOfView _fieldOfViewR;
        private FieldOfView _fieldOfViewL;
        
        private Animator _animator;
        
        #region Animation Triggers
        
        private static readonly int IsWalking = Animator.StringToHash("isWalking");
        private static readonly int IsRunning = Animator.StringToHash("isRunning");

        private float _zVel;
        private int _zVelHash;

        private int _sequence;
        private float _timeRemaining;
        
        #endregion

        public FollowState(GameObject go, StateMachine sm, int sequence = 0, float timeRemaining = 0) : base(go, sm)
        {
            _sequence = sequence;
            timeRemaining = timeRemaining;
            
            //AIManager.current.OnAttackStateChangeReq += OnAttackStateChange;
        }

        public override void Enter()
        {
            base.Enter();
            _fieldOfView = _go.GetComponent<FieldOfView>();
            _fieldOfViewR = _go.transform.Find("RightFOV").GetComponent<FieldOfView>();
            _fieldOfViewL = _go.transform.Find("LeftFOV").GetComponent<FieldOfView>();
            _animator = _go.GetComponent<Animator>();
            _moveSpeed = 8f;
            _zVelHash = Animator.StringToHash("enemyVelZ");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            float distanceToPlayer = Vector3.Distance(_go.transform.position, _fieldOfView.Player.transform.position);
          
           
            if (_fieldOfView.Player != null && distanceToPlayer >= 1.5)
            {
                _zVel = 2;
                _go.transform.LookAt(_fieldOfView.Player.transform.position);
                _go.transform.position += _go.transform.forward * _moveSpeed * Time.deltaTime;
                _animator.SetFloat(_zVelHash, _zVel);
            }

            if (distanceToPlayer <= 1.5)
            {
                _zVel = 0;
                _animator.SetFloat(_zVelHash, _zVel);
                _sm._CurState = new AttackingState(_go, _sm, _sequence, _timeRemaining);
            }
        }

        //private void OnAttackStateChange()
        //{
       //     Debug.Log("Event Triggered for changing to Attacking State");
       //}
    }
}