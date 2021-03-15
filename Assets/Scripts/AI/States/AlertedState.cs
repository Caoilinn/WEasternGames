using System.Collections.Generic;
using UnityEngine;

namespace AI.States
{
    public class AlertedState : State
    {
        public AlertedState(GameObject go, StateMachine sm, List<IAIAttribute> attributes) : base(go, sm, attributes)
        {
        }
    }
}