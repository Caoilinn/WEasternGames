using AI;
using UnityEngine;
using UnityTemplateProjects.AI;

namespace New_folder.Scripts.AIStateMachine
{
    public class TauntingState : FleeingState
    {
        private CharacterMotor _characterMotor;
        private CatchParticipant _catchParticipant;
        
        public TauntingState(GameObject go, StateMachine sm) : base(go, sm)
        {
        }

        public override void Enter()
        {
            _characterMotor = _go.GetComponent<CharacterMotor>();
            _catchParticipant = _go.GetComponent<CatchParticipant>();
        }

        public override void FixedUpdate()
        {
            Vector3 fleeVec = (_go.transform.position - CatchParticipant._catcher.transform.position).normalized;
            
            Vector3 targetPos = CatchParticipant._catcher.transform.position + fleeVec * 5;
            //They are faster than you because they are not normalized 
            _characterMotor._moveDir = (targetPos - _go.transform.position);
            
            if(_catchParticipant._catchRole == CatchParticipant.CatchRole.Catcher)
                _sm._CurState = new ChasingState(_go, _sm);
        }
    }
}