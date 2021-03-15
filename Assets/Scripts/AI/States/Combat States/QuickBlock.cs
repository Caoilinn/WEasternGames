using System.Collections.Generic;
using AI;
using UnityEngine;
using UnityTemplateProjects.Animation;

public class QuickBlock : State
{
        private Animator _anim;
        private AnimationAction _blockAction;
        private AIController _aiController;
        private bool _alreadyBlocked = false;
        private float _animTime;

        private int _blockAnimHash = Animator.StringToHash("getPlayerPerfectBlockImpact");
        
        public QuickBlock(GameObject go, StateMachine sm, List<IAIAttribute> attributes) : base(go, sm, attributes)
        {
        }

        public override void Enter()
        {
                base.Enter();
                Debug.Log("Entered QuickBlock");
                
                _anim = _go.GetComponent<Animator>();
                
                //_aiController = (AIController) _attributes.Find(x => x.GetType() == typeof(AIController));
                _aiController = _go.GetComponent<AIController>();
                
                Debug.Log("AIController ID: " + _aiController.id);
                
                foreach (AnimationClip clip in _anim.runtimeAnimatorController.animationClips)
                {
                        if (!clip.name.Contains("Enter Block")) continue;
            
                        _blockAction = new AnimationAction(clip);
                        _animTime = _blockAction.AnimationClipLength;
                }
                
                Debug.Log(_animTime);
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
                        _sm._CurState = new AttackingState(_go, _sm, _attributes);
                }
}

        private void DoBlock()
        {
                _anim.SetTrigger(_blockAnimHash);
        }
}
