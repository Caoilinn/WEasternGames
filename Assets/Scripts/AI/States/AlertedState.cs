using System.Collections.Generic;
using UnityEngine;

namespace AI.States
{
    public class AlertedState : State
    {
        public AlertedState(GameObject go, StateMachine sm, List<IAIAttribute> attributes, Animator animator) : base(go, sm, attributes, animator)
        {
        }
    }
}