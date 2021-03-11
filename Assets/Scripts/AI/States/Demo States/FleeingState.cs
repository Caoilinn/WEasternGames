using AI;
using UnityEngine;
using UnityTemplateProjects.AI;

namespace New_folder.Scripts.AIStateMachine
{
    public class FleeingState : State
    {
        protected CharacterMotor _characterMotor;
        protected CatchParticipant _catchParticipant;
        
        public FleeingState(GameObject go, StateMachine sm) : base(go, sm)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _characterMotor = _go.GetComponent<CharacterMotor>();
            _catchParticipant = _go.GetComponent<CatchParticipant>();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            Vector3 fleeVec = (_go.transform.position - CatchParticipant._catcher.transform.position).normalized;
            _characterMotor._moveDir = fleeVec;
            
            if(_catchParticipant._catchRole == CatchParticipant.CatchRole.Catcher)
                _sm._CurState = new ChasingState(_go, _sm);
        }
    }
}