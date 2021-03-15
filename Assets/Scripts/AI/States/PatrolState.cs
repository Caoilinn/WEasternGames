using System.Collections.Generic;
using UnityEngine;

namespace AI.States
{
    public class PatrolState : State
    {
        public PatrolState(GameObject go, StateMachine sm, List<IAIAttribute> attributes) : base(go, sm, attributes)
        {
        }
    }
}