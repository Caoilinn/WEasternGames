using System.Collections.Generic;
using AI;
using UnityEngine;
using UnityTemplateProjects.Animation;

public class QuickBlock : State
{
        private AnimationAction _blockAction;
        private AIController _aiController;
        private bool _alreadyBlocked = false;
        private float _animTime;

        private int _blockAnimHash = Animator.StringToHash("getPlayerPerfectBlockImpact");
        
        public QuickBlock(GameObject go, StateMachine sm, List<IAIAttribute> attributes, Animator animator) : base(go, sm, attributes, animator)
        {
        }

        public override void Enter()
        {
                base.Enter();
                Debug.Log("Entered QuickBlock");
                
                _animator = _go.GetComponent<Animator>();
                
                _aiController = (AIController) _attributes.Find(x => x.GetType() == typeof(AIController));
                //_aiController = _go.GetComponent<AIController>();
                
                Debug.Log("AIController ID: " + _aiController.id);
                
                foreach (AnimationClip clip in _animator.runtimeAnimatorController.animationClips)
                {
                        if (!clip.name.Contains("Enter Block")) continue;
                        
                        _blockAction = new AnimationAction(clip);
                        _animTime = _blockAction.AnimationClipLength;
                }
        }

        public override void FixedUpdate()
        {
                base.FixedUpdate();

                if (!_alreadyBlocked)
                {
                        _alreadyBlocked = true;
                        DoBlock();
                }

                _animTime -= Time.fixedDeltaTime;
                
                if (_animTime <= 0)
                { 
                        _aiController.done = false;
                        _sm._CurState = new AttackingState(_go, _sm, _attributes, _animator);
                }
}

        private void DoBlock()
        {
                _animator.SetTrigger(_blockAnimHash);
        }
}
