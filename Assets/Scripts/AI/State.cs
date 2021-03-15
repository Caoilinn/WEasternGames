using System.Collections.Generic;
using UnityEngine;
//Using the state pattern from: https://www.udemy.com/course/ai-in-unity
//All subsequent states will follow this design pattern however their implementation will be unique  
namespace AI
{
    public abstract class State
    {
        protected GameObject _go;
        protected StateMachine _sm;
        protected List<IAIAttribute> _attributes;
        protected Animator _animator;

        protected State(GameObject go, StateMachine sm, List<IAIAttribute> attributes, Animator animator)
        {
            _go = go;
            _sm = sm;
            _attributes = attributes;
            _animator = animator;
        }

        //Called when the state is entered
        public virtual void Enter(){}
        //Like GameObject.Update()
        public virtual void Update(){}
        //Like GameObject.FixedUpdate()        
        public virtual void FixedUpdate(){}
        //Called when the state is exited
        public virtual void Exit(){}
    }
}