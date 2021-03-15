using System.Collections.Generic;
using UnityEngine;

namespace AI.States
{
    public class ReturnToIdleState : State
    {
        public ReturnToIdleState(GameObject go, StateMachine sm, List<IAIAttribute> attributes, Animator animator) : base(go, sm, attributes, animator)
        {
        }
    }
}