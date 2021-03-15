using System.Collections.Generic;
using UnityEngine;

namespace AI.States
{
    public class ReturnToIdleState : State
    {
        public ReturnToIdleState(GameObject go, StateMachine sm, List<IAIAttribute> attributes) : base(go, sm, attributes)
        {
        }
    }
}