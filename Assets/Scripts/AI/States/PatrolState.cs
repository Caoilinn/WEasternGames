using System.Collections.Generic;
using UnityEngine;

namespace AI.States
{
    public class PatrolState : State
    {
        public PatrolState(GameObject go, StateMachine sm, List<IAIAttribute> attributes, Animator animator) : base(go, sm, attributes, animator)
        {
        }
    }
}